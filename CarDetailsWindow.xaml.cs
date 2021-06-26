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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace CarHelper
{
    /// <summary>
    /// Logika interakcji dla klasy CarDetailsWindow.xaml
    /// </summary>
    public partial class CarDetailsWindow : Window
    {
        SqlConnection connection;
        public string carID;
        public int rates;
        public int votes;
        public string sql;
        public CarDetailsWindow(string brand, string model, string engine, string conn)
        {
            InitializeComponent();
            connection = new SqlConnection(conn);
            connection.Open();

            SqlCommand cmd = new SqlCommand("select * from Cars WHERE carBrand = '"+ brand + "' AND carModel = '" + model + "' AND engineDesignation = '" + engine + "'", connection);
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                titleLabel.Content += sdr[1] + " " + sdr[2] + " " + sdr[4] + "  (" + sdr[13] + " - " + sdr[14] + ")";
                bodyLabel.Content += " " + sdr[3];
                capacityLabel.Content += " " + sdr[5];
                horseLabel.Content += " " + sdr[6];
                torqueLabel.Content += " " + sdr[7];
                accLabel.Content += " " + sdr[8];
                speedLabel.Content += " " + sdr[9];
                fuelLabel.Content += " " + sdr[10];
                oilLabel.Content += " " + sdr[11];
                carID += sdr[0];
            }
            sdr.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            SqlCommand cmdRate = new SqlCommand("SELECT rate, amountOfvotes FROM carsRating WHERE versionID="+ Convert.ToInt32(carID) + "", connection);
            SqlDataReader sdr = cmdRate.ExecuteReader();
            while (sdr.Read())
            {
                rates = Convert.ToInt32(sdr[0]);
                votes = Convert.ToInt32(sdr[1]);
            }
            sdr.Close();
            votes += 1;

            if(rad1.IsChecked == true)
            {
                rates += Convert.ToInt32(rad1.Content);
            }
            else if (rad2.IsChecked == true)
            {
                rates += Convert.ToInt32(rad2.Content);
            }
            else if (rad3.IsChecked == true)
            {
                rates += Convert.ToInt32(rad3.Content);
            }
            else if (rad4.IsChecked == true)
            {
                rates += Convert.ToInt32(rad4.Content);
            }
            else
            {
                rates += Convert.ToInt32(rad5.Content);
            }

            if(votes == 1)
            {
                sql = "INSERT INTO carsRating (versionID, rate, amountOfvotes)  VALUES (" + carID + ", " + rates + ", " + votes + ")";
            }
            else
            {
                sql = "UPDATE carsRating SET rate=" + rates + ", amountOfvotes=" + votes + " WHERE versionID=" + carID + "";
            }
            SqlCommand cmdAddRate = new SqlCommand(sql , connection);
            cmdAddRate.ExecuteNonQuery();
            rateBtn.IsEnabled = false;

        }

    }
}
