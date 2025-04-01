using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salon_Cosmetic
{
    public class Program
    {
        static void Main(string[] args)
        {
            string fisierClienti = "clienti.txt";
            string fisierProgramari = "programari.txt";
            AdministrareClientiFisier adminClienti = new AdministrareClientiFisier(fisierClienti);
            AdministrareProgramariFisier adminProgramari = new AdministrareProgramariFisier(fisierProgramari);
            List<Client> clienti = adminClienti.CitesteDinFisier();
            List<Programare> programari = adminProgramari.CitesteProgramari(clienti);

            int optiune;
            do
            {
                Console.WriteLine("\nMeniu:");
                Console.WriteLine("1. Adauga client");
                Console.WriteLine("2. Afiseaza clienti");
                Console.WriteLine("3. Cauta client dupa nume");
                Console.WriteLine("4. Modifica client");
                Console.WriteLine("5. Cauta client dupa programare");
                Console.WriteLine("6. Iesire");
                Console.Write("Alege o optiune: ");

                if (!int.TryParse(Console.ReadLine(), out optiune))
                {
                    Console.WriteLine("Optiune invalida!");
                    continue;
                }

                switch (optiune)
                {
                    case 1:
                        AdaugaClient(clienti, adminClienti, adminProgramari, programari);
                        break;
                    case 2:
                        AfiseazaClienti(clienti);
                        break;
                    case 3:
                        CautaClient(clienti);
                        break;
                    case 4:
                        ModificaClient(clienti, adminClienti);
                        break;
                    case 5:
                        CautaClientDupaProgramare(programari);
                        break;
                    case 6:
                        Console.WriteLine("La revedere!");
                        break;
                    default:
                        Console.WriteLine("Optiune invalida!");
                        break;
                }
            } while (optiune != 6);
        }

        static void AfiseazaClienti(List<Client> clienti)
        {
            foreach (var client in clienti)
            {
                Console.WriteLine(client);
            }
        }

        
    
        static void CautaClient(List<Client> clienti)
        {
            Console.Write("Introduceti numele clientului: ");
            string nume = Console.ReadLine();
            var client = clienti.FirstOrDefault(c => c.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase));
            if (client != null)
            {
                Console.WriteLine(client);
            }
            else
            {
                Console.WriteLine("Clientul nu a fost gasit.");
            }
        }

        static void AdaugaClient(List<Client> clienti, AdministrareClientiFisier adminClienti, AdministrareProgramariFisier adminProgramari, List<Programare> programari)
        {
            Console.Write("\nID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID invalid!");
                return;
            }

            Console.Write("Nume: ");
            string nume = Console.ReadLine();
            Console.Write("Telefon: ");
            string telefon = Console.ReadLine();
            Console.Write("Adresa: ");
            string adresa = Console.ReadLine();

            Client clientNou = new Client(id, nume, telefon, adresa);
            clienti.Add(clientNou);
            adminClienti.AdaugaClient(clientNou);
            Console.WriteLine("Client adaugat cu succes!");

            Console.Write("Doriti sa adaugati o programare? (da/nu): ");
            string raspuns = Console.ReadLine().ToLower();
            if (raspuns == "da")
            {
                AdaugaProgramare(clientNou, adminProgramari, programari);
            }
        }


        static void AdaugaProgramare(Client client, AdministrareProgramariFisier adminProgramari, List<Programare> programari)
        {
            Console.Write("Data si ora programarii (yyyy-MM-dd HH:mm): ");
            DateTime dataOra;
            while (!DateTime.TryParse(Console.ReadLine(), out dataOra))
            {
                Console.Write("Format invalid! Introduceti din nou: ");
            }

            Console.WriteLine("Selectati serviciul:");
            foreach (Procedura procedura in Enum.GetValues(typeof(Procedura)))
            {
                Console.WriteLine($"{(int)procedura}. {procedura}");
            }
            Console.Write("Optiune: ");
            int optiune;
            while (!int.TryParse(Console.ReadLine(), out optiune) || !Enum.IsDefined(typeof(Procedura), optiune))
            {
                Console.Write("Optiune invalida! Reintroduceti: ");
            }
            Procedura serviciu = (Procedura)optiune;

            Console.Write("Pret: ");
            decimal pret = decimal.Parse(Console.ReadLine());

            Console.Write("Avans: ");
            string avans = Console.ReadLine();

            Programare programareNoua = new Programare(programari.Count + 1, client, dataOra, serviciu.ToString(), pret, avans);
            adminProgramari.AdaugaProgramare(programareNoua);
            Console.WriteLine("Programare adaugata cu succes!");
        }
        static void ModificaClient(List<Client> clienti, AdministrareClientiFisier adminClienti)
        {
            Console.Write("Introduceti ID-ul clientului de modificat: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID invalid!");
                return;
            }

            var client = clienti.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                Console.WriteLine("Clientul nu a fost gasit.");
                return;
            }

            Console.Write("Nume nou: ");
            client.Nume = Console.ReadLine();
            Console.Write("Telefon nou: ");
            client.Telefon = Console.ReadLine();
            Console.Write("Adresa noua: ");
            client.Adresa = Console.ReadLine();

            adminClienti.AdaugaClient(client);
            Console.WriteLine("Client modificat cu succes!");
        }
        static void CautaClientDupaProgramare(List<Programare> programari)
        {
            Console.Write("Introduceti ID-ul programarii: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID invalid!");
                return;
            }

            var programare = programari.FirstOrDefault(p => p.Id == id);
            if (programare != null)
            {
                Console.WriteLine(programare.Client);
            }
            else
            {
                Console.WriteLine("Programarea nu a fost gasita.");
            }
        }
       
    }
}


