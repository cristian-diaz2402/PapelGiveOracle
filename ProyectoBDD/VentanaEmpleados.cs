using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProyectoBDD
{
    public partial class VentanaEmpleados : Form
    {
        public VentanaEmpleados()
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
        public static string NombreUsuario = "";
        public static string Password = "";
        public static string Rol = "";
        public static string Direccion = "";
        public static decimal Sueldo;
        public static string Ocupacion = "";
        public static string Sede = "";

        private void btnsalir_Click(object sender, EventArgs e)
        {


        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void EnableUpdateButton()
        {
            bool allFieldsFilled = !string.IsNullOrWhiteSpace(txtPrimerNombre.Text) &&
                                   !string.IsNullOrWhiteSpace(txtSegundoNombre.Text) &&
                                   !string.IsNullOrWhiteSpace(txtPrimerApellido.Text) &&
                                   !string.IsNullOrWhiteSpace(txtSegundoApellido.Text) &&
                                   !string.IsNullOrWhiteSpace(txtCedula.Text) &&
                                   !string.IsNullOrWhiteSpace(txtsucursal.Text) &&
                                   !string.IsNullOrWhiteSpace(txtCorreo.Text) &&
                                   !string.IsNullOrWhiteSpace(txtDireccion.Text) &&
                                   cmbRol.SelectedIndex != -1 &&
                                   !string.IsNullOrWhiteSpace(txtTelef.Text) &&
                                   !string.IsNullOrWhiteSpace(txtOcupacion.Text) &&
                                   !string.IsNullOrWhiteSpace(txtSueldo.Text) &&
                                   !string.IsNullOrWhiteSpace(txtUsuario.Text) &&
                                   !string.IsNullOrWhiteSpace(txtpassword.Text);
            btnAgregar.Enabled = allFieldsFilled;
        }

        private void VentanaEmpleados_Load(object sender, EventArgs e)
        {
            CenterToParent();
            // Utiliza la sintaxis de System.Data.OracleClient
            string strComm = "DECLARE cursorMemoria SYS_REFCURSOR; BEGIN sp_BuscarNom_Emple(:cursorMemoria, :p_buscar); END;";

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
                dataGridViewEmpleados.DataSource = tabla;
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
            dataGridViewEmpleados.ReadOnly = true;
        }
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;

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

        private void txtOcupacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es una letra ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void txtSueldo_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, el punto decimal, la tecla de retroceso y la tecla de suprimir
            if (e.KeyChar != '\b' && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto decimal
            if (e.KeyChar == ',' && (sender as System.Windows.Forms.TextBox).Text.Contains(","))
            {
                e.Handled = true;
            }

            // Permitir solo dos dígitos después del punto
            if ((sender as System.Windows.Forms.TextBox).Text.Contains(","))
            {
                string[] parts = (sender as System.Windows.Forms.TextBox).Text.Split(',');
                if (parts.Length > 1 && parts[1].Length >= 2 && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != '\b')
            {
                // Si el carácter no es válido, suprimirlo
                e.Handled = true;
            }
        }

        private void txtpassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '$' && e.KeyChar != '@' && e.KeyChar != '!' && e.KeyChar != '%' && e.KeyChar != '#' && e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != '\b')
            {
                // Si el carácter no es válido, suprimirlo
                e.Handled = true;
            }
        }

        private void txtDireccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != ' ' && e.KeyChar != '\b')
            {
                // Si el carácter no es válido, suprimirlo
                e.Handled = true;
            }
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

        private void txtOcupacion_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtSueldo_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtpassword_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtDireccion_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            bool errorEnTelefono = false;
            bool errorEnCedula = false;
            bool errorSed = false;

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
            if (!this.txtsucursal.Text.Equals("Quito", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("No se puede ingresar un dato de otra sede");
                errorSed = true;
            }
            if (!errorEnTelefono && !errorEnCedula && !errorSed)
            {

                decimal sueldodecimal = decimal.Parse(txtSueldo.Text);
                Form Confirmar = new VentanaConfirmarAddEmple();
                PrimerNombre = txtPrimerNombre.Text;
                SegundoNombre = txtSegundoNombre.Text;
                PrimerApellido = txtPrimerApellido.Text;
                SegundoApellido = txtSegundoApellido.Text;
                Cedula = txtCedula.Text;
                Correo = txtCorreo.Text;
                Telefono = txtTelef.Text;
                Sueldo = sueldodecimal;
                Ocupacion = txtOcupacion.Text;
                Direccion = txtDireccion.Text;
                Rol = cmbRol.SelectedItem.ToString();
                NombreUsuario = txtUsuario.Text;
                Password = txtpassword.Text;
                Sede = txtsucursal.Text;
                Confirmar.ShowDialog();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string strComm = "DECLARE cursorMemoria SYS_REFCURSOR; BEGIN sp_BuscarNom_Emple(:cursorMemoria, :p_buscar); END;";

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
                dataGridViewEmpleados.DataSource = tabla;
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

        private void txtCajaBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es una letra ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
            if (txtCajaBuscar.Text.Length >= 10 && e.KeyChar != '\b')
            {
                // Si tiene 10 o más caracteres y la tecla presionada no es una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                string strComm = "DECLARE cursorMemoria SYS_REFCURSOR; BEGIN sp_BuscarNom_Emple(:cursorMemoria, :p_buscar); END;";

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
                    dataGridViewEmpleados.DataSource = tabla;
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

                txtCajaBuscar.Clear();

                // Evitar que el Enter genere un sonido de "beep"
                e.Handled = true;
            }
        }

        private void dataGridViewEmpleados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string contenidoCelda = dataGridViewEmpleados.SelectedCells[0].Value.ToString(); // Contenido de la celda [0]
            string[] partes = contenidoCelda.Split(' '); // Dividir la cadena en palabras utilizando el espacio como separador
            if (partes.Length >= 4)
            {
                txtPrimerNombre.Text = partes[0];
                txtSegundoNombre.Text = partes[1];
                txtPrimerApellido.Text = partes[2];
                txtSegundoApellido.Text = partes[3];
            }
            txtCedula.Text = dataGridViewEmpleados.SelectedCells[1].Value.ToString();
            txtCorreo.Text = dataGridViewEmpleados.SelectedCells[5].Value.ToString();
            txtTelef.Text = dataGridViewEmpleados.SelectedCells[4].Value.ToString();
            txtDireccion.Text = dataGridViewEmpleados.SelectedCells[6].Value.ToString();
            cmbRol.SelectedItem = dataGridViewEmpleados.SelectedCells[3].Value.ToString();
            txtUsuario.Text = dataGridViewEmpleados.SelectedCells[7].Value.ToString();
            txtpassword.Text = dataGridViewEmpleados.SelectedCells[8].Value.ToString();
            txtOcupacion.Text = dataGridViewEmpleados.SelectedCells[2].Value.ToString();
            txtSueldo.Text = dataGridViewEmpleados.SelectedCells[10].Value.ToString();
            txtsucursal.Text = dataGridViewEmpleados.SelectedCells[9].Value.ToString();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            bool errorEnTelefono = false;
            bool errorEnCedula = false;
            bool errorEnId = false;
            bool errorSede = false;


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
            if (!this.txtsucursal.Text.Equals("Quito", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("No se puede ingresar un dato de otra sede");
                errorSede = true;
            }
            string strCom = "SELECT id_empleado FROM empleados_uio WHERE id_empleado = '" + this.txtCedula.Text + "' AND ROWNUM <= 1";
            comm = new OracleCommand(strCom, conn); // Asignar la conexión a comm
            conn.Open(); // Abrir la conexión
            object resultado = comm.ExecuteScalar();
            conn.Close(); // Cerrar la conexión después de usarla
            if (resultado == null)
            {
                MessageBox.Show(" ¡¡ERROR!!, no puedes modificar la C.I.");
                errorEnId = true;
            }
            if (!errorEnTelefono && !errorEnCedula && !errorEnId && !errorSede)
            {
                decimal sueldodecimal = decimal.Parse(txtSueldo.Text);
                Form Confirmar = new VentanaConfirmarModEmple();
                PrimerNombre = txtPrimerNombre.Text;
                SegundoNombre = txtSegundoNombre.Text;
                PrimerApellido = txtPrimerApellido.Text;
                SegundoApellido = txtSegundoApellido.Text;
                Cedula = txtCedula.Text;
                Correo = txtCorreo.Text;
                Telefono = txtTelef.Text;
                Sueldo = sueldodecimal;
                Ocupacion = txtOcupacion.Text;
                Direccion = txtDireccion.Text;
                Rol = cmbRol.SelectedItem.ToString();
                NombreUsuario = txtUsuario.Text;
                Password = txtpassword.Text;
                Sede = txtsucursal.Text;
                Confirmar.ShowDialog();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Form Confirmar = new VentanaConfirmarBorrEmple();
            Cedula = txtCedula.Text;
            Confirmar.ShowDialog();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtPrimerNombre.Clear();
            txtSegundoNombre.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();
            txtCedula.Clear();
            txtCorreo.Clear();
            txtTelef.Clear();
            txtDireccion.Clear();
            cmbRol.SelectedIndex = -1;
            txtUsuario.Clear();
            txtpassword.Clear();
            txtOcupacion.Clear();
            txtSueldo.Clear();
            txtsucursal.Clear();
        }

        private void txtsucursal_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }
    }
}
