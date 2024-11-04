using System;
using System.Collections.Generic;
using System.Windows;

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
    }
}
