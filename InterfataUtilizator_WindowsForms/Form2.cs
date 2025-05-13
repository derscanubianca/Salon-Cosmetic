using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Salon_Cosmetic;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Form2 : Form
    {
        private AdministrareClientiFisier adminClienti;
        private TextBox txtId, txtNume, txtTelefon, txtAdresa;
        private Label lblId, lblNume, lblTelefon, lblAdresa, lblPlata, lblServicii;
        private RadioButton rbCard, rbNumerar;
        private CheckBox chkCoafor, chkManichiura, chkPedichiura, chkMachiaj, chkEpilare, chkMasaj;
        private Button btnAdauga, btnActualizeaza;

        public Form2()
        {
            InitializeComponent();
            this.Text = "Salon Cosmetic - Gestionare Clienți";
            this.Size = new Size(400, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(230, 216, 242);

            string caleClienti = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Clienti.txt");

            if (!File.Exists(caleClienti))
            {
                MessageBox.Show($"Fișierul {caleClienti} nu există! Va fi creat unul nou.", "Avertisment",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                File.Create(caleClienti).Close();
            }

            adminClienti = new AdministrareClientiFisier(caleClienti);
            InitializeUI();
        }

        private void InitializeUI()
        {
            lblId = new Label { Text = "ID:", Location = new Point(30, 30), AutoSize = true };
            lblNume = new Label { Text = "Nume:", Location = new Point(30, 70), AutoSize = true };
            lblTelefon = new Label { Text = "Telefon:", Location = new Point(30, 110), AutoSize = true };
            lblAdresa = new Label { Text = "Adresa:", Location = new Point(30, 150), AutoSize = true };
            lblPlata = new Label { Text = "Modalitate de plată:", Location = new Point(30, 190), AutoSize = true };
            lblServicii = new Label { Text = "Servicii dorite:", Location = new Point(30, 250), AutoSize = true };

            txtId = new TextBox { Location = new Point(150, 30), Width = 200 };
            txtNume = new TextBox { Location = new Point(150, 70), Width = 200 };
            txtTelefon = new TextBox { Location = new Point(150, 110), Width = 200 };
            txtAdresa = new TextBox { Location = new Point(150, 150), Width = 200 };

            rbCard = new RadioButton { Text = "Card", Location = new Point(150, 190), AutoSize = true, Checked = true };
            rbNumerar = new RadioButton { Text = "Numerar", Location = new Point(250, 190), AutoSize = true };

            chkCoafor = new CheckBox { Text = "Coafor", Location = new Point(150, 250), AutoSize = true };
            chkManichiura = new CheckBox { Text = "Manichiura", Location = new Point(150, 280), AutoSize = true };
            chkPedichiura = new CheckBox { Text = "Pedichiura", Location = new Point(150, 310), AutoSize = true };
            chkMachiaj = new CheckBox { Text = "Machiaj", Location = new Point(150, 340), AutoSize = true };
            chkEpilare = new CheckBox { Text = "Epilare", Location = new Point(150, 370), AutoSize = true };
            chkMasaj = new CheckBox { Text = "Masaj", Location = new Point(150, 400), AutoSize = true };

            btnAdauga = new Button { Text = "Adaugă Client", Location = new Point(60, 450), Width = 120 };
            btnAdauga.Click += BtnAdauga_Click;

            btnActualizeaza = new Button { Text = "Actualizează Client", Location = new Point(200, 450), Width = 140 };
            btnActualizeaza.Click += BtnActualizeaza_Click;

            this.Controls.AddRange(new Control[] {
                lblId, lblNume, lblTelefon, lblAdresa, lblPlata, lblServicii,
                txtId, txtNume, txtTelefon, txtAdresa,
                rbCard, rbNumerar,
                chkCoafor, chkManichiura, chkPedichiura, chkMachiaj, chkEpilare, chkMasaj,
                btnAdauga, btnActualizeaza
            });

            foreach (var control in new[] { txtId, txtNume, txtTelefon, txtAdresa })
            {
                control.GotFocus += (s, e) => ((TextBox)s).BackColor = Color.Yellow;
                control.LostFocus += (s, e) => ((TextBox)s).BackColor = Color.FromArgb(248, 216, 230);
            }
        }

        private void BtnAdauga_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) ||
                string.IsNullOrWhiteSpace(txtNume.Text) ||
                string.IsNullOrWhiteSpace(txtTelefon.Text) ||
                string.IsNullOrWhiteSpace(txtAdresa.Text))
            {
                MessageBox.Show("Toate câmpurile sunt obligatorii!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("ID-ul trebuie să fie un număr!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var existingClients = adminClienti.CitesteDinFisier();
            if (existingClients.Any(c => c.Id == id))
            {
                MessageBox.Show("ID-ul există deja!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string metodaPlata = rbCard.Checked ? "Card" : "Numerar";
            List<string> servicii = new List<string>();
            if (chkCoafor.Checked) servicii.Add("Coafor");
            if (chkManichiura.Checked) servicii.Add("Manichiura");
            if (chkPedichiura.Checked) servicii.Add("Pedichiura");
            if (chkMachiaj.Checked) servicii.Add("Machiaj");
            if (chkEpilare.Checked) servicii.Add("Epilare");
            if (chkMasaj.Checked) servicii.Add("Masaj");

            Client clientNou = new Client(id, txtNume.Text, txtTelefon.Text, txtAdresa.Text);
            adminClienti.AdaugaClient(clientNou);

            string caleClientiExtra = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "ClientiExtra.txt");
            using (StreamWriter sw = new StreamWriter(caleClientiExtra, true))
            {
                sw.WriteLine($"{id},{metodaPlata},{string.Join(";", servicii)}");
            }

            ClearInputs();
            MessageBox.Show("Client adăugat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnActualizeaza_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("ID invalid!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var clienti = adminClienti.CitesteDinFisier();
            var index = clienti.FindIndex(c => c.Id == id);
            if (index == -1)
            {
                MessageBox.Show("Clientul nu există!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clienti[index].Nume = txtNume.Text;
            clienti[index].Telefon = txtTelefon.Text;
            clienti[index].Adresa = txtAdresa.Text;

            adminClienti.SalveazaInFisier(clienti);

            string caleClientiExtra = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "ClientiExtra.txt");

            string metodaPlata = rbCard.Checked ? "Card" : "Numerar";
            List<string> servicii = new List<string>();
            if (chkCoafor.Checked) servicii.Add("Coafor");
            if (chkManichiura.Checked) servicii.Add("Manichiura");
            if (chkPedichiura.Checked) servicii.Add("Pedichiura");
            if (chkMachiaj.Checked) servicii.Add("Machiaj");
            if (chkEpilare.Checked) servicii.Add("Epilare");
            if (chkMasaj.Checked) servicii.Add("Masaj");

            var linii = File.ReadAllLines(caleClientiExtra).ToList();
            bool gasit = false;

            for (int i = 0; i < linii.Count; i++)
            {
                if (linii[i].StartsWith($"{id},"))
                {
                    linii[i] = $"{id},{metodaPlata},{string.Join(";", servicii)}";
                    gasit = true;
                    break;
                }
            }

            if (!gasit)
            {
                linii.Add($"{id},{metodaPlata},{string.Join(";", servicii)}");
            }

            File.WriteAllLines(caleClientiExtra, linii);

            MessageBox.Show("Datele au fost actualizate!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ClearInputs()
        {
            txtId.Clear();
            txtNume.Clear();
            txtTelefon.Clear();
            txtAdresa.Clear();
            rbCard.Checked = true;
            chkCoafor.Checked = false;
            chkManichiura.Checked = false;
            chkPedichiura.Checked = false;
            chkMachiaj.Checked = false;
            chkEpilare.Checked = false;
            chkMasaj.Checked = false;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            
            
        }
    }
}
