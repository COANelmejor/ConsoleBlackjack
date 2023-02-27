using System;
namespace ConsoleBlackjack;

public class GameUtils
{
    // Show hand in the version that is chosen.
    public static string ShowHand(string[] hand, bool isASCIIArt, bool shouldBeBigStyle)
    {
        if (!isASCIIArt)
        {
            return $"| {lang.infoHand}: {string.Join(" ", hand)}";
        }
        else
        {
            if (shouldBeBigStyle)
            {
                return bigASCIIArt(hand);
            }
            else
            {
                return shortASCIIStyle(hand);
            }
        }
    }

    // Generate a big version of cards
    static string bigASCIIArt(string[] hand)
    {
        string line1 = string.Empty,
               line2 = string.Empty,
               line3 = string.Empty,
               line4 = string.Empty,
               line5 = string.Empty,
               line6 = string.Empty,
               line7 = string.Empty,
               line8 = string.Empty,
               line9 = string.Empty;
        for (int i = 0; i < hand.Length; i++)
        {
            string card = hand[i];
            string cardValue = card.Substring(0, card.Length - 1);
            string s = card.Substring(card.Length - 1); // Card Symbol

            line1 += "┌───────┐";
            line2 += card.Length == 2 ? $"│{card}     │" : $"│{card}    │";

            switch (cardValue)
            {
                case "A":
                    line3 += "│       │";
                    line4 += "│       │";
                    line5 += $"│   {s}   │";
                    line6 += "│       │";
                    line7 += "│       │";
                    break;
                case "2":
                    line3 += "│       │";
                    line4 += $"│   {s}   │";
                    line5 += "│       │";
                    line6 += $"│   {s}   │";
                    line7 += "│       │";
                    break;
                case "3":
                    line3 += "│       │";
                    line4 += $"│    {s}  │";
                    line5 += $"│   {s}   │";
                    line6 += $"│  {s}    │";
                    line7 += "│       │";
                    break;
                case "4":
                    line3 += "│       │";
                    line4 += $"│  {s} {s}  │";
                    line5 += "│       │";
                    line6 += $"│  {s} {s}  │";
                    line7 += "│       │";
                    break;
                case "5":
                    line3 += "│       │";
                    line4 += $"│  {s} {s}  │";
                    line5 += $"│   {s}   │";
                    line6 += $"│  {s} {s}  │";
                    line7 += "│       │";
                    break;
                case "6":
                    line3 += "│       │";
                    line4 += $"│  {s} {s}  │";
                    line5 += $"│  {s} {s}  │";
                    line6 += $"│  {s} {s}  │";
                    line7 += "│       │";
                    break;
                case "7":
                    line3 += "│       │";
                    line4 += $"│  {s} {s}  │";
                    line5 += $"│ {s} {s} {s} │";
                    line6 += $"│  {s} {s}  │";
                    line7 += "│       │";
                    break;
                case "8":
                    line3 += "│       │";
                    line4 += $"│  {s} {s}  │";
                    line5 += $"│ {s} {s} {s} │";
                    line6 += $"│  {s} {s}  │";
                    line7 += "│       │";
                    break;
                case "9":
                    line3 += "│       │";
                    line4 += $"│ {s} {s} {s} │";
                    line5 += $"│ {s} {s} {s} │";
                    line6 += $"│ {s} {s} {s} │";
                    line7 += "│       │";
                    break;
                case "10":
                    line3 += $"│    {s}  │";
                    line4 += $"│ {s} {s} {s} │";
                    line5 += $"│  {s} {s}  │";
                    line6 += $"│ {s} {s} {s} │";
                    line7 += $"│  {s}    │";
                    break;
                case "J":
                    line3 += $"│  {s}{s}{s}{s} │";
                    line4 += $"│    {s}{s} │";
                    line5 += $"│    {s}{s} │";
                    line6 += $"│ {s}  {s}{s} │";
                    line7 += $"│  {s}{s}{s}  │";
                    break;
                case "Q":
                    line3 += $"│  {s}{s}{s}  │";
                    line4 += $"│ {s}{s} {s}{s} │";
                    line5 += $"│ {s}{s} {s}{s} │";
                    line6 += $"│ {s}{s} {s}  │";
                    line7 += $"│  {s}{s} {s} │";
                    break;
                case "K":
                    line3 += $"│ {s}{s} {s}{s} │";
                    line4 += $"│ {s}{s} {s}  │";
                    line5 += $"│ {s}{s}{s}   │";
                    line6 += $"│ {s}{s} {s}  │";
                    line7 += $"│ {s}{s} {s}{s} │";
                    break;

            }
            line8 += card.Length == 2 ? $"│     {s}{cardValue}│" : $"│    {s}{cardValue}│";
            line9 += "└───────┘";


        }

        return $"\n{line1}\n{line2}\n{line3}\n{line4}\n{line5}" +
               $"\n{line6}\n{line7}\n{line8}\n{line9}";
    }

    // Generate a shot version of cards
    static string shortASCIIStyle(string[] hand)
    {
        string line1 = string.Empty,
               line2 = string.Empty,
               line3 = string.Empty,
               line4 = string.Empty;

        for (int i = 0; i < hand.Length; i++)
        {
            string card = hand[i];
            string cardValue = card.Length == 2 ? $"{card.Substring(0, card.Length - 1)} " : card.Substring(0, card.Length - 1);
            string cardSymbol = card.Substring(card.Length - 1);
            line1 += "┌──┐";
            line2 += $"│{cardValue}│";
            line3 += $"│ {cardSymbol}│";
            line4 += "└──┘";
        }

        return $"\n{line1}\n{line2}\n{line3}\n{line4}";
    }

    // Check the value of hand of cards for both player and dealer
    public static int CalculateHand(string[] hand)
    {
        int totalHand = 0;
        int asInHand = 0;
        // check every card in the hand.
        for (int i = 0; i < hand.Length; i++)
        {
            int cardValue = CalculateCardValue(hand[i]);
            if (cardValue != 1)
            {
                totalHand += cardValue;
            }
            else
            {
                // except for As. These cards has an special behaviour
                asInHand++;
            }
        }

        // Every As in hand is check individually
        // if the hand passes 21 with a value of 11 the card values 1
        // otherwise the value of the card is 11
        for (int a = 0; a < asInHand; a++)
        {
            if (totalHand + 11 > 21)
            {
                totalHand += 1;
            }
            else
            {
                totalHand += 11;
            }
        }

        return totalHand;
    }

    // Calculates the value of a single card.
    static int CalculateCardValue(string carta)
    {
        string cardValue = carta.Substring(0, carta.Length - 1);
        switch (cardValue)
        {
            case "A":
                return 1;
            case "J":
            case "Q":
            case "K":
                return 10;
            default:
                return int.Parse(cardValue);
        }
    }

    // Show the final status of players walllet with colors for visual help.
    public static void WriteWalletEndMessage(int wallet, int walletInitialAmount)
    {
        if (wallet < walletInitialAmount)
        {
            Console.Write("│ ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{lang.infoYourWallet}: ${wallet}.");
            Console.ResetColor();
            Console.Write("│ ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{lang.infoYouLost} ${walletInitialAmount - wallet}");
        }
        else
        {
            Console.Write("│ ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{lang.infoYourWallet}: ${wallet}.");
            Console.ResetColor();
            Console.Write("│ ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{lang.infoYouWon} ${wallet - walletInitialAmount}");
        }
        Console.ResetColor();
    }

    // Show the record of games in current session.
    public static void WriteFinalScoreboard(int gamesPlayed, int gamesWon, int gamesLost, int gamesTie)
    {
                                                        Console.Write($"│ {lang.infoGames}: {gamesPlayed} ");
        Console.ForegroundColor = ConsoleColor.Green;   Console.Write($"│ {lang.infoWon}: {gamesWon} ");
        Console.ForegroundColor = ConsoleColor.Red;     Console.Write($"│ {lang.infoLost}: {gamesLost} ");
        Console.ForegroundColor = ConsoleColor.Yellow;  Console.Write($"│ {lang.infoTied}: {gamesTie} ");
        Console.ResetColor();                           Console.WriteLine("|");
    }
}
