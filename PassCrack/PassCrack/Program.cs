using System;

class Program
{
    static void Logo()
    {
        Console.WriteLine("*        ____  ___   __________ ________________  ___   ________ __");
        Console.WriteLine("*       / __ \\/   | / ___/ ___// ____/ ____/ __ \\/   | / ____/ //_/");
        Console.WriteLine("*      / /_/ / /| | \\__ \\\\__ \\/ /   / /   / /_/ / /| |/ /   / ,<   ");
        Console.WriteLine("*     / ____/ ___ |___/ /__/ / /___/ /___/ _, _/ ___ / /___/ /| |  ");
        Console.WriteLine("*    /_/   /_/  |_/____/____/\\____/\\____/_/ |_/_/  |_\\____/_/ |_|  ");
        Console.WriteLine("");
    }
    public static void Main()
    {
        int ClientsCount = 0;
        int Method = 0;
        while (true)
        {

            Logo();
            Console.WriteLine("Wpisz 'exit', aby wyjść z programu.");
            Console.WriteLine("Wpisz ilość klientów.");
            string input = Console.ReadLine();
            if (input.ToLower() == "exit")
                break;
            if (!int.TryParse(input, out int result))
            {
                Console.WriteLine("Nie podano liczby.");
                break;
            }
            else
            {
                ClientsCount = result;
            }
            Console.WriteLine("Ilość klientów {0}.", ClientsCount);
            Console.WriteLine("1 - metoda słownikowa.");
            Console.WriteLine("2 - metoda brute force.");
            Console.WriteLine("Wpisz nr metody:");
            input = Console.ReadLine();
            if (input.ToLower() == "exit")
                break;
            if (!int.TryParse(input, out result))
            {
                Console.WriteLine("Nie podano liczby.");
                break;
            }
            else
            {
                if (result!=1 || result!=2)
                {
                    Console.WriteLine("Nie podano nr metody.");
                    break;
                }
                Method = result;
            }
            Console.WriteLine("Metoda {0}.", Method);
            Console.WriteLine("Wpisz tekst.");
            input = Console.ReadLine();
            if (input.ToLower() == "exit")
                break;
            else
            {
                Console.Clear(); // Czyszczenie konsoli
                Console.WriteLine("Wprowadzony tekst: " + input);
            }
        }
    }
}
