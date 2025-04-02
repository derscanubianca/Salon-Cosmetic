using System;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Setări generale pentru formă
            this.Text = "Adăugare Client Nou";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.LightBlue;

            // Schimbarea fontului pentru toate controalele
            this.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inițializare dacă este necesar la încărcare
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
