using System;
using System.Collections.Generic;
using System.Data;
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

namespace SqlMonitor
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseHandler DatabaseHandler;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DatabaseHandler = new DatabaseHandler(Host_TextBox.Text, User_TextBox.Text, Password_PasswordBox.Password);
                DataTable table = DatabaseHandler.RunQuery("SELECT name FROM master.sys.databases");
                List<string> dbs = table.AsEnumerable().Select(r => r.Field<string>("name")).ToList();
                TreeView_TreeView.Items.Clear();
                foreach (string db in dbs)
                {
                    TreeView_TreeView.Items.Add(db);
                }
            }
            catch(Exception ex)
            {
                LogConsole(ex.Message);
            }
            
            
        }

        private void LogConsole(string s)
        {
            textBoxDebug.AppendText(Environment.NewLine + s);
            textBoxDebug.ScrollToEnd();
        }

        private void Query_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key.Equals(Key.Enter))
            {
                RunQuery();
            }
        }

        private void RunQuery()
        {
            try
            {
                DataTable table = DatabaseHandler.RunQuery(new TextRange(Query_TextBox.Document.ContentStart, Query_TextBox.Document.ContentEnd).Text);
                DataGrid.ItemsSource = table.DefaultView;
            }
            catch (Exception ex)
            {
                LogConsole(ex.Message);
            }
        }
        
        private void TreeView_TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            LogConsole(TreeView_TreeView.SelectedItem.ToString() + " wurde ausgewählt.");
            DatabaseHandler.setDatabase(TreeView_TreeView.SelectedItem.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RunQuery();
        }
    }
}
