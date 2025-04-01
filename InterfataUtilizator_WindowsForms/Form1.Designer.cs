using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

public class Form1 : Form, IDropTarget, ISynchronizeInvoke, IWin32Window, IBindableComponent, IComponent, IDisposable, IContainerControl
{
    private Label lblTitlu;
    private Label lblNume;

    public Form1()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.lblTitlu = new Label();
        this.lblNume = new Label();
        // Additional initialization code here
    }
}
