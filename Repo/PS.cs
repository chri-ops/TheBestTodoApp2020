using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class PS
    {
        private static Random random = new Random();
        public static string CS(string i)
        {
            var b = System.Text.Encoding.UTF8.GetBytes(i);
            using (var h = System.Security.Cryptography.SHA512.Create())
            {
                var hIB = h.ComputeHash(b);
                var hISB = new System.Text.StringBuilder(128);
                foreach (var bS in hIB)
                    hISB.Append(bS.ToString("X2"));
                return hISB.ToString();
            }
        }

        public static string HP(string p)
        {
            string s = RandomString(50);
            string sP = $"{p}:{s}";

            string SsP = CS(sP);
            return $"{SsP}:{s}";
        }

        private static string RandomString(int n)
        {
            const string c = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-*~½!#¤%&()=?";
            return new string(Enumerable.Repeat(c, n)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
