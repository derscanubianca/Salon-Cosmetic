using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Salon_Cosmetic;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Form1 : Form
    {
        private Label lblTitlu;
        private Label lblNume;

        public Form1()
        {
            InitializeComponent();
            // Inițializare lblTitlu
            lblTitlu = new Label();
            lblTitlu.Text = "Salon Cosmetic";
            lblTitlu.Top = 20;
            lblTitlu.Left = 50;
            lblTitlu.AutoSize = true;

            // Inițializare lblNume
            lblNume = new Label();
            lblNume.Text = "Derscanu Bianca";
            lblNume.Top = 60;
            lblNume.Left = 50;
            lblNume.AutoSize = true;

            // Adăugare pe formă
            this.Controls.Add(lblTitlu);
            this.Controls.Add(lblNume);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
