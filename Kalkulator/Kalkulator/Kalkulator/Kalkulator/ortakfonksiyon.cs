using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Kalkulator
{
    public static class ortakfonksiyon
    {
        static char ayrac = ':';
        public static string ingKarakter(string trendegistirici)
        {
            char[] trkarsilik = { 'ş', 'ı', 'ü', 'ç', 'ğ' };
            char[] enkarsilik = { 's', 'i', 'u', 'c', 'g' };
            for (int i = 0; i < trkarsilik.Length; i++)
            {
                trendegistirici = trendegistirici.Replace(trkarsilik[i], enkarsilik[i]);
            }
            return trendegistirici;
        }

        public static bool sayiylaBaslar(string sayikontrolu)
        {
            bool sayiylaBaslamiyor = false;
            int x = 0;
            for (int i = 0; i < 10; i++)
            {
                if (sayikontrolu.StartsWith(i.ToString()))
                {
                    sayiylaBaslamiyor = false;
                }
                else
                {
                    x++;
                }
            }
            if (x == 10)
            {
                sayiylaBaslamiyor = true;
            }
            return sayiylaBaslamiyor;
        }
       public  static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static bool kullanicikontrol(string sayiyabakan)
        {
            bool kontrolcu = true;
            foreach (var item in File.ReadLines("users.txt"))
            {
                if (sayiyabakan==item.Split(ayrac)[0])
                {
                    kontrolcu = false;
                        break;
                }
            }
            return kontrolcu;
        }
        public static bool parolakontrol(string sifre,string kullanici)
        {
            bool bakici = true;
            foreach (var item in File.ReadLines("users.txt"))
            {
                if (kullanici==item.Split(ayrac)[0])
                {
                    if (sifre==item.Split(ayrac)[1])
                    {
                        bakici = false;
                        break;
                    }
                }
            }
            return bakici;
        }
        public static string adgozukmesi;
        public static void appendToLogFile(string srUserName, string srText)
        {
            File.AppendAllText(srUserName + ".txt", srText + "\r\n");
        }
        public static void readToListBox(MainWindow mm, string srUserName)
        {
            if (!File.Exists(srUserName + ".txt"))
                return;
            mm.listeM.Items.Clear();
            List<string> lstUserLogs = File.ReadAllLines(srUserName + ".txt").ToList();
            lstUserLogs.Reverse();
            foreach (var item in lstUserLogs)
            {
                mm.listeM.Items.Add(item);
            }
        }
    }
}
