using System;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    partial class Form2 : System.Windows.Forms.Form // Ensure Form2 inherits from System.Windows.Forms.Form
    {
        /// <summary>
        /// Varibilele necesare pentru componentele formularului.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Curăță resursele utilizate.
        /// </summary>
        /// <param name="disposing">True dacă gestionarul resurselor externe trebuie să fie apelat; false dacă nu.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region "Cod generat de Designer"

        /// <summary>
        /// Metoda necesară pentru inițializarea componentelor formularului.
        /// Acesta este apelat în constructorul formularului.
        /// </summary>
        private void InitializeComponent()
        {
            // Inițializare și setare a componentelor (controale) formularului
            this.SuspendLayout();

            // Form2
            this.ClientSize = new System.Drawing.Size(800, 450); // Dimensiunea formularului
            this.Name = "Form2"; // Numele formularului
            this.Text = "Formular 2"; // Titlul formularului
            this.Load += new System.EventHandler(this.Form2_Load); // Atașează evenimentul Form2_Load la formular

            this.ResumeLayout(false);
        }

        #endregion
    }
}
