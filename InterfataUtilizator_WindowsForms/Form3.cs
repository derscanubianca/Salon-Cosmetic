using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Salon_Cosmetic;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Form3 : Form
    {
        private AdministrareProgramariFisier adminProgramari;
        private AdministrareClientiFisier adminClienti;

        private TextBox txtId, txtDataOra, txtServiciu, txtPret, txtAvans, txtCautareData;
        private ComboBox cmbClienti;
        private Label lblId, lblDataOra, lblClient, lblServiciu, lblPret, lblAvans;
        private ListBox listProgramari;
        private Button btnSterge, btnNext, btnAfiseaza, btnAdauga;

        private List<Programare> programari;
        private List<Client> clienti;

        private int currentPage = 0;
        private int programariPerPage = 10;

        private Size initialSize;
        private bool isExpanded = false;

        public Form3()
        {
            InitializeComponent();
            this.Text = "Salon Cosmetic - Gestionare Programări";
            this.Size = new Size(600, 700);
            this.initialSize = this.Size;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(230, 216, 242);
            this.Click += Form_Click;

            string caleClienti = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Clienti.txt");
            string caleProgramari = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Programari.txt");

            if (!File.Exists(caleClienti))
            {
                MessageBox.Show($"Fișierul {caleClienti} nu există! Va fi creat unul nou.", "Avertisment",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                File.Create(caleClienti).Close();
            }

            if (!File.Exists(caleProgramari))
            {
                MessageBox.Show($"Fișierul {caleProgramari} nu există! Va fi creat unul nou.", "Avertisment",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                File.Create(caleProgramari).Close();
            }

            adminClienti = new AdministrareClientiFisier(caleClienti);
            adminProgramari = new AdministrareProgramariFisier(caleProgramari);

            InitializeUI();
            this.Load += Form3_Load;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadClients();
            LoadProgramari();
        }

        private void LoadClients()
        {
            clienti = adminClienti.CitesteDinFisier();

            if (cmbClienti == null)
            {
                MessageBox.Show("ComboBox-ul pentru clienți nu a fost inițializat!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cmbClienti.Items.Clear();
            foreach (var client in clienti)
                cmbClienti.Items.Add(client);

            cmbClienti.DisplayMember = "Nume";
            if (cmbClienti.Items.Count > 0)
                cmbClienti.SelectedIndex = 0;
        }

        private void Txt_GotFocus(object sender, EventArgs e) => ((TextBox)sender).BackColor = Color.Yellow;
        private void Txt_LostFocus(object sender, EventArgs e) => ((TextBox)sender).BackColor = Color.FromArgb(248, 216, 230);

        private void BtnAfiseaza_Click(object sender, EventArgs e)
        {
            if (isExpanded) return;

            int newWidth = initialSize.Width * 2;
            int maxScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            this.Size = new Size(Math.Min(newWidth, maxScreenWidth), this.Size.Height);
            isExpanded = true;

            CenterToScreen();
            listProgramari.Visible = true;
            currentPage = 0;
            LoadProgramari();
        }

        private void Form_Click(object sender, EventArgs e)
        {
            if (!isExpanded) return;

            this.Size = initialSize;
            isExpanded = false;
            listProgramari.Visible = false;
            btnNext.Visible = false;
            currentPage = 0;

            CenterToScreen();
        }

        private void TxtCautareData_TextChanged(object sender, EventArgs e)
        {
            currentPage = 0;
            LoadProgramari(txtCautareData.Text);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadProgramari(txtCautareData.Text);
        }

        private void BtnSterge_Click(object sender, EventArgs e)
        {
            if (listProgramari.SelectedIndex < 0)
            {
                MessageBox.Show("Selectați o programare pentru a o șterge!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show("Sunteți sigur că doriți să ștergeți această programare?", "Confirmare", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No) return;

            var programare = programari[listProgramari.SelectedIndex];
            programari.RemoveAt(listProgramari.SelectedIndex);

            string caleFisier = (string)adminProgramari
                .GetType()
                .GetField("caleFisier", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(adminProgramari);

            using (StreamWriter sw = new StreamWriter(caleFisier, false))
            {
                foreach (var p in programari)
                    sw.WriteLine($"{p.Id},{p.Client.Id},{p.DataOra:yyyy-MM-dd HH:mm},{p.Serviciu},{p.Pret},{p.Avans}");
            }

            currentPage = 0;
            LoadProgramari();
            MessageBox.Show("Programare ștearsă cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnAdauga_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) ||
                string.IsNullOrWhiteSpace(txtDataOra.Text) ||
                string.IsNullOrWhiteSpace(txtServiciu.Text) ||
                string.IsNullOrWhiteSpace(txtPret.Text) ||
                string.IsNullOrWhiteSpace(txtAvans.Text) ||
                cmbClienti.SelectedItem == null)
            {
                MessageBox.Show("Toate câmpurile sunt obligatorii!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("ID-ul trebuie să fie un număr!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!DateTime.TryParseExact(txtDataOra.Text, "yyyy-MM-dd HH:mm", null,
                System.Globalization.DateTimeStyles.None, out DateTime dataOra))
            {
                MessageBox.Show("DataOra trebuie să fie în format yyyy-MM-dd HH:mm!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtPret.Text, out decimal pret))
            {
                MessageBox.Show("Prețul trebuie să fie un număr decimal!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var existing = adminProgramari.CitesteProgramari(new List<Client>());
            if (existing.Any(p => p.Id == id))
            {
                MessageBox.Show("ID-ul există deja!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Client client = (Client)cmbClienti.SelectedItem;
            Programare noua = new Programare(id, client, dataOra, txtServiciu.Text, pret, txtAvans.Text);
            adminProgramari.AdaugaProgramare(noua);

            txtId.Clear();
            txtDataOra.Clear();
            txtServiciu.Clear();
            txtPret.Clear();
            txtAvans.Clear();

            currentPage = 0;
            LoadProgramari();

            MessageBox.Show("Programare adăugată cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadProgramari(string filterData = "")
        {
            programari = adminProgramari.CitesteProgramari(clienti);
            programari.Sort((x, y) => x.DataOra.CompareTo(y.DataOra));

            listProgramari.Items.Clear();

            int maxScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            programariPerPage = (listProgramari.Height - 20) / 20;

            int requiredHeight = programari.Count * 20 + 40;
            if (requiredHeight > maxScreenHeight)
            {
                listProgramari.Height = maxScreenHeight / 2;
                btnNext.Visible = true;
            }
            else
            {
                listProgramari.Height = requiredHeight;
                btnNext.Visible = false;
                currentPage = 0;
            }

            int startIndex = currentPage * programariPerPage;
            int endIndex = Math.Min(startIndex + programariPerPage, programari.Count);

            Console.WriteLine("--- Programări ---");
            if (programari.Count == 0)
            {
                Console.WriteLine("Nu există programări.");
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                var programare = programari[i];
                string programareString = $"ID: {programare.Id}, Data: {programare.DataOra:yyyy-MM-dd HH:mm}, Client: {programare.Client.Nume}, Serviciu: {programare.Serviciu}, Preț: {programare.Pret}, Avans: {programare.Avans}";
                if (string.IsNullOrEmpty(filterData) || programare.DataOra.ToString("yyyy-MM-dd").Contains(filterData))
                {
                    listProgramari.Items.Add(programareString);
                    Console.WriteLine(programareString); // Afișăm în consolă
                }
            }

            btnNext.Visible = endIndex < programari.Count;
            btnSterge.Enabled = listProgramari.Items.Count > 0;
        }

        private void InitializeUI()
        {
            // Labels
            lblId = new Label { Text = "ID:", Location = new Point(30, 30) };
            lblDataOra = new Label { Text = "Data și Ora:", Location = new Point(30, 70) };
            lblClient = new Label { Text = "Client:", Location = new Point(30, 110) };
            lblServiciu = new Label { Text = "Serviciu:", Location = new Point(30, 150) };
            lblPret = new Label { Text = "Preț:", Location = new Point(30, 190) };
            lblAvans = new Label { Text = "Avans:", Location = new Point(30, 230) };

            // Textboxes
            txtId = new TextBox { Location = new Point(150, 30), Width = 200 };
            txtDataOra = new TextBox { Location = new Point(150, 70), Width = 200 };
            cmbClienti = new ComboBox { Location = new Point(150, 110), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            txtServiciu = new TextBox { Location = new Point(150, 150), Width = 200 };
            txtPret = new TextBox { Location = new Point(150, 190), Width = 200 };
            txtAvans = new TextBox { Location = new Point(150, 230), Width = 200 };
            txtCautareData = new TextBox { Location = new Point(30, 270), Width = 200 };
            txtCautareData.TextChanged += TxtCautareData_TextChanged;

            // ListBox
            listProgramari = new ListBox { Location = new Point(30, 310), Size = new Size(520, 200), Visible = false };

            // Buttons
            btnAfiseaza = new Button { Text = "Afișează Programări", Location = new Point(250, 270), Width = 150 };
            btnAfiseaza.Click += BtnAfiseaza_Click;

            btnAdauga = new Button { Text = "Adaugă", Location = new Point(30, 530), Width = 100 };
            btnAdauga.Click += BtnAdauga_Click;

            btnSterge = new Button { Text = "Șterge", Location = new Point(140, 530), Width = 100 };
            btnSterge.Click += BtnSterge_Click;

            btnNext = new Button { Text = "Mai Mult", Location = new Point(250, 530), Width = 100, Visible = false };
            btnNext.Click += BtnNext_Click;

            
            this.Controls.AddRange(new Control[] {
                lblId, lblDataOra, lblClient, lblServiciu, lblPret, lblAvans,
                txtId, txtDataOra, cmbClienti, txtServiciu, txtPret, txtAvans, txtCautareData,
                listProgramari, btnAfiseaza, btnAdauga, btnSterge, btnNext
            });

            
            foreach (var control in new[] { txtId, txtDataOra, txtServiciu, txtPret, txtAvans })
            {
                control.GotFocus += Txt_GotFocus;
                control.LostFocus += Txt_LostFocus;
            }
        }
    }
}
