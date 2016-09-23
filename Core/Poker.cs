using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    public static class HandExtensions
    {
        public static Card HighCard(this List<Card> hand)
        {
            return hand.OrderBy(card => card.Value).ToList().Last();
        }
    }

    public enum Suits
    {
        H, C, S, D
    }

    public enum Cards
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }

    public enum BestHand
    {
        HighCard = 1,
        OnePair = 2,
        TwoPair = 3,
        ThreeOfAKind = 4,
        Straight = 5,
        Flush = 6,
        FullHouse = 7,
        FourOfAKind = 8,
        StrightFlush = 9,
        RoyalFlush = 10,
    }

    public class Card
    {
        public Suits Suit { get; set; }
        public Cards Value { get; set; }

        public Card(string card)
        {
            this.Suit = GetSuit(card[1]);
            this.Value = GetValue(card[0]);
        }

        public Cards GetValue(char val)
        {
            switch (val)
            {
                case 'A':
                    return Cards.Ace;
                case 'K':
                    return Cards.King;
                case 'Q':
                    return Cards.Queen;
                case 'J':
                    return Cards.Jack;
                case 'T':
                    return Cards.Ten;

                default:
                    return (Cards)int.Parse(val.ToString());
            }
        }

        public Suits GetSuit(char val)
        {
            return (Suits)Enum.Parse(typeof(Suits), val.ToString());
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", this.Value, this.Suit);
        }
    }

    public class PokerHand
    {
        private List<Card> hand = new List<Card>();

        public List<Card> IsRoyalFlush
        {
            get
            {
                if (IsFlush != null && hand.Any(card => card.Value == Cards.Ten)
                                    && hand.Any(card => card.Value == Cards.Jack)
                                    && hand.Any(card => card.Value == Cards.Queen)
                                    && hand.Any(card => card.Value == Cards.King)
                                    && hand.Any(card => card.Value == Cards.Ace))
                {
                    return hand.OrderBy(card => card.Value).ToList();
                }

                return null;
            }
        }

        public List<Card> IsFlush
        {
            get
            {
                if (hand.All(card => hand.First().Suit == card.Suit))
                {
                    return hand.OrderBy(card => card.Value).ToList();
                }

                return null;
            }
        }

        public List<Card> IsOnePair
        {
            get
            {
                var pair = hand.GroupBy(card => card.Value).Where(group => group.Count() == 2);

                if (pair.Count() == 1)
                {
                    return pair.First().ToList();
                }

                return null;
            }
        }

        public List<Card> IsTwoPair
        {
            get
            {
                var pair = hand.GroupBy(card => card.Value).Where(group => group.Count() == 2);

                if (pair.Count() == 2)
                {
                    return pair.Where(p => p.Count() == 2).SelectMany(p => p).ToList();
                }

                return null;
            }
        }

        public List<Card> IsThreeOfAKind
        {
            get
            {
                var threeOfAKind = hand.GroupBy(card => card.Value).Where(group => group.Count() == 3);
                if (threeOfAKind.Any())
                {
                    return threeOfAKind.Single().ToList();
                }

                return null;
            }
        }

        public List<Card> IsFourOfAKind
        {
            get
            {
                var fourOfAKind = hand.GroupBy(card => card.Value).Where(group => group.Count() == 4);
                if (fourOfAKind.Any())
                {
                    return fourOfAKind.Single().ToList();
                }

                return null;
            }
        }

        public List<Card> IsStraight
        {
            get
            {
                var straight = hand.OrderBy(card => card.Value);

                bool isStraight = true;
                Card previousCard = null;

                foreach (var card in straight)
                {
                    if (previousCard != null && card.Value - 1 != previousCard.Value)
                    {
                        isStraight = false;
                    }

                    previousCard = card;
                }

                if (isStraight)
                {
                    return straight.ToList();
                }

                return null;
            }
        }

        public List<Card> IsStraightFlush
        {
            get
            {
                var isStraight = IsStraight;
                var isFlush = IsFlush;

                if (isStraight != null)
                {
                    if (isFlush != null)
                    {
                        return isStraight;
                    }
                }

                return null;
            }
        }

        public List<Card> IsFullHouse
        {
            get
            {
                var threeOfAKind = IsThreeOfAKind;
                var onePair = IsOnePair;

                if (threeOfAKind != null && onePair != null)
                {
                    var returnHand = new List<Card>();

                    returnHand.AddRange(onePair);
                    returnHand.AddRange(threeOfAKind);

                    return returnHand;
                }

                return null;
            }
        }

        public Card HighCard
        {
            get
            {
                return hand.OrderBy(card => card.Value).ToList().Last();
            }
        }

        public Tuple<BestHand, List<Card>> GetBestHand()
        {
            List<Card> bestHand;

            bestHand = IsRoyalFlush;
            if (bestHand != null)
            {
                return Tuple.Create(BestHand.RoyalFlush, bestHand);
            }

            bestHand = IsStraightFlush;
            if (bestHand != null)
            {
                return Tuple.Create(BestHand.StrightFlush, bestHand);
            }

            bestHand = IsFourOfAKind;
            if (bestHand != null)
            {
                return Tuple.Create(BestHand.FourOfAKind, bestHand);
            }

            bestHand = IsFullHouse;
            if (bestHand != null)
            {
                return Tuple.Create(BestHand.FullHouse, bestHand);
            }

            bestHand = IsFlush;
            if (bestHand != null)
            {
                return Tuple.Create(BestHand.Flush, bestHand);
            }

            bestHand = IsStraight;
            if (bestHand != null)
            {
                return Tuple.Create(BestHand.Straight, bestHand);
            }

            bestHand = IsThreeOfAKind;
            if (bestHand != null)
            {
                return Tuple.Create(BestHand.ThreeOfAKind, bestHand);
            }

            bestHand = IsTwoPair;
            if (bestHand != null)
            {
                return Tuple.Create(BestHand.TwoPair, bestHand);
            }

            bestHand = IsOnePair;
            if (bestHand != null)
            {
                return Tuple.Create(BestHand.OnePair, bestHand);
            }

            return Tuple.Create(BestHand.HighCard, this.hand);
        }

        public PokerHand(string[] cards)
        {
            foreach (var card in cards)
            {
                hand.Add(new Card(card));
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            hand.ForEach(c => sb.Append(c.ToString() + " "));
            return sb.ToString();
        }
    }
    class Poker
    {
    }
}
