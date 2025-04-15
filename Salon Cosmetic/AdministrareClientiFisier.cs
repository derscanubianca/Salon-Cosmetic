using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Salon_Cosmetic
{
    public class AdministrareClientiFisier
    {
        string caleFisier;

        public AdministrareClientiFisier(string caleFisier)
        {
            this.caleFisier = caleFisier;
        }

        public void AdaugaClient(Client client)
        {
            using (StreamWriter sw = new StreamWriter(caleFisier, true))
            {
                sw.WriteLine($"{client.Id},{client.Nume},{client.Telefon},{client.Adresa}");
            }
        }

        public List<Client> CitesteDinFisier()
        {
            List<Client> clienti = new List<Client>();
            if (!File.Exists(caleFisier)) return clienti;

            using (StreamReader sr = new StreamReader(caleFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    var campuri = linie.Split(',');
                    if (campuri.Length == 4 && int.TryParse(campuri[0], out int id))
                    {
                        clienti.Add(new Client(id, campuri[1], campuri[2], campuri[3]));
                    }
                }
            }
            return clienti;
        }

        public List<Client> GetClient(string nume)
        {
            List<Client> matchingClients = new List<Client>();
            var clienti = CitesteDinFisier();

            matchingClients = clienti
                .Where(c => c.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return matchingClients;
        }
    }
}