using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace FrmProductos
{
    public partial class frmProductos : Form
    {

        // Creación de la conexión
        SqlConnection conn = new SqlConnection(@"server = (local); 
                        integrated security = true; database = AdventureWorks2014");
        public frmProductos()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Cargará automáticamente los productos en la lista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductos_Load(object sender, EventArgs e)
        {

            // Query para traer los productos
            string sqlSelect = @"SELECT ProductID, Name
                                FROM Production.Product
                                WHERE FinishedGoodsFlag = 1";

            // Creamos el comando 
            SqlCommand cmd = new SqlCommand(sqlSelect, conn);

            try
            {
                // Abrimos Conexión
                conn.Open();

                // Ejecutamos el query
                SqlDataReader rdr = cmd.ExecuteReader();

                // Mientras lea los campos, se va a ir agregando al listbox
                while (rdr.Read())
                {
                    //lstProductos.Items.Add(rdr[0]);
                    lstProductos.Items.Add(rdr[1]);
                }

            }
            catch (SqlException ex)
            {
                // Capturamos los posibles errores
                MessageBox.Show(ex.Message + ex.StackTrace, "! Detalles en la excepción!");
            }
            finally
            {
                // Cerramos la conexión
                conn.Close();

            }

        }

        /// <summary>
        /// Se encarga de Cerrar la aplicación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_Click(object sender, EventArgs e)
        {
            // Cerramos el formulario
            this.Close();
        }

        /// <summary>
        /// Se encarga de Agregar la reseña/análisis del producto elegido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // Crear el comando que va a llamar al Stored Procedure
            SqlCommand cmd = new SqlCommand("sp_IngresarResena", conn);

            // Establecer el comando como un Stored Procedure
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                // Verifica si se llenaron los campo
                if (txtNombreCompleto.Text == "" || txtCorreo.Text == "" || txtValoracion.Text == "")

                {
                    MessageBox.Show("Favor llenar los campos antes de ejecutar la acción", "Información");
                }
                else
                {
                    // Verificar si esta selccionado un producto
                    if (lstProductos.SelectedIndex == -1)
                    {
                        MessageBox.Show("Favor seleccionar un producto antes de ejecutar la acción de eliminar", "Información");
                    }
                    else
                    {
                        // Parámetro del Stored Procedure
                        cmd.Parameters.Add(new SqlParameter("@productName", SqlDbType.NVarChar, 50));
                        cmd.Parameters["@productName"].Value = lstProductos.SelectedItems[0];

                        cmd.Parameters.Add(new SqlParameter("@reviewerName", SqlDbType.NVarChar, 50));
                        cmd.Parameters["@reviewerName"].Value = txtNombreCompleto.Text;

                        cmd.Parameters.Add(new SqlParameter("@reviewDate", SqlDbType.DateTime));
                        cmd.Parameters["@reviewDate"].Value = System.DateTime.Now;

                        cmd.Parameters.Add(new SqlParameter("@emailAddress", SqlDbType.NVarChar, 50));
                        cmd.Parameters["@emailAddress"].Value = txtCorreo.Text;

                        cmd.Parameters.Add(new SqlParameter("@rating", SqlDbType.Int));
                        cmd.Parameters["@rating"].Value = txtValoracion.Text.ToString();

                        cmd.Parameters.Add(new SqlParameter("@comments", SqlDbType.NVarChar, 3850));
                        cmd.Parameters["@comments"].Value = txtComentarios.Text;

                        cmd.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.DateTime));
                        cmd.Parameters["@modifiedDate"].Value = System.DateTime.Now;

                        // Establecer la conexión
                        conn.Open();

                        // Ejecutamos el query para registar la reseña
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Reseña Registrada Exitosamente", "Información");
                    }                 
                                      
                    
                }
                                
            }
            // Atrapamos las excepciones
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace, "Detalles en la excepción");
            }
            finally
            {
                // Cerrar la conexión
                conn.Close();
            }

        }

        private void txtValoracion_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
