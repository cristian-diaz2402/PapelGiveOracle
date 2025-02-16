using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VentanaCompras : Form
    {
        public VentanaCompras()
        {
            InitializeComponent();
        }
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public static string NumeroOrden;
        public static string Fecha = "";
        public static string CodigoBarra;
        public static string Cantidad;
        public static string NombreProducto = "";
        public static string NombreProveedor = "";
        public static string RUC = "";
        public static string ModoPago = "";
        private void EnableUpdateButton()
        {
            bool allFieldsFilled = !string.IsNullOrWhiteSpace(txtCodBarra.Text) &&
                                   !string.IsNullOrWhiteSpace(txtNomProd.Text) &&
                                   !string.IsNullOrWhiteSpace(txtCantidad.Text) &&
                                   !string.IsNullOrWhiteSpace(txtNomProv.Text) &&
                                   !string.IsNullOrWhiteSpace(txtRUC.Text) &&
                                   cmbProveedores.SelectedIndex != -1 &&
                                   rdbEfectivo.Checked || rdbTransferencia.Checked;

            btnAceptar.Enabled = allFieldsFilled;
        }
        private void VentanaCompras_Load(object sender, EventArgs e)
        {
            CenterToParent();
            Random random = new Random();
            int count = 1;
            while (count != 0)
            {
                int numeroAleatorio = random.Next(0, 100000);
                string numeroFormateado = numeroAleatorio.ToString("D5");
                string strC = "SELECT COUNT(*) FROM NumeroOrdenCompra WHERE num_orden = '" + numeroFormateado + "'";
                comm = new OracleCommand(strC, conn);

                try
                {
                    conn.Open();
                    count = Convert.ToInt32(comm.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha surgido un error: " + ex.Message);
                    return;
                }
                finally
                {
                    conn.Close();
                }
                if (count == 0)
                {
                    this.txtNorden.Text = numeroFormateado;
                }
            }
            string strComm = "DECLARE cursorMemoria SYS_REFCURSOR; BEGIN sp_BuscarNom_Prod(:cursorMemoria, :p_buscar); END;";

            OracleCommand cmd = new OracleCommand(strComm, conn);
            cmd.CommandType = CommandType.Text;

            // Crear parámetro de entrada
            OracleParameter paramBuscar = new OracleParameter(":p_buscar", OracleType.NVarChar);
            paramBuscar.Value = "";
            cmd.Parameters.Add(paramBuscar);

            // Crear parámetro de salida para el cursor
            OracleParameter cursorParam = new OracleParameter(":cursorMemoria", OracleType.Cursor);
            cursorParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(cursorParam);

            OracleDataAdapter adaptador = new OracleDataAdapter(cmd);
            DataTable tabla = new DataTable();

            try
            {
                conn.Open();
                adaptador.Fill(tabla);
                dataGridViewCompras.DataSource = tabla;
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            DateTime fechaActual = DateTime.Now;
            txtfecha.Text = fechaActual.ToString("yyyy-MM-dd");

            string comando = "SELECT nombre_prov FROM Proveedores_uio";
            comm = new OracleCommand(comando, conn);
            conn.Open();
            OracleDataAdapter adaptad = new OracleDataAdapter(comm);
            DataTable abcd = new DataTable();
            adaptad.Fill(abcd);

            // Una vez que tienes los datos en un DataTable, puedes agregarlos al ComboBox
            foreach (DataRow row in abcd.Rows)
            {
                cmbProveedores.Items.Add(row["nombre_prov"].ToString());
            }
            conn.Close();


            EnableUpdateButton();
            dataGridViewCompras.ReadOnly = true;
        }

        private void txtCodBarra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es un número ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
            if (txtCodBarra.Text.Length >= 5 && e.KeyChar != '\b')
            {
                // Si tiene 10 o más caracteres y la tecla presionada no es una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void txtNomProd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es una letra ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es un número ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void txtRUC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es un número ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
            if (txtRUC.Text.Length >= 13 && e.KeyChar != '\b')
            {
                // Si tiene 10 o más caracteres y la tecla presionada no es una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void txtNomProv_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != ' ' && e.KeyChar != '\b')
            {
                // Si el carácter no es válido, suprimirlo
                e.Handled = true;
            }
        }

        private void txtCodBarra_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtNomProd_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtNomProv_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtRUC_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void rdbEfectivo_CheckedChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void rdbTransferencia_CheckedChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int count = 1;
            while (count != 0)
            {
                int numeroAleatorio = random.Next(0, 100000);
                string numeroFormateado = numeroAleatorio.ToString("D5");
                string strC = "SELECT COUNT(*) FROM NumeroOrdenCompra WHERE num_orden = '" + numeroFormateado + "'";
                comm = new OracleCommand(strC, conn);

                try
                {
                    conn.Open();
                    count = Convert.ToInt32(comm.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha surgido un error: " + ex.Message);
                    return;
                }
                finally
                {
                    conn.Close();
                }
                if (count == 0)
                {
                    this.txtNorden.Text = numeroFormateado;
                    this.txtNomProd.Clear();
                    this.txtCodBarra.Clear();
                    this.txtNomProv.Clear();
                    this.txtRUC.Clear();
                    this.txtCantidad.Clear();
                    this.rdbEfectivo.Checked = false;
                    this.rdbTransferencia.Checked = false;
                    cmbProveedores.SelectedIndex = -1;
                }

            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            bool errorId = false;

            string strCom = "SELECT num_orden FROM NumeroOrdenCompra where num_orden = '" + this.txtNorden.Text + "' AND ROWNUM <= 1";
            comm = new OracleCommand(strCom, conn); // Asignar la conexión a comm
            conn.Open(); // Abrir la conexión
            object resultado = comm.ExecuteScalar();
            conn.Close(); // Cerrar la conexión después de usarla
            if (resultado != null)
            {
                MessageBox.Show("El numero de orden ya existe");
                errorId = true;
            }
            if (!errorId)
            {
                Form Confirmar = new VentanaConfirmarCompra();
                NumeroOrden = txtNorden.Text;
                Fecha = txtfecha.Text;
                CodigoBarra = txtCodBarra.Text;
                NombreProducto = txtNomProd.Text;
                Cantidad = txtCantidad.Text;
                NombreProveedor = txtNomProv.Text;
                RUC = txtRUC.Text;
                if (rdbEfectivo.Checked)
                {
                    ModoPago = rdbEfectivo.Text;
                }
                if (rdbTransferencia.Checked)
                {
                    ModoPago = rdbTransferencia.Text;
                }
                Confirmar.ShowDialog();
            }
        }

        private void txtNorden_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es un número ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
            if (txtNorden.Text.Length >= 5 && e.KeyChar != '\b')
            {
                // Si tiene 10 o más caracteres y la tecla presionada no es una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Utiliza la sintaxis de System.Data.OracleClient
            string strComm = "DECLARE cursorMemoria SYS_REFCURSOR; BEGIN sp_BuscarNom_Prod(:cursorMemoria, :p_buscar); END;";

            OracleCommand cmd = new OracleCommand(strComm, conn);
            cmd.CommandType = CommandType.Text;

            // Crear parámetro de entrada
            OracleParameter paramBuscar = new OracleParameter(":p_buscar", OracleType.NVarChar);
            paramBuscar.Value = this.txtCajaBuscar.Text;
            cmd.Parameters.Add(paramBuscar);

            // Crear parámetro de salida para el cursor
            OracleParameter cursorParam = new OracleParameter(":cursorMemoria", OracleType.Cursor);
            cursorParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(cursorParam);

            OracleDataAdapter adaptador = new OracleDataAdapter(cmd);
            DataTable tabla = new DataTable();

            try
            {
                conn.Open();
                adaptador.Fill(tabla);
                dataGridViewCompras.DataSource = tabla;
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void dataGridViewCompras_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewCompras_CellBorderStyleChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewCompras_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNomProd.Text = dataGridViewCompras.SelectedCells[1].Value.ToString();
            txtCodBarra.Text = dataGridViewCompras.SelectedCells[0].Value.ToString();
        }

        private void cmbProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProveedores.SelectedIndex != -1)
            {
                string elementoSeleccionado = cmbProveedores.SelectedItem.ToString();
                string com = "SELECT nombre_prov,id_proveedor FROM Proveedores_uio Where nombre_prov='" + elementoSeleccionado + "'";
                conn.Open();
                comm = new OracleCommand(com, conn);
                conn.Close();
                OracleDataAdapter adaptado = new OracleDataAdapter(comm);
                DataTable ada = new DataTable();
                adaptado.Fill(ada);
                txtNomProv.Text = ada.Rows[0]["nombre_prov"].ToString();
                txtRUC.Text = ada.Rows[0]["id_proveedor"].ToString();

            }
        }
    }
    
}
