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
            Console.Write("Введите имя файла: ");
            string readPath = Console.ReadLine();
            string writePath = readPath.Replace(".","_BAR.");
            string line;
            string[] row;
            string data = "";
            char[] trimChars = { '"', '?', '_', '%', ';' };
            string checkDigit = "BAR";
            using (StreamReader sr = new StreamReader(readPath, System.Text.Encoding.Default))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    row = line.Split('|');
                    foreach (string s in row)
                    {
                        if (row[2] == s && s != "\"MagstripeRead3\"")
                        {
                            data = "94" + s.Trim(trimChars);
                            checkDigit = CalcEAN13(data).ToString();
                        }
                    }
                    line += "|\"" + data + checkDigit + "\"";
                    using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            FileInfo fileInf = new FileInfo(writePath);
            if (fileInf.Exists)
            {
                Console.WriteLine("Создан файл с баркодами:");
                Console.WriteLine("Имя файла: {0}", fileInf.Name);
                Console.WriteLine("Время создания: {0}", fileInf.CreationTime);
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
            }
            int check = (10 - (sum % 10)) % 10;
            return check;
        }
    }
}
