using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devil_Survivor_Script_Editor
{
    class fuzzy     // Levenshtein distance
    {

        public static int matchPercent = 0;

        public static string match(string s, string t)
        {
            matchPercent = 0;
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return "";
            }

            if (m == 0)
            {
                return "";
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            //return d[n, m];
            //return (100 -(d[n, m] * 100) / n);
            int value = (100 - (d[n, m] * 100) / n);
            if (value >= 70)
            {
                matchPercent = value;
                //Console.WriteLine(value + "%");
                return t;
            }
            else return "";
        }
    }
}
