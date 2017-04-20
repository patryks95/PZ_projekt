using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace PZ_projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private OpenFileDialog openFileDialogFile1=new OpenFileDialog();
        private string _File1Stream { get; set; }
        private string _File2Stream { get; set; }

        private string _File1Name { get; set; }
        private string _File2Name { get; set; }
        private int _MatrixCount { get; set; }

        List<string> one;
        List<string> two;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialogFile1.ShowDialog() == true)
            {
                if (openFileDialogFile1.FileName.IndexOf(".out") > -1)
                {
                    if (string.IsNullOrEmpty(_File1Stream))
                    {
                        _File1Stream = openFileDialogFile1.FileName;
                        _File1Name = _File1Stream.Substring(_File1Stream.LastIndexOf('\\') + 1);
                        labelFile1.Content += _File1Name;
                        labelFile1.Visibility = Visibility.Visible;
                        Usuń1.Visibility = Visibility.Visible;

                    }
                    else if (string.IsNullOrEmpty(_File1Stream) || string.IsNullOrEmpty(_File2Stream))
                    {
                        if (_File1Stream != openFileDialogFile1.FileName)
                        {
                            _File2Stream = openFileDialogFile1.FileName;
                            _File2Name = _File2Stream.Substring(_File2Stream.LastIndexOf('\\') + 1);
                            labelFile2.Content += _File2Name;
                            labelFile2.Visibility = Visibility.Visible;
                            Usuń2.Visibility=Visibility.Visible;
                            Generuj.IsEnabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Podany plik już instnieje", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Dodano maksymalna liczbe plików", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
                else
                {

                    MessageBox.Show("Błędny format pliku", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);


                }
            }

        }



        private void Generuj_Click(object sender, RoutedEventArgs e)
        {
            System.IO.StreamReader sr = new
            System.IO.StreamReader(this._File1Stream);
            List<string> temp = new List<string>();
            while (!sr.EndOfStream)
            {
                temp.Add(sr.ReadLine());
            }
            one = dane(temp.ToArray<string>());

            

            _MatrixCount = Convert.ToInt32(one[(one.Count - 4)]);
            comboBoxpodmacierz.Items.Clear();

            for (int i = 1; i <= _MatrixCount; i++)
            {
                
                comboBoxpodmacierz.Items.Add(i);
            }
            sr.Close();

            sr = new
               System.IO.StreamReader(_File2Stream);
            temp = new List<string>();
            while (!sr.EndOfStream)
            {
                temp.Add(sr.ReadLine());
            }
            two = dane(temp.ToArray<string>());

            sr.Close();
            comboBoxpodmacierz.SelectedItem = 1;

            

        }
        private void dodaj_do_panelu()
        {
            Expander temp;

            panel.Children.Clear();
            for (int i = 0; i < one.Count; i += 4)
            {
                if (Convert.ToInt32(one[i]) == Convert.ToInt32(comboBoxpodmacierz.SelectedItem.ToString()))
                {
                    temp = new Expander();
                    temp.Header = string.Format("{0,-10}{1,-15}{2,-15}", one[i], one[i + 2], one[i + 3]);
                    StackPanel m_panel = new StackPanel();
                    temp.Content = m_panel;

                    temp.Expanded += (s, e) =>
                    {

                        Expander dzialania = (Expander)s;
                        string tytul = dzialania.Header.ToString();
                        StackPanel doczyszczenia = (StackPanel)dzialania.Content;
                        doczyszczenia.Children.Clear();
                        /* Dlaczego działa m_panel.Children.Clear()*/
                        List<string> obliczone = oblicz(tytul.Substring(0, 10), tytul.Substring(10, 15), two);
                        foreach (string obliczenia in obliczone)
                        {
                            TextBox dodany = new TextBox();
                            dodany.IsReadOnly = true;
                            dodany.Text = obliczenia;
                            doczyszczenia.Children.Add(dodany);
                        }
                    };

                    panel.Children.Add(temp);
                }
            }

        }
        private void Usuń1_Click(object sender, RoutedEventArgs e)
        {
            labelFile1.Content = "Plik1:  ";
            if (string.IsNullOrEmpty(this._File2Stream))
            {
                this._File1Name = null;
                this._File1Stream = null;
                labelFile1.Visibility = Visibility.Hidden;
                Usuń1.Visibility = Visibility.Hidden;
            }
            else
            {
                labelFile2.Content = "Plik2:  ";
                this._File1Name = this._File2Name;
                labelFile1.Content += this._File1Name;
                this._File1Stream = this._File2Stream;
                this._File2Name = null;
                this._File2Stream = null;
                labelFile2.Visibility = Visibility.Hidden;
                Usuń2.Visibility = Visibility.Hidden;
            }
        }

        private void Usuń2_Click(object sender, RoutedEventArgs e)
        {
            labelFile2.Content = "Plik2:";
            this._File2Name = null;
            this._File2Stream = null;
            labelFile2.Visibility = Visibility.Hidden;
            Usuń2.Visibility = Visibility.Hidden;
        }


        static List<string> dane(string[] tab)
        {
            string[] dane = null;
            List<string> lista = new List<string>();
            string pattern = @"^\s[1-9]\s+[1-9][0-9]*\s+[-]*[1-9]+[0-9]+.[0-9][0-9]*";
            string pattern1 = @"^\s[1-9]\s+[1-9][0-9]*\s+[-]*[1-9]+[0-9]+.[0-9][0-9]*\s+[-]*[1-9][0-9]*.[0-9][0-9]*.{0,71}";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            Regex rgx1 = new Regex(pattern1, RegexOptions.IgnoreCase);
            for (int i = 0; i < tab.Length; i++)
            {
                string input;
                input = tab[i];
                MatchCollection matches = rgx.Matches(input);
                MatchCollection matches1 = rgx1.Matches(input);
                if (matches.Count > 0)
                {
                    int n = 0;
                    //Console.WriteLine("{0} ({1} matches):", input, matches.Count);
                    foreach (Match match in matches)
                    {
                        //Console.WriteLine("   " + match.Value);
                        String[] splitTab = match.Value.Split(' ');
                        for (int j = 0; j < splitTab.Length; j++)
                        {
                            if (splitTab[j] != "")
                            {

                                Console.WriteLine(splitTab[j]);
                                lista.Add(splitTab[j]);

                            }

                        }
                        foreach (Match match1 in matches1)
                        {
                            Console.WriteLine(match1.Value.Substring(match1.Value.Length - 10));
                            lista.Add(match1.Value.Substring(match1.Value.Length - 10));
                        }
                    }
                }
            }
            return lista;
        }
        static List<String> oblicz(String numer_macierzy, string energia, List<string> Two)
        {
            List<String> wynik = new List<String>();
            for (int i = 0; i < Two.Count; i++)
            {
                if (i % 4 == 0)
                {
                    if (Int32.Parse(numer_macierzy) - 1 == Int32.Parse(Two[i]) || Int32.Parse(numer_macierzy) == Int32.Parse(Two[i]) || Int32.Parse(numer_macierzy) + 1 == Int32.Parse(Two[i]))
                    {

                        Double Energia_one = Math.Round(Double.Parse(energia.Replace(".", ",")),4);
                        Double Energia_two = Math.Round(Double.Parse(Two[i + 2].Replace(".", ",")),4);
                        Double Liczba_falowa = Math.Round(Math.Abs(Energia_two - Energia_one),4, MidpointRounding.ToEven);
                        Double dlugosc=0;
                        if (Liczba_falowa >= 50000)
                        {
                            dlugosc = Math.Round((1000000000 / Liczba_falowa), 4); 
                        }
                        if (Liczba_falowa < 50000)
                        {
                            Double n =  1 + (8060.51 + 2480990 / (132.2474 - Math.Pow((Liczba_falowa / 10000), 2) + 17455.7 / (39.32957 - Math.Pow((Liczba_falowa / 1000), 2)))) * 0.00000001;
                            dlugosc = Math.Round((1000000000 / (Liczba_falowa * n)), 4);


                        }
                        if ((Energia_two - Energia_one) > 0) {
                            wynik.Add(String.Format("{0,15}{1,40}{2,40}{3,40}{4,20}{5,40}", Two[i].ToString(), Two[i + 2].ToString(),  Liczba_falowa.ToString(), dlugosc.ToString(), "↑",Two[i + 3].ToString()));
                        }
                        if ((Energia_two - Energia_one) == 0)
                        {
                            wynik.Add(String.Format("{0,15}{1,40}{2,40}{3,40}{4,20}{5,40}", Two[i].ToString(), Two[i + 2].ToString(), Liczba_falowa.ToString(), dlugosc.ToString(), "◊", Two[i + 3].ToString()));
                        }
                        if ((Energia_two - Energia_one) < 0)
                        {
                            wynik.Add(String.Format("{0,15}{1,40}{2,40}{3,40}{4,20}{5,40}", Two[i].ToString(), Two[i + 2].ToString(), Liczba_falowa.ToString(), dlugosc.ToString(), "↓", Two[i + 3].ToString()));
                        }



                    }


                }



            }

            return wynik;
        }

        private void comboBoxpodmacierz_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxpodmacierz.Items.Count > 0)
            {
                dodaj_do_panelu();
            }
            
        }
    }
}
