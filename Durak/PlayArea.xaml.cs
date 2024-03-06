using System.Windows;
using System.Windows.Controls;

namespace Durak
{
    public partial class PlayArea : UserControl
    {

        public PlayArea()
        {
            InitializeComponent();
            BuildBoard();

        }
        private void BuildBoard()
        {
            spPlayArea.Height = mainGrid.Height;
            spAttack.Height = mainGrid.Height / 2;
            spDefend.Height = mainGrid.Height / 2;
            foreach (CardBox box in spAttack.Children)
            {
                int newMargin = ((spAttack.Children.Count / 2) - 12) * -1;
                box.Margin = new Thickness(newMargin);
                spAttack.Width = (box.Width + newMargin) * spAttack.Children.Count;
            }
            foreach (CardBox box in spDefend.Children)
            {
                int newMargin = ((spDefend.Children.Count / 2) - 12) * -1;
                box.Margin = new Thickness(newMargin);
                spDefend.Width = (box.Width + newMargin) * spDefend.Children.Count;
            }

            if (spAttack.Width > spDefend.Width)
            {
                mainGrid.Width = spAttack.Width;
                spPlayArea.Width = mainGrid.Width;
            }
            else
            {
                mainGrid.Width = spDefend.Width;
                spPlayArea.Width = mainGrid.Width;
            }
        }

        public void AddAttackCard(CardBox attackCard)
        {
            attackCard.Margin = new Thickness(3);
            spAttack.Children.Add(attackCard);
            BuildBoard();
        }

        public void AddDefenseCard(CardBox defenseCard)
        {
            defenseCard.Margin = new Thickness(3);
            spDefend.Children.Add(defenseCard);
            BuildBoard();
        }
        public void Destroy()
        {
            spDefend.Children.Clear();
            spAttack.Children.Clear();
        }
    }
}
