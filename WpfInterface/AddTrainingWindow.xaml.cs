using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataLayer;
using DomainLibrary.Domain;

namespace WpfInterface
{
    /// <summary>
    /// Interaction logic for AddTrainingWindow.xaml
    /// </summary>
    public partial class AddTrainingWindow : Window
    {
        TrainingManager m;
        public AddTrainingWindow()
        {
            InitializeComponent();
            m = new TrainingManager(new UnitOfWork(new TrainingContext("Production")));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TrainingType trainingType = TrainingType.Endurance;
            switch (cmbTypeTraining.SelectedItem.ToString())
            {
                case "Interval":
                    trainingType = TrainingType.Interval;
                    break;
                case "Endurance":
                    trainingType = TrainingType.Endurance;
                    break;
                case "Recuperation":
                    trainingType = TrainingType.Recuperation;
                    break;
            }

            BikeType bikeType = BikeType.CityBike;
            switch (cmbTypeTraining.SelectedItem.ToString())
            {
                case "CityBike":
                    bikeType = BikeType.CityBike;
                    break;
                case "IndoorBike":
                    bikeType = BikeType.IndoorBike;
                    break;
                case "MountainBike":
                    bikeType = BikeType.MountainBike;
                    break;
                case "RacingBike":
                    bikeType = BikeType.RacingBike;
                    break;
            }


            if (RunningRadioButton.IsChecked==true)
            {
                m.AddRunningTraining(new DateTime(2020, 4, 23, 17, 48, 00), int.Parse(txtDistance.Text), new TimeSpan(0, 31, 17), float.Parse(txtAverageSpeed.Text), trainingType, txtComments.Text);
            }
            else
            {
                m.AddCyclingTraining(new DateTime(2020, 4, 23, 17, 48, 00), int.Parse(txtDistance.Text), new TimeSpan(0, 31, 17), float.Parse(txtAverageSpeed.Text), int.Parse(txtWattage.Text), trainingType, txtComments.Text, bikeType);
            }
        }
    }
}
