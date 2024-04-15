using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassCrack.Host
{
    class GlobalData
    {
        private static int _number;

        public static int GetNumber()
        {
            return _number;
        }
        public static void SetNumber(int number) 
        { 
            _number = number;
        }
    }
}
