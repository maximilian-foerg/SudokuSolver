using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuLibrary
{
    public class ConstraintPropagationSudokuSolver : ISudokuSolver
    {
        private CancellationTokenSource cts;

        public async Task SolveSudokuAsync(Sudoku sudoku)
        {
            cts = new();
            Task task = Task.Run(() => SolveSudoku(sudoku), cts.Token);
            await task;
        }

        public void Cancel()
        {
            if (cts != null)
                cts.Cancel();
        }

        private void SolveSudoku(Sudoku sudoku)
        {
            DomainStore dstore = new(sudoku);
            Search(dstore, sudoku);
        }

        private void Search(DomainStore dstore, Sudoku sudoku)
        {
            if (cts.IsCancellationRequested)
            {
                return;
            }
            // Try to solve sudoku via constraint propagation only
            while(Propagate(dstore)) {}
            dstore.UpdateSudoku(sudoku);
            if (sudoku.IsSolved())
            {
                return;
            }
            // If that didn't work, perform a recursive search
            SearchRec(dstore, sudoku);
        }

        private void SearchRec(DomainStore dstore, Sudoku sudoku)
        {
            // Select the sudoku cell with the smallest domain and try to fill it first
            Field f = dstore.GetFieldWithSmallestDomain();
            if (f == null)
            {
                // There's no cell with a domain size > 1 left, the sudoku is filled completely
                dstore.UpdateSudoku(sudoku);
                return;
            }
            // Try to solve the sudoku with every possible value of the cell's domain
            HashSet<int> domain = dstore.GetDomain(f);
            foreach (int digit in domain)
            {
                DomainStore dstoreClone = new(dstore);
                dstoreClone.SetDomain(f, new HashSet<int>() {digit});
                Search(dstoreClone, sudoku);
                if (sudoku.IsSolved())
                {
                    return;
                }
                else
                {
                    // Prune this try and take a step back
                    dstore.UpdateSudoku(sudoku);
                }
            }
        }

        private bool Propagate(DomainStore dstore)
        {
            if (cts.IsCancellationRequested)
            {
                return false;
            }
            bool domainsHaveChanged = EliminateRowValues(dstore);
            domainsHaveChanged ^= EliminateColumnValues(dstore);
            domainsHaveChanged ^= EliminateRegionValues(dstore);
            return domainsHaveChanged;
        }

        private bool Eliminate(DomainStore dstore, IEnumerable<Field> fields)
        {
            bool domainsHaveChanged = false;
            // Go through the cells and remeber the digits you see
            HashSet<int> seenDigits = new();
            foreach (Field f in fields)
            {
                if (dstore.GetDomainSize(f) == 1)
                {
                    HashSet<int> domain = dstore.GetDomain(f);
                    foreach (int digit in domain) // Workaround to access first HashSet value quickly
                    {
                        seenDigits.Add(digit);
                        break;
                    }
                }
            }
            // Now, go through the cells again and update each cell's domain considering the digits you saw in the other cells
            foreach (Field f in fields)
            {
                // If a cells's domain has just a single digit left, it does not have to be changed anymore
                if (dstore.GetDomainSize(f) == 1)
                {
                    continue;
                }
                HashSet<int> domain = dstore.GetDomain(f);
                HashSet<int> impossibleDigits = new(seenDigits);
                HashSet<int> subtraction = Util.Subtract(domain, impossibleDigits);
                // Prune this try if no digit can't be assigned to the cell
                if (subtraction.Count == 0)
                {
                    return false;
                }
                // If a cell's domain has to be updated based on the seen digits, do it and note that there was a change
                if (!domain.SetEquals(subtraction))
                {
                    domainsHaveChanged = true;
                    dstore.SetDomain(f, subtraction);
                }
            }
            return domainsHaveChanged;
        }

        private bool EliminateRowValues(DomainStore dstore)
        {
            bool domainsHaveChanged = false;
            for (int x = 0; x < Sudoku.Size; x++)
            {
                List<Field> row = new();
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    Field f = new (x, y);
                    row.Add(f);
                }
                domainsHaveChanged ^= Eliminate(dstore, row);
            }
            return domainsHaveChanged;
        }

        private bool EliminateColumnValues(DomainStore dstore)
        {
            bool domainsHaveChanged = false;
            for (int y = 0; y < Sudoku.Size; y++)
            {
                List<Field> column = new();
                for (int x = 0; x < Sudoku.Size; x++)
                {
                    Field f = new(x, y);
                    column.Add(f);
                }
                domainsHaveChanged ^= Eliminate(dstore, column);
            }
            return domainsHaveChanged;
        }

        private bool EliminateRegionValues(DomainStore dstore)
        {
            bool domainsHaveChanged = false;
            for (int i = 0; i < Sudoku.Size; i++)
            {
                List<Field> region = new();
                for (int j = 0; j < Sudoku.Size; j++)
                {
                    int x = (i / Sudoku.RegionSize) * Sudoku.RegionSize + j / Sudoku.RegionSize;
                    int y = (i % Sudoku.RegionSize) * Sudoku.RegionSize + j % Sudoku.RegionSize;
                    Field f = new(x, y);
                    region.Add(f);
                }
                domainsHaveChanged ^= Eliminate(dstore, region);
            }
            return domainsHaveChanged;
        }
    }

    class DomainStore
    {
        private readonly HashSet<int>[,] store = new HashSet<int>[Sudoku.Size, Sudoku.Size];
        private readonly HashSet<Field>[] fieldsByDomainSize = new HashSet<Field>[Sudoku.Size];

        public DomainStore()
        {
            InitFieldsByDomainSize();
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    this.SetDomain(x, y, Sudoku.PossibleDigits);
                }
            }
        }

        public DomainStore(Sudoku sudoku)
        {
            InitFieldsByDomainSize();
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    if (sudoku.IsUnassigned(x, y))
                    {
                        this.SetDomain(x, y, Sudoku.PossibleDigits);
                    }
                    else
                    {
                        this.AddDomainValue(x, y, sudoku.GetCellDigit(x, y));
                    }
                }
            }
        }

        public DomainStore(DomainStore dstore)
        {
            InitFieldsByDomainSize();
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    this.SetDomain(x, y, dstore.GetDomain(x, y));
                }
            }
        }

        private void InitFieldsByDomainSize()
        {
            for (int i = 0; i < Sudoku.Size; i++)
            {
                fieldsByDomainSize[i] = new HashSet<Field>();
            }
        }

        public HashSet<int> GetDomain(int x, int y)
        {
            return store[x, y];
        }

        public HashSet<int> GetDomain(Field f)
        {
            return this.GetDomain(f.X, f.Y);
        }

        public int GetDomainSize(int x, int y)
        {
            return this.GetDomain(x, y).Count;
        }

        public int GetDomainSize(Field f)
        {
            return this.GetDomainSize(f.X, f.Y);
        }

        public void SetDomain(int x, int y, IEnumerable<int> domain)
        {
            Field f = new(x, y);
            if (this.GetDomain(f) == null)
            {
                store[x, y] = new HashSet<int>();
            }
            else
            {
                int oldDomainSize = this.GetDomainSize(x, y);
                fieldsByDomainSize[oldDomainSize - 1].Remove(f);
            }
            store[x, y] = new HashSet<int>(domain);
            int newDomainSize = this.GetDomainSize(x, y);
            fieldsByDomainSize[newDomainSize - 1].Add(f);
        }

        public void SetDomain(Field f, IEnumerable<int> domain)
        {
            this.SetDomain(f.X, f.Y, domain);
        }

        public void AddDomainValue(int x, int y, int val)
        {
            Field f = new(x, y);
            if (this.GetDomain(f) == null)
            {
                store[x, y] = new HashSet<int>() {val};
                fieldsByDomainSize[0].Add(f);
            }
            else
            {
                int oldDomainSize = this.GetDomainSize(x, y);
                fieldsByDomainSize[oldDomainSize - 1].Remove(f);
                store[x, y].Add(val);
                fieldsByDomainSize[oldDomainSize].Add(f);
            }
        }

        public void AddDomainValue(Field f, int val)
        {
            this.AddDomainValue(f.X, f.Y, val);
        }

        public void RemoveDomainValue(int x, int y, int val)
        {
            int domainSize = this.GetDomainSize(x, y);
            if (domainSize > 1) // You can't remove the last value
            {
                Field f = new(x, y);
                fieldsByDomainSize[domainSize - 1].Remove(f);
                store[x, y].Remove(val);
                fieldsByDomainSize[domainSize - 2].Add(f);
            }
        }

        public void RemoveDomainValue(Field f, int val)
        {
            this.RemoveDomainValue(f.Y, f.Y, val);
        }

        public Field GetFieldWithSmallestDomain()
        {
            Field field = null;
            // Cells with a domain size of 1 (index 0) are ignored, as their digit is already fixed
            for (int i = 1; i < Sudoku.Size; i++)
            {
                if (fieldsByDomainSize[i].Count > 0)
                {
                    foreach (Field f in fieldsByDomainSize[i]) // Workaround to access first HashSet value quickly
                    {
                        field = f;
                        break;
                    }
                    break;
                }
            }
            return field;
        }

        public void UpdateSudoku(Sudoku sudoku)
        {
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    HashSet<int> domain = this.GetDomain(x, y);
                    if (domain.Count == 1)
                    {
                        foreach (int val in domain) // Workaround to access first HashSet value quickly
                        {
                            sudoku.SetCellDigit(x, y, val);
                            break;
                        }
                    }
                    else
                    {
                        sudoku.ClearCell(x, y);
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder dstoreAsString = new StringBuilder();
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    dstoreAsString.Append(String.Format("Field {0},{1}:", x, y));
                    foreach (int val in store[x, y])
                    {
                        dstoreAsString.Append(" " + val);
                    }
                    dstoreAsString.Append('\n');
                }
            }
            return dstoreAsString.ToString();
        }
    }
}
