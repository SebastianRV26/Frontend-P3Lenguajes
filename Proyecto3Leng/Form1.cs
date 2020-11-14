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
        Boolean flag = true; // true editable, false consult

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

        /// <summary>
        /// 
        /// Al precionar el botón cargar.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            flag = true;
            lblStatus.Text = "Llenando matriz";

            PlQuery query = new PlQuery("max(3,2,X)."); // se hace la consulta

            foreach (PlQueryVariables z in query.SolutionVariables)
            {
                Console.WriteLine(z["X"].ToString());
                MessageBox.Show("X " + z["X"].ToString(), "Print", MessageBoxButtons.OKCancel);
            }
            query.NextSolution();
            query.Dispose(); // se cierra la conexión
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

        /// <summary>
        /// 
        /// Crear tabla.
        /// 
        /// </summary>
        /// <param name="tamannio">
        /// Tamaño de la matriz NxN.
        /// </param>
        private void fillDataGridView(int tamannio)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(" ");
            for (int i = 1; i <= tamannio; i++)
            {
                dt.Columns.Add(i.ToString());
                dt.Rows.Add(i.ToString());
            }
            dataGridView1.DataSource = dt;
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        /// <summary>
        /// 
        /// On item click on data grid view.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                // cell.Value
                String x = e.ColumnIndex.ToString();
                String y = (e.RowIndex + 1).ToString();
                if (flag)
                {
                    // dibujar punto

                    if (x != "0")
                    {
                        if (cell.Value == "O")
                        {
                            cell.Value = " ";
                        }
                        else
                        {
                            cell.Value = "O";
                        }
                    }
                }
                else
                {
                    // consultar por el grupo del punto seleccionado

                }
                
                
            }
            catch
            {

            }
            
        }

        /// <summary>
        /// 
        /// Consultar por el tamaño de los grupos.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultGroups_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// Terminar de editar la matriz y proceder a realizar consultas.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveMatriz_Click(object sender, EventArgs e)
        {
            flag = false;
            lblStatus.Text = "Consultando";
        }

    }
}
