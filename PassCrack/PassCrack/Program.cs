using PassCrack.Host;
using System;

class Program
{
    public int ClientsCount { get; private set; }
    public int Method { get; private set; }

    static void Logo()
    {
        Console.WriteLine("*        ____  ___   __________ ________________  ___   ________ __");
        Console.WriteLine("*       / __ \\/   | / ___/ ___// ____/ ____/ __ \\/   | / ____/ //_/");
        Console.WriteLine("*      / /_/ / /| | \\__ \\\\__ \\/ /   / /   / /_/ / /| |/ /   / ,<   ");
        Console.WriteLine("*     / ____/ ___ |___/ /__/ / /___/ /___/ _, _/ ___ / /___/ /| |  ");
        Console.WriteLine("*    /_/   /_/  |_/____/____/\\____/\\____/_/ |_/_/  |_\\____/_/ |_|  ");
        Console.WriteLine("");
    }
    public bool Config()
    {
        Console.WriteLine("Wpisz ilość klientów.");
        string input = Console.ReadLine();
        if (input.ToLower() == "exit")
            return true;
        if (!int.TryParse(input, out int result))
        {
            Console.WriteLine("Nie podano liczby.");
            return false;
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
            return true;
        if (!int.TryParse(input, out result))
        {
            Console.WriteLine("Nie podano liczby.");
            return false;
        }
        if (result != 1 && result != 2)
        {
            Console.WriteLine("Nie podano nr metody.");
            return false;
        }
        Method = result;
        Console.WriteLine("Metoda {0}.", Method);
        return true;
    }
    public bool MainLoop()
    {
        string input;
        bool config = true;
        bool start = true;

        Console.WriteLine("Wpisz 'exit', aby wyjść z programu.");
        Logo();
        while (true)
        {
            if (config)
            {
                if (!Config())
                    return false;
                else
                    config = false;
            }
/*            Console.WriteLine("Wpisz tekst.");
            input = Console.ReadLine();
            if (input.ToLower() == "exit")
                return true;
            else
            {
                Console.Clear(); // Czyszczenie konsoli
                Console.WriteLine("Wprowadzony tekst: " + input);*/
            if (start)
            {
                start = false;
                Server server = new Server(ClientsCount);
                server.Start();
            }
            //}
        }
    }
    public static void Main()
    {
        Program program = new Program();
        program.MainLoop();
    }
}
