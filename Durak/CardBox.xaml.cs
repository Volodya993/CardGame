using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CardLib;


namespace Durak
{
    public partial class CardBox : UserControl
    {

        public CardBox()
        {
            InitializeComponent();
            myOrientation = Orientation.Vertical;
            myCard = null;
            imgCardDisplay.Source = GetCardImage();
        }

        /// <param name="card"> Card Object </param>
        /// <param name="orientation"></param>
        public CardBox(PlayingCard card, Orientation orientation = Orientation.Vertical, int iTurnId = 0)
        {
            InitializeComponent();
            myOrientation = orientation;
            myCard = card;
            imgCardDisplay.Source = GetCardImage();
            this.Tag = iTurnId;
        }

        private PlayingCard myCard;
        public PlayingCard Card
        {
            set
            {

                myCard = value;
                imgCardDisplay.Source = GetCardImage();
            }
            get
            {
                return myCard;
            }
        }

        public bool FaceUp
        {
            set
            {
                if (myCard.Faceup != value)
                {
                    myCard.Faceup = value;
                    UpdateCardImage();

                    if (CardFlipped != null)
                    {
                        CardFlipped(this, new EventArgs());
                    }
                }
            }
            get
            {
                return Card.Faceup;
            }
        }

        private Orientation myOrientation;

        public Orientation CardOrientation
        {
            set
            {
                if (myOrientation != value)
                {
                    myOrientation = value;
                    this.RenderSize = new Size(RenderSize.Height, RenderSize.Width);
                    UpdateCardImage();
                }
            }
            get
            {
                return myOrientation;
            }
        }

        public void UpdateCardImage()
        {
            imgCardDisplay.Source = GetCardImage();
        }

        public override string ToString()
        {
            String output = "";
            if (FaceUp)
            {
                output = myCard.ToString();
            }
            else
            {
                output = "A face down card";
            }
            return output;
        }
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardBox_Load(object sender, EventArgs e)
        {
            UpdateCardImage();
        }

        public void biggerImage()
        {
            mainGrid.Height = mainGrid.Height + 17;
            mainGrid.Width = mainGrid.Width + 17;
            imgCardDisplay.Height = imgCardDisplay.Height + 17;
            imgCardDisplay.Width = imgCardDisplay.Width + 17;

        }

        public void smallerImage()
        {
            mainGrid.Height = mainGrid.Height - 17;
            mainGrid.Width = mainGrid.Width - 17;
            imgCardDisplay.Height = imgCardDisplay.Height - 17;
            imgCardDisplay.Width = imgCardDisplay.Width - 17;

        }
        public event EventHandler CardFlipped;

        public event EventHandler CardBoxClick;

        void CardBox_MouseEnter(object sender, EventArgs e)
        {
            this.biggerImage();
        }

        void CardBox_MouseLeave(object sender, EventArgs e)
        {
            this.smallerImage();
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardBox_MouseLeftButtonDown(object sender, EventArgs e)
        {
            // if Eventhandler is set
            if (CardBoxClick != null)
            {
                CardBoxClick(this, e);
            }
        }
        public BitmapImage GetCardImage()
        {
            string imageName;
            imageName = myCard.rank.ToString().ToLower() + "_" + myCard.suit.ToString().ToLower() + ".png";
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(@"/Durak;component/images/" + imageName, UriKind.Relative);
            if (CardOrientation == Orientation.Horizontal)
            {
                mainGrid.Height = 56;
                mainGrid.Width = 82;
                bitimg.Rotation = Rotation.Rotate90;
            }
            bitimg.EndInit();
            return bitimg;
        }
    }
}