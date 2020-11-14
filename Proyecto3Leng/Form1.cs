using SbsSW.SwiPlCs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto3Leng
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Environment.SetEnvironmentVariable("SWI_HOME_DIR", @"C:\\Program Files (x86)\\swipl");
            Environment.SetEnvironmentVariable("Path", @"C:\\Program Files (x86)\\swipl\\bin");
            string[] p = { "-q", "-f", @"main.pl" };
            // Connect to Prolog Engine
            PlEngine.Initialize(p);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            PlQuery cargar = new PlQuery("punto(3,2)");
            Console.WriteLine("cargar " + cargar);
            cargar.NextSolution();
            try
            {
                int len = int.Parse(tbMatriz.Text);
            }
            catch
            {
                MessageBox.Show("Ingrese un número Válido", "Advertencia", MessageBoxButtons.OKCancel);
            }
        }
    }
}
