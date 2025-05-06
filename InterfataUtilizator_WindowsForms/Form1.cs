using System;
using System.Drawing;
using System.Windows.Forms;
using Salon_Cosmetic;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Setări generale pentru formă
            this.Text = "Salon Cosmetic - Meniu Principal";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(230, 216, 242); // Mov pastel

            InitializeUI();
        }

        private void InitializeUI()
        {
            // Titlu
            Label lblTitlu = new Label();
            lblTitlu.Text = "Bun venit la Salon Cosmetic!";
            lblTitlu.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitlu.ForeColor = Color.FromArgb(139, 87, 165);
            lblTitlu.AutoSize = true;
            lblTitlu.Top = 20;
            lblTitlu.Left = 50;
            this.Controls.Add(lblTitlu);

            // Buton „Clienți”
            Button btnClienti = new Button();
            btnClienti.Text = "Gestionare Clienți";
            btnClienti.BackColor = Color.FromArgb(248, 216, 230);
            btnClienti.ForeColor = Color.FromArgb(139, 87, 165);
            btnClienti.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnClienti.Top = 100;
            btnClienti.Left = 120;
            btnClienti.Width = 150;
            btnClienti.Click += (s, e) => { new Form2().Show(); }; // Apelarea unui nou Form2
            this.Controls.Add(btnClienti);

            // Buton „Programări”
            Button btnProgramari = new Button();
            btnProgramari.Text = "Gestionare Programări";
            btnProgramari.BackColor = Color.FromArgb(248, 216, 230);
            btnProgramari.ForeColor = Color.FromArgb(139, 87, 165);
            btnProgramari.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnProgramari.Top = 150;
            btnProgramari.Left = 120;
            btnProgramari.Width = 150;
            btnProgramari.Click += (s, e) => { new Form3().Show(); }; // Apelarea unui nou Form3
            this.Controls.Add(btnProgramari);
        }
    }
}
