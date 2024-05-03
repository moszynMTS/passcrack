using Newtonsoft.Json;
using PassCrack.Host;

class Program
{
    public ConfigEntity ConfigEntity;

    static void Logo()
    {
        Console.WriteLine("*        ____  ___   __________ ________________  ___   ________ __");
        Console.WriteLine("*       / __ \\/   | / ___/ ___// ____/ ____/ __ \\/   | / ____/ //_/");
        Console.WriteLine("*      / /_/ / /| | \\__ \\\\__ \\/ /   / /   / /_/ / /| |/ /   / ,<   ");
        Console.WriteLine("*     / ____/ ___ |___/ /__/ / /___/ /___/ _, _/ ___ / /___/ /| |  ");
        Console.WriteLine("*    /_/   /_/  |_/____/____/\\____/\\____/_/ |_/_/  |_\\____/_/ |_|  ");
        Console.WriteLine("");
    }
    public bool Config() //used when config.json isnt used
    {
        Console.WriteLine("Wpisz ilość klientów.");
        ConfigEntity = new ConfigEntity();
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
            ConfigEntity.ClientsCount = result;
        }
        Console.WriteLine("Ilość klientów {0}.", ConfigEntity.ClientsCount);
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
        ConfigEntity.Method = result;
        Console.WriteLine("1 - hashowanie md5.");
        Console.WriteLine("2 - hashowanie sha1.");
        Console.WriteLine("Wpisz nr hashowania:");
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
            Console.WriteLine("Nie podano nr hashowania.");
            return false;
        }
        Console.WriteLine("Hashowanie {0}.", ConfigEntity.Method);
        return true;
    }
    public bool MainLoop()
    {
        string input;
        bool config = false; //false - load from file, true - let user write
        bool start = true;
        bool end = false;

        Console.WriteLine("Wpisz 'exit', aby wyjść z programu.");
        Logo();
        while (!end)
        {
            if (config)
            {
                if (!Config())
                    return false;
                else
                    config = false;
            }
            else
            {
                ConfigFromFile();
            }
            if (start)
            {
                start = false;
                Server server = new (ConfigEntity.ClientsCount, ConfigEntity.Method, ConfigEntity.Hash, ConfigEntity.CharacterKeys);
                var passwords = new List<string>()
                {
                    "test",
                    "test2",
                    "ala",
                    "lalat",
                };
                server.Start(passwords, ConfigEntity);
                end = true;
            }
        }
        return true;
    }
    public static void Main()
    {
        Program program = new Program();
        program.MainLoop();
    }
    public void ConfigFromFile()
    {
        string fileName = "config.json";
        string currentDirectory = Directory.GetCurrentDirectory();
        string sourceDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;
        string filePath = Path.Combine(sourceDirectory, fileName);
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Plik konfiguracyjny nie istnieje.");
            return;
        } else
        {
            string jsonConfig = File.ReadAllText(filePath);
            var configFile = JsonConvert.DeserializeObject<ConfigEntity>(jsonConfig);
            if(configFile != null)
            {
                ConfigEntity = configFile;
            }
        }
    }
}
