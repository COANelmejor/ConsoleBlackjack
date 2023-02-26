using System;
namespace ConsoleBlackjack
{
	public class ShowHand
	{
		public static string Write(string[] hand, bool isASCIIArt, bool shouldBeBigStyle)
		{
			if (!isASCIIArt) {
				return $"| { lang.infoHand}: {string.Join(" ", hand)}";
			} else {
				if (shouldBeBigStyle) {
					return bigASCIIArt(hand);
                } else {
					return shortASCIIStyle(hand);
                }
            }
		}

		static string bigASCIIArt(string[] hand)
		{
			return string.Join(" ", hand);
		}

		static string shortASCIIStyle(string[] hand) {
            string line1 = string.Empty,
                   line2 = string.Empty,
                   line3 = string.Empty,
                   line4 = string.Empty;

			for (int i = 0; i < hand.Length; i++) {
				string card = hand[i];
				string cardValue = card.Length == 2 ? $"{card.Substring(0, card.Length - 1)} " : card.Substring(0, card.Length - 1);
				string cardSymbol = card.Substring(card.Length - 1);
                line1 +=  "┌──┐";
				line2 += $"│{cardValue}│";
				line3 += $"│ {cardSymbol}│";
				line4 +=  "└──┘";
            }
            
            return $"\n{line1}\n{line2}\n{line3}\n{line4}";
        }
    }
}


