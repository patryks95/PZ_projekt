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
        double dlugosc;
        string przejscie;
        string opis;

        public Lista(int nr_podmacierzy, float energia, double liczba_falowa, double dlugosc, string przejscie, string opis)
        {
            this.Nr_podmacerzy = nr_podmacierzy;
            this.Energia = energia;
            this.Liczba_falowa = liczba_falowa;
            this.Dlugosc = dlugosc;
            this.Przejscie = przejscie;
            this.Opis = opis;
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

        public string Opis
        {
            get
            {
                return opis;
            }

            set
            {
                opis = value;
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
    }
}
