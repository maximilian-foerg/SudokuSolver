using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SudokuControl
{
    /// <summary>
    /// Interaction logic for SudokuRegionControl.xaml
    /// </summary>
    public partial class SudokuRegionControl : UserControl
    {
        private Boolean inEditState = false;
        private Button clickSource;
        private readonly Dictionary<Button, object> fieldContents;
        private readonly Style standardFieldStyle;
        private readonly Style editableFieldStyle;

        public SudokuRegionControl()
        {
            InitializeComponent();
            standardFieldStyle = (Style)this.FindResource("StandardFieldStyle");
            editableFieldStyle = (Style)this.FindResource("EditableFieldStyle");
            fieldContents = new();
        }

        void HandleClick(object sender, RoutedEventArgs e)
        {
            SwitchState(sender);
        }

        private void SwitchState(object sender)
        {
            inEditState = !inEditState;
            if (inEditState)
            {
                clickSource = (Button)sender;
                this.SaveFieldContents();
                field00.Content = "1";
                field01.Content = "2";
                field02.Content = "3";
                field10.Content = "4";
                field11.Content = "5";
                field12.Content = "6";
                field20.Content = "7";
                field21.Content = "8";
                field22.Content = "9";
            }
            else
            {
                Button valueSource = (Button)sender;
                fieldContents[clickSource] = valueSource.Content;
                LoadFieldContents();
                fieldContents.Clear();
            }
            this.SwitchStyle(inEditState);
        }

        private void SwitchStyle(Boolean editable)
        {
            if(editable)
            {
                field00.Style = editableFieldStyle;
                field01.Style = editableFieldStyle;
                field02.Style = editableFieldStyle;
                field10.Style = editableFieldStyle;
                field11.Style = editableFieldStyle;
                field12.Style = editableFieldStyle;
                field20.Style = editableFieldStyle;
                field21.Style = editableFieldStyle;
                field22.Style = editableFieldStyle;
            }
            else
            {
                field00.Style = standardFieldStyle;
                field01.Style = standardFieldStyle;
                field02.Style = standardFieldStyle;
                field10.Style = standardFieldStyle;
                field11.Style = standardFieldStyle;
                field12.Style = standardFieldStyle;
                field20.Style = standardFieldStyle;
                field21.Style = standardFieldStyle;
                field22.Style = standardFieldStyle;
            }
        }

        private void SaveFieldContents()
        {
            fieldContents[field00] = field00.Content;
            fieldContents[field01] = field01.Content;
            fieldContents[field02] = field02.Content;
            fieldContents[field10] = field10.Content;
            fieldContents[field11] = field11.Content;
            fieldContents[field12] = field12.Content;
            fieldContents[field20] = field20.Content;
            fieldContents[field21] = field21.Content;
            fieldContents[field22] = field22.Content;
        }

        private void LoadFieldContents()
        {
            foreach(KeyValuePair<Button, object> field in fieldContents)
            {
                field.Key.Content = fieldContents[field.Key];
            }
        }
    }
}
