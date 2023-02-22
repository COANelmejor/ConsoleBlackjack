// Declaración de variables, arrys y listas
using ConsoleBlackjack;
using System.Globalization;

int totalJugador,
    totalDealer,
    cartaATomar,
    juegosGanados = 0,
    juegosPerdidos = 0,
    juegosEmpatados = 0,
    juegosJugados = 0,
    billetera = 0,
    billeteraInicial = 0,
    apuesta = 0;

bool billeteraActiva = false;
bool apuestaActiva = false;
var pedirCarta = String.Empty;
var volverAJugar = String.Empty;
var stringIngreasadoPorJugador = String.Empty;

string[] cartasJugador = Array.Empty<string>();
string[] cartasDealer = Array.Empty<string>();

string[] Baraja = {
    "A♥", "2♥", "3♥", "4♥", "5♥", "6♥", "7♥", "8♥", "9♥", "10♥", "J♥", "Q♥", "K♥",
    "A♦", "2♦", "3♦", "4♦", "5♦", "6♦", "7♦", "8♦", "9♦", "10♦", "J♦", "Q♦", "K♦",
    "A♣", "2♣", "3♣", "4♣", "5♣", "6♣", "7♣", "8♣", "9♣", "10♣", "J♣", "Q♣", "K♣",
    "A♠", "2♠", "3♠", "4♠", "5♠", "6♠", "7♠", "8♠", "9♠", "10♠", "J♠", "Q♠", "K♠",
};

List<string> manoList;

// Selección de Idioma
Console.WriteLine(lang.languageSelection);
Console.WriteLine("1. English (Default)");
Console.WriteLine("2. Español");
Console.Write(lang.languageSelectionSelect);
stringIngreasadoPorJugador = Console.ReadLine();

switch (stringIngreasadoPorJugador) {
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

juegoInicia:
// Inicilizando valores del juego
// Totales a 0
totalJugador = 0;
totalDealer = 0;
apuestaActiva = false;
// Limpiar arrays de cartas
cartasJugador = cartasJugador.Where(val => false).ToArray();
cartasDealer = cartasDealer.Where(val => false).ToArray();

// Jugador
if (!billeteraActiva) {
    Console.WriteLine($"{lang.playerWelcome}\n");
    Console.WriteLine(lang.playerWalletAsk);
    Console.WriteLine(lang.infoWalletMultiplier);
    
    while (!billeteraActiva) {
        Console.Write("$");
        stringIngreasadoPorJugador = Console.ReadLine();
        if (!int.TryParse(stringIngreasadoPorJugador, out billetera)) {
            Console.WriteLine(lang.errorOnlyNumbers);
            continue;
        }
        if (billetera == 0) {
            Console.WriteLine(lang.errorNumberZero);
        } else if (billetera % 50 != 0) { 
            Console.WriteLine(lang.errorFiftyMultiplier);
        } else {
            billeteraInicial = billetera;
            billeteraActiva = true;
        }

    }
}

Console.Clear();
while (!apuestaActiva) {
    Console.WriteLine($"{lang.playerBetInfoWalletRemain} ${billetera}");
    Console.WriteLine(lang.playerBetInfoTenMultiplier);
    while (!apuestaActiva) {
        Console.Write("$");
        stringIngreasadoPorJugador = Console.ReadLine();
        if (!int.TryParse(stringIngreasadoPorJugador, out apuesta)) {
            Console.WriteLine(lang.errorOnlyNumbers);
            continue;
        }
        if (apuesta == 0) {
            Console.WriteLine(lang.errorNumberZero);
        } else if (apuesta % 10 != 0) {
            Console.WriteLine(lang.errorTenMultiplier);
        } else if (apuesta > billetera) {
            Console.WriteLine(lang.errorTryToBetMore);
        } else {
            billetera -= apuesta;
            apuestaActiva = true;
        }
    }
}

Console.Clear();
while (totalJugador < 21) {
    {
        RevisarBaraja();
        Console.WriteLine(lang.playerCardAsk);
        pedirCarta = Console.ReadLine();
        switch (pedirCarta) {
            case "s":
            case "S":
            case "y":
            case "Y":
                Console.Clear();
                // Obtener carta
                cartaATomar = random.Next(0, Baraja.Length);
                string carta = Baraja[cartaATomar];

                manoList = cartasJugador.ToList();
                manoList.Add(carta);
                cartasJugador = manoList.ToArray();

                // Eliminar carta de la Baraja
                Baraja = Baraja.Where(val => val != carta).ToArray();

                // Calcualr valor de la carta
                totalJugador = CalcularMano(cartasJugador);

                // Mostrar Carta en Baraja (Solo para Debug)
                // Console.WriteLine($"Cartas en Baraja:\n{string.Join(", ", Baraja)}");
                // Console.WriteLine($"Cartas en Baraja {Baraja.Length}");

                // Mostrar Estado actual del jugador
                Console.WriteLine();
                Console.WriteLine($"{lang.infoTotal}: {totalJugador} | {lang.infoHand}: {string.Join(" ", cartasJugador)}");
                Console.WriteLine();
                break;
            case "n":
            case "N":
                goto jugadorTermina;
            default:
                Console.WriteLine(lang.errorUseYOrN);
                break;
        }
    }
}

jugadorTermina:
if (totalJugador == 21 && cartasJugador.Length == 2) {
    goto mensajeFinal;
}

// Dealer
if (totalJugador < 22) {
    Console.WriteLine(lang.infoDealerPlays);
    while (totalDealer < 21) {
        RevisarBaraja();
        cartaATomar = random.Next(0, Baraja.Length);
        string carta = Baraja[cartaATomar];

        manoList = cartasDealer.ToList();
        manoList.Add(carta);
        cartasDealer = manoList.ToArray();

        // Eliminar carta de la Baraja
        Baraja = Baraja.Where(val => val != carta).ToArray();

        // Calcular valor de la carta
        totalDealer = CalcularMano(cartasDealer);

        // Mostrar Carta en Baraja (Solo para Debug)
        // Console.WriteLine($"Cartas en Baraja:\n{string.Join(", ", Baraja)}");
        // Console.WriteLine($"Cartas en Baraja {Baraja.Length}");

        // Mostrar estado actual del Dealer
        Thread.Sleep(750);
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine(
            $"{lang.infoPlayerHand}\n" +
            $"{lang.infoTotal}: {totalJugador} | {lang.infoHand}: {string.Join(" ", cartasJugador)}\n\n" +
            $"{lang.infoDealerPlays}\n\n" +
            $"{lang.infoTotal}: {totalDealer} | {lang.infoHand}: {string.Join(" ", cartasDealer)}");
        if (totalDealer > totalJugador || totalDealer > 16) {
            break;
        }

    }
}

mensajeFinal:
Console.WriteLine($"{CrearMensajeFinal(totalDealer, totalJugador)}\n");
MostrarMarcador();
if (billetera > 9) {
    Console.WriteLine($"\n{lang.infoSoftGameOver}");
    volverAJugar = Console.ReadLine();
} else {
    Console.WriteLine($"\n{lang.infoHardGameOver}");
    Console.ReadLine();
    goto juegoTermina;
}

while (true) {
    switch (volverAJugar) {
        case "s":
        case "S":
        case "y":
        case "Y":
            Console.Clear();
            goto juegoInicia;
        case "N": 
        case "n":
            goto juegoTermina;
        default:
            Console.WriteLine(lang.errorUseYOrN);
            volverAJugar = Console.ReadLine();
            break;
    }
}

juegoTermina:
Console.Clear();
Console.WriteLine( "┌────────────────────");
Console.WriteLine($"│ {lang.infoFinalScore}");
MensajeBilletera();
MostrarMarcador();
Console.WriteLine( "└────────────────────");
if (billetera < billeteraInicial) {
    Console.WriteLine($"\n{lang.infoGoodLuck}");
}
Console.WriteLine($"\n{lang.infoExit} $_$"); //👍🏽🖐🏼🃏
Console.ReadLine();

void MostrarMarcador () {
                                                    Console.Write($"│ {lang.infoGames}: {juegosJugados} ");
    Console.ForegroundColor = ConsoleColor.Green;   Console.Write($"│ {lang.infoWon}: {juegosGanados} ");
    Console.ForegroundColor = ConsoleColor.Red;     Console.Write($"│ {lang.infoLost}: {juegosPerdidos} ");
    Console.ForegroundColor = ConsoleColor.Yellow;  Console.Write($"│ {lang.infoTied}: {juegosEmpatados} ");
    Console.ResetColor();                           Console.WriteLine("|");
}

void MensajeBilletera () { 
    if (billetera < billeteraInicial) {
        Console.Write("│ ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{lang.infoYourWallet}: ${billetera}.");
        Console.ResetColor();
        Console.Write("│ ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{lang.infoYouLost} ${billeteraInicial - billetera}");
    } else {
        Console.Write("│ ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{lang.infoYourWallet}: ${billetera}.");
        Console.ResetColor();
        Console.Write("│ ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{lang.infoYouWon} ${billetera - billeteraInicial}");
    }
    Console.ResetColor();
}

int CalcularValorCarta(string carta) {
    string valorCarta = carta.Substring(0, carta.Length - 1);
    switch (valorCarta) {
        case "A":
            return 1;
        case "J":
        case "Q":
        case "K":
            return 10;
        default:
            return int.Parse(valorCarta);
    }
}

string CrearMensajeFinal(int totalDealer, int totalJugador) {
    juegosJugados++;
    if (totalJugador == 21 && cartasJugador.Length == 2) {
        billetera += Convert.ToInt32(apuesta * 3);
        juegosGanados++;
        return $"{lang.infoPlayerWonWithBlackjack} ${Convert.ToInt32(apuesta * 3)}!";
    } else if (totalJugador == 21 && totalDealer != 21) {
        billetera += Convert.ToInt32(apuesta * 2);
        juegosGanados++;
        return $"{lang.infoPlayerWonWithTwentyOne} ${Convert.ToInt32(apuesta * 2)}!";
    } else if (totalJugador > 21) {
        juegosPerdidos++;
        return $"{lang.infoPlayerLostWithOverTwentyOne} ${apuesta}!";
    } else if (totalDealer > 21) {
        billetera += Convert.ToInt32(apuesta * 2);
        juegosGanados++;
        return $"{lang.infoDealerLostWithOverTwentyOne} ${Convert.ToInt32(apuesta * 2)}!";
    } else if (totalJugador == totalDealer) {
        billetera += apuesta;
        juegosEmpatados++;
        return $"{lang.infoGameTie}";
    } else if (totalJugador < 21 && totalDealer > totalJugador) {
        juegosPerdidos++;
        return $"{lang.infoDealerWonWithOverPlayer} ${apuesta}";
    } else {
        billetera += Convert.ToInt32(apuesta * 2);
        juegosGanados++;
        return $"{lang.infoPlayerWonWithOverDealer} ${Convert.ToInt32(apuesta * 2)}";
    }
}

void RevisarBaraja () {
    if (Baraja.Length == 0) {
        Baraja = new string[] {
            "A♥", "2♥", "3♥", "4♥", "5♥", "6♥", "7♥", "8♥", "9♥", "10♥", "J♥", "Q♥", "K♥",
            "A♦", "2♦", "3♦", "4♦", "5♦", "6♦", "7♦", "8♦", "9♦", "10♦", "J♦", "Q♦", "K♦",
            "A♣", "2♣", "3♣", "4♣", "5♣", "6♣", "7♣", "8♣", "9♣", "10♣", "J♣", "Q♣", "K♣",
            "A♠", "2♠", "3♠", "4♠", "5♠", "6♠", "7♠", "8♠", "9♠", "10♠", "J♠", "Q♠", "K♠"
        };
    }
    return;
}

int CalcularMano (string[] mano) {
    int totalMano = 0;
    int asEnMano = 0;
    // Revisar cada carta de la mano
    for (int i = 0; i < mano.Length; i++) {
        int valorCarta = CalcularValorCarta(mano[i]);
        if (valorCarta != 1) {
            totalMano += valorCarta;
        } else {
            // Excepto los As
            asEnMano++;
        }
    }

    // Revisar cada As
    for (int a = 0; a < asEnMano; a++) {
        if (totalMano + 11 > 21) { 
            totalMano += 1;
        } else {
            totalMano += 11;
        }
    }

    return totalMano;
}