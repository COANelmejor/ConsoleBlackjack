// Declaración de variables, arrys y listas
int totalJugador,
        totalDealer,
        cartaATomar,
        juegosGanados = 0,
        juegosPerdidos = 0,
        juegosEmpatados = 0,
        juegosJugados = 0;

var     pedirCarta = String.Empty;
var     volverAJugar = String.Empty;

string[] cartasJugador = { };
string[] cartasDealer = { };

string[] Baraja = {
    "A♥", "2♥", "3♥", "4♥", "5♥", "6♥", "7♥", "8♥", "9♥", "10♥", "J♥", "Q♥", "K♥",
    "A♦", "2♦", "3♦", "4♦", "5♦", "6♦", "7♦", "8♦", "9♦", "10♦", "J♦", "Q♦", "K♦",
    "A♣", "2♣", "3♣", "4♣", "5♣", "6♣", "7♣", "8♣", "9♣", "10♣", "J♣", "Q♣", "K♣",
    "A♠", "2♠", "3♠", "4♠", "5♠", "6♠", "7♠", "8♠", "9♠", "10♠", "J♠", "Q♠", "K♠",
};

List<string> manoList;

// Declaracion de métodos globales
Random random = new();

juegoInicia:
// Inicilizando valores del juego
// Totales a 0
totalJugador = 0;
totalDealer = 0;
// Limpiar arrays de cartas
cartasJugador = cartasJugador.Where(val => false).ToArray();
cartasDealer = cartasDealer.Where(val => false).ToArray();


// Jugador
Console.WriteLine("Listo Jugador?");
while (totalJugador < 21) {
    {
        RevisarBaraja();
        Console.WriteLine("¿Pedir carta? (s/n)");
        pedirCarta = Console.ReadLine();
        switch (pedirCarta) {
            case "s":
            case "S":
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
                Console.WriteLine($"Total: {totalJugador} | Cartas: {string.Join(" ", cartasJugador)}");
                Console.WriteLine();
                break;
            case "n":
            case "N":
                goto jugadorTermina;
            default:
                Console.WriteLine("Parfavor solo usa 's' o 'n'.");
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
    Console.WriteLine("Dealer Juega");
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
            $"Total: {totalJugador} | Cartas: {string.Join(" ", cartasJugador)}\n\n" +
            $"Dealer Juega\n\n" +
            $"Total: {totalDealer} | Cartas: {string.Join(" ", cartasDealer)}");
        if (totalDealer > totalJugador || totalDealer > 16) {
            break;
        }

    }
}

mensajeFinal:
Console.WriteLine($"{CrearMensajeFinal(totalDealer, totalJugador)}\n");
MostrarMarcador();
Console.WriteLine("\nFin del Juego. ¿Volver a Jugar? s/n.");
volverAJugar = Console.ReadLine();

while (true) {
    switch (volverAJugar) {
        case "s":
        case "S":
            Console.Clear();
            goto juegoInicia;
        case "N": 
        case "n":
            goto juegoTermina;
        default:
            Console.WriteLine("Porfavor usa solo 's' o 'n'");
            volverAJugar = Console.ReadLine();
            break;
    }
}

juegoTermina:
Console.Clear();
Console.WriteLine("| Marcador Final");
MostrarMarcador();
Console.WriteLine("\nPulsa [Enter] para salir... $_$"); //👍🏽🖐🏼🃏
Console.ReadLine();

void MostrarMarcador () {
                                                    Console.Write($"| Juegos: {juegosJugados} ");
    Console.ForegroundColor = ConsoleColor.Green;   Console.Write($"| Ganados: {juegosGanados} ");
    Console.ForegroundColor = ConsoleColor.Red;     Console.Write($"| Perdidos: {juegosPerdidos} ");
    Console.ForegroundColor = ConsoleColor.Yellow;  Console.Write($"| Empatados: {juegosEmpatados} ");
    Console.ResetColor();                           Console.WriteLine("|");
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
        juegosGanados++;
        return "Conseguiste 21 con BlackJack. Ganaste!";
    } else if (totalJugador == 21 && totalDealer != 21) {
        juegosGanados++;
        return "Llegaste a 21, Ganaste!";
    } else if (totalJugador > 21) {
        juegosPerdidos++;
        return "Te has pasado. Perdiste!";
    } else if (totalDealer > 21) {
        juegosGanados++;
        return "El Dealer se ha pasado. Ganaste!";
    } else if (totalJugador == totalDealer) {
        juegosEmpatados++;
        return "Ambos han sacado lo mismo. Empate.";
    } else if (totalJugador < 21 && totalDealer > totalJugador) {
        juegosPerdidos++;
        return "El dealear tiene más que tú. Perdiste";
    } else {
        juegosGanados++;
        return "Ganaste";
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