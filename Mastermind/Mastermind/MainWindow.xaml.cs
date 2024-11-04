using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mastermind
{
    public partial class MainWindow : Window
    {
        List<string> kleuren = new List<string> { "Red", "Yellow", "Orange", "White", "Green", "Blue" };
        List<string> Random = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            RandomKleur();
            ComboBoxes();
        }

        private void RandomKleur()
        {
            var random = new Random();
            Random.Clear();

            for (int i = 0; i < 4; i++)
            {
                Random.Add(kleuren[random.Next(kleuren.Count)]);
            }

            Title = $"MasterMind ({string.Join(", ", Random)})";
        }

        private void ComboBoxes()
        {
            ComboBox1.ItemsSource = kleuren;
            ComboBox2.ItemsSource = kleuren;
            ComboBox3.ItemsSource = kleuren;
            ComboBox4.ItemsSource = kleuren;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.SelectedItem != null)
            {
                string selectedColor = comboBox.SelectedItem.ToString();
                Brush brushColor = (Brush)new BrushConverter().ConvertFromString(selectedColor);

                switch (comboBox.Name)
                {
                    case "ComboBox1":
                        Label1.Background = brushColor;
                        break;
                    case "ComboBox2":
                        Label2.Background = brushColor;
                        break;
                    case "ComboBox3":
                        Label3.Background = brushColor;
                        break;
                    case "ComboBox4":
                        Label4.Background = brushColor;
                        break;
                }
            }
        }

        private void CheckCodeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}