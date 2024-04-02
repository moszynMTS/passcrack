using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            To = to;
        }
        public override void Resolve()
        {
            ulong i = From;
            while (i < To)
            {
                var tmp = DecToString(i);
                var hash = CalculateMD5Hash(tmp);
                if (CheckPasswords(hash))
                    Console.WriteLine("Znaleziono {0} : {1}", tmp, hash);
                i++;
            }
        }
    }
}
