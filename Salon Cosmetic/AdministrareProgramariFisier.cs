using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Salon_Cosmetic
{
    public class AdministrareProgramariFisier
    {
        string caleFisier;
        public AdministrareProgramariFisier(string caleFisier)
        {
            this.caleFisier = caleFisier;
        }

        public void AdaugaProgramare(Programare programare)
        {
            using (StreamWriter sw = new StreamWriter(caleFisier, true))
            {
                sw.WriteLine($"{programare.Id},{programare.Client.Id},{programare.DataOra},{programare.Serviciu},{programare.Pret},{programare.Avans}");
            }
        }

        public List<Programare> CitesteProgramari(List<Client> clienti)
        {
            List<Programare> programari = new List<Programare>();
            if (!File.Exists(caleFisier)) return programari;

            using (StreamReader sr = new StreamReader(caleFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    var campuri = linie.Split(',');
                    if (campuri.Length == 6 && int.TryParse(campuri[0], out int id) && int.TryParse(campuri[1], out int clientId))
                    {
                        Client client = clienti.FirstOrDefault(c => c.Id == clientId);
                        if (client != null)
                        {
                            programari.Add(new Programare(id, client, DateTime.Parse(campuri[2]), campuri[3], decimal.Parse(campuri[4]), campuri[5]));
                        }
                    }
                }
            }
            return programari;
        }
    }
}