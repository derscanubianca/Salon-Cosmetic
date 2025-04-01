using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salon_Cosmetic
{
      public class Programare
    {

        public int Id { get; set; }
        public Client Client { get; set; }
        public DateTime DataOra { get; set; }
        public string Serviciu { get; set; }
        public decimal Pret { get; set; }
        public string Avans { get; set; }

        public Programare(int id, Client client, DateTime dataOra, string serviciu, decimal pret, string avans)
        {
            Id = id;
            Client = client;
            DataOra = dataOra;
            Serviciu = serviciu;
            Pret = pret;
            Avans = avans;
        }
    }
}
