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
            string guess1 = ComboBox1.SelectedItem?.ToString();
            string guess2 = ComboBox2.SelectedItem?.ToString();
            string guess3 = ComboBox3.SelectedItem?.ToString();
            string guess4 = ComboBox4.SelectedItem?.ToString();

         

            CheckGuesses(guess1, guess2, guess3, guess4);
        }

        private void CheckGuesses(string guess1, string guess2, string guess3, string guess4)
        {
            List<string> guesses = new List<string> { guess1, guess2, guess3, guess4 };

            ClearBorders();

            for (int i = 0; i < guesses.Count; i++)
            {
                if (guesses[i] == Random[i])
                {
                    GetLabelForIndex(i).BorderBrush = Brushes.DarkRed; 
                    GetLabelForIndex(i).BorderThickness = new Thickness(2); 
                }
                else if (Random.Contains(guesses[i]))
                {
                    GetLabelForIndex(i).BorderBrush = Brushes.Wheat; 
                    GetLabelForIndex(i).BorderThickness = new Thickness(2); 
                }
            }

        }

        private void ClearBorders()
        {
        
            Label1.BorderBrush = Brushes.Transparent;
            Label2.BorderBrush = Brushes.Transparent;
            Label3.BorderBrush = Brushes.Transparent;
            Label4.BorderBrush = Brushes.Transparent;
        }

        private Label GetLabelForIndex(int index)
        {
            switch (index)
            {
                case 0: return Label1;
                case 1: return Label2;
                case 2: return Label3;
                case 3: return Label4;
                default: return null;
            }
        }
    }
}
