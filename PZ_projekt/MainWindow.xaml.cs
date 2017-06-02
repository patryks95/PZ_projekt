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
            Zapisz.IsEnabled = true;
            etykiety.Visibility = Visibility.Visible;

            

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
                    temp.Header = string.Format("{0,5}{1,30:N4}         {2,-20:N4}", one[i], one[i + 2], one[i + 3]);
                    StackPanel m_panel = new StackPanel();
                    temp.Content = m_panel;
                    temp.Tag = i.ToString();
                    temp.Expanded += (s, e) =>
                    {
                        
                        Expander dzialania = (Expander)s;
                        StackPanel doczyszczenia = (StackPanel)dzialania.Content;
                        doczyszczenia.Children.Clear();
                        /* Dlaczego działa m_panel.Children.Clear()*/
                        List<string> obliczone = oblicz(one[Convert.ToInt32(dzialania.Tag)], one[Convert.ToInt32(dzialania.Tag)+2], two);
                        foreach (string obliczenia in obliczone)
                        {
                            Grid grid = new Grid();
                           // for (int j = 0; j < one.Count; j += 4)
                           // {
                                String[] skladowe = obliczenia.Split(new Char[] { '-' });
                            
                                
                            
                                for (int a = 0; a<skladowe.Length;a++)
                                {
                                
                                    if (one[Convert.ToInt32(dzialania.Tag)] == "1" && skladowe[0] == "1" && even.IsChecked == true)
                                    {
                                        var chuj = 0;
                                    }
                                    else
                                    {
                                        Label etykieta = new Label();
                                        etykieta.Content = skladowe[a];
                                        etykieta.Margin = new Thickness((a * 80) + 35, 0, 0, 0);
                                        grid.Children.Add(etykieta);
                                    }
                                }
                            //}
                            doczyszczenia.Children.Add(grid);
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

                            Double Energia_one = Double.Parse(energia.Replace(".", ","));
                            Double Energia_two = Double.Parse(Two[i + 2].Replace(".", ","));
                            Double Liczba_falowa = Math.Abs(Energia_two - Energia_one);
                            Double dlugosc = 0;

                            if (Liczba_falowa >= 50000)
                            {
                                dlugosc = Math.Round((100000000 / Liczba_falowa),5);
                            }
                        
                        if (Liczba_falowa < 50000)
                            {

                            
                               Double n = 1 + (8060.51 + 2480990 / (132.2474 - Math.Pow((Liczba_falowa / 10000), 2)) + 17455.7 / (39.32957 - Math.Pow((Liczba_falowa / 10000), 2))) * 0.00000001;
                                dlugosc = Math.Round((100000000 / (Liczba_falowa * n)),5);


                            }
                        Liczba_falowa = Math.Round(Liczba_falowa, 5);
                            if ((Energia_two - Energia_one) > 0)
                            {
                                wynik.Add(String.Format("{0}-{1:N4}-{2:N4}-{3:N4}-{4:N4}-{5}", Two[i].ToString().Trim(), Two[i + 2].ToString().Trim(), Two[i + 3].ToString().Trim(), Liczba_falowa.ToString().Trim(), dlugosc.ToString().Trim(), "↓"));
                            }
                            if ((Energia_two - Energia_one) == 0)
                            {
                                wynik.Add(String.Format("{0}-{1:N4}-{2:N4}-{3:N4}-{4:N4}-{5}", Two[i].ToString().Trim(), Two[i + 2].ToString().Trim(), Two[i + 3].ToString().Trim(), Liczba_falowa.ToString().Trim(), dlugosc.ToString().Trim(), "◊"));
                            }
                            if ((Energia_two - Energia_one) < 0)
                            {
                                wynik.Add(String.Format("{0}-{1:N4}-{2:N4}-{3:N4}-{4:N4}-{5}", Two[i].ToString().Trim(), Two[i + 2].ToString().Trim(), Two[i + 3].ToString().Trim(), Liczba_falowa.ToString().Trim(), dlugosc.ToString().Trim(), "↑"));
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
        
        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Plik tekstowy|*.txt";
            saveFileDialog1.Title = "Zapisz do pliku";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                List<String> wynik = new List<String>();
                List<Lista> lista = new List<Lista>();
                StreamWriter writer = File.CreateText(saveFileDialog1.FileName);
                writer.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------");
                writer.WriteLine(String.Format("{0,15}{1,15:N4}{2,20:N4}{3,8:N4}{4,20}{5,20}{6,8}{7,20}{8,10}", "Długość", "Liczba falowa", "E_u", "J_u", "Opis_u", "E_l", "J_l", "Opis_l", "Przejscie"));
                writer.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------");
                for (int j = 0; j < one.Count; j += 4)
                {
                    //writer.WriteLine(String.Format("{0,5}{1,15}{2,15}", one[j] , one[j + 2] , one[j + 3])); 

                    for (int i = 0; i < two.Count; i++)
                    {
                        if (i % 4 == 0)
                        {
                            if (Int32.Parse(one[j]) - 1 == Int32.Parse(two[i]) || Int32.Parse(one[j]) == Int32.Parse(two[i]) || Int32.Parse(one[j]) + 1 == Int32.Parse(two[i]))
                            {

                                if (one[j] == "1" && two[i] == "1" && even.IsChecked == true)
                                {
                                    
                                }
                                else
                                {
                                    Lista temp_lista = new Lista(one[j + 3].ToString(), Convert.ToDouble((one[j + 2]).Replace(".", ",")), Convert.ToDouble(one[j]), two[i + 3].ToString(), Convert.ToDouble((two[i + 2]).Replace(".", ",")), Convert.ToDouble(two[i]), "parzyste");
                                    lista.Add(temp_lista);
                                }
                                //writer.WriteLine(String.Format("{0,15}{1,40:N4}{2,40:N4}{3,40:N4}{4,20}{5,40}{6,40}{7,40}", lista[0].Dlugosc.ToString(),lista[0].Liczba_falowa.ToString(),lista[0].Energia_one.ToString(), lista[0].J_one.ToString(), lista[0].Opis_one.ToString(), lista[0].Energia_two.ToString(), lista[0].J_two.ToString(), lista[0].Opis_two.ToString()));

                            }

                        }

                    }                

                }
                IEnumerable<Lista> SortedList = lista.OrderBy(o => o.Dlugosc);
                foreach (Lista listeczka in SortedList)
                {

                    writer.WriteLine(String.Format("{0,15}{1,15}{2,20}{3,8}{4,20}{5,20}{6,8}{7,20}{8,10}", listeczka.Dlugosc.ToString(), listeczka.Liczba_falowa.ToString(), listeczka.Energia_one.ToString(), listeczka.J_one.ToString(), listeczka.Opis_one.ToString(), listeczka.Energia_two.ToString(), listeczka.J_two.ToString(), listeczka.Opis_two.ToString(),listeczka.Przejscie.ToString()));
                }
                writer.Close();
            }

            
        }
        
       
    }
}
