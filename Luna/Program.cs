using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** АудитПарсер v. 0.1 *****");
            Console.WriteLine();
            string readPath = @"123.dat";
            string writePath = @"123_BAR.dat";
            string line;
            string[] row;
            string data = "";
            char[] trimChars = { '"', '?', '_', '%', ';' };
            string checkDigit = "BAR";
            using (StreamReader sr = new StreamReader(readPath, System.Text.Encoding.Default))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    row = line.Split('|');
                    foreach (string s in row)
                    {
                        if (row[2] == s && s != "\"MS3\"")
                        {
                            data = "94" + s.Trim(trimChars);
                            checkDigit = CalcEAN13(data).ToString();
                            Console.WriteLine(data + checkDigit);
                            Console.WriteLine();
                        }
                    }
                    line += "|\"" + data + checkDigit + "\"";
                    using (StreamWriter sw = new StreamWriter(writePath, true))
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            Console.ReadLine();
        }

        public static bool CheckLuhn(string data)
        {
            int sum = 0;
            int len = data.Length;
            for (int i = 0; i < len; i++)
            {
                int add = Int32.Parse(data[i].ToString()) * (2 - (i + len) % 2);
                add -= add > 9 ? 9 : 0;
                sum += add;
                Console.WriteLine((data[i]) + "   " + add + "   " + sum);
            }
            return sum % 10 == 0;
        }

        public static int CalcEAN13(string data)
        {
            int sum = 0;
            int len = data.Length;
            int add = 0;
            for (int i = 0; i < len; i++)
            {
                if ((i + 1)%2 == 0)
                {
                    add = Int32.Parse(data[i].ToString()) * 3;
                }
                else
                {
                    add = Int32.Parse(data[i].ToString());
                }
                sum += add;
                //Console.WriteLine(data[i] + "    " + add + "   " + (len - i));
            }
            Console.WriteLine($"check сумма = {sum}");
            Console.WriteLine(10 - sum % 10);
            int check = 10 - sum % 10 == 10 ? 0 : 10 - sum % 10;
            Console.WriteLine($"check digit = {check}");
            return check;
        }
    }
}
