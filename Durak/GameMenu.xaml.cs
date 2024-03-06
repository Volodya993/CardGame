using System;
using System.Windows;
using System.Windows.Controls;
using CardLib;
using GetRandom;

namespace Durak
{

    public partial class GameMenu : Window
    {
        private int trumpsuit = -1;
        private int numPlayers;
        private int deckSize;
        private bool gameStarted = false;
        public int Result = 0;
        public bool GameStarted
        {
            get
            {
                return gameStarted;
            }
        }
        public int TrumpSuit
        {
            set
            {
                trumpsuit = value;
            }
            get
            {
                return trumpsuit;
            }
        }
        public int NumPlayers
        {
            set
            {
                numPlayers = value;
            }
            get
            {
                return numPlayers;
            }
        }
        public int DeckSize
        {
            set
            {
                deckSize = value;
            }
            get
            {
                return deckSize;
            }

        }
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }
        private void ResetForm()
        {
            rbnRandom.IsChecked = true;
            rbnPlayers2.IsChecked = true;
            rbnSize36.IsChecked = true;
        }
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            foreach (RadioButton rbTemp in spTrumps.Children)
            {
                if (rbTemp != null && rbTemp.IsChecked == true)
                {
                    TrumpSuit = Int32.Parse(rbTemp.Tag.ToString());
                }
            }
            foreach (RadioButton rbTemp in spDeckSize.Children)
            {
                if (rbTemp != null && rbTemp.IsChecked == true)
                {
                    DeckSize = (Int32.Parse(rbTemp.Content.ToString()));
                }
            }
            foreach (RadioButton rbTemp in spNumPlayers.Children)
            {
                if (rbTemp != null && rbTemp.IsChecked == true)
                {
                    NumPlayers = (Int32.Parse(rbTemp.Content.ToString()));
                }
            }
            if (TrumpSuit == 4)
            {
                TrumpSuit = (int)RangedRandom.GenerateUnsignedNumber(4, 0);
            }
            GameGui.numPlayers = NumPlayers;
            Result = --DeckSize + (int)DeckFlags.AceHigh + (int)DeckFlags.UseTrump + (TrumpSuit << 9);
            this.Close();
        }
        public void ShowMenu()
        {
            this.Show();
        }
        public GameMenu()
        {
            InitializeComponent();
            ResetForm();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnPlayers6_Checked(object sender, RoutedEventArgs e)
        {
            rbnSize20.IsEnabled = false;
            rbnSize20.IsChecked = false;
            rbnSize36.IsChecked = true;
        }
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnPlayers5_Checked(object sender, RoutedEventArgs e)
        {
            rbnSize20.IsEnabled = false;
            rbnSize20.IsChecked = false;
            rbnSize36.IsChecked = true;
        }
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnPlayers4_Checked(object sender, RoutedEventArgs e)
        {
            rbnSize20.IsEnabled = false;
            rbnSize20.IsChecked = false;
            rbnSize36.IsChecked = true;
        }
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnPlayers3_Checked(object sender, RoutedEventArgs e)
        {
            rbnSize20.IsEnabled = true;
        }
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnPlayers2_Checked(object sender, RoutedEventArgs e)
        {
            rbnSize20.IsEnabled = true;
        }
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@".\help\oop_project_group_1.chm");
        }
    }
}
