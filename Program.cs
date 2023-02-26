using ConsoleBlackjack;
using System.Globalization;

int totalPlayer, // points of the player
    totalDealer, // points of the dealer
    cardToTake,     // card to take from the deck
    gamesWon = 0,       // games won by the player
    gamesLost = 0,      // games lost by the player
    gamesTie = 0,       // games tied by the player
    gamesPlayed = 0,    // games played by the player
    wallet = 0,                 // wallet of the player
    walletInitialAmount = 0,    // initial amount in wallet of the player
    betAmount = 0;              // bet amount of the player

bool isWalletActive = false,    // is the wallet active?
     isBetActive = false,       // is the bet active?
     showHandInASCCIArt = true,    // Use ASCII art for show hands to the player
     useBigASCIIArtStyle = false;   // Use Big style of ASCII Art


var askForCard = String.Empty;
var playAgain = String.Empty;
var stringWritedByPlayer = String.Empty;

string[] cardsPlayer = Array.Empty<string>(); // cards of the player in the current game
string[] cardsDealer = Array.Empty<string>(); // cards of the dealer in the current game

string[] DeckOfCards = {
    "A♥", "2♥", "3♥", "4♥", "5♥", "6♥", "7♥", "8♥", "9♥", "10♥", "J♥", "Q♥", "K♥",
    "A♦", "2♦", "3♦", "4♦", "5♦", "6♦", "7♦", "8♦", "9♦", "10♦", "J♦", "Q♦", "K♦",
    "A♣", "2♣", "3♣", "4♣", "5♣", "6♣", "7♣", "8♣", "9♣", "10♣", "J♣", "Q♣", "K♣",
    "A♠", "2♠", "3♠", "4♠", "5♠", "6♠", "7♠", "8♠", "9♠", "10♠", "J♠", "Q♠", "K♠",
}; // Deck of cards

List<string> handList; // List of cards of the player or dealer. 

// Language Selection
Console.WriteLine(lang.languageSelection);
Console.WriteLine("1. English (Default)");
Console.WriteLine("2. Español");
Console.Write(lang.languageSelectionSelect);
stringWritedByPlayer = Console.ReadLine();

switch (stringWritedByPlayer) {
    case "1":
    case "en":
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
        break;  
    case "2":
    case "es":
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
        break;  
    default:  
        Console.WriteLine("Invalid selection. English selected by default.");
        Console.WriteLine("Press [Enter] to continue.");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
        Console.ReadLine();
        break;
}
Console.Clear();

// Import methods to be used in the game
Random random = new();

// Every game starts here
gameStart:
// Initilize game values every time the game starts
// Totals to zero
totalPlayer = 0;
totalDealer = 0;
isBetActive = false;
// Clean player and dealer cards
cardsPlayer = Array.Empty<string>();
cardsDealer = Array.Empty<string>();

// Check if the wallet is active
if (!isWalletActive) {
    Console.WriteLine($"{lang.playerWelcome}\n");
    Console.WriteLine(lang.playerWalletAsk);
    Console.WriteLine(lang.infoWalletMultiplier);
    
    while (!isWalletActive) {
        Console.Write("$");
        stringWritedByPlayer = Console.ReadLine();
        if (!int.TryParse(stringWritedByPlayer, out wallet)) {
            Console.WriteLine(lang.errorOnlyNumbers);
            continue;
        }
        if (wallet == 0) {
            Console.WriteLine(lang.errorNumberZero);
        } else if (wallet % 50 != 0) { 
            Console.WriteLine(lang.errorFiftyMultiplier);
        } else {
            walletInitialAmount = wallet;
            isWalletActive = true;
        }

    }
}
// Clear screen after wallet is active
Console.Clear();

// Check if the player has money to bet, ask for the bet amount and then bet
while (!isBetActive) {
    Console.WriteLine($"{lang.playerBetInfoWalletRemain} ${wallet}");
    Console.WriteLine(lang.playerBetInfoTenMultiplier);
    while (!isBetActive) {
        Console.Write("$");
        stringWritedByPlayer = Console.ReadLine();
        if (!int.TryParse(stringWritedByPlayer, out betAmount)) {
            Console.WriteLine(lang.errorOnlyNumbers);
            continue;
        }
        if (betAmount == 0) {
            Console.WriteLine(lang.errorNumberZero);
        } else if (betAmount % 10 != 0) {
            Console.WriteLine(lang.errorTenMultiplier);
        } else if (betAmount > wallet) {
            Console.WriteLine(lang.errorTryToBetMore);
        } else {
            wallet -= betAmount;
            isBetActive = true;
        }
    }
}
// Clear screen after bet is active
Console.Clear();

// Start the game. Player's part
while (totalPlayer < 21) {
    {
        // Always check the deck before ask for a card
        CheckDeck();

        // Ask for a card
        Console.WriteLine(lang.playerCardAsk);
        askForCard = Console.ReadLine();
        switch (askForCard) {
            // Player wants a card
            case "s": // Spanish yes (si)
            case "S": // Spanish Yes (Si)
            case "y":
            case "Y":
                Console.Clear();
                // player gets a card and adds it to the player's hand
                cardToTake = random.Next(0, DeckOfCards.Length);
                string card = DeckOfCards[cardToTake];

                handList = cardsPlayer.ToList();
                handList.Add(card);
                cardsPlayer = handList.ToArray();

                // Remove card from the deck
                DeckOfCards = DeckOfCards.Where(val => val != card).ToArray();

                // Calculate total of the player's hand
                totalPlayer = CalculateHand(cardsPlayer);

                // Show player's hand
                Console.WriteLine();
                Console.WriteLine($"{lang.infoTotal}: {totalPlayer} {ShowHand.Write(cardsPlayer, showHandInASCCIArt, useBigASCIIArtStyle)}");
                Console.WriteLine();
                break;
            // Player doesn't want a card
            case "n":
            case "N":
                goto playerFinish;
            // Player doesn't input a valid value
            default:
                Console.WriteLine(lang.errorUseYOrN);
                break;
        }
    }
}

// player finish
playerFinish:

// First check if the player has a blackjack
if (totalPlayer == 21 && cardsPlayer.Length == 2) {
    goto gameResult;
}

// Dealer's Part

// Check player didn't lose
if (totalPlayer < 22) {
    Console.WriteLine(lang.infoDealerPlays);

    // Wait a little beat
    Thread.Sleep(500);

    // check if dealer's hand is less than 21
    while (totalDealer < 21) {

        // Always check the deck before ask for a card
        CheckDeck();

        // Dealer's gets a card and adds it to the dealer's hand
        cardToTake = random.Next(0, DeckOfCards.Length);
        string card = DeckOfCards[cardToTake];

        handList = cardsDealer.ToList();
        handList.Add(card);
        cardsDealer = handList.ToArray();

        // Remove card from the deck
        DeckOfCards = DeckOfCards.Where(val => val != card).ToArray();

        // Calculate total of the dealer's hand
        totalDealer = CalculateHand(cardsDealer);

        // Show current status of the game
        Thread.Sleep(750);
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine(
            $"{lang.infoPlayerHand}\n" +
            $"{lang.infoTotal}: {totalPlayer} {ShowHand.Write(cardsPlayer, showHandInASCCIArt, useBigASCIIArtStyle)}\n\n" +
            $"{lang.infoDealerPlays}\n\n" +
            $"{lang.infoTotal}: {totalDealer} {ShowHand.Write(cardsDealer, showHandInASCCIArt, useBigASCIIArtStyle)}");
       
        // Dealer stops when gets more than player's hand or more than 16
        if (totalDealer > totalPlayer || totalDealer > 16) {
            break;
        }

    }
}

// Game results part
gameResult:

// Show Scoreboard and some comments
Console.WriteLine($"{CreateFinalMessage(totalDealer, totalPlayer)}\n");
ShowScoreboard();

// Verify player's wallet
if (wallet > 9) {
    // ask for new game if there $10 or more
    Console.WriteLine($"\n{lang.infoSoftGameOver}");
    playAgain = Console.ReadLine();
} else {
    // Continue to "Game Over" if there is not enough money.
    Console.WriteLine($"\n{lang.infoHardGameOver}");
    Console.ReadLine();
    goto gameEnds;
}

// verify if the player wants to play again.
while (true) {
    switch (playAgain) {
        case "s":
        case "S":
        case "y":
        case "Y":
            Console.Clear();
            goto gameStart;
        case "N": 
        case "n":
            goto gameEnds;
        default:
            Console.WriteLine(lang.errorUseYOrN);
            playAgain = Console.ReadLine();
            break;
    }
}

// Game Over part
gameEnds:

// Show final score.
Console.Clear();
Console.WriteLine( "┌────────────────────");
Console.WriteLine($"│ {lang.infoFinalScore}");
WalletEndMessage();
ShowScoreboard();
Console.WriteLine( "└────────────────────");
if (wallet < walletInitialAmount) {
    Console.WriteLine($"\n{lang.infoGoodLuck}");
}
Console.WriteLine($"\n{lang.infoExit} $_$"); //👍🏽🖐🏼🃏
Console.ReadLine();

// Show the record of games in current session.
void ShowScoreboard () {
                                                    Console.Write($"│ {lang.infoGames}: {gamesPlayed} ");
    Console.ForegroundColor = ConsoleColor.Green;   Console.Write($"│ {lang.infoWon}: {gamesWon} ");
    Console.ForegroundColor = ConsoleColor.Red;     Console.Write($"│ {lang.infoLost}: {gamesLost} ");
    Console.ForegroundColor = ConsoleColor.Yellow;  Console.Write($"│ {lang.infoTied}: {gamesTie} ");
    Console.ResetColor();                           Console.WriteLine("|");
}

// Show the final status of players walllet with colors for visual help.
void WalletEndMessage () { 
    if (wallet < walletInitialAmount) {
        Console.Write("│ ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{lang.infoYourWallet}: ${wallet}.");
        Console.ResetColor();
        Console.Write("│ ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{lang.infoYouLost} ${walletInitialAmount - wallet}");
    } else {
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

// Calculates the value of a single card.
int CalculateCardValue(string carta) {
    string cardValue = carta.Substring(0, carta.Length - 1);
    switch (cardValue) {
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

// Create the final message based on dealer and player points and updates record of games.
string CreateFinalMessage(int totalDealer, int totalPlayer) {
    gamesPlayed++;
    if (totalPlayer == 21 && cardsPlayer.Length == 2) {
        wallet += Convert.ToInt32(betAmount * 3);
        gamesWon++;
        return $"{lang.infoPlayerWonWithBlackjack} ${Convert.ToInt32(betAmount * 3)}!";
    } else if (totalPlayer == 21 && totalDealer != 21) {
        wallet += Convert.ToInt32(betAmount * 2);
        gamesWon++;
        return $"{lang.infoPlayerWonWithTwentyOne} ${Convert.ToInt32(betAmount * 2)}!";
    } else if (totalPlayer > 21) {
        gamesLost++;
        return $"{lang.infoPlayerLostWithOverTwentyOne} ${betAmount}!";
    } else if (totalDealer > 21) {
        wallet += Convert.ToInt32(betAmount * 2);
        gamesWon++;
        return $"{lang.infoDealerLostWithOverTwentyOne} ${Convert.ToInt32(betAmount * 2)}!";
    } else if (totalPlayer == totalDealer) {
        wallet += betAmount;
        gamesTie++;
        return $"{lang.infoGameTie}";
    } else if (totalPlayer < 21 && totalDealer > totalPlayer) {
        gamesLost++;
        return $"{lang.infoDealerWonWithOverPlayer} ${betAmount}";
    } else {
        wallet += Convert.ToInt32(betAmount * 2);
        gamesWon++;
        return $"{lang.infoPlayerWonWithOverDealer} ${Convert.ToInt32(betAmount * 2)}";
    }
}

// check if the current deck has enough cards. If there is not enough cards a new deck is "opened"
void CheckDeck () {
    if (DeckOfCards.Length == 0) {
        DeckOfCards = new string[] {
            "A♥", "2♥", "3♥", "4♥", "5♥", "6♥", "7♥", "8♥", "9♥", "10♥", "J♥", "Q♥", "K♥",
            "A♦", "2♦", "3♦", "4♦", "5♦", "6♦", "7♦", "8♦", "9♦", "10♦", "J♦", "Q♦", "K♦",
            "A♣", "2♣", "3♣", "4♣", "5♣", "6♣", "7♣", "8♣", "9♣", "10♣", "J♣", "Q♣", "K♣",
            "A♠", "2♠", "3♠", "4♠", "5♠", "6♠", "7♠", "8♠", "9♠", "10♠", "J♠", "Q♠", "K♠"
        };
    }
    return;
}

// Check the value of hand of cards for both player and dealer
int CalculateHand (string[] hand) {
    int totalHand = 0;
    int asInHand = 0;
    // check every card in the hand.
    for (int i = 0; i < hand.Length; i++) {
        int cardValue = CalculateCardValue(hand[i]);
        if (cardValue != 1) {
            totalHand += cardValue;
        } else {
            // except for As. These cards has an special behaviour
            asInHand++;
        }
    }

    // Every As in hand is check individually
    // if the hand passes 21 with a value of 11 the card values 1
    // otherwise the value of the card is 11
    for (int a = 0; a < asInHand; a++) {
        if (totalHand + 11 > 21) { 
            totalHand += 1;
        } else {
            totalHand += 11;
        }
    }

    return totalHand;
}
