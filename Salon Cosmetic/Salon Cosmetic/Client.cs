using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salon_Cosmetic
{
    public class Client
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Telefon { get; set; }
        public string Adresa { get; set; }

        public Client(int id, string nume, string telefon, string adresa)
        {
            Id = id;
            Nume = nume;
            Telefon = telefon;
            Adresa = adresa;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Nume: {Nume}, Telefon: {Telefon}, Adresa: {Adresa}";
        }
    }
}
