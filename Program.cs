Random random = new Random();
// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

int     totalJugador,
        totalDealer,
        cartaATomar;

var     pedirCarta = String.Empty;
var     volverAJugar = String.Empty;

string cartasJugador = String.Empty;
string cartasDealer = String.Empty;

string[] Baraja = {
    "A♥", "2♥", "3♥", "4♥", "5♥", "6♥", "7♥", "8♥", "9♥", "10♥", "J♥", "Q♥", "K♥",
    "A♦", "2♦", "3♦", "4♦", "5♦", "6♦", "7♦", "8♦", "9♦", "10♦", "J♦", "Q♦", "K♦",
    "A♣", "2♣", "3♣", "4♣", "5♣", "6♣", "7♣", "8♣", "9♣", "10♣", "J♣", "Q♣", "K♣",
    "A♠", "2♠", "3♠", "4♠", "5♠", "6♠", "7♠", "8♠", "9♠", "10♠", "J♠", "Q♠", "K♠"
};

juegoInicia:

// Inicilizando valores del juego
totalJugador = 0;
totalDealer = 0;
cartasJugador = String.Empty;
cartasDealer = String.Empty;


// Jugador
Console.WriteLine("Listo Jugador?");
while (totalJugador < 21) {
    {
        Console.WriteLine("¿Desea pedir otra carta? (s/n)");

        pedirCarta = Console.ReadLine();
        switch (pedirCarta) {
            case "s":
            case "S":
                // Obtener carta
                cartaATomar = random.Next(0, Baraja.Length);
                string carta = Baraja[cartaATomar];
                cartasJugador = $"{cartasJugador} {carta}";

                // Eliminar carta de la Baraja
                Baraja = Baraja.Where(val => val != carta).ToArray();

                // Calcualr valor de la carta
                string valorCarta = carta.Substring(0, carta.Length - 1);
                totalJugador += CalcularValorCarta(valorCarta, totalJugador);

                // Mostrar Carta en Baraja
                // Console.WriteLine($"Cartas en Baraja:\n{string.Join(", ", Baraja)}");
                // Console.WriteLine($"Cartas en Baraja {Baraja.Length}");

                // Mostrar Estado actual del jugador
                Console.WriteLine($"Total: {totalJugador} | Cartas:{cartasJugador}");
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

// Dealer
if (totalJugador != 21 && totalJugador < 21) {
    Console.WriteLine("Dealer Juega");
    while (totalDealer < 21) {
        cartaATomar = random.Next(0, Baraja.Length);
        string carta = Baraja[cartaATomar];
        cartasDealer = $"{cartasDealer} {carta}";

        // Eliminar carta de la Baraja
        Baraja = Baraja.Where(val => val != carta).ToArray();

        // Calcualr valor de la carta
        string valorCarta = carta.Substring(0, carta.Length - 1);
        
        totalDealer += CalcularValorCarta(valorCarta, totalDealer);

        // Mostrar Carta en Baraja
        // Console.WriteLine($"Cartas en Baraja:\n{string.Join(", ", Baraja)}");
        // Console.WriteLine($"Cartas en Baraja {Baraja.Length}");

        // Mostrar estado actual del Dealer
        Thread.Sleep(750);
        Console.WriteLine($"Total: {totalDealer} | Cartas:{cartasDealer}");
        if (totalDealer >= totalJugador) {
            break;
        }

    }
}

Console.WriteLine(CrearMensajeFinal(totalDealer, totalJugador));
Console.WriteLine("\nFin del Juego. ¿Volver a Jugar? s/n.\n");
volverAJugar = Console.ReadLine();
while (true) {
    switch (volverAJugar) {
        case "s":
        case "S":
            goto juegoInicia;
        case "N":
        case "n":
            goto juegoTermina;
        default:
            Console.WriteLine("Porfavor usa solo 's' o 'n'");
            break;
    }
}

juegoTermina:
Console.WriteLine("Pulsa cualquier tecla para salir... :D.");
Console.ReadLine();

int CalcularValorCarta(string carta, int total) {
    int valor = 0;
    switch (carta) {
        case "A":
            if (total + 11 > 21) {
                valor = 1;
            } else {
                valor = 11;
            }
            break;
        case "J":
        case "Q":
        case "K":
            valor = 10;
            break;
        default:
            valor = int.Parse(carta);
            break;
    }
    return valor;
}

string CrearMensajeFinal(int totalDealer, int totalJugador) {
    if (totalJugador == 21) {
        return "Llegaste a 21, Ganaste BlackJack ";
    } else if (totalJugador > 21) {
        return "Te has pasado. Perdiste!";
    } else if (totalDealer > 21) {
        return "El Dealer se ha pasado. Ganaste!";
    } else if (totalJugador == totalDealer) {
        return "Ambos han sacado lo mismo. Empate.";
    } else if (totalJugador < 21 && totalDealer > totalJugador) {
        return "El dealear tiene más que tú. Perdiste";
    } else {
        return "Ganaste";
    }
}