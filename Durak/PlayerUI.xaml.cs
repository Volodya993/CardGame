using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CardLib;
namespace Durak
{ 
    public partial class PlayerUI : UserControl
    {
        private const int POP = 25;
        private Orientation myOrientation;
        public PlayerUI(int playerNum, Orientation orientation = Orientation.Horizontal)
        {
            InitializeComponent();
            spPlayerHand.Orientation = orientation;
            myOrientation = orientation;
            if (orientation == Orientation.Vertical)
            {
                mainGrid.Height = 700;
                mainGrid.Width = 100;
            }
            lblPlayerNum.Content = "Player " + playerNum.ToString();
            
            BuildBoard();
        }
        public void BuildBoard()
        {

            spPlayerHand.Height = mainGrid.Height;
            RealignCards(spPlayerHand);
            //if (spPlayerHand.Children.Count > 12)
            //{
            //    newMargin -= ((Convert.ToDouble(spPlayerHand.Children.Count) / Convert.ToDouble(100)) * Convert.ToDouble(10)) * 2.5;
            //}
            //foreach (CardBox box in spPlayerHand.Children.OfType<CardBox>())
            //{
            //    box.Margin = new Thickness(newMargin);
            //}
        }

        public void AddCard(CardBox card)
        {
            spPlayerHand.Children.Add(card);
            RealignCards(spPlayerHand);
        }

        public void DeleteCard(CardBox card)
        {
            spPlayerHand.Children.Remove(card);
            RealignCards(spPlayerHand);
        }

        public int GetNumCards()
        {
            return (spPlayerHand.Children.Count);
        }
        /// <param name="hand"></param>
        public void Update(int playerNum, PlayingCards hand)
        {
            spPlayerHand.Children.Clear();
            foreach (PlayingCard card in hand)
            {
                spPlayerHand.Children.Add(new CardBox(card, (Orientation)((0 == (int)myOrientation) ? 1 : 0)));
                RealignCards(spPlayerHand);
            }
            lblPlayerNum.Content = "Player " + playerNum.ToString();
        }

        private void RealignCards(StackPanel panelHand)
        {
            int myCount = panelHand.Children.OfType<CardBox>().Count();
            if (myCount > 0)
            {
                int cardWidth = (int)panelHand.Children.OfType<CardBox>().ElementAt(0).RenderSize.Width;
                int startPoint = ((int)panelHand.RenderSize.Width - cardWidth) / 2;
                int offset = 0;
                if (myCount > 1)
                {
                    offset = ((int)panelHand.RenderSize.Width - cardWidth / 2 * 17) / (myCount);
                    if (offset > cardWidth)
                        offset = cardWidth;
                    int allCardsWidth = (myCount) * offset + cardWidth;
                    startPoint = ((int)panelHand.RenderSize.Width - allCardsWidth) / 2;
                }
                panelHand.Children.OfType<CardBox>().ElementAt(myCount - 1).Margin = new Thickness(POP,0,0,0);
                System.Diagnostics.Debug.Write(panelHand.Children[myCount - 1].ToString() + "\n");
                for (int index = myCount - 1; index >= 0; index--)
                {
                    panelHand.Children.OfType<CardBox>().ElementAt(index).Margin = new Thickness(offset,0,0,0);
                    panelHand.Children.OfType<CardBox>().ElementAt(index).VerticalAlignment = VerticalAlignment.Bottom;
                    //panelHand.Children.OfType<CardBox>().ElementAt(index).Left = panelHand.Children[index + 1].Left + offset;
                }
            }
        }
    }
}
