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
using DomainLibrary;
using DataLayer;
using DomainLibrary.Domain;

namespace WpfInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContext("Production")));

            if (MonthRadioButton.IsChecked==true)
            {
                if (RunningRadioButton.IsChecked == true)
                {
                    Report report = m.GenerateMonthlyRunningReport(2020, (cmbMonth.SelectedIndex) + 1);
                    dgTraining.ItemsSource = report.Runs;
                    txtBestDistance.Text = report.MaxDistanceSessionRunning.ToString();
                    txtHighestAverageSpeed.Text = report.MaxSpeedSessionRunning.ToString();
                }
                else if (CyclingRadioButton.IsChecked == true)
                {
                    Report report = m.GenerateMonthlyCyclingReport(2020, (cmbMonth.SelectedIndex) + 1);
                    dgTraining.ItemsSource = report.Rides;
                    txtBestDistance.Text = report.MaxDistanceSessionCycling.ToString();
                    txtHighestAverageSpeed.Text = report.MaxSpeedSessionCycling.ToString();
                    txtHighestWattage.Text = report.MaxWattSessionCycling.ToString();
                }
                    
                else
                    dgTraining.ItemsSource = m.GenerateMonthlyTrainingsReport(2020, (cmbMonth.SelectedIndex) + 1).TimeLine;
            }
            else
            {
                if (RunningRadioButton.IsChecked == true)
                    dgTraining.ItemsSource = m.GetPreviousRunningSessions(int.Parse(txtSessionAmount.Text));
                else if (CyclingRadioButton.IsChecked == true)
                    dgTraining.ItemsSource = m.GetPreviousCyclingSessions(int.Parse(txtSessionAmount.Text));
            }  
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddTrainingWindow addTrainingWindow = new AddTrainingWindow();
            addTrainingWindow.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContext("Production")));

            if (CyclingRadioButton.IsChecked == true)
            {
                CyclingSession selectedItem = (CyclingSession)dgTraining.SelectedItem;
                m.RemoveTrainings(new List<int> { selectedItem.Id }, new List<int>());
            }
            else
            {
                RunningSession selectedItem = (RunningSession)dgTraining.SelectedItem;
                m.RemoveTrainings(new List<int>(), new List<int> { selectedItem.Id });
            }
        }
    }
}
