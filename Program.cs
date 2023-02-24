using ConsoleBlackjack;
using System.Globalization;

int totalPlayer,
    totalDealer,
    cardToTake,
    gamesWon = 0,
    gamesLost = 0,
    gamesTie = 0,
    gamesPlayed = 0,
    wallet = 0,
    walletInitialAmount = 0,
    betAmount = 0;

bool isWalletActive = false;
bool isBetActive = false;

var askForCard = String.Empty;
var playAgain = String.Empty;
var stringWritedByPlayer = String.Empty;

string[] cardsPlayer = Array.Empty<string>();
string[] cardsDealer = Array.Empty<string>();

string[] DeckOfCards = {
    "A♥", "2♥", "3♥", "4♥", "5♥", "6♥", "7♥", "8♥", "9♥", "10♥", "J♥", "Q♥", "K♥",
    "A♦", "2♦", "3♦", "4♦", "5♦", "6♦", "7♦", "8♦", "9♦", "10♦", "J♦", "Q♦", "K♦",
    "A♣", "2♣", "3♣", "4♣", "5♣", "6♣", "7♣", "8♣", "9♣", "10♣", "J♣", "Q♣", "K♣",
    "A♠", "2♠", "3♠", "4♠", "5♠", "6♠", "7♠", "8♠", "9♠", "10♠", "J♠", "Q♠", "K♠",
};

List<string> handList;

// Selección de Idioma
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

// Declaracion de métodos globales
Random random = new();

gameStart:
// Inicilizando valores del juego
// Totales a 0
totalPlayer = 0;
totalDealer = 0;
isBetActive = false;
// Limpiar arrays de cartas
cardsPlayer = cardsPlayer.Where(val => false).ToArray();
cardsDealer = cardsDealer.Where(val => false).ToArray();

// Jugador
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

Console.Clear();
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

Console.Clear();
while (totalPlayer < 21) {
    {
        CheckDeck();
        Console.WriteLine(lang.playerCardAsk);
        askForCard = Console.ReadLine();
        switch (askForCard) {
            case "s": // Spanish yes (si)
            case "S": // Spanish Yes (Si)
            case "y":
            case "Y":
                Console.Clear();
                // Obtener carta
                cardToTake = random.Next(0, DeckOfCards.Length);
                string carta = DeckOfCards[cardToTake];

                handList = cardsPlayer.ToList();
                handList.Add(carta);
                cardsPlayer = handList.ToArray();

                // Eliminar carta de la Baraja
                DeckOfCards = DeckOfCards.Where(val => val != carta).ToArray();

                // Calcualr valor de la carta
                totalPlayer = CalculateHand(cardsPlayer);

                // Mostrar Carta en Baraja (Solo para Debug)
                // Console.WriteLine($"Cartas en Baraja:\n{string.Join(", ", Baraja)}");
                // Console.WriteLine($"Cartas en Baraja {Baraja.Length}");

                // Mostrar Estado actual del jugador
                Console.WriteLine();
                Console.WriteLine($"{lang.infoTotal}: {totalPlayer} | {lang.infoHand}: {string.Join(" ", cardsPlayer)}");
                Console.WriteLine();
                break;
            case "n":
            case "N":
                goto playerFinish;
            default:
                Console.WriteLine(lang.errorUseYOrN);
                break;
        }
    }
}

playerFinish:
if (totalPlayer == 21 && cardsPlayer.Length == 2) {
    goto finalMessage;
}

// Dealer
if (totalPlayer < 22) {
    Console.WriteLine(lang.infoDealerPlays);
    while (totalDealer < 21) {
        CheckDeck();
        cardToTake = random.Next(0, DeckOfCards.Length);
        string carta = DeckOfCards[cardToTake];

        handList = cardsDealer.ToList();
        handList.Add(carta);
        cardsDealer = handList.ToArray();

        // Eliminar carta de la Baraja
        DeckOfCards = DeckOfCards.Where(val => val != carta).ToArray();

        // Calcular valor de la carta
        totalDealer = CalculateHand(cardsDealer);

        // Mostrar Carta en Baraja (Solo para Debug)
        // Console.WriteLine($"Cartas en Baraja:\n{string.Join(", ", Baraja)}");
        // Console.WriteLine($"Cartas en Baraja {Baraja.Length}");

        // Mostrar estado actual del Dealer
        Thread.Sleep(750);
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine(
            $"{lang.infoPlayerHand}\n" +
            $"{lang.infoTotal}: {totalPlayer} | {lang.infoHand}: {string.Join(" ", cardsPlayer)}\n\n" +
            $"{lang.infoDealerPlays}\n\n" +
            $"{lang.infoTotal}: {totalDealer} | {lang.infoHand}: {string.Join(" ", cardsDealer)}");
        if (totalDealer > totalPlayer || totalDealer > 16) {
            break;
        }

    }
}

finalMessage:
Console.WriteLine($"{CreateFinalMessage(totalDealer, totalPlayer)}\n");
ShowScoreboard();
if (wallet > 9) {
    Console.WriteLine($"\n{lang.infoSoftGameOver}");
    playAgain = Console.ReadLine();
} else {
    Console.WriteLine($"\n{lang.infoHardGameOver}");
    Console.ReadLine();
    goto gameEnds;
}

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

gameEnds:
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

void ShowScoreboard () {
                                                    Console.Write($"│ {lang.infoGames}: {gamesPlayed} ");
    Console.ForegroundColor = ConsoleColor.Green;   Console.Write($"│ {lang.infoWon}: {gamesWon} ");
    Console.ForegroundColor = ConsoleColor.Red;     Console.Write($"│ {lang.infoLost}: {gamesLost} ");
    Console.ForegroundColor = ConsoleColor.Yellow;  Console.Write($"│ {lang.infoTied}: {gamesTie} ");
    Console.ResetColor();                           Console.WriteLine("|");
}

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

int CalculateHand (string[] hand) {
    int totalHand = 0;
    int asInHand = 0;
    // Revisar cada carta de la mano
    for (int i = 0; i < hand.Length; i++) {
        int cardValue = CalculateCardValue(hand[i]);
        if (cardValue != 1) {
            totalHand += cardValue;
        } else {
            // Excepto los As
            asInHand++;
        }
    }

    // Revisar cada As
    for (int a = 0; a < asInHand; a++) {
        if (totalHand + 11 > 21) { 
            totalHand += 1;
        } else {
            totalHand += 11;
        }
    }

    return totalHand;
}