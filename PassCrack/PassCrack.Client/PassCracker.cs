using System.Security.Cryptography;
using System.Text;

namespace PassCrack.Client
{
    public abstract class PassCracker
    {
        public string Keys = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,./;'[]\\-=<>?:\"{}|_+!@#$%^&*() ";
        public List<string> Passwords = new List<string>();
        public string DecToString(ulong number)// dodac sprawdzanie od ktorego znaku zaczynac
        {
            string result = "";
            var lastLetter = false;
            if (number == 0)
                return "a";
            while (number > 0)
            {
                int remainder = (int)(number % (ulong)Keys.Length);
                if (lastLetter)
                    result = Keys[remainder - 1] + result;
                else
                    result = Keys[remainder] + result;
                number /= (ulong)Keys.Length;
                if (number < (ulong)Keys.Length)
                    lastLetter = true;
            }
            return result;
        }
        public bool CheckPasswords(string hash)
        {
            for (int j = 0; j < Passwords.Count; j++)
            {
                if (hash == Passwords[j])
                    return true;
            }
            return false;
        }
        public static string CalculateSHA1Hash(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                // Konwertuj input na tablicę bajtów i oblicz skrót SHA-1
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);

                // Konwertuj tablicę bajtów na ciąg szesnastkowy
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static string CalculateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                // Konwertuj input na tablicę bajtów i oblicz skrót MD5
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Konwertuj tablicę bajtów na ciąg szesnastkowy
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public abstract bool Resolve();

    }
}
