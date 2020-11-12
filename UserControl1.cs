using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frontend_P3Lenguajes
{
    public partial class UserControl1: UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
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
