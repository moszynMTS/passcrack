namespace PassCrack.Client
{
    public class DictionaryMethodHandler : PassCracker
    {
        public List<string> Words = new List<string>();
        public DictionaryMethodHandler(List<string> words, int hash, string keys, List<string> passwords) : base(hash, keys, passwords)
        {
            SetNewSize(words);
        }
        public void SetNewSize(List<string> words)
        {
            Words = words;
        }
        public override string Resolve()
        {
            foreach (string word in Words)
            {
                var hashString = HashWord(word);
                if(CheckPasswords(hashString, word))
                    Console.WriteLine("Znaleziono {0} : {1}", word, hashString);
                for (ulong i = 0; i < 10000; i++)//ile dodatkowych wariacji słowa sprawdzić
                {
                    var tmp = DecToString(i);
                    hashString = HashWord(word+tmp);
                    if (CheckPasswords(hashString, word + tmp))
                    {
                        Console.WriteLine("Znaleziono {0} : {1}", tmp, hashString);
                    }
                }
            }
            return FoundedPasswords;
        }
    }
}
