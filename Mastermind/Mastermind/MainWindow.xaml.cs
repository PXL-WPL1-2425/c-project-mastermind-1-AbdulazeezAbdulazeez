using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Mastermind
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private List<string> kleuren = new List<string> { "Red", "Yellow", "Orange", "White", "Green", "Blue" };
        private List<string> Random = new List<string>();
        private int attempts = 1;
        private const int maxAttempts = 10; // Maximale aantal pogingen
        private int countdownSeconds = 0; // Houdt de huidige timer-seconden bij
        private const int maxTime = 10; // Maximale tijd (10 seconden)

        public MainWindow()
        {
            InitializeComponent();
            RandomKleur();
            ComboBoxes();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            StartCountdown(); // Timer starten bij het genereren van de eerste code

            this.Closing += MainWindow_Closing; // Voeg het Closing event toe
        }

        private void RandomKleur()
        {
            var random = new Random();
            Random.Clear();

            for (int i = 0; i < 4; i++)
            {
                Random.Add(kleuren[random.Next(kleuren.Count)]); // Genereer willekeurige kleuren
            }

            // Zorg ervoor dat de geheime code in de titel staat aan het begin van het spel
            UpdateTitleWithCode(); // De geheime code wordt getoond in de titel

            StartCountdown(); // Timer resetten bij het genereren van een nieuwe code
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            countdownSeconds++;
            timerLabel.Content = $"Tijd: {countdownSeconds}s";

            if (countdownSeconds >= maxTime)
            {
                StopCountdown(); // Timer stoppen als tijd is verstreken
                LoseTurn(); // Beurt verliezen
            }
        }

        private void StartCountdown()
        {
            countdownSeconds = 0; // Reset de timer
            timer.Start();        // Start de timer
        }

        private void StopCountdown()
        {
            timer.Stop(); // Timer stoppen
        }

        private void LoseTurn()
        {
            MessageBox.Show("Te laat! Je verliest deze beurt.", "Te laat", MessageBoxButton.OK, MessageBoxImage.Warning);
            attempts++;

            if (attempts > maxAttempts)
            {
                EndGame(false); // Spel beëindigen als maximale pogingen is bereikt
            }
            else
            {
                UpdateTitle(); // Titel bijwerken na verlies beurt
                StartCountdown(); // Timer opnieuw starten voor de volgende beurt
            }
        }

        private void CheckCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string guess1 = ComboBox1.SelectedItem?.ToString();
            string guess2 = ComboBox2.SelectedItem?.ToString();
            string guess3 = ComboBox3.SelectedItem?.ToString();
            string guess4 = ComboBox4.SelectedItem?.ToString();

            int score = CheckGuesses(guess1, guess2, guess3, guess4); // Score wordt nu correct berekend en teruggegeven

            // Score weergeven in de UI
            ScoreLabel.Content = $"Score: {score}";

            StopCountdown(); // Timer stoppen na poging

            if (score == 0 && guess1 == Random[0] && guess2 == Random[1] && guess3 == Random[2] && guess4 == Random[3])
            {
                // Als de code gekraakt is (score 0)
                EndGame(true); // Winstbeëindiging
            }
            else
            {
                attempts++;
                if (attempts > maxAttempts)
                {
                    EndGame(false); // Spel beëindigen als maximum pogingen is bereikt
                }
                else
                {
                    StartCountdown(); // Timer opnieuw starten
                    UpdateTitle(); // Titel bijwerken na de poging
                }
            }
        }

        private int CheckGuesses(string guess1, string guess2, string guess3, string guess4)
        {
            List<string> guesses = new List<string> { guess1, guess2, guess3, guess4 };

            // Voeg feedback toe in de vorm van een StackPanel van Border objecten
            StackPanel feedbackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal, // Horizontaal naast elkaar
                Margin = new Thickness(5)
            };

            int correctPositions = 0; // Aantal kleuren op de juiste positie
            int correctColors = 0;    // Aantal kleuren die correct zijn, maar niet op de juiste positie
            int wrongColors = 0;      // Aantal kleuren die niet in de geheime code zitten

            // Maak een kopie van de geheime code om deze te vergelijken
            List<string> secretCode = new List<string>(Random);

            // Eerste stap: check de kleuren op de juiste positie
            for (int i = 0; i < guesses.Count; i++)
            {
                if (guesses[i] == secretCode[i])
                {
                    correctPositions++;
                    secretCode[i] = null; // Verwijder de correcte kleur uit de geheime code
                }
            }

            // Tweede stap: check of de kleur ergens anders in de geheime code voorkomt (maar op de verkeerde positie)
            for (int i = 0; i < guesses.Count; i++)
            {
                if (guesses[i] != null && secretCode.Contains(guesses[i]))
                {
                    correctColors++;
                    secretCode[secretCode.IndexOf(guesses[i])] = null; // Verwijder de kleur uit de geheime code
                }
            }

            // Aantal kleuren die niet in de geheime code voorkomen
            wrongColors = guesses.Count - (correctPositions + correctColors);

            // Bereken de score
            int score = (wrongColors * 2) + (correctColors * 1); // 2 strafpunten voor foute kleuren, 1 voor juiste kleur op verkeerde plaats

            // Voeg de feedback toe aan de StackPanel
            for (int i = 0; i < guesses.Count; i++)
            {
                Border feedbackBorder = new Border
                {
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(5),
                    BorderBrush = Brushes.Red, // Foute feedbackkleur
                    BorderThickness = new Thickness(1)
                };

                // Verwerk de feedback (rood/wit)
                if (guesses[i] == Random[i])
                {
                    feedbackBorder.Background = (Brush)new BrushConverter().ConvertFromString(guesses[i]);
                }
                else
                {
                    feedbackBorder.Background = Brushes.White;
                }

                feedbackPanel.Children.Add(feedbackBorder);
            }

            PreviousGuessesPanel.Children.Add(feedbackPanel); // Voeg de feedback toe aan het StackPanel

            return score; // Geef de berekende score terug
        }

        private void EndGame(bool isWin)
        {
            string message = isWin ? "Gefeliciteerd! Je hebt de code gekraakt!" : $"Helaas, je hebt verloren. De code was: {string.Join(", ", Random)}";
            MessageBoxResult result = MessageBox.Show(message, "Einde Spel", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                // Nieuw spel starten
                ResetGame();
            }
            else
            {
                // Applicatie sluiten
                Application.Current.Shutdown();
            }
        }

        private void ResetGame()
        {
            // Reset alle waarden en begin opnieuw
            RandomKleur();
            attempts = 1;
            PreviousGuessesPanel.Children.Clear(); // Verwijder alle vorige pogingen
            UpdateTitle(); // Titel bijwerken
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Vraag de speler of ze willen afsluiten
            MessageBoxResult result = MessageBox.Show("Weet je zeker dat je het spel wilt afsluiten?", "Bevestig afsluiten", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true; // Annuleer het sluiten van het venster
            }
        }

        private void UpdateTitle()
        {
            // Titel bijwerken met huidige poging
            this.Title = $"Poging {attempts}/{maxAttempts} | Tijd: {countdownSeconds}s";
        }

        private void UpdateTitleWithCode()
        {
            // Zet de geheime code aan het begin van het spel in de titel
            this.Title = $"Geheime Code: {string.Join(", ", Random)} | Poging {attempts}/{maxAttempts} | Tijd: {countdownSeconds}s";
        }
    }
}
