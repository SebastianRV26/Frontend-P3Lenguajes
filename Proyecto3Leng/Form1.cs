﻿using SbsSW.SwiPlCs;
using System;
using System.Collections;
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
        private List<List<List<string>>> listasGrupos;
        private string groupGlobal = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Se establecen la ruta de la ubicación de prolog.
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
        /// Al precionar el botón cargar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            flag = true;
            lblStatus.Text = "Llenando matriz";
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
        /// Crear tabla.
        /// </summary>
        /// <param name="tamannio">
        /// Tamaño de la matriz NxN.
        /// </param>
        private void fillDataGridView(int tamannio)
        {
            vacearCombobox();
            dataGridView1.ClearSelection();
            dataGridView1.CellClick -= dataGridView1_CellClick;
            eliminarTodo();
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
        /// On item click on data grid view.
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
                            eliminarPunto(x, y);
                        }
                        else
                        {
                            cell.Value = "O";
                            guardarPunto(x, y);
                        }
                    }
                }
                else
                {
                    string message = "";
                    if (exite_punto(x, y))
                    {
                        string n = get_grupo(x, y);
                        message = "El punto seleccionado se encuentra en un grupo de tamaño "+n+"\nEl grupo es "+groupGlobal;
                    } else
                    {
                        message = "¡El punto no se encuentra en ningún grupo!";
                    }
                    MessageBox.Show(message, "Consulta: tamaño al grupo del punto");
                
                }


            }
            catch { }
            
        }

        /// <summary>
        /// Calculen el número de grupos distintos y sus tamaños para toda la cuadrícula.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultGroups_Click(object sender, EventArgs e)
        {
            vacearCombobox();
            // llamar a consultar tamaño de grupos
            List<List<List<string>>> listGropus = get_grupos();
            pintar_grupos(listGropus);
            string groups = contar_grupos(listGropus);
            //string grupos = "Existen N grupos de tamaño M";
            MessageBox.Show(groups, "Consulta");
            llenarCombobox(listGropus);
            listasGrupos = listGropus;
        }

        /// <summary>
        /// Terminar de editar la matriz y proceder a realizar consultas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveMatriz_Click(object sender, EventArgs e)
        {
            flag = false;
            lblStatus.Text = "Consultando";
        }

        /// <summary>
        /// Llamar a regla de prolog que genere los puntos de manera aleatoria.
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
                    if (dt.Rows[row][column] != "O")
                    {
                        dt.Rows[row][column] = "O";
                        guardarPunto(column.ToString(), (row + 1).ToString());
                    }
                    
                    tamannio--;
                }
                Console.Write("Listo Random");
            }
            catch
            {
                MessageBox.Show("Ingrese un número Válido", "Advertencia");
            }
        }

        //////////////////// consultas prolog ////////////////////

        /// <summary>
        /// Guardar en memoria el punto dado.
        /// assert(punto(X,Y)).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void guardarPunto(string x, string y)
        {
            PlQuery query = new PlQuery("guardar_punto(" + x + "," + y + ")."); // se hace la consulta
            query.NextSolution();
            query.Dispose(); // se cierra la conexión
        }

        /// <summary>
        /// Eliminar de memoria un punto.
        /// retract(punto(X,Y)).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void eliminarPunto(string x, string y)
        {
            PlQuery query = new PlQuery("eliminar_punto(" + x + "," + y + ")."); // se hace la consulta
            query.NextSolution();
            query.Dispose(); // se cierra la conexión
        }

        /// <summary>
        /// Eliminar de memoria todos los puntos.
        /// retractall(punto(_,_)).
        /// </summary>
        private void eliminarTodo()
        {
            PlQuery query = new PlQuery("eliminar_todo."); // se hace la consulta
            query.NextSolution();
            query.Dispose(); // se cierra la conexión
        }

        private bool exite_punto(string x, string y)
        {
            PlQuery query = new PlQuery("punto(" + x + "," + y + ").");
            query.Dispose();
            return query.NextSolution();
        }

        /// <summary>
        /// Obtiene el grupo dado.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string get_grupo(string x, string y)
        {
            PlQuery query = new PlQuery("grupo_punto(" + x + "," + y + ",R,N).");
            query.Dispose();
            string n = "";
            string list1 = "";
            foreach (PlQueryVariables z in query.SolutionVariables)
            {
                list1 = z["R"].ToString();
                n = z["N"].ToString();
                Console.WriteLine("Listo grupo");
            }
            query.NextSolution();
            groupGlobal = list1;
            if (n != "0")
            {
                string resultQuery = list1.Replace("],[", "];[");
                resultQuery = resultQuery.Replace("]]", "");
                resultQuery = resultQuery.Replace("[[", "");
                List<string> listQuery = new List<string>(resultQuery.Split(';'));
                List<string> list2;
                List<List<String>> finalList = new List<List<string>>();

                foreach(string list in listQuery)
                {
                    string temporal = list;
                    temporal = temporal.Replace("]", "");
                    temporal = temporal.Replace("[", "");
                    list2 = new List<string>(temporal.Split(','));
                    List<string> sublist = new List<string>();
                    foreach (string subElement in list2)
                    {
                        sublist.Add(subElement);
                    }
                    finalList.Add(sublist);
                }

                pintar_grupo(finalList, System.Drawing.Color.Yellow);
            }
            return n;
        }

        /// <summary>
        /// Pintar un grupo de amarrillo.
        /// </summary>
        /// <param name="arlist"></param>
        private void pintar_grupo(List<List<string>> arlist, System.Drawing.Color color)
        {
            foreach (List<string> a in arlist)
            {
                int row = int.Parse((a[1]).ToString());
                int column = int.Parse((a[0]).ToString());
                DataGridViewCell cell = dataGridView1.Rows[row-1].Cells[column];
                cell.Style.BackColor = color;
            }
        }

        /// <summary>
        /// Obtiene todos los grupos.
        /// </summary>
        /// <returns></returns>
        private List<List<List<string>>> get_grupos()
        {
            PlQuery query = new PlQuery("todos_grupos(S).");
            string list1 = "";
            foreach (PlQueryVariables z in query.SolutionVariables)
            {
                list1 = z["S"].ToString();
                Console.WriteLine("Listo grupos");
            }
            query.Dispose();
            query.NextSolution();
            List<List<List<string>>> listGrops = new List<List<List<string>>>();
            string resultQuery = list1.Replace("[],", "");
            resultQuery = resultQuery.Replace("]],[[", ";");
            resultQuery = resultQuery.Replace("]]]", "");
            resultQuery = resultQuery.Replace("[[[", "");
            List<string> listQuery = new List<string>(resultQuery.Split(';'));
            foreach (string list in listQuery)
            {
                List<List<string>> finalList = new List<List<string>>();
                string temporal = list;
                temporal = temporal.Replace("],[", ".");
                List<string> listaTriple = new List<string>(temporal.Split('.'));
                foreach (string list3 in listaTriple)
                {
                    List<string> list2 = new List<string>(list3.Split(','));
                    List<string> subElement = new List<string>();
                    subElement.Add(list2[0]);
                    subElement.Add(list2[1]);
                    finalList.Add(subElement);
                }
                listGrops.Add(finalList);
            }
            return listGrops;

        }

        /// <summary>
        /// Pinta los grupos de disintos colores.
        /// </summary>
        /// <param name="listas"></param>
        private void pintar_grupos(List<List<List<string>>> listas)
        {
            Random rnd = new Random();
            foreach (List<List<string>> list in listas)
            {
                int r = rnd.Next(0, 256);
                int g = rnd.Next(0, 256);
                int b = rnd.Next(0, 256);
                foreach (List<string> subList in list)
                {
                    DataGridViewCell color = dataGridView1.Rows[int.Parse(subList[1])-1].Cells[int.Parse(subList[0])];
                    color.Style.BackColor = System.Drawing.Color.FromArgb(r, g, b);
                }
            }
        }

        /// <summary>
        /// Retorna una cadena con los tamñanos de los grupos.
        /// </summary>
        /// <param name="listas"></param>
        /// <returns></returns>
        private string contar_grupos(List<List<List<string>>> listas)
        {
            var l1 = contar_elementos(listas);
            var g = l1.GroupBy(i => i);
            string groups = "Hay "+ puntos_individuales() + " puntos individuales\n";
            foreach (var grp in g)
            {
                groups += "Hay "+ grp.Count()+" grupos de "+ grp.Key  + " puntos\n";
            }
            return groups;
        }

        /// <summary>
        /// Cuantos elementos tiene cada lista.
        /// </summary>
        /// <param name="listas"></param>
        /// <returns></returns>
        private List<int> contar_elementos(List<List<List<string>>> listas)
        {
            List<int> lens = new List<int>();
            foreach (List<List<string>> lista in listas)
            {
                lens.Add(lista.Count());
            }
            return lens;
        }

        /// <summary>
        /// Retorna cuánto puntos individuales existen.
        /// </summary>
        /// <returns></returns>
        private string puntos_individuales()
        {
            PlQuery query = new PlQuery("puntosIndividuales(N).");
            string n = "";
            foreach (PlQueryVariables z in query.SolutionVariables)
            {
                n = z["N"].ToString();
                Console.WriteLine("Listo grupos");
            }
            query.Dispose();
            query.NextSolution();
            return n;
        }

        /// <summary>
        /// Mostrar los grupos en un list box.
        /// </summary>
        /// <param name="listas"></param>
        private void llenarCombobox(List<List<List<string>>> listas)
        {
            int i = 1;
            foreach (List<List<string>> list in listas)
            {
                comboBox1.Items.Add("Grupo " + i.ToString());
                listBox1.Items.Add("Grupo " + i.ToString());
                i++;
                foreach (List<string> subList in list)
                {
                    listBox1.Items.Add("("+ subList[0].ToString() + "," + subList[1].ToString() + ")");
                }
            }
        }

        /// <summary>
        /// Limpiar datos del combobox y el list box.
        /// </summary>
        private void vacearCombobox()
        {
            comboBox1.Items.Clear();
            listBox1.Items.Clear();
        }

        /// <summary>
        /// Colorear el grupo seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGroups_Click(object sender, EventArgs e)
        {
            int pos = comboBox1.SelectedIndex;
            List<List<string>> listas = listasGrupos[pos];
            pintar_grupo(listas, System.Drawing.Color.Green);
        }
    }
}
