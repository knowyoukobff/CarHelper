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
using System.Data;
using System.Data.SqlClient;

namespace CarHelper
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string conn = "Server=KONRAD\\SQLEXPRESS;Database=CarHelper;Integrated Security=true;";
        public string brand;
        public string model;
        public string engine;

        SqlConnection connection;
        public MainWindow()
        {
            InitializeComponent();



            connection = new SqlConnection(conn);
            connection.Open();
            ratedEngines.Text += Environment.NewLine + Environment.NewLine;
            SqlCommand cmdRate = new SqlCommand("SELECT carBrand, carModel, engineDesignation, (rate/amountOfvotes) FROM carsRating INNER JOIN Cars ON carsRating.versionID = Cars.CarID order by (rate/amountOfvotes) DESC", connection);
            SqlDataReader sdrRate = cmdRate.ExecuteReader();
            int a = 1;
            while (sdrRate.Read())
            {
                ratedEngines.Text += a + ". " + sdrRate[0] + " " + sdrRate[1] + " " + sdrRate[2] + " :      " + sdrRate[3] + "/5" + Environment.NewLine;
                a++;
            }
            sdrRate.Close();

            SqlCommand cmd = new SqlCommand("SELECT carBrand FROM Cars group by carBrand", connection);
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                brandBox.Items.Add(sdr[0]);
            }
            sdr.Close();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            brand = brandBox.Text;
            modelBox.IsEnabled = true;
            modelBtn.IsEnabled = true;
            SqlCommand cmd1 = new SqlCommand("SELECT carModel FROM Cars WHERE carBrand='"+ brand +"' group by carModel", connection);
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            while (sdr1.Read())
            {
                modelBox.Items.Add(sdr1[0]);
            }
            sdr1.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            model = modelBox.Text;
            engineBox.IsEnabled = true;
            engineBtn.IsEnabled = true;
            SqlCommand cmd1 = new SqlCommand("SELECT engineDesignation FROM Cars WHERE carModel='" + model + "'", connection);
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            while (sdr1.Read())
            {
                engineBox.Items.Add(sdr1[0]);
            }
            sdr1.Close();
        }

        private void engineBtn_Click(object sender, RoutedEventArgs e)
        {
            engine = engineBox.Text;
            var CarDetailsWindow = new CarDetailsWindow(brand, model, engine, conn);
            CarDetailsWindow.Show();
            modelBox.Items.Clear();
            engineBox.Items.Clear();
            engineBox.IsEnabled = false;
            engineBtn.IsEnabled = false;
            modelBox.IsEnabled = false;
            modelBtn.IsEnabled = false;
        }
    }
}
