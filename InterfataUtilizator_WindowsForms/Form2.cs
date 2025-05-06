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
        private Button btnAdauga;

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
            this.Load += Form2_Load;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // No additional loading required for now
        }

        private void InitializeUI()
        {
            // Labels
            lblId = new Label { Text = "ID:", Location = new Point(30, 30), AutoSize = true };
            lblNume = new Label { Text = "Nume:", Location = new Point(30, 70), AutoSize = true };
            lblTelefon = new Label { Text = "Telefon:", Location = new Point(30, 110), AutoSize = true };
            lblAdresa = new Label { Text = "Adresa:", Location = new Point(30, 150), AutoSize = true };
            lblPlata = new Label { Text = "Modalitate de plată:", Location = new Point(30, 190), AutoSize = true };
            lblServicii = new Label { Text = "Servicii dorite:", Location = new Point(30, 250), AutoSize = true };

            // Textboxes
            txtId = new TextBox { Location = new Point(150, 30), Width = 200 };
            txtNume = new TextBox { Location = new Point(150, 70), Width = 200 };
            txtTelefon = new TextBox { Location = new Point(150, 110), Width = 200 };
            txtAdresa = new TextBox { Location = new Point(150, 150), Width = 200 };

            // Radio Buttons for Payment Method
            rbCard = new RadioButton { Text = "Card", Location = new Point(150, 190), AutoSize = true, Checked = true };
            rbNumerar = new RadioButton { Text = "Numerar", Location = new Point(250, 190), AutoSize = true };

            // Checkboxes for Services
            chkCoafor = new CheckBox { Text = "Coafor", Location = new Point(150, 250), AutoSize = true };
            chkManichiura = new CheckBox { Text = "Manichiura", Location = new Point(150, 280), AutoSize = true };
            chkPedichiura = new CheckBox { Text = "Pedichiura", Location = new Point(150, 310), AutoSize = true };
            chkMachiaj = new CheckBox { Text = "Machiaj", Location = new Point(150, 340), AutoSize = true };
            chkEpilare = new CheckBox { Text = "Epilare", Location = new Point(150, 370), AutoSize = true };
            chkMasaj = new CheckBox { Text = "Masaj", Location = new Point(150, 400), AutoSize = true };

            // Button
            btnAdauga = new Button { Text = "Adaugă Client", Location = new Point(150, 450), Width = 100 };
            btnAdauga.Click += BtnAdauga_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblId, lblNume, lblTelefon, lblAdresa, lblPlata, lblServicii,
                txtId, txtNume, txtTelefon, txtAdresa,
                rbCard, rbNumerar,
                chkCoafor, chkManichiura, chkPedichiura, chkMachiaj, chkEpilare, chkMasaj,
                btnAdauga
            });

            // Add focus effects to textboxes
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

            // Check if ID already exists
            var existingClients = adminClienti.CitesteDinFisier();
            if (existingClients.Any(c => c.Id == id))
            {
                MessageBox.Show("ID-ul există deja!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get payment method
            string metodaPlata = rbCard.Checked ? "Card" : "Numerar";

            // Get selected services
            List<string> servicii = new List<string>();
            if (chkCoafor.Checked) servicii.Add("Coafor");
            if (chkManichiura.Checked) servicii.Add("Manichiura");
            if (chkPedichiura.Checked) servicii.Add("Pedichiura");
            if (chkMachiaj.Checked) servicii.Add("Machiaj");
            if (chkEpilare.Checked) servicii.Add("Epilare");
            if (chkMasaj.Checked) servicii.Add("Masaj");

            // Create new client
            Client clientNou = new Client(id, txtNume.Text, txtTelefon.Text, txtAdresa.Text);
            adminClienti.AdaugaClient(clientNou);

            // Save additional client info (payment method and services) to a separate file
            string caleClientiExtra = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "ClientiExtra.txt");
            using (StreamWriter sw = new StreamWriter(caleClientiExtra, true))
            {
                sw.WriteLine($"{id},{metodaPlata},{string.Join(";", servicii)}");
            }

            // Clear inputs
            txtId.Clear();
            txtNume.Clear();
            txtTelefon.Clear();
            txtAdresa.Clear();
            rbCard.Checked = true;
            rbNumerar.Checked = false;
            chkCoafor.Checked = false;
            chkManichiura.Checked = false;
            chkPedichiura.Checked = false;
            chkMachiaj.Checked = false;
            chkEpilare.Checked = false;
            chkMasaj.Checked = false;

            MessageBox.Show("Client adăugat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
