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
        private Boolean flag = true; // true editable, false consult
        private DataTable dt = new DataTable();

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
            dt = new DataTable();
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
                    string message = "";
                    // calcultar el tamaño del grupo al que pertenece el punto
                    if (x == "0")
                    {
                        // llamo a consulta
                        message = "El punto seleccionado se encuentra en un grupo de tamaño ";
                    }
                    else
                    {
                        message = "¡El punto no se encuentra en ningún grupo!";
                    }
                    MessageBox.Show(message, "Consulta: tamaño al grupo del punto");
                }


            }
            catch { }
            
        }

        /// <summary>
        /// 
        /// Calculen el número de grupos distintos y sus tamaños para toda la cuadrícula.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultGroups_Click(object sender, EventArgs e)
        {
            // llamar a consultar tamaño de grupos
            string grupos = "Existen N grupos de tamaño M";
            MessageBox.Show(grupos, "Consulta");
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

        /// <summary>
        /// 
        /// Llamar a regla de prolog que genere los puntos de manera aleatoria.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateRandom_Click(object sender, EventArgs e)
        {
            flag = false;
            lblStatus.Text = "Consultando";

            try
            {
                int len = int.Parse(tbMatriz.Text);

                fillDataGridView(len);

                Random rnd1 = new Random();
                int tamannio = rnd1.Next(1, len * len + 1); ;
                
                while (tamannio > 0)
                {
                    int column = rnd1.Next(1, len + 1);
                    int row = rnd1.Next(0, len);
                    if (dt.Rows[row][column] != "0")
                    {
                        dt.Rows[row][column] = "O";
                    }
                    
                    tamannio--;
                }
            }
            catch
            {
                MessageBox.Show("Ingrese un número Válido", "Advertencia");
            }
        }
    }
}
