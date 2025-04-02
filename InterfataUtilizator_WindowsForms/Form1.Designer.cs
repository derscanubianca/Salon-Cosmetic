namespace InterfataUtilizator_WindowsForms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitlu;
        private System.Windows.Forms.Label lblNume;

        private void InitializeComponent()
        {
            this.lblTitlu = new System.Windows.Forms.Label();
            this.lblNume = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitlu
            // 
            this.lblTitlu.AutoSize = true;
            this.lblTitlu.Location = new System.Drawing.Point(50, 20);
            this.lblTitlu.Name = "lblTitlu";
            this.lblTitlu.Size = new System.Drawing.Size(80, 13);
            this.lblTitlu.TabIndex = 0;
            this.lblTitlu.Text = "Salon Cosmetic";
            // 
            // lblNume
            // 
            this.lblNume.AutoSize = true;
            this.lblNume.Location = new System.Drawing.Point(50, 60);
            this.lblNume.Name = "lblNume";
            this.lblNume.Size = new System.Drawing.Size(89, 13);
            this.lblNume.TabIndex = 1;
            this.lblNume.Text = "Derscanu Bianca";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.lblTitlu);
            this.Controls.Add(this.lblNume);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
