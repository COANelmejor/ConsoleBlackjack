﻿// Declaración de variables, arrys y listas
using ConsoleBlackjack;

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

Console.WriteLine($"{LangEN.hello} {LangEN.world}");
Console.WriteLine($"{LangES.hello} {LangES.world}");

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
apuestaActiva = false;
// Limpiar arrays de cartas
cartasJugador = cartasJugador.Where(val => false).ToArray();
cartasDealer = cartasDealer.Where(val => false).ToArray();

// Jugador
if (!billeteraActiva) {
    Console.WriteLine("Hola Jugador. ¿Listo para Jugar?\n");
    Console.WriteLine("¿Cuanto dinero traes para apostar?");
    Console.WriteLine("Debe ser un multiplo de 50. ($50, $100, $750, etc)");
    
    while (!billeteraActiva) {
        Console.Write("$");
        stringIngreasadoPorJugador = Console.ReadLine();
        if (!int.TryParse(stringIngreasadoPorJugador, out billetera)) {
            Console.WriteLine("Ingresa solo un números por favor.");
            continue;
        }
        if (billetera == 0) {
            Console.WriteLine("Ingresa un valor mayor a 0 por favor");
        } else if (billetera % 50 != 0) { 
            Console.WriteLine("Debe ser un multiplo de 50. ($50, $100, $750, etc)");
        } else {
            billeteraInicial = billetera;
            billeteraActiva = true;
        }

    }
}

Console.Clear();
while (!apuestaActiva) {
    Console.WriteLine($"¿Cuanto dinero quieres apostar? Restante: ${billetera}");
    Console.WriteLine("Debe ser un multiplo de 10. ($10, $20, $150, etc)");
    while (!apuestaActiva) {
        Console.Write("$");
        stringIngreasadoPorJugador = Console.ReadLine();
        if (!int.TryParse(stringIngreasadoPorJugador, out apuesta)) {
            Console.WriteLine("Ingresa solo un números por favor.");
            continue;
        }
        if (apuesta == 0) {
            Console.WriteLine("Ingresa un valor mayor a 0 por favor");
        } else if (apuesta % 10 != 0) {
            Console.WriteLine("Debe ser un multiplo de 10. ($10, $20, $150, etc.)");
        } else if (apuesta > billetera) {
            Console.WriteLine("No puedes apostar más de lo que tienes.");
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
        Console.WriteLine("¿Pedir carta? [s|n]");
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
                Console.WriteLine("Por favor solo usa [s|n]'.");
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
if (billetera > 9) {
    Console.WriteLine("\nFin del Juego. ¿Volver a Jugar? [s|n].");
    volverAJugar = Console.ReadLine();
} else {
    Console.WriteLine("\nFin del Juego. Pulsa [Enter] Para continuar.");
    Console.ReadLine();
    goto juegoTermina;
}

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
            Console.WriteLine("Porfavor usa solo [s|n]");
            volverAJugar = Console.ReadLine();
            break;
    }
}

juegoTermina:
Console.Clear();
Console.WriteLine("┌──────────");
Console.WriteLine("│ Marcador Final");
MensajeBilletera();
MostrarMarcador();
Console.WriteLine("└──────────");
if (billetera < billeteraInicial) {
    Console.WriteLine("\nBuena Suerte para la próxima");
}
Console.WriteLine("\nPulsa [Enter] para salir... $_$"); //👍🏽🖐🏼🃏
Console.ReadLine();

void MostrarMarcador () {
                                                    Console.Write($"│ Juegos: {juegosJugados} ");
    Console.ForegroundColor = ConsoleColor.Green;   Console.Write($"│ Ganados: {juegosGanados} ");
    Console.ForegroundColor = ConsoleColor.Red;     Console.Write($"│ Perdidos: {juegosPerdidos} ");
    Console.ForegroundColor = ConsoleColor.Yellow;  Console.Write($"│ Empatados: {juegosEmpatados} ");
    Console.ResetColor();                           Console.WriteLine("|");
}

void MensajeBilletera () { 
    if (billetera < billeteraInicial) {
        Console.Write("│ ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Tu billetera: ${billetera}.");
        Console.ResetColor();
        Console.Write("│ ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Has perdido ${billeteraInicial - billetera}");
    } else {
        Console.Write("│ ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Tu billetera: ${billetera}.");
        Console.ResetColor();
        Console.Write("│ ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Has ganado ${billetera - billeteraInicial}");
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
        return $"Conseguiste 21 con BlackJack. Ganaste ${Convert.ToInt32(apuesta * 3)}!";
    } else if (totalJugador == 21 && totalDealer != 21) {
        billetera += Convert.ToInt32(apuesta * 2);
        juegosGanados++;
        return $"Llegaste a 21, Ganaste ${Convert.ToInt32(apuesta * 2)}!";
    } else if (totalJugador > 21) {
        juegosPerdidos++;
        return $"Te has pasado. Perdiste ${apuesta}!";
    } else if (totalDealer > 21) {
        billetera += Convert.ToInt32(apuesta * 2);
        juegosGanados++;
        return $"El Dealer se ha pasado. Ganaste ${Convert.ToInt32(apuesta * 2)}!";
    } else if (totalJugador == totalDealer) {
        billetera += apuesta;
        juegosEmpatados++;
        return $"Ambos han sacado lo mismo. Empate.";
    } else if (totalJugador < 21 && totalDealer > totalJugador) {
        juegosPerdidos++;
        return $"El dealear tiene más que tú. Perdiste ${apuesta}";
    } else {
        billetera += Convert.ToInt32(apuesta * 2);
        juegosGanados++;
        return $"Tienes más que el dealer. Ganaste ${Convert.ToInt32(apuesta * 2)}";
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