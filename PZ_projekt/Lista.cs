using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ_projekt
{
    class Lista : IEquatable<Lista>, IComparable<Lista>
    {
        int nr_podmacierzy;
        float energia;
        double liczba_falowa;
        public double dlugosc;
        string przejscie;

        string opis_one;
        double energia_one;
        double macierz_one;
        double j_one;

        string opis_two;
        double energia_two;
        double macierz_two;
        double j_two;

        string liczba_elektronow;


        public Lista(string opis_one, double energia_one, double macierz_one,  string opis_two, double energia_two, double macierz_two,  string liczba_elektronow)
        {

            this.liczba_falowa = Math.Round(Math.Abs(energia_two - energia_one), 4);
            
            if( liczba_falowa >= 50000)
            {
                this.dlugosc = Math.Round((100000000 / liczba_falowa), 5);
            }
            if (liczba_falowa < 50000)
            {
                Double n = 1 + (8060.51 + 2480990 / (132.2474 - Math.Pow((Liczba_falowa / 10000), 2)) + 17455.7 / (39.32957 - Math.Pow((Liczba_falowa / 10000), 2))) * 0.00000001;
                this.dlugosc = Math.Round((100000000 / (Liczba_falowa * n)), 5);
               
            }
            
            if ((energia_two - energia_one) > 0)
            {
                this.przejscie = "e -> o";
            }
            if ((energia_two - energia_one) == 0)
            {
                this.przejscie = "brak";
            }
            if ((energia_two - energia_one) < 0)
            {
                this.przejscie = "o -> e";
            }

            this.opis_one = opis_one;
            this.energia_one = energia_one;
            this.macierz_one = macierz_one;

            this.liczba_elektronow = liczba_elektronow;
            if (liczba_elektronow == "parzyste")
            {
                this.j_one = macierz_one - 1;
                this.j_two = macierz_two - 1;
            }
            else
            {
                this.j_one = macierz_one -(float) 0.5;
                this.j_two = macierz_two - (float)0.5;
            }

            this.opis_two = opis_two;
            this.energia_two = energia_two;
            this.macierz_two = macierz_two;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Lista objAsPart = obj as Lista;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public int PoDlugosci(double dlugosc1, double dlugosc2)
        {

            return dlugosc1.CompareTo(dlugosc2);
        }

        // Default comparer for Part type.
        public int CompareTo(Lista compareLista)
        {
            // A null value means that this object is greater.
            if (compareLista == null)
                return 1;

            else
                return this.nr_podmacierzy.CompareTo(compareLista.nr_podmacierzy);
        }

        public bool Equals(Lista other)
        {
            if (other == null) return false;
            return (this.nr_podmacierzy.Equals(other.nr_podmacierzy));
        }
        public int Nr_podmacerzy
        {
            get
            {
                return nr_podmacierzy;
            }

            set
            {
                nr_podmacierzy = value;
            }
        }

        public int Nr_podmacierzy
        {
            get
            {
                return nr_podmacierzy;
            }

            set
            {
                nr_podmacierzy = value;
            }
        }

        public float Energia
        {
            get
            {
                return energia;
            }

            set
            {
                energia = value;
            }
        }

        public double Liczba_falowa
        {
            get
            {
                return liczba_falowa;
            }

            set
            {
                liczba_falowa = value;
            }
        }

        public double Dlugosc
        {
            get
            {
                return dlugosc;
            }

            set
            {
                dlugosc = value;
            }
        }

        public string Przejscie
        {
            get
            {
                return przejscie;
            }

            set
            {
                przejscie = value;
            }
        }

        public string Opis_one
        {
            get
            {
                return opis_one;
            }

            set
            {
                opis_one = value;
            }
        }

        public double Energia_one
        {
            get
            {
                return energia_one;
            }

            set
            {
                energia_one = value;
            }
        }

        public double Macierz_one
        {
            get
            {
                return macierz_one;
            }

            set
            {
                macierz_one = value;
            }
        }

        public double J_one
        {
            get
            {
                return j_one;
            }

            set
            {
                j_one = value;
            }
        }

        public string Opis_two
        {
            get
            {
                return opis_two;
            }

            set
            {
                opis_two = value;
            }
        }

        public double Energia_two
        {
            get
            {
                return energia_two;
            }

            set
            {
                energia_two = value;
            }
        }

        public double Macierz_two
        {
            get
            {
                return macierz_two;
            }

            set
            {
                macierz_two = value;
            }
        }

        public double J_two
        {
            get
            {
                return j_two;
            }

            set
            {
                j_two = value;
            }
        }

        public string Liczba_elektronow
        {
            get
            {
                return liczba_elektronow;
            }

            set
            {
                liczba_elektronow = value;
            }
        }
    }
}
