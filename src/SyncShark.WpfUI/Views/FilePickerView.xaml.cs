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

namespace SyncShark.WpfUI.Views
{
    /// <summary>
    /// Interaction logic for FilePickerView.xaml
    /// </summary>
    public partial class FilePickerView : UserControl
    {
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(FilePickerView), new PropertyMetadata(""));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FilePickerView), new PropertyMetadata(""));

        public string Description { get { return (string)GetValue(DescriptionProperty); } set { SetValue(DescriptionProperty, value); } }
        public string Text { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }

        public FilePickerView()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void BrowseFolder(object sender, RoutedEventArgs e)
        {
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                dlg.Description = Description;
                dlg.SelectedPath = Text;
                dlg.ShowNewFolderButton = true;
                var result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Text = dlg.SelectedPath;
                }
            }
        }
    }
}
