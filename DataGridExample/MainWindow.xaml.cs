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

namespace DataGridExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Otwieramy polaczenie z db z jakimiś dziwnym parametrem
        //LINQtoSQLclassExample1DataContext databaseConnection = new LINQtoSQLclassExample1DataContext(Properties.Settings.Default.DatabaseTest1ConnectionString);

        //Otwieramy polaczenie z DB bez parametru:
        LINQtoSQLclassExample1DataContext databaseConnection = new LINQtoSQLclassExample1DataContext();

       

        public MainWindow()
        {
            InitializeComponent();

            //sprawdzamy czy baza istnieje i jezeli tak to uzupelniamy datagrid
            //if (databaseConnection.DatabaseExists())
            //{
            //    datagrid2.ItemsSource = databaseConnection.Orders;
            //}

            //Nadpisujemy DB danymi z datagrid:

            // databaseConnection.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);

            //wyswietlamy dane w datagrid

            // datagrid2.ItemsSource = databaseConnection.Customers;    

            datagrid2.ItemsSource = databaseConnection.Customers;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Savebutton_Click(object sender, RoutedEventArgs e)
        {
            //Wpisuje kolumne CustomerID
            var allCustomers = from t in databaseConnection.Customers
                               select t.CustomerID;



            //Wyszukuje najwieksza liczbe z kolumny CustomerID konwertujac wczesniej do INT32, bo na samym stringu nie zadziala

            var maxCustomerIdFromDB = (from max_db in databaseConnection.Customers
                            where max_db.CustomerID != null
                            select Convert.ToInt32(max_db.CustomerID)).Max();

            int max = maxCustomerIdFromDB + 1;
            
            string nextCustomerID = max.ToString();


            //Nowy obiekt ktory przechowuje to co bedziemy chcieli zapisac w DB
            Customer customerInsert= new Customer
            {

                CustomerID = nextCustomerID,
                CompanyName = "dsarfg",
                ContactName = "asfa",
                Phone = "124456789"
                
            };

            //szykujemy rozkaz do wyslania
            databaseConnection.Customers.InsertOnSubmit(customerInsert);
            

            //wysylamy rozkaz z obsluga bledow
            try
            {
                databaseConnection.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
            }
            catch (Exception exeption_db1)
            {
                Console.WriteLine(exeption_db1);

                databaseConnection.SubmitChanges();
            }

        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {

            datagrid2.UpdateLayout();
        }

        private void LoadButton2_Click(object sender, RoutedEventArgs e)
        {
            //Wyszukuje najwieksza liczbe z kolumny CustomerID konwertujac wczesniej do INT32, bo na samym stringu nie zadziala

            var maxCustomerIdFromDB = (from max_db in databaseConnection.Customers
                                       where max_db.CustomerID != null
                                       select Convert.ToInt32(max_db.CustomerID)).Max();

            int max = maxCustomerIdFromDB + 1;

            string nextCustomerID = max.ToString();


            //Nowy obiekt ktory przechowuje to co bedziemy chcieli zapisac w DB
            Customer customerInsert = new Customer
            {

                CustomerID = nextCustomerID,
                CompanyName = textBox.Text,
                ContactName = textBox2.Text,
                Phone = textBox3.Text

            };

            //szykujemy rozkaz do wyslania
            databaseConnection.Customers.InsertOnSubmit(customerInsert);


            //wysylamy rozkaz z obsluga bledow
            try
            {
                databaseConnection.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
            }
            catch (Exception exeption_db1)
            {
                Console.WriteLine(exeption_db1);

                databaseConnection.SubmitChanges();
            }
        }
    }
}
