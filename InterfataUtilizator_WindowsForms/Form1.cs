using System;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Inițializare proprietăți pentru formă
            this.Text = "Salon Cosmetic"; // Titlul ferestrei
            this.Size = new System.Drawing.Size(400, 300); // Dimensiunea ferestrei
            this.StartPosition = FormStartPosition.CenterScreen; // Centrare pe ecran
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Tip de fereastră

            // Schimbarea culorii de fundal
            this.BackColor = System.Drawing.Color.LightPink;

            // Schimbarea fontului pentru toate controalele
            this.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Se execută la încărcarea formei (dacă ai nevoie de inițializări suplimentare)
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
