namespace PassCrack.Client
{
    public class BruteForceHandler : PassCracker
    {
        public ulong From;
        public ulong To;
        public BruteForceHandler(ulong from, ulong to) {
            SetNewSize(from, to);
        }
        public void SetNewSize(ulong from, ulong to)
        {
            From = from;
            To = from+to;
        }
        public override bool Resolve()
        {
            Console.WriteLine("Szukam od {0} przez {1}", From, To);
            ulong i = From;
            var tmp = "";
            while (i < To)
            {
                tmp = DecToString(i);
                if (tmp.ToString() == "Ala")
                    Console.Write("Sprawdzam {0};", tmp);
                var hash = CalculateMD5Hash(tmp);
                if (CheckPasswords(hash))
                {
                    Console.WriteLine("Znaleziono {0} : {1}", tmp, hash);
                    return true;
                }
                i++;
            }
            Console.WriteLine("Ostatni {0}", tmp);
            return false;
        }
    }
}
