Random random = new Random();
// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

int     totalJugador = 0,
        totalDealer = 0;

string  message;
var     pedirCarta = String.Empty;

string[] Baraja = {
    "A♥", "2♥", "3♥", "4♥", "5♥", "6♥", "7♥", "8♥", "9♥", "10♥", "J♥", "Q♥", "K♥",
    "A♦", "2♦", "3♦", "4♦", "5♦", "6♦", "7♦", "8♦", "9♦", "10♦", "J♦", "Q♦", "K♦",
    "A♣", "2♣", "3♣", "4♣", "5♣", "6♣", "7♣", "8♣", "9♣", "10♣", "J♣", "Q♣", "K♣",
    "A♠", "2♠", "3♠", "4♠", "5♠", "6♠", "7♠", "8♠", "9♠", "10♠", "J♠", "Q♠", "K♠"
};


string cartasJugador = String.Empty;
string cartasDealer = String.Empty;

// Blackjack, Juntar 21 pidiendo cartas o en caso de tener menos
// de 21 tener mayor puntuación que el dealer

// Jugador

Console.WriteLine("Listo Jugador?");
while (totalJugador < 21) {
    {
        Console.WriteLine("¿Desea pedir otra carta? (s/n)");

        pedirCarta = Console.ReadLine();
        if (pedirCarta == "s") {
            // Obtener carta
            int cartaATomar = random.Next(1, 11);
            string carta = Baraja[cartaATomar];
            cartasJugador = $"{cartasJugador} {carta}";

            // Eliminar carta de la Baraja
            Baraja = Baraja.Where(val => val != carta).ToArray();

            // Calcualr valor de la carta
            string valorCarta = carta.Substring(0, carta.Length - 1);
            int valor = 0;
            switch (valorCarta) {
                case "A":
                    if (totalJugador + 11 > 21) {
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
                    valor = int.Parse(valorCarta);
                    break;
            }
            totalJugador += valor;

            // Mostrar Carta en Baraja
            // Console.WriteLine($"Cartas en Baraja:\n{string.Join(", ", Baraja)}");
            // Console.WriteLine($"Cartas en Baraja {Baraja.Length}");

            // Imprimir listado de cartas
            Console.WriteLine($"Total: {totalJugador} | Cartas:{cartasJugador}");
        } else if (pedirCarta == "n") { 
            break;
        } else {
            Console.WriteLine("Parfavor solo usa 's' o 'n' minúsculas.");
        }

    }
}

// Dealer
if (totalJugador != 21 && totalJugador < 21) {
    Console.WriteLine("Dealer Juega");
    while (totalDealer < 21) {
        int cartaATomar = random.Next(1, 11);
        string carta = Baraja[cartaATomar];
        cartasDealer = $"{cartasDealer} {carta}";

        // Eliminar carta de la Baraja
        Baraja = Baraja.Where(val => val != carta).ToArray();

        // Calcualr valor de la carta
        string valorCarta = carta.Substring(0, carta.Length - 1);
        int valor = 0;
        switch (valorCarta) {
            case "A":
                if (totalDealer + 11 > 21) {
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
                valor = int.Parse(valorCarta);
                break;
        }
        totalDealer += valor;

        // Mostrar Carta en Baraja
        // Console.WriteLine($"Cartas en Baraja:\n{string.Join(", ", Baraja)}");
        // Console.WriteLine($"Cartas en Baraja {Baraja.Length}");

        // Imprimir listado de cartas
        Console.WriteLine($"Total: {totalDealer} | Cartas:{cartasDealer}");
        if (totalDealer >= totalJugador) {
            break;
        }

    }
}

if (totalJugador == 21) {
    message = "Llegaste a 21, Ganaste BlackJack ";
} else if (totalJugador > 21) {
    message = "Te has pasado. Perdiste!";
} else if (totalDealer > 21) {
    message = "El Dealer se ha pasado. Ganaste!";
} else if (totalJugador == totalDealer) {
    message = "Ambos han sacado lo mismo. Empate.";
} else if (totalJugador < 21 && totalDealer > totalJugador) {
    message = "El dealear tiene más que tú. Perdiste";
} else {
    message = "Ganaste";
}

Console.WriteLine(message);
