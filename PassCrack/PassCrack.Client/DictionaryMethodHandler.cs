namespace PassCrack.Client
{
    public class DictionaryMethodHandler : PassCracker
    {
        public List<string> Words = new List<string>();
        public DictionaryMethodHandler(List<string> words, int hash): base(hash)
        {
            SetNewSize(words);
        }
        public void SetNewSize(List<string> words)
        {
            Words = words;
        }
        public override bool Resolve()
        {
            foreach (string word in Words)
            {
                var hashString = HashWord(word);
                if(CheckPasswords(hashString))
                    Console.WriteLine("Znaleziono {0} : {1}", word, hashString);
                for (ulong i = 0; i < 10000; i++)//ile dodatkowych wariacji słowa sprawdzić
                {
                    var tmp = DecToString(i);
                    hashString = HashWord(word+tmp);
                    if (CheckPasswords(hashString))
                    {
                        Console.WriteLine("Znaleziono {0} : {1}", tmp, hashString);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
