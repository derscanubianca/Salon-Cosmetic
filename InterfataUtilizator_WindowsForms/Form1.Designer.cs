using System;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Form1
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // Form1
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }

        // Add the missing Form1_Load method
        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialization logic for when the form loads
        }
    }
}
