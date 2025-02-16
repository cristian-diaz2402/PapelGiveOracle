using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VentanaClientes : Form
    {
        public VentanaClientes()
        {
            InitializeComponent();
        }
        public static string PrimerNombre = "";
        public static string SegundoNombre = "";
        public static string PrimerApellido = "";
        public static string SegundoApellido = "";
        public static string Cedula = "";
        public static string Correo = "";
        public static string Telefono = "";
        public static string NombreEmpresa = "";
        public static string RUC = "";
        public static string Direccion = "";
        public static string Sede = "";
        private void btnsalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VentanaClientes_Load(object sender, EventArgs e)
        {
            CenterToParent();
            // Utiliza la sintaxis de System.Data.OracleClient
            string strComm = "DECLARE cursorMemoria SYS_REFCURSOR; BEGIN sp_BuscarNom_Cli(:cursorMemoria, :p_buscar); END;";

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
                dataGridViewClientes.DataSource = tabla;
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
            dataGridViewClientes.ReadOnly = true;
        }
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Utiliza la sintaxis de System.Data.OracleClient
            string strComm = "DECLARE cursorMemoria SYS_REFCURSOR; BEGIN sp_BuscarNom_Cli(:cursorMemoria, :p_buscar); END;";

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
                dataGridViewClientes.DataSource = tabla;
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            bool errorEnTelefono = false;
            bool errorEnCedula = false;
            bool errorEnRUC = false;
            bool errorS = false;


            string input = txtTelef.Text;
            // Verificar si los dos primeros caracteres no son "09"
            if (input.Length < 2 || input.Substring(0, 2) != "09" || input.Length != 10)
            {
                // Si los dos primeros caracteres no son "09", mostrar un mensaje de error
                MessageBox.Show("El numero celular es invalido, debe comenzar con 09 y tener 10 digitos");
                errorEnTelefono = true;
            }
            if (this.txtCedula.Text.Length != 10)
            {
                MessageBox.Show("Numero de Cedula invalido, debe contener 10 digitos");
                errorEnCedula = true;
            }
            if (this.txtRUC.Text.Length != 13)
            {
                MessageBox.Show("Numero de RUC invalido, debe contener 13 digitos");
                errorEnRUC = true;
            }
            if (!this.txtsucursal.Text.Equals("Quito", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("No se puede ingresar un dato de otra sede");
                errorS = true;
            }

            if (!errorEnTelefono && !errorEnCedula && !errorEnRUC && !errorS)
            {
                Form Confirmar = new VentanaConfirmarAddCliente();
                PrimerNombre = txtPrimerNombre.Text;
                SegundoNombre = txtSegundoNombre.Text;
                PrimerApellido = txtPrimerApellido.Text;
                SegundoApellido = txtSegundoApellido.Text;
                Cedula = txtCedula.Text;
                Correo = txtCorreo.Text;
                Telefono = txtTelef.Text;
                NombreEmpresa = txtNomEmpresa.Text;
                RUC = txtRUC.Text;
                Direccion = txtDir.Text;
                Sede = txtsucursal.Text;
                Confirmar.ShowDialog();
            }
        }
        private void EnableUpdateButton()
        {
            bool allFieldsFilled = !string.IsNullOrWhiteSpace(txtPrimerNombre.Text) &&
                                   !string.IsNullOrWhiteSpace(txtSegundoNombre.Text) &&
                                   !string.IsNullOrWhiteSpace(txtPrimerApellido.Text) &&
                                   !string.IsNullOrWhiteSpace(txtSegundoApellido.Text) &&
                                   !string.IsNullOrWhiteSpace(txtCedula.Text) &&
                                   !string.IsNullOrWhiteSpace(txtCorreo.Text) &&
                                   !string.IsNullOrWhiteSpace(txtDir.Text) &&
                                   !string.IsNullOrWhiteSpace(txtNomEmpresa.Text) &&
                                   !string.IsNullOrWhiteSpace(txtRUC.Text) &&
                                   !string.IsNullOrWhiteSpace(txtTelef.Text);
            btnAgregar.Enabled = allFieldsFilled;
        }

        private void txtPrimerNombre_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtSegundoNombre_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtPrimerApellido_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtSegundoApellido_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtCedula_TextChanged(object sender, EventArgs e)
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

        private void txtNomEmpresa_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtRUC_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtDir_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtPrimerNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es una letra ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void txtSegundoNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es una letra ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void txtPrimerApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es una letra ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void txtSegundoApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es una letra ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void txtCedula_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es un número ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
            if (txtCedula.Text.Length >= 10 && e.KeyChar != '\b')
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

        private void txtDir_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != ' ' && e.KeyChar != '\b')
            {
                // Si el carácter no es válido, suprimirlo
                e.Handled = true;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtPrimerNombre.Clear();
            txtSegundoNombre.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();
            txtCedula.Clear();
            txtNomEmpresa.Clear();
            txtRUC.Clear();
            txtTelef.Clear();
            txtCorreo.Clear();
            txtDir.Clear();
            txtsucursal.Clear();
        }

        private void dataGridViewClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewClientes.SelectedRows.Count > 0)
            {
                // Habilitar el botón si se ha seleccionado una fila
                button1.Enabled = true;
            }
        }

        private void dataGridViewClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string contenidoCelda = dataGridViewClientes.SelectedCells[0].Value.ToString(); // Contenido de la celda [0]
            string[] partes = contenidoCelda.Split(' '); // Dividir la cadena en palabras utilizando el espacio como separador
            if (partes.Length >= 4)
            {
                txtPrimerNombre.Text = partes[0];
                txtSegundoNombre.Text = partes[1];
                txtPrimerApellido.Text = partes[2];
                txtSegundoApellido.Text = partes[3];
            }
            txtCedula.Text = dataGridViewClientes.SelectedCells[1].Value.ToString();
            txtNomEmpresa.Text = dataGridViewClientes.SelectedCells[2].Value.ToString();
            txtRUC.Text = dataGridViewClientes.SelectedCells[3].Value.ToString();
            txtTelef.Text = dataGridViewClientes.SelectedCells[4].Value.ToString();
            txtCorreo.Text = dataGridViewClientes.SelectedCells[5].Value.ToString();
            txtDir.Text = dataGridViewClientes.SelectedCells[6].Value.ToString();
            txtsucursal.Text = dataGridViewClientes.SelectedCells[7].Value.ToString();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Form Confirmar = new VentanaConfirmarBorrCliente();
            Cedula = txtCedula.Text;
            Confirmar.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool errorEnTelefono = false;
            bool errorEnCedula = false;
            bool errorEnRUC = false;
            bool errorEnId = false;
            bool errorSe = false;


            string input = txtTelef.Text;
            // Verificar si los dos primeros caracteres no son "09"
            if (input.Length < 2 || input.Substring(0, 2) != "09" || input.Length != 10)
            {
                // Si los dos primeros caracteres no son "09", mostrar un mensaje de error
                MessageBox.Show("El numero celular es invalido, debe comenzar con 09 y tener 10 digitos");
                errorEnTelefono = true;
            }
            if (this.txtCedula.Text.Length != 10)
            {
                MessageBox.Show("Numero de Cedula invalido, debe contener 10 digitos");
                errorEnCedula = true;
            }
            if (this.txtRUC.Text.Length != 13)
            {
                MessageBox.Show("Numero de RUC invalido, debe contener 13 digitos");
                errorEnRUC = true;
            }
            if (!this.txtsucursal.Text.Equals("Quito", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("No se puede ingresar un dato de otra sede");
                errorSe = true;
            }

            string strCom = "SELECT id_cliente FROM clientes_uio WHERE id_cliente = '" + this.txtCedula.Text + "' AND ROWNUM <= 1";
            comm = new OracleCommand(strCom, conn); // Asignar la conexión a comm
            conn.Open(); // Abrir la conexión
            object resultado = comm.ExecuteScalar();
            conn.Close(); // Cerrar la conexión después de usarla
            if (resultado == null)
            {
                MessageBox.Show(" ¡¡ERROR!!, no puedes modificar la C.I.");
                errorEnId = true;
            }
            if (!errorEnTelefono && !errorEnCedula && !errorEnRUC && !errorEnId && !errorSe)
            {
                Form Confirmar = new VentanaConfirmarModCliente();
                PrimerNombre = txtPrimerNombre.Text;
                SegundoNombre = txtSegundoNombre.Text;
                PrimerApellido = txtPrimerApellido.Text;
                SegundoApellido = txtSegundoApellido.Text;
                Cedula = txtCedula.Text;
                Correo = txtCorreo.Text;
                Telefono = txtTelef.Text;
                NombreEmpresa = txtNomEmpresa.Text;
                RUC = txtRUC.Text;
                Direccion = txtDir.Text;
                Sede = txtsucursal.Text;
                Confirmar.ShowDialog();
            }
        }

        private void txtsucursal_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }
    }
    
}
