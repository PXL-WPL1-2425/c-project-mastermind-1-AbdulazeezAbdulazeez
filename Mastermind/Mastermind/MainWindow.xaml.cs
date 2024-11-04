using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Mastermind
{
    public partial class MainWindow : Window
    {
        List<string> kleuren = new List<string> { "Red", "Yellow", "Orange", "White", "Green", "Blue" };
        List<string> gegenereerdeCode = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            RandomKleur();  
            ComboBoxes(); 
        }

        private void RandomKleur()
        {
            var random = new Random();
            gegenereerdeCode.Clear();

            for (int i = 0; i < 4; i++)
            {
                gegenereerdeCode.Add(kleuren[random.Next(kleuren.Count)]);
            }

                Title = $"MasterMind ({string.Join(", ", gegenereerdeCode)})";
        }

        private void ComboBoxes()
        {
            ComboBox1.ItemsSource = kleuren;
            ComboBox2.ItemsSource = kleuren;
            ComboBox3.ItemsSource = kleuren;
            ComboBox4.ItemsSource = kleuren;
        }
    }
}
