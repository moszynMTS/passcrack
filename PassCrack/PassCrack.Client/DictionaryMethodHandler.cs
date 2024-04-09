namespace PassCrack.Client
{
    public class DictionaryMethodHandler : PassCracker
    {
        public List<string> Words = new List<string>();
        public DictionaryMethodHandler(string words)
        {
            SetNewSize(words);
        }
        public void SetNewSize(string words)
        {
            Words = words.Split(' ').ToList();
        }
        public override void Resolve()
        {
            foreach (string word in Words)
            {
                var hash = CalculateMD5Hash(word);
                if(CheckPasswords(hash))
                    Console.WriteLine("Znaleziono {0} : {1}", word, hash);
                for (ulong i = 0; i < 10000; i++)//ile dodatkowych wariacji słowa sprawdzić
                {
                    var tmp = DecToString(i);
                    hash = CalculateMD5Hash(word+tmp);
                    if (CheckPasswords(hash))
                        Console.WriteLine("Znaleziono {0} : {1}", word, hash);
                }
            }
        }
    }
}
