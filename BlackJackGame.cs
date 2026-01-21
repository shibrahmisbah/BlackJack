using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BlackJack
{
    internal class BlackJackGame
    {

        Deck deck = new Deck();
        List<Card> playerCards = new List<Card>();
        List<Card> dealerCards = new List<Card>();

        public void Start()
        {
            DealCardToPlayer();
            DealCardToDealer();
            DealCardToPlayer();
            DealFacedownCardToDealer();
        }

        public void Hit()
        {
            DealCardToPlayer();

        }

        public void Stand()
        {

        }
        public Card DealCardToPlayer()
        {
            Card c = deck.GetTopCard();
            playerCards.Add(c);
            ((MainWindow)System.Windows.Application.Current.MainWindow).ShowImage(c.GetName() + ".png");
            ((MainWindow)System.Windows.Application.Current.MainWindow).ScorePanel.Children.Clear();
            ((MainWindow)System.Windows.Application.Current.MainWindow).ScoreText((GetPlayerSum()).ToString());

            if (GetPlayerSum() < 21)
            {
                return c;
            }
            else if (GetPlayerSum() > 21)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).btnHit.IsEnabled = false;
                ((MainWindow)System.Windows.Application.Current.MainWindow).btnStand.IsEnabled = false;
                ScoreCheck();
                return c;
            }
            else
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).btnHit.IsEnabled = false;
                ((MainWindow)System.Windows.Application.Current.MainWindow).btnStand.IsEnabled = false;
                ScoreCheck();
            }

            return c;
        }

        public void DealCardToDealer()
        {
            Card c = deck.GetTopCard();
            dealerCards.Add(c);
            ((MainWindow)System.Windows.Application.Current.MainWindow).ShowImageDealer(c.GetName() + ".png");
        }

        public void DealFacedownCardToDealer()
        {
            Card c = deck.GetTopCard();
            dealerCards.Add(c);
            ((MainWindow)System.Windows.Application.Current.MainWindow).ShowImageDealer("CardBack.png");
        }

        internal int GetDealerSum()
        {
            int sum = 0;
            foreach (Card c in dealerCards)
            {
                sum += GetBlackjackValue(c);
            }
            return sum;
        }

        public List<Card> GetDealerCards()
        {
            return dealerCards;
        }

        internal int GetPlayerSum()
        {
            int sum = 0;
            foreach (Card c in playerCards)
            {
                sum += GetBlackjackValue(c);
            }
            return sum;
        }

        public int GetBlackjackValue(Card c)
        {
            // Simplification assumption: We will always count
            // the Aces as 1
            if (c.Rank > 10) return 10;
            return c.Rank;
        }

        public List<Card> GetPlayerCards()
        {
            return playerCards;
        }

        public void DeckRedo()
        {
            foreach (Card c in playerCards.ToList())
            {
                playerCards.Remove(c);
                deck.Insert(c);
            }
            foreach (Card c in dealerCards.ToList())
            {
                dealerCards.Remove(c);
                deck.Insert(c);
            }
            ((MainWindow)System.Windows.Application.Current.MainWindow).btnHit.IsEnabled = true;
            ((MainWindow)System.Windows.Application.Current.MainWindow).btnStand.IsEnabled = true;
            deck.Shuffle();
            DealCardToPlayer();
            DealCardToDealer();
            DealCardToPlayer();
            DealFacedownCardToDealer();
        }

        public String ScoreCheck()
        {
            while (GetDealerSum() < 17)
            {
                DealFacedownCardToDealer();
            }

            string winner = " ";

            ((MainWindow)System.Windows.Application.Current.MainWindow).DealerCardPanel.Children.Clear();
            
            foreach (Card c in dealerCards)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ShowImageDealer(c.GetName() + ".png");
            }

            if (GetDealerSum() > 21 && GetPlayerSum() < 21)
            {
                winner = "YOU";
            }
            
            else if(GetPlayerSum() > 21 && GetDealerSum() < 21)
            {
                winner = "DEALER";
            }
            else if(GetDealerSum() > 21 && GetPlayerSum() > 21)
            {
                winner = "DEALER";
            }
            else if (GetDealerSum() == 21)
            {
                winner = "DEALER";
                ((MainWindow)System.Windows.Application.Current.MainWindow).btnHit.IsEnabled = false;
                ((MainWindow)System.Windows.Application.Current.MainWindow).btnStand.IsEnabled = false;
            }
            else if (GetPlayerSum() == 21)
            {
                winner = "BLACKJACK";

            }
            else
            {

                if (GetDealerSum() > GetPlayerSum())
                {
                    winner = "DEALER";
                }
                else if(GetPlayerSum() > GetDealerSum())
                {
                    winner = "YOU";
                }
                else
                {
                    winner = "DRAW";
                }

            }

            if(winner == "YOU" || winner == "BLACKJACK")
            {
                System.IO.Stream str = Properties.Resources.win;
                System.Media.SoundPlayer playerwin = new System.Media.SoundPlayer(str);
                playerwin.Play();
            }
            else if(winner == "DEALER")
            {
                System.IO.Stream str = Properties.Resources.lose;
                System.Media.SoundPlayer playerlose = new System.Media.SoundPlayer(str);
                playerlose.Play();
            }

            ((MainWindow)System.Windows.Application.Current.MainWindow).DealerScorePanel.Children.Clear();
            ((MainWindow)System.Windows.Application.Current.MainWindow).DealerScoreText((GetDealerSum()).ToString());
            ((MainWindow)System.Windows.Application.Current.MainWindow).WinnerText(winner);
            return winner;

        }

    }
}
