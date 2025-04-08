using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Salon_Cosmetic;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Form1 : Form
    {
        private AdministrareClientiFisier adminClienti;
        private TextBox txtId, txtNume, txtTelefon, txtAdresa, txtCautare;
        private Label lblId, lblNume, lblTelefon, lblAdresa;
        private Label lblErrorId, lblErrorNume, lblErrorTelefon, lblErrorAdresa;
        private ListBox listClienti, listIstoric;
        private Button btnSterge, btnEditeaza, btnSalveaza, btnSort;
        private List<Client> clienti;
        private bool isEditing = false;
        private int editIndex = -1;
        private bool sortByName = true; // Implicit sortăm după nume

        // Constante pentru validare
        private const int MAX_NUME_LENGTH = 20;
        private const int MAX_ADRESA_LENGTH = 50;
        private const string TELEFON_PATTERN = @"^\d{10}$"; // 10 cifre

        public Form1()
        {
            InitializeComponent();

            // Setări generale pentru formă
            this.Text = "Salon Cosmetic - Gestionare Clienți";
            this.Size = new Size(900, 700); // Mărime ajustată pentru a acomoda istoricul mai mare
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(230, 216, 242); // Mov pastel

            // Inițializare cale fișier
            string caleFisier = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                "Clienti.txt");
            adminClienti = new AdministrareClientiFisier(caleFisier);

            // Verificăm dacă fișierul există
            if (!File.Exists(caleFisier))
            {
                MessageBox.Show($"Fișierul {caleFisier} nu există! Va fi creat unul nou.", "Avertisment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                File.Create(caleFisier).Close();
            }

            InitializeUI();
            LoadClients();
        }

        private void InitializeUI()
        {
            // Titlu
            Label lblTitlu = new Label();
            lblTitlu.Text = "Lista Clienți";
            lblTitlu.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitlu.ForeColor = Color.FromArgb(139, 87, 165);
            lblTitlu.AutoSize = true;
            lblTitlu.Top = 20;
            lblTitlu.Left = 20;
            this.Controls.Add(lblTitlu);

            // Căutare
            Label lblCautare = new Label();
            lblCautare.Text = "Caută după nume:";
            lblCautare.ForeColor = Color.FromArgb(64, 64, 64);
            lblCautare.AutoSize = true;
            lblCautare.Top = 60;
            lblCautare.Left = 20;
            this.Controls.Add(lblCautare);

            txtCautare = new TextBox();
            txtCautare.BackColor = Color.FromArgb(248, 216, 230);
            txtCautare.ForeColor = Color.Black;
            txtCautare.Top = 60;
            txtCautare.Left = 150;
            txtCautare.Width = 400;
            txtCautare.TextChanged += TxtCautare_TextChanged;
            this.Controls.Add(txtCautare);

            // ListBox pentru afișare clienți
            listClienti = new ListBox();
            listClienti.Top = 100;
            listClienti.Left = 20;
            listClienti.Width = 540;
            listClienti.Height = 150;
            listClienti.BackColor = Color.FromArgb(248, 216, 230);
            listClienti.ForeColor = Color.FromArgb(64, 64, 64);
            listClienti.Font = new Font("Segoe UI", 10);
            this.Controls.Add(listClienti);

            // Buton „Sortare”
            btnSort = new Button();
            btnSort.Text = "Sortare Nume";
            btnSort.BackColor = Color.FromArgb(248, 216, 230);
            btnSort.ForeColor = Color.FromArgb(139, 87, 165);
            btnSort.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSort.Top = 260;
            btnSort.Left = 70;
            btnSort.Width = 120;
            btnSort.Click += BtnSort_Click;
            this.Controls.Add(btnSort);

            // Buton „Editează”
            btnEditeaza = new Button();
            btnEditeaza.Text = "Editează";
            btnEditeaza.BackColor = Color.FromArgb(248, 216, 230);
            btnEditeaza.ForeColor = Color.FromArgb(139, 87, 165);
            btnEditeaza.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnEditeaza.Top = 260;
            btnEditeaza.Left = 200;
            btnEditeaza.Width = 100;
            btnEditeaza.Enabled = false;
            btnEditeaza.Click += BtnEditeaza_Click;
            this.Controls.Add(btnEditeaza);

            // Buton „Șterge”
            btnSterge = new Button();
            btnSterge.Text = "Șterge";
            btnSterge.BackColor = Color.FromArgb(248, 216, 230);
            btnSterge.ForeColor = Color.FromArgb(139, 87, 165);
            btnSterge.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSterge.Top = 260;
            btnSterge.Left = 310;
            btnSterge.Width = 100;
            btnSterge.Enabled = false;
            btnSterge.Click += BtnSterge_Click;
            this.Controls.Add(btnSterge);

            // Secțiunea „Istoric Acțiuni”
            Label lblIstoric = new Label();
            lblIstoric.Text = "Istoric Acțiuni";
            lblIstoric.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblIstoric.ForeColor = Color.FromArgb(139, 87, 165);
            lblIstoric.AutoSize = true;
            lblIstoric.Top = 20;
            lblIstoric.Left = 580;
            this.Controls.Add(lblIstoric);

            listIstoric = new ListBox();
            listIstoric.Top = 50;
            listIstoric.Left = 580;
            listIstoric.Width = 300; // Lățime mărită pentru a afișa liniile complet
            listIstoric.Height = 240;
            listIstoric.BackColor = Color.FromArgb(248, 216, 230);
            listIstoric.ForeColor = Color.FromArgb(64, 64, 64);
            listIstoric.Font = new Font("Segoe UI", 10);
            this.Controls.Add(listIstoric);

            // Secțiunea „Adaugă/Editează Client”
            Label lblAdauga = new Label();
            lblAdauga.Text = "Adaugă/Editează Client";
            lblAdauga.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblAdauga.ForeColor = Color.FromArgb(139, 87, 165);
            lblAdauga.AutoSize = true;
            lblAdauga.Top = 310;
            lblAdauga.Left = 20;
            this.Controls.Add(lblAdauga);

            // Etichete și TextBox-uri
            string[] labels = { "ID:", "Nume:", "Telefon:", "Adresa:" };
            for (int i = 0; i < 4; i++)
            {
                Label lbl = new Label();
                lbl.Text = labels[i];
                lbl.ForeColor = Color.FromArgb(64, 64, 64);
                lbl.Top = 340 + i * 40;
                lbl.Left = 20;
                lbl.AutoSize = true;
                this.Controls.Add(lbl);

                TextBox txt = new TextBox();
                txt.BackColor = Color.FromArgb(248, 216, 230);
                txt.ForeColor = Color.Black;
                txt.Top = 340 + i * 40;
                txt.Left = 100;
                txt.Width = 300;
                this.Controls.Add(txt);

                Label lblError = new Label();
                lblError.ForeColor = Color.Red;
                lblError.AutoSize = true;
                lblError.Top = 340 + i * 40;
                lblError.Left = 410;
                this.Controls.Add(lblError);

                if (i == 0)
                {
                    lblId = lbl;
                    txtId = txt;
                    lblErrorId = lblError;
                }
                else if (i == 1)
                {
                    lblNume = lbl;
                    txtNume = txt;
                    lblErrorNume = lblError;
                }
                else if (i == 2)
                {
                    lblTelefon = lbl;
                    txtTelefon = txt;
                    lblErrorTelefon = lblError;
                }
                else
                {
                    lblAdresa = lbl;
                    txtAdresa = txt;
                    lblErrorAdresa = lblError;
                }
            }

            // Buton „Adaugă”
            Button btnAdauga = new Button();
            btnAdauga.Text = "Adaugă";
            btnAdauga.BackColor = Color.FromArgb(248, 216, 230);
            btnAdauga.ForeColor = Color.FromArgb(139, 87, 165);
            btnAdauga.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAdauga.Top = 520;
            btnAdauga.Left = 190;
            btnAdauga.Width = 100;
            btnAdauga.Click += BtnAdauga_Click;
            this.Controls.Add(btnAdauga);

            // Buton „Salvează” (pentru editare)
            btnSalveaza = new Button();
            btnSalveaza.Text = "Salvează";
            btnSalveaza.BackColor = Color.FromArgb(248, 216, 230);
            btnSalveaza.ForeColor = Color.FromArgb(139, 87, 165);
            btnSalveaza.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSalveaza.Top = 520;
            btnSalveaza.Left = 310;
            btnSalveaza.Width = 100;
            btnSalveaza.Enabled = false;
            btnSalveaza.Click += BtnSalveaza_Click;
            this.Controls.Add(btnSalveaza);
        }

        private void LoadClients(string filter = "")
        {
            clienti = adminClienti.CitesteDinFisier();
            if (sortByName)
            {
                clienti.Sort((x, y) => x.Nume.CompareTo(y.Nume));
            }
            else
            {
                clienti.Sort((x, y) => x.Id.CompareTo(y.Id));
            }

            listClienti.Items.Clear();
            foreach (var client in clienti)
            {
                string clientString = $"ID: {client.Id}, Nume: {client.Nume}, Telefon: {client.Telefon}, Adresa: {client.Adresa}";
                if (string.IsNullOrEmpty(filter) || client.Nume.ToLower().Contains(filter.ToLower()))
                {
                    listClienti.Items.Add(clientString);
                }
            }

            // Activăm butoanele „Editează” și „Șterge” doar dacă există clienți
            btnEditeaza.Enabled = listClienti.Items.Count > 0;
            btnSterge.Enabled = listClienti.Items.Count > 0;
        }

        private void TxtCautare_TextChanged(object sender, EventArgs e)
        {
            LoadClients(txtCautare.Text);
        }

        private void BtnSort_Click(object sender, EventArgs e)
        {
            sortByName = !sortByName;
            btnSort.Text = sortByName ? "Sortare Nume" : "Sortare ID";
            LoadClients(txtCautare.Text);
            AddToHistory($"Sortare efectuată: {btnSort.Text}");
        }

        private void BtnAdauga_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                MessageBox.Show("Sunteți în modul de editare! Salvați sau anulați modificările.", "Avertisment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int errorCode = ValidateInput();
            if (errorCode == 0)
            {
                int id = int.Parse(txtId.Text);
                string nume = txtNume.Text;
                string telefon = txtTelefon.Text;
                string adresa = txtAdresa.Text;

                Client clientNou = new Client(id, nume, telefon, adresa);
                adminClienti.AdaugaClient(clientNou);

                // Curățăm câmpurile
                ResetForm();

                // Reîncărcăm lista
                LoadClients();

                AddToHistory($"Client adăugat: ID {id}, {nume}");
                MessageBox.Show("Client adăugat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnEditeaza_Click(object sender, EventArgs e)
        {
            if (listClienti.SelectedIndex < 0)
            {
                MessageBox.Show("Selectați un client pentru a-l edita!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            isEditing = true;
            editIndex = listClienti.SelectedIndex;
            var client = clienti[listClienti.SelectedIndex];

            txtId.Text = client.Id.ToString();
            txtId.Enabled = false; // Dezactivăm ID-ul în modul de editare
            txtNume.Text = client.Nume;
            txtTelefon.Text = client.Telefon;
            txtAdresa.Text = client.Adresa;

            btnSalveaza.Enabled = true;
        }

        private void BtnSalveaza_Click(object sender, EventArgs e)
        {
            int errorCode = ValidateInput(true); // Ignorăm validarea ID-ului unic
            if (errorCode == 0)
            {
                var client = clienti[editIndex];
                client.Nume = txtNume.Text;
                client.Telefon = txtTelefon.Text;
                client.Adresa = txtAdresa.Text;

                // Rescriem fișierul
                string caleFisier = (string)adminClienti.GetType().GetField("caleFisier", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(adminClienti);
                using (StreamWriter sw = new StreamWriter(caleFisier, false))
                {
                    foreach (var c in clienti)
                    {
                        sw.WriteLine($"{c.Id},{c.Nume},{c.Telefon},{c.Adresa}");
                    }
                }

                AddToHistory($"Client actualizat: ID {client.Id}, {client.Nume}");
                ResetForm();
                LoadClients();
                MessageBox.Show("Client actualizat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSterge_Click(object sender, EventArgs e)
        {
            if (listClienti.SelectedIndex < 0)
            {
                MessageBox.Show("Selectați un client pentru a-l șterge!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirmare ștergere
            var result = MessageBox.Show("Sunteți sigur că doriți să ștergeți acest client?", "Confirmare Ștergere", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            var client = clienti[listClienti.SelectedIndex];
            clienti.RemoveAt(listClienti.SelectedIndex);

            // Rescriem fișierul
            string caleFisier = (string)adminClienti.GetType().GetField("caleFisier", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(adminClienti);
            using (StreamWriter sw = new StreamWriter(caleFisier, false))
            {
                foreach (var c in clienti)
                {
                    sw.WriteLine($"{c.Id},{c.Nume},{c.Telefon},{c.Adresa}");
                }
            }

            AddToHistory($"Client șters: ID {client.Id}, {client.Nume}");
            ResetForm();
            LoadClients();
            MessageBox.Show("Client șters cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddToHistory(string action)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            listIstoric.Items.Add($"[{timestamp}] {action}");
            listIstoric.TopIndex = listIstoric.Items.Count - 1; // Derulăm la ultima acțiune
        }

        private int ValidateInput(bool skipIdUniqueness = false)
        {
            int errorCode = 0;
            ResetValidation();

            // Validare ID
            if (!int.TryParse(txtId.Text, out int id))
            {
                errorCode |= 1;
                lblId.ForeColor = Color.Red;
                lblErrorId.Text = "ID invalid!";
            }
            else if (!skipIdUniqueness)
            {
                var clienti = adminClienti.CitesteDinFisier();
                if (clienti.Any(c => c.Id == id))
                {
                    errorCode |= 2;
                    lblId.ForeColor = Color.Red;
                    lblErrorId.Text = "ID-ul există deja!";
                }
            }

            // Validare Nume
            if (string.IsNullOrWhiteSpace(txtNume.Text))
            {
                errorCode |= 4;
                lblNume.ForeColor = Color.Red;
                lblErrorNume.Text = "Numele este obligatoriu!";
            }
            else if (txtNume.Text.Length > MAX_NUME_LENGTH)
            {
                errorCode |= 8;
                lblNume.ForeColor = Color.Red;
                lblErrorNume.Text = $"Numele nu poate depăși {MAX_NUME_LENGTH} caractere!";
            }

            // Validare Telefon
            if (string.IsNullOrWhiteSpace(txtTelefon.Text))
            {
                errorCode |= 16;
                lblTelefon.ForeColor = Color.Red;
                lblErrorTelefon.Text = "Telefonul este obligatoriu!";
            }
            else if (!Regex.IsMatch(txtTelefon.Text, TELEFON_PATTERN))
            {
                errorCode |= 32;
                lblTelefon.ForeColor = Color.Red;
                lblErrorTelefon.Text = "Telefonul trebuie să aibă 10 cifre!";
            }

            // Validare Adresa
            if (string.IsNullOrWhiteSpace(txtAdresa.Text))
            {
                errorCode |= 64;
                lblAdresa.ForeColor = Color.Red;
                lblErrorAdresa.Text = "Adresa este obligatorie!";
            }
            else if (txtAdresa.Text.Length > MAX_ADRESA_LENGTH)
            {
                errorCode |= 128;
                lblAdresa.ForeColor = Color.Red;
                lblErrorAdresa.Text = $"Adresa nu poate depăși {MAX_ADRESA_LENGTH} caractere!";
            }

            return errorCode;
        }

        private void ResetValidation()
        {
            lblId.ForeColor = Color.FromArgb(64, 64, 64);
            lblNume.ForeColor = Color.FromArgb(64, 64, 64);
            lblTelefon.ForeColor = Color.FromArgb(64, 64, 64);
            lblAdresa.ForeColor = Color.FromArgb(64, 64, 64);
            lblErrorId.Text = "";
            lblErrorNume.Text = "";
            lblErrorTelefon.Text = "";
            lblErrorAdresa.Text = "";
        }

        private void ResetForm()
        {
            txtId.Clear();
            txtNume.Clear();
            txtTelefon.Clear();
            txtAdresa.Clear();
            txtId.Enabled = true;
            isEditing = false;
            editIndex = -1;
            btnSalveaza.Enabled = false;
            ResetValidation();
        }
    }
}