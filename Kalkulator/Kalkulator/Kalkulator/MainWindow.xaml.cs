using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace Kalkulator
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            sayi0.ToolTip = "this option adds 0 to your process";
            sayi1.ToolTip = "this option adds 1 to your process";
            sayi2.ToolTip = "this option adds 2 to your process";
            sayi3.ToolTip = "this option adds 3 to your process";
            sayi4.ToolTip = "this option adds 4 to your process";
            sayi5.ToolTip = "this option adds 5 to your process";
            sayi6.ToolTip = "this option adds 6 to your process";
            sayi7.ToolTip = "this option adds 7 to your process";
            sayi8.ToolTip = "this option adds 8 to your process";
            sayi9.ToolTip = "this option adds 9 to your process";
            toplayan.ToolTip = "this option adds + to your process";
            cikaran.ToolTip = "this option adds - to your process";
            carpan.ToolTip = "this option adds x to your process";
            bolen.ToolTip = "this option adds / to your process";
            ilkparantez.ToolTip = "this option adds ( to your process";
            ikinciparantez.ToolTip = "this option adds ) to your process";
            nokta.ToolTip = "this option adds . to your process";
            virgul.ToolTip = "this option adds , to your process";
            hepsinisilen.ToolTip = "this option deletes your all process";
            azsilen.ToolTip = "this option deletes only one process";
            uzay.ToolTip = "this option adds space to your process";
            esitlik.ToolTip = "this option ends your process";
            hesaplayiciTab.IsEnabled = false;
            File.AppendAllText("users.txt", "");
        }
        char ayirici = ':';

        private void registerButton_Click_1(object sender, RoutedEventArgs e)
        {

            string kullaniciText = regUNtext.Text; //textimize bir değişken atadık.
            kullaniciText = kullaniciText.ToLower(); //kullanıcı adını küçük harf yaptık.
            kullaniciText = ortakfonksiyon.ingKarakter(kullaniciText); //kullanıcı adımızdaki tr karakterleri değiştirdik.
            kullaniciText = kullaniciText.Trim(); //kullanıcı adımızın başı ve sonundaki boşlukları sildik.
            if (!ortakfonksiyon.sayiylaBaslar(kullaniciText))
            {
                MessageBox.Show("Username can't start with number!");
                return;
            }
            List<string> yasaklikarakterler = new List<string> { "%", "-", "$", "/", "\\", "(", ")", "[", "]", ",", "'", "´", "`", "!", "?", "=", "&", "\"", ";" };
            for (int q = 0; q < yasaklikarakterler.Count; q++)
            {
                if (kullaniciText.Contains(yasaklikarakterler[q]))
                {
                    MessageBox.Show($"Your username contains this ({yasaklikarakterler[q]}) forbidden characters.");
                    return;
                }
            }
            string regpaw;
            if (!(regpwbox01.Password == regpwbox02.Password))
            {
                MessageBox.Show("Your passwords do not match.");
                return;
            }
            else
            {
                regpaw = regpwbox01.Password.ToString();
            }
            if (regpaw.Contains(":"))
            {
                MessageBox.Show("Your password can not contain (:).");
                return;
            }
            if (regpaw.Length < 8)
            {
                MessageBox.Show("Your password should be at least 8 characters.");
                return;
            }
            if (!(regpaw.Any(char.IsDigit) && regpaw.Any(char.IsLower) && regpaw.Any(char.IsUpper)))
            {
                MessageBox.Show("Your password must contains a lowercase letter, an uppercase letter and a number.");
                return;
            }
            bool yasakKarakterVar = false;
            string yasakolanKar = "";
            List<char> yasaklıListe = new List<char>();
            foreach (var item in System.IO.Path.GetInvalidFileNameChars())
            {
                yasaklıListe.Add(item);
            }
            foreach (var item in yasaklıListe)
            {
                if (kullaniciText.IndexOf(item) != -1)
                {
                    yasakKarakterVar = true;
                    yasakolanKar = item.ToString();
                    break;
                }

            }
            checkUserAndPw();
            if (dicUsers.ContainsKey(kullaniciText))
            {
                MessageBox.Show("this username already taken!");
                return;
            }
            if (yasakKarakterVar)
            {
                MessageBox.Show("there is a forbidden number!" + yasakolanKar.ToString());
                return;
            }
            regpaw = ortakfonksiyon.ComputeSha256Hash(regpaw);
            File.AppendAllText("users.txt", kullaniciText + ayirici + regpaw + "\r\n");
            bool basarilikayit = true;
            if (basarilikayit == true)
            {
                ortakfonksiyon.adgozukmesi = kullaniciText;
                hesaplayiciTab.IsEnabled = true;
                giristab.IsEnabled = false;
                kayitTab.IsEnabled = false;
                hesaplayiciTab.IsSelected = true;
                adinburada.Content = ortakfonksiyon.adgozukmesi;
                MessageBox.Show("Account has created!");
                yazilanyer.Focus();
            }
           

        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string sifrelisifre = ortakfonksiyon.ComputeSha256Hash(LogpaW.Password);
            string kullaniciLog = logText.Text.Trim().ToLower();
            kullaniciLog = ortakfonksiyon.ingKarakter(kullaniciLog);
            
            checkUserAndPw();

            if (dicUsers.ContainsKey(kullaniciLog))
            {
                if (dicUsers[kullaniciLog] == sifrelisifre)
                {
                    ortakfonksiyon.adgozukmesi = kullaniciLog;
                    hesaplayiciTab.IsEnabled = true;
                    kayitTab.IsEnabled = false;
                    giristab.IsEnabled = false;
                    hesaplayiciTab.IsSelected = true;
                    adinburada.Content = ortakfonksiyon.adgozukmesi;
                    ortakfonksiyon.readToListBox(this, adinburada.Content.ToString());
                    MessageBox.Show("Login successful!");
                    yazilanyer.Focus();
                }
                else
                {
                    MessageBox.Show("your password is wrong check it!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("your username is wrong check it!");
            }
        }
        Dictionary<string, string> dicUsers;
        private void checkUserAndPw()
        {          
                dicUsers = new Dictionary<string, string>();
                foreach (var item in File.ReadLines("users.txt"))
                {
                    if (!dicUsers.ContainsKey(item.Split(':').First()))
                    {    //first one username second one password
                        dicUsers.Add(item.Split(':')[0], item.Split(':')[1]);//first one key , second value
                    }
                }
        }
        private void sayi2_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "2";
        }

        private void sayi1_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "1";
        }

        private void sayi3_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "3";
        }

        private void sayi4_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "4";
        }

        private void sayi5_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "5";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "6";
        }

        private void sayi7_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "7";
        }

        private void sayi8_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "8";
        }

        private void sayi9_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "9";
        }

        private void sayi0_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "0";
        }

        private void toplayan_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "+";
        }

        private void cikaran_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "-";
        }

        private void carpan_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "*";
        }

        private void bolen_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "/";
        }

        private void ilkparantez_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += "(";
        }

        private void ikinciparantez_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += ")";
        }

        private void nokta_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += ".";
        }

        private void virgul_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += ",";
        }

        private void hepsinisilen_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text = "";
        }

        private void azsilen_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text = (yazilanyer.Text.Remove(yazilanyer.Text.Length - 1, 1));
        }

        private void uzay_Click(object sender, RoutedEventArgs e)
        {
            yazilanyer.Text += " ";
        }

        private void esitlik_Click(object sender, RoutedEventArgs e)
        {
            double dblResult;
            try
            {
                dblResult = Convert.ToDouble(new DataTable().Compute(yazilanyer.Text, null));
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message.ToString());
                return;
            }
            var vrtext = yazilanyer.Text + "\t: " + dblResult.ToString("N2");

            ortakfonksiyon.appendToLogFile(adinburada.Content.ToString(), vrtext);
            listeM.Items.Insert(0, vrtext);
        }

        private void cikisyapiyorum_Click(object sender, RoutedEventArgs e)
        {
            adinburada.Content = "";
            listeM.Items.Clear();
            yazilanyer.Clear();
            hesaplayiciTab.IsEnabled = false;
            giristab.IsEnabled = true;
            kayitTab.IsEnabled = true;
            giristab.IsSelected = true;
            logText.Clear();
            LogpaW.Clear();
            regUNtext.Clear();
            regpwbox01.Clear();
            regpwbox02.Clear();
                
        }

       
    }

}
