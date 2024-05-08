namespace PassCrack.Client
{
    public class BruteForceHandler : PassCracker
    {
        public ulong From;
        public ulong To;
        public BruteForceHandler(ulong from, ulong to, int hash, string keys, List<string> passwords) : base(hash, keys, passwords)
        {
            SetNewSize(from, to);
        }
        public void SetNewSize(ulong from, ulong to)
        {
            From = from;
            To = from+to;
        }
        public override string Resolve()
        {
            ulong i = From;
            var tmp = "";
            while (i < To)
            {
                tmp = DecToString(i);
                var hashString = HashWord(tmp);
                if (CheckPasswords(hashString,tmp))
                {
                    Console.WriteLine("Znaleziono {0} : {1}", tmp, hashString);
                }
                i++;
            }
            return FoundedPasswords;
        }
    }
}
