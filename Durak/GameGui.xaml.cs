using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CardLib;
using GetRandom;


namespace Durak
{
    
    public partial class GameGui : Window
    {
        public static int numPlayers = 0;
        protected PlayerUIs UIs = new PlayerUIs();
        private int m_nGameFlags = 0;
        private Game m_Game = null;
        public PlayArea m_BattleArea = null;
        PlayerUI m_PersonPlayerUI = null;

        /// <param name="numPlayers"></param>
        public GameGui()
        {
            InitializeComponent();
            RangedRandom.PrimeRandomNumberGenerator();
            WindowState = WindowState.Maximized;
            Initialize();
            Round temp = new Round();
        }

        private void Initialize()
        {
            m_Game = null;
            GameMenu gm = new GameMenu();
            gm.ShowDialog();
            m_nGameFlags = gm.Result;
            RangedRandom.PrimeRandomNumberGenerator();
            m_Game = new Game(numPlayers, m_nGameFlags, this);
        }

        public void SetupGui()
        { 
            m_BattleArea = new PlayArea();
            Grid.SetRow(m_BattleArea, 1);
            Grid.SetColumn(m_BattleArea, 1);
            Grid.SetRowSpan(m_BattleArea, 5);
            Grid.SetColumnSpan(m_BattleArea, 8);
            mainGrid.Children.Add(m_BattleArea);
            ShowTrump();
            ShowDeck(true);
        }

        /// <param name="playerNum"></param>
        /// <param name="row"></param>
        /// <param name="rowSpan"></param>
        /// <param name="col"></param>
        /// <param name="colSpan"></param>
        /// <param name="cardOrientation"></param>
        /// <param name="handOrientation"></param>
        /// <param name="faceUp"></param>
        private void BuildComputerPlayer(int playerNum, int row, int rowSpan, int col,
            int colSpan, Orientation cardOrientation, Orientation handOrientation = Orientation.Horizontal,
            bool faceUp = false)
        {
            PlayerUI compPlayer = new PlayerUI(playerNum, handOrientation);
            UIs.Add(compPlayer);
            Grid.SetRow(compPlayer, row);
            Grid.SetColumn(compPlayer, col);
            Grid.SetRowSpan(compPlayer, rowSpan);
            Grid.SetColumnSpan(compPlayer, colSpan);
            mainGrid.Children.Add(compPlayer);
            
        }

        /// <param name="players"></param>
        public void UpdateDisplay(Players players)
        {
            if (UIs.Count == 0)
            {
                m_PersonPlayerUI = new PlayerUI(0);
                Grid.SetRow(m_PersonPlayerUI, 6);
                Grid.SetColumn(m_PersonPlayerUI, 4);
                Grid.SetRowSpan(m_PersonPlayerUI, 2);
                Grid.SetColumnSpan(m_PersonPlayerUI, 3);
                mainGrid.Children.Add(m_PersonPlayerUI);
                UIs.Add(m_PersonPlayerUI);
                if (players.Count == 2)
                {
                    BuildComputerPlayer(1, 0, 2, 4, 3, Orientation.Vertical);
                }
                else if (players.Count == 3)
                {
                    BuildComputerPlayer(1, 0, 2, 2, 3, Orientation.Vertical);
                    BuildComputerPlayer(2, 0, 2, 6, 3, Orientation.Vertical);
                }
                else if (players.Count == 4)
                {
                    BuildComputerPlayer(1, 2, 5, 0, 2, Orientation.Horizontal, Orientation.Vertical);
                    BuildComputerPlayer(2, 0, 2, 2, 3, Orientation.Vertical);
                    BuildComputerPlayer(3, 0, 2, 6, 3, Orientation.Vertical);
                }
                else if (players.Count == 5)
                {
                    BuildComputerPlayer(1, 4, 4, 0, 2, Orientation.Horizontal, Orientation.Vertical);
                    BuildComputerPlayer(2, 0, 4, 0, 2, Orientation.Horizontal, Orientation.Vertical);
                    BuildComputerPlayer(3, 0, 2, 2, 3, Orientation.Vertical);
                    BuildComputerPlayer(4, 0, 2, 6, 3, Orientation.Vertical);

                }
                else if (players.Count == 6)
                {
                    BuildComputerPlayer(1, 5, 3, 0, 2, Orientation.Horizontal, Orientation.Vertical);
                    BuildComputerPlayer(2, 1, 3, 0, 2, Orientation.Horizontal, Orientation.Vertical);
                    BuildComputerPlayer(3, 0, 2, 2, 3, Orientation.Vertical);
                    BuildComputerPlayer(4, 0, 2, 6, 3, Orientation.Vertical);
                    BuildComputerPlayer(5, 2, 3, 8, 2, Orientation.Horizontal, Orientation.Vertical);
                }
            }
        }

        /// <param name="players"></param>
        public void UpdateElements(Players players)
        { 
            for (int i = 0; i < players.Count; i++)
            {
                UIs.ElementAt(i).Update(i, players[i].m_Hand);
            }
            UpdateEventHandlers();
        }

        /// <param name="UI"></param>
        /// <param name="row"></param>
        /// <param name="rowSpan"></param>
        /// <param name="col"></param>
        /// <param name="colSpan"></param>
        private void SendUIToScreen(PlayerUI UI, int row, int rowSpan, int col, int colSpan)
        {
            Grid.SetRow(UI, row);
            Grid.SetColumn(UI, col);
            Grid.SetRowSpan(UI, rowSpan);
            Grid.SetColumnSpan(UI, colSpan);
            mainGrid.Children.Add(UI);
        }

        public void UpdateEventHandlers()
        {
            foreach (PlayerUI gui in UIs)
            {
                foreach (CardBox box in gui.spPlayerHand.Children)
                {
                    box.CardBoxClick += OnCustomButtonClick;
                }
            }
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCustomButtonClick(object sender, EventArgs e)
        {
            m_Game.CardBoxClicked(sender);
        }

        private void ShowTrump()
        {
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(@"/Durak;component/images/suit-" + Util.SuitToStr(PlayingCard.trump).ToLower() + ".png", UriKind.Relative);
            bitimg.EndInit();
            imgTrumpDisplay.Source = bitimg;
        }
        /// <param name="flipped"></param>
        private void ShowDeck(bool flipped)
        {
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            if (flipped)
                bitimg.UriSource = new Uri(@"/Durak;component/images/back-navy.png", UriKind.Relative);
            else
                bitimg.UriSource = new Uri(@"/Durak;component/images/back-blank.png", UriKind.Relative);
            bitimg.EndInit();
            imgDeck.Source = bitimg;

        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForfeit_Click(object sender, RoutedEventArgs e)
        {
            if (GameUtil.ShallIDoThisForYou("Do you wish to forfeit your turn?"))
            {
                m_Game.ForfeitTurn();
            }
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            m_Game.DoneClicked(true);
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dtDestroy_Tick(object sender, EventArgs e)
        {
            m_BattleArea.Destroy();
            ((DispatcherTimer)sender).Stop();
            Players players = Game.m_Players;
            if (!Game.m_bDeckOutFlag)
            {
                players.DealHands(players.Count, m_Game.m_HandSize, ref m_Game.m_Deck);
                if (Game.m_bDeckOutFlag) 
                {
                    MessageBox.Show("Deck has been emptied");
                    ShowDeck(false);
                }
            }
            UpdateElements(players);
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dtNext_Tick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            Players players = Game.m_Players;
            UpdateElements(players);
            m_Game.DoneClicked(false);
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dtBypass_Tick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            Players players = Game.m_Players;
            UpdateElements(players);
            m_Game.DoneClicked(false);
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Deck_LastCardDrawn(object sender, EventArgs e)
        {
            Game.m_bDeckOutFlag = true;
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Deck_CardDealt(object sender, EventArgs e)
        {
            gbxDeck.Header = "Deck: " + ((Deck)sender).DeckCount + " cards";
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Help_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@".\help\oop_project_group_1.chm");
        }
    }
}
