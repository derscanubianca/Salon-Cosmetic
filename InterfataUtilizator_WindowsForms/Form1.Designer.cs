﻿using System;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Form1
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
          
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
    }
}
