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
            try
            {
                Environment.SetEnvironmentVariable("SWI_HOME_DIR", @"C:\Program Files (x86)\swipl");
                Environment.SetEnvironmentVariable("Path", @"C:\Program Files (x86)\swipl\bin");
                string[] p = { "-q", "-f", @"main.pl" };

                // Connect to Prolog Engine
                PlEngine.Initialize(p);
            }
            catch
            {
                Console.WriteLine("Error en conectar con prolog");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            PlQuery query = new PlQuery("max(3,2,X).");

            foreach (PlQueryVariables z in query.SolutionVariables)
            {
                Console.WriteLine(z["X"].ToString());
                MessageBox.Show("X " + z["X"].ToString(), "Print", MessageBoxButtons.OKCancel);
            }
            query.NextSolution();
            try
            {
                int len = int.Parse(tbMatriz.Text);
                fillDataGridView(len);
            }
            catch
            {
                MessageBox.Show("Ingrese un número Válido", "Advertencia", MessageBoxButtons.OKCancel);
            }
        }

        private void fillDataGridView(int tamannio)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(" ");
            for (int i = 1; i<=tamannio; i++)
            {
                dt.Columns.Add(i.ToString());
                dt.Rows.Add(i.ToString());
            }

            
            //dt.Columns.Add("Name");

            //dt.Rows.Add("01", "Sebas");
            //dt.Rows.Add("02", "EdBinns");

            dataGridView1.DataSource = dt;

            dataGridView1.CellClick += dataGridView1_CellClick;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                // cell.Value
                String x = e.ColumnIndex.ToString();
                String y = (e.RowIndex + 1).ToString();
                if (cell.Value == "O")
                {
                    cell.Value = " ";
                }
                else
                {
                    cell.Value = "O";
                }
            }
            catch
            {

            }
            
        }

        private void btnConsultGroups_Click(object sender, EventArgs e)
        {

        }
    }
}
