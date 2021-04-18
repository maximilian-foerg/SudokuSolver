using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver
{
    public class ConstraintPropagationSudokuSolver : ISudokuSolver
    {
        public Sudoku SolveSudoku(Sudoku sudoku)
        {
            DomainStore dstore = new DomainStore(sudoku);
            return Search(dstore);
        }

        private Sudoku Search(DomainStore dstore)
        {
            // Try to solve sudoku via constraint propagation only
            while(Propagate(dstore)) {}
            Sudoku assignment = dstore.ToSudoku();
            if (assignment.IsSolved())
            {
                return assignment;
            }
            // If this didn't work, perform a recursive search
            return SearchRec(dstore);
        }

        private Sudoku SearchRec(DomainStore dstore)
        {
            // Select the sudoku field with the smallest domain and try to fill it first
            Field f = dstore.GetFieldWithSmallestDomain();
            if (f == null)
            {
                // There's no field with a domain.Count > 1 left, the sudoku is filled completely
                return dstore.ToSudoku();
            }
            HashSet<int> domain = dstore.GetDomain(f);
            foreach (int val in domain)
            {
                // Try to solve the sudoku with every possible value of the field's domain
                DomainStore dstoreClone = dstore.Clone();
                dstoreClone.SetDomain(f, new HashSet<int>() {val});
                try
                {
                    Sudoku assignment = this.Search(dstoreClone);
                    if (assignment.IsSolved())
                    {
                        return assignment;
                    }
                }
                catch (UnsolvableStateException)
                {
                    // Prune this DomainStore
                    continue;
                }
            }
            return dstore.ToSudoku();
        }

        private Boolean Propagate(DomainStore dstore)
        {
            Boolean domainsHaveChanged = EliminateRowValues(dstore);
            domainsHaveChanged ^= EliminateColumnValues(dstore);
            domainsHaveChanged ^= EliminateRegionValues(dstore);
            return domainsHaveChanged;
        }

        private Boolean Eliminate(DomainStore dstore, IEnumerable<Field> fields)
        {
            Boolean domainsHaveChanged = false;
            // Go through the fields and remeber the values you see
            HashSet<int> seenValues = new HashSet<int>();
            foreach (Field f in fields)
            {
                if (dstore.GetDomainSize(f) == 1)
                {
                    HashSet<int> domain = dstore.GetDomain(f);
                    foreach (int val in domain)
                    {
                        seenValues.Add(val);
                        break;
                    }
                }
            }
            // Now, go through the fields again and update each fields domain considering the values you saw in the other fields
            foreach (Field f in fields)
            {
                // If a field's domain has just a single value left, it does not have to be changed anymore
                if (dstore.GetDomainSize(f) == 1)
                {
                    continue;
                }
                HashSet<int> domain = dstore.GetDomain(f);
                HashSet<int> impossibleValues = new HashSet<int>(seenValues);
                HashSet<int> subtraction = Util.Subtract(domain, impossibleValues);
                // Prune this try if no value can't be assigned to a field
                if (subtraction.Count == 0)
                {
                    throw new UnsolvableStateException();
                }
                // If a field's domain has to be updated based on the seen values, do it and note that there was a change
                if (!domain.SetEquals(subtraction))
                {
                    domainsHaveChanged = true;
                    dstore.SetDomain(f, subtraction);
                }
            }
            return domainsHaveChanged;
        }

        private Boolean EliminateRowValues(DomainStore dstore)
        {
            Boolean domainsHaveChanged = false;
            for (int x = 0; x < Sudoku.Size; x++)
            {
                List<Field> row = new List<Field>();
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    Field f = new Field(x, y);
                    row.Add(f);
                }
                domainsHaveChanged ^= Eliminate(dstore, row);
            }
            return domainsHaveChanged;
        }

        private Boolean EliminateColumnValues(DomainStore dstore)
        {
            Boolean domainsHaveChanged = false;
            for (int y = 0; y < Sudoku.Size; y++)
            {
                List<Field> column = new List<Field>();
                for (int x = 0; x < Sudoku.Size; x++)
                {
                    Field f = new Field(x, y);
                    column.Add(f);
                }
                domainsHaveChanged ^= Eliminate(dstore, column);
            }
            return domainsHaveChanged;
        }

        private Boolean EliminateRegionValues(DomainStore dstore)
        {
            Boolean domainsHaveChanged = false;
            for (int i = 0; i < Sudoku.Size; i++)
            {
                List<Field> region = new List<Field>();
                for (int j = 0; j < Sudoku.Size; j++)
                {
                    int x = (i / Sudoku.RegionSize) * Sudoku.RegionSize + j / Sudoku.RegionSize;
                    int y = (i % Sudoku.RegionSize) * Sudoku.RegionSize + j % Sudoku.RegionSize;
                    Field f = new Field(x, y);
                    region.Add(f);
                }
                domainsHaveChanged ^= Eliminate(dstore, region);
            }
            return domainsHaveChanged;
        }
    }

    [System.Serializable]
    public class UnsolvableStateException : System.Exception
    {
        public UnsolvableStateException() {}
    }

    class DomainStore
    {
        private HashSet<int>[,] store = new HashSet<int>[Sudoku.Size, Sudoku.Size];
        private HashSet<Field>[] fieldsByDomainSize = new HashSet<Field>[Sudoku.Size];

        public DomainStore()
        {
            InitFieldsByDomainSize();
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    this.SetDomain(x, y, Sudoku.PossibleValues);
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
                        this.SetDomain(x, y, Sudoku.PossibleValues);
                    }
                    else
                    {
                        this.AddDomainValue(x, y, sudoku.GetFieldValue(x, y));
                    }
                }
            }
        }

        public DomainStore Clone()
        {
            return (DomainStore) this.MemberwiseClone();
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
            return this.GetDomain(f.x, f.y);
        }

        public int GetDomainSize(int x, int y)
        {
            return this.GetDomain(x, y).Count;
        }

        public int GetDomainSize(Field f)
        {
            return this.GetDomainSize(f.x, f.y);
        }

        public void SetDomain(int x, int y, IEnumerable<int> domain)
        {
            Field f = new Field(x, y);
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
            this.SetDomain(f.x, f.y, domain);
        }

        public void AddDomainValue(int x, int y, int val)
        {
            Field f = new Field(x, y);
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
            this.AddDomainValue(f.x, f.y, val);
        }

        public void RemoveDomainValue(int x, int y, int val)
        {
            int domainSize = this.GetDomainSize(x, y);
            if (domainSize > 1)
            {
                Field f = new Field(x, y);
                fieldsByDomainSize[domainSize - 1].Remove(f);
                store[x, y].Remove(val);
                fieldsByDomainSize[domainSize - 2].Add(f);
            }
        }

        public void RemoveDomainValue(Field f, int val)
        {
            this.RemoveDomainValue(f.x, f.y, val);
        }

        public Field GetFieldWithSmallestDomain()
        {
            Field field = null;
            for (int i = 1; i < Sudoku.Size; i++)
            {
                if (fieldsByDomainSize[i].Count > 0)
                {
                    foreach (Field f in fieldsByDomainSize[i])
                    {
                        field = f;
                        break;
                    }
                    break;
                }
            }
            return field;
        }

        public Sudoku ToSudoku()
        {
            Sudoku sudoku = new Sudoku();
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    HashSet<int> domain = this.GetDomain(x, y);
                    if (domain.Count == 1)
                    {
                        foreach (int val in domain)
                        {
                            sudoku.SetFieldValue(x, y, val);
                            break;
                        }
                    }
                    else
                    {
                        sudoku.ClearField(x, y);
                    }
                }
            }
            return sudoku;
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
                    dstoreAsString.Append("\n");
                }
            }
            return dstoreAsString.ToString();
        }
    }
}
