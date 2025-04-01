using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Salon_Cosmetic
{
    public class AdministareClienti
    {
        private List<Client> clienti;
        private AdministrareClientiFisier adminFisier;

        // Constructor must have the same name as the class
        public AdministareClienti(AdministrareClientiFisier adminFisier)
        {
            this.adminFisier = adminFisier;
            clienti = adminFisier.CitesteDinFisier();
        }

        public void AdaugaClient(Client client)
        {
            clienti.Add(client);
            adminFisier.AdaugaClient(client);
        }

        public List<Client> GetClienti()
        {
            return new List<Client>(clienti);
        }

        public Client CautaClient(string nume)
        {
            return clienti.Find(c => c.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase));
        }
    }
}
