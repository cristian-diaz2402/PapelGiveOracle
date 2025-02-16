using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VentanaCierreCaja : Form
    {
        public static string Fecha = "";
        public static string MontoInicial = "";
        public static string CodigoCaja = "";
        public static string NombreUsuario = "";
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;
        OracleCommand com = null;
        public VentanaCierreCaja()
        {
            InitializeComponent();
        }
        private void EnableUpdateButton()
        {
            bool allFieldsFilled = !string.IsNullOrWhiteSpace(txtUsuario.Text) &&
                                   !string.IsNullOrWhiteSpace(txtfecha.Text) &&
                                   !string.IsNullOrWhiteSpace(txtCodCaja.Text) &&
                                   !string.IsNullOrWhiteSpace(TxtMontoInicial.Text);
            btncierrarcaja.Enabled = allFieldsFilled;
        }
        private void VentanaCierreCaja_Load(object sender, EventArgs e)
        {
            CenterToParent();
            EnableUpdateButton();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCodCaja_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtCodCaja_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es un número ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
            if (txtCodCaja.Text.Length >= 5 && e.KeyChar != '\b')
            {
                // Si tiene 10 o más caracteres y la tecla presionada no es una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void TxtMontoInicial_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtfecha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void TxtMontoInicial_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtfecha_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void btncierrarcaja_Click(object sender, EventArgs e)
        {
            conn.Close();
            bool errorNfact = false;
            bool errorfecha = false;
            bool errorj = false;
            bool errora = false;
            bool errore = false;
            bool errorUs = false;
            string formatoFecha = "yyyy-MM-dd";
            // Validación del número de factura
            if (this.txtCodCaja.Text.Length != 5)
            {
                MessageBox.Show("Numero de Factura invalido, debe contener 5 digitos");
                errorNfact = true;
            }
            if (!DateTime.TryParseExact(txtfecha.Text, formatoFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                // La fecha ingresada tiene el formato correcto
                MessageBox.Show("Fecha invalida, debe tener el formato yyyy-MM-dd");
                errorfecha = true;
            }
            //

            if (DateTime.TryParseExact(txtfecha.Text, formatoFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaValidada))
            {
                string fecha = fechaValidada.ToString("yyyy-MM-dd");
                string strCo = "SELECT montoCierre FROM Caja WHERE  fecha = TO_DATE('" + fecha + "', 'YYYY-MM-DD')";
                comm = new OracleCommand(strCo, conn);
                conn.Open();
                object cod = comm.ExecuteScalar();
                conn.Close();
                if (cod != null)
                {
                    MessageBox.Show("Caja cerrada, verifica la fecha ");
                    errorj = true;
                }

                string o = "SELECT fecha_adq FROM NumeroOrdenCompra WHERE fecha_adq = TO_DATE('" + fecha + "', 'YYYY-MM-DD')";
                comm = new OracleCommand(o, conn);
                conn.Open();
                object ls = comm.ExecuteScalar();
                conn.Close();
                if (ls == null)
                {
                    MessageBox.Show("Aun no se han realizad transacciones en esa fecha");
                    errore = true;
                }


            }
            string us = "SELECT nomb_usuario FROM Empleados_uio WHERE nomb_usuario = '" + txtUsuario.Text + "'";
            comm = new OracleCommand(us, conn);
            conn.Open();
            object lu = comm.ExecuteScalar();
            conn.Close();
            if (lu == null)
            {
                MessageBox.Show("No existe el usuario");
                errorUs = true;
            }

            string a = "SELECT codigoCierreCaja FROM Caja WHERE codigoCierreCaja = '" + txtCodCaja.Text + "'";
            com = new OracleCommand(a, conn);
            conn.Open();
            object codci = com.ExecuteScalar();
            conn.Close();
            if (codci != null)
            {
                MessageBox.Show("El codigo de la Caja ya existe");
                errora = true;
            }
            // Verificar si hay errores
            if (!errora && !errorj && !errorNfact && !errorfecha && !errore && !errorUs)
            {

                Form MC = new VentanaConfirmarCierreCaja();
                Fecha = txtfecha.Text;
                NombreUsuario = txtUsuario.Text;
                MontoInicial = TxtMontoInicial.Text;
                CodigoCaja = txtCodCaja.Text;
                MC.ShowDialog();

            }
        }
    }
}
