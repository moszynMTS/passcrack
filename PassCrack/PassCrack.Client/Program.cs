using PassCrack.Client;
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
    public void MainLoop()
    {
        Client client = new Client();
        Logo();
        client.Start();
    }
    public static void Main()
    {
        Program program = new Program();
        program.MainLoop();
    }
}
