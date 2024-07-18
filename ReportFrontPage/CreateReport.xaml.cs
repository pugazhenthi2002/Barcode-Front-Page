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

namespace ReportFrontPage
{
    /// <summary>
    /// Interaction logic for CreateReport.xaml
    /// </summary>
    public partial class CreateReport : Window
    {
        public event EventHandler<ReportProfile> ReportCreate;

        public CreateReport()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReportCreate?.Invoke(this, new ReportProfile()
            {
                ReportName = reportNameTextBox.Text,
                DefectCount = 12,
                OperatorName = "Mr. XYZ",
                Prefix = "AA"
            });
        }
    }
}