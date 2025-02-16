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
    public partial class VentanaProveedores : Form
    {
        public VentanaProveedores()
        {
            InitializeComponent();
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            
        }
        private void EnableUpdateButton()
        {
            bool allFieldsFilled = !string.IsNullOrWhiteSpace(txtCorreo.Text) &&
                                   !string.IsNullOrWhiteSpace(txtDir.Text) &&
                                   !string.IsNullOrWhiteSpace(txtNomEmpresa.Text) &&
                                   !string.IsNullOrWhiteSpace(txtRUC.Text) &&
                                   !string.IsNullOrWhiteSpace(txtsucursal.Text) &&
                                   !string.IsNullOrWhiteSpace(txtTelef.Text);
            btnAgregar.Enabled = allFieldsFilled;
        }
        public static string Correo = "";
        public static string Telefono = "";
        public static string NombreEmpresa = "";
        public static string RUC = "";
        public static string Direccion = "";
        public static string Sede = "";
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VentanaProveedores_Load(object sender, EventArgs e)
        {
            CenterToParent();
            // Utiliza la sintaxis de System.Data.OracleClient
            string strComm = "DECLARE cursorMemoria SYS_REFCURSOR; BEGIN sp_BuscarNom_Prov(:cursorMemoria, :p_buscar); END;";

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
                dataGridViewProveedores.DataSource = tabla;
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
            EnableUpdateButton();
            dataGridViewProveedores.ReadOnly = true;
        }
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Utiliza la sintaxis de System.Data.OracleClient
            string strComm = "DECLARE cursorMemoria SYS_REFCURSOR; BEGIN sp_BuscarNom_Prov(:cursorMemoria, :p_buscar); END;";

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
                dataGridViewProveedores.DataSource = tabla;
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

        private void txtNomEmpresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != ' ' && e.KeyChar != '\b')
            {
                // Si el carácter no es válido, suprimirlo
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

        private void txtCorreo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '@' && e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != '\b')
            {
                // Si el carácter no es válido, suprimirlo
                e.Handled = true;
            }
        }

        private void txtTelef_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es un número ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
            if (txtTelef.Text.Length >= 10 && e.KeyChar != '\b')
            {
                // Si tiene 10 o más caracteres y la tecla presionada no es una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void txtDir_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != ' ' && e.KeyChar != '\b')
            {
                // Si el carácter no es válido, suprimirlo
                e.Handled = true;
            }
        }

        private void txtNomEmpresa_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtRUC_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtCorreo_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtTelef_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtDir_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            bool errorEnTelefono = false;
            bool errorEnRUC = false;
            bool errorSd = false;


            string input = txtTelef.Text;
            // Verificar si los dos primeros caracteres no son "09"
            if (input.Length < 2 || input.Substring(0, 2) != "09" || input.Length != 10)
            {
                // Si los dos primeros caracteres no son "09", mostrar un mensaje de error
                MessageBox.Show("El numero celular es invalido, debe comenzar con 09 y tener 10 digitos");
                errorEnTelefono = true;
            }
            if (this.txtRUC.Text.Length != 13)
            {
                MessageBox.Show("Numero de RUC invalido, debe contener 13 digitos");
                errorEnRUC = true;
            }
            if (!this.txtsucursal.Text.Equals("Quito", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("No se puede ingresar un dato de otra sede");
                errorSd = true;
            }
            if (!errorEnTelefono && !errorEnRUC && !errorSd)
            {
                Form Confirmar = new VentanaConnfirmarAddProv();
                Correo = txtCorreo.Text;
                Telefono = txtTelef.Text;
                NombreEmpresa = txtNomEmpresa.Text;
                RUC = txtRUC.Text;
                Direccion = txtDir.Text;
                Sede = txtsucursal.Text;
                Confirmar.ShowDialog();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            bool errorEnTelefono = false;
            bool errorEnRUC = false;
            bool errorEnId = false;
            bool errorSde = false;


            string input = txtTelef.Text;
            // Verificar si los dos primeros caracteres no son "09"
            if (input.Length < 2 || input.Substring(0, 2) != "09" || input.Length != 10)
            {
                // Si los dos primeros caracteres no son "09", mostrar un mensaje de error
                MessageBox.Show("El numero celular es invalido, debe comenzar con 09 y tener 10 digitos");
                errorEnTelefono = true;
            }
            if (this.txtRUC.Text.Length != 13)
            {
                MessageBox.Show("Numero de RUC invalido, debe contener 13 digitos");
                errorEnRUC = true;
            }
            if (!this.txtsucursal.Text.Equals("Quito", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("No se puede ingresar un dato de otra sede");
                errorSde = true;
            }
            string strCom = "SELECT id_proveedor FROM proveedores_uio WHERE id_proveedor = '" + this.txtRUC.Text + "' AND ROWNUM <= 1";
            comm = new OracleCommand(strCom, conn); // Asignar la conexión a comm
            conn.Open(); // Abrir la conexión
            object resultado = comm.ExecuteScalar();
            conn.Close(); // Cerrar la conexión después de usarla
            if (resultado == null)
            {
                MessageBox.Show(" ¡¡ERROR!!, no puedes modificar el RUC");
                errorEnId = true;
            }
            if (!errorEnTelefono && !errorEnRUC && !errorEnId && !errorSde)
            {
                Form Confirmar = new VentanaConfirmarModProv();
                Correo = txtCorreo.Text;
                Telefono = txtTelef.Text;
                NombreEmpresa = txtNomEmpresa.Text;
                RUC = txtRUC.Text;
                Direccion = txtDir.Text;
                Sede = txtsucursal.Text;
                Confirmar.ShowDialog();
            }
        }

        private void dataGridViewProveedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNomEmpresa.Text = dataGridViewProveedores.SelectedCells[0].Value.ToString();
            txtRUC.Text = dataGridViewProveedores.SelectedCells[1].Value.ToString();
            txtTelef.Text = dataGridViewProveedores.SelectedCells[2].Value.ToString();
            txtCorreo.Text = dataGridViewProveedores.SelectedCells[3].Value.ToString();
            txtDir.Text = dataGridViewProveedores.SelectedCells[4].Value.ToString();
            txtsucursal.Text = dataGridViewProveedores.SelectedCells[5].Value.ToString();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Form Confirmar = new VentanaConfirmarBorrProv();
            RUC = txtRUC.Text;
            Confirmar.ShowDialog();
        }

        private void dataGridViewProveedores_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewProveedores.SelectedRows.Count > 0)
            {
                // Habilitar el botón si se ha seleccionado una fila
                btnModificar.Enabled = true;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNomEmpresa.Clear();
            txtRUC.Clear();
            txtTelef.Clear();
            txtCorreo.Clear();
            txtDir.Clear();
            txtsucursal.Clear();
        }

        private void txtsucursal_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }
    }
}
