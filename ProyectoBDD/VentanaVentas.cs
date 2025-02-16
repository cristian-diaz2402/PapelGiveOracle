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
    public partial class VentanaVentas : Form
    {
        public VentanaVentas()
        {
            InitializeComponent();
        }
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;
        OracleCommand com = null;

        public static string NumeroFactura;
        public static string Fecha = "";
        public static string CodigoBarra = "";
        public static string Cantidad = "";
        public static string NombreProducto = "";
        public static string ModoPago = "";
        public static string CedulaRUC = "";
        private void EnableUpdateButton()
        {
            bool allFieldsFilled = !string.IsNullOrWhiteSpace(txtNombreProd.Text) &&
                                   !string.IsNullOrWhiteSpace(cantidad.Text) &&
                                   !string.IsNullOrWhiteSpace(txtCodBarra.Text) &&
                                   !string.IsNullOrWhiteSpace(txtCI.Text) &&
                                   rdbEfectivo.Checked || rdbTransferencia.Checked;
            btnAceptar.Enabled = allFieldsFilled;
        }
        private void VentanaVentas_Load(object sender, EventArgs e)
        {
            CenterToParent();
            Random random = new Random();
            int count = 1;
            while (count != 0)
            {
                int numeroAleatorio = random.Next(0, 100000);
                string numeroFormateado = numeroAleatorio.ToString("D5");
                string strC = "SELECT COUNT(*) FROM FacturasV WHERE num_factura = '" + numeroFormateado + "'";
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
                    this.txtNumFactura.Text = numeroFormateado;
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
                dataGridViewVentas.DataSource = tabla;
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
            EnableUpdateButton();
            dataGridViewVentas.ReadOnly = true;

        }


        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombreProd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es una letra ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void cantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es un número ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
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

        private void txtNombreProd_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void cantidad_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtCodBarra_TextChanged(object sender, EventArgs e)
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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            conn.Close();
            bool errorNfact = false;
            bool errorCodbarr = false;
            bool errorCI = false;
            bool errorCant = false;
            bool errorb = false;

            // Validación del número de factura
            if (this.txtNumFactura.Text.Length != 5)
            {
                MessageBox.Show("Numero de Factura invalido, debe contener 5 digitos");
                errorNfact = true;
            }

            // Validación del código de barras
            if (this.txtCodBarra.Text.Length != 5)
            {
                MessageBox.Show("Codigo de barras invalido, debe contener 5 digitos");
                errorCodbarr = true;
            }

            // Validación de la cédula
            if (this.txtCI.Text.Length != 10)
            {
                MessageBox.Show("Cedula invalida, debe contener 10 digitos");
                errorCI = true;
            }

            // Verificar si el producto existe
            string strCo = "SELECT cant FROM Productos WHERE id_producto = '" + txtCodBarra.Text + "' and nombre_prod = '" + txtNombreProd.Text + "'";
            com = new OracleCommand(strCo, conn);
            conn.Open();
            object cod = com.ExecuteScalar();
            conn.Close();
            if (cod != DBNull.Value)
            {
                int codint = Convert.ToInt32(cod);
                int cant = Convert.ToInt32(cantidad.Text);
                if (codint <= cant)
                {
                    MessageBox.Show(" ¡¡ERROR!!, no hay suficientes paquetes para vender, se suguiere comprar más");
                    errorCant = true;
                }
                if (cant == 0)
                {
                    MessageBox.Show(" ¡¡ERROR!!, La cantidad no puede ser 0");
                    errorb = true;
                }
                if (!errorCodbarr && !errorNfact && !errorCI && !errorCant && !errorb)
                {
                    Form Confirmar = new VentanaConfirmarVenta();
                    NumeroFactura = txtNumFactura.Text;
                    Fecha = txtfecha.Text;
                    CodigoBarra = txtCodBarra.Text;
                    NombreProducto = txtNombreProd.Text;
                    Cantidad = cantidad.Text;
                    CedulaRUC = txtCI.Text;
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
            else
            {
                MessageBox.Show("No hay productos");
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
                dataGridViewVentas.DataSource = tabla;
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

        private void txtNumFactura_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es un número ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
            if (txtNumFactura.Text.Length >= 5 && e.KeyChar != '\b')
            {
                // Si tiene 10 o más caracteres y la tecla presionada no es una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int count = 1;
            while (count != 0)
            {
                int numeroAleatorio = random.Next(0, 100000);
                string numeroFormateado = numeroAleatorio.ToString("D5");
                string strC = "SELECT COUNT(*) FROM FacturasV WHERE num_factura = '" + numeroFormateado + "'";
                comm = new OracleCommand(strC, conn);

                try
                {
                    conn.Open();
                    count = (int)comm.ExecuteScalar();
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
                    this.txtNumFactura.Text = numeroFormateado;
                    this.txtNombreProd.Clear();
                    this.txtCodBarra.Clear();
                    this.txtCI.Clear();
                    this.txtNombreCli.Clear();
                    this.cantidad.Clear();
                    this.rdbEfectivo.Checked = false;
                    this.rdbTransferencia.Checked = false;
                }
            }
        }

        private void dataGridViewVentas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNombreProd.Text = dataGridViewVentas.SelectedCells[1].Value.ToString();
            txtCodBarra.Text = dataGridViewVentas.SelectedCells[0].Value.ToString();
        }

        private void txtCI_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es un número ni una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
            if (txtCI.Text.Length >= 10 && e.KeyChar != '\b')
            {
                // Si tiene 10 o más caracteres y la tecla presionada no es una tecla de borrar, se ignora la entrada del usuario
                e.Handled = true;
            }
        }

        private void txtCI_TextChanged(object sender, EventArgs e)
        {
            if (this.txtCI.Text.Length == 10 && !string.IsNullOrEmpty(txtCI.Text))
            {
                string strC = "SELECT TO_CHAR(PrimerNom_cli) || ' ' || TO_CHAR(SegundoNom_cli) || ' ' || TO_CHAR(PrimerApp_cli) || ' ' || TO_CHAR(SegundoApp_cli) FROM Clientes_uio WHERE id_cliente = '" + txtCI.Text + "'";
                comm = new OracleCommand(strC, conn);
                conn.Open();
                object resultado = comm.ExecuteScalar();

                if (resultado == null)
                {
                    MessageBox.Show("No existe el cliente, debe registrarlo previamente");

                }
                else
                {
                    txtNombreCli.Text = resultado.ToString();
                }
                conn.Close();
            }
            EnableUpdateButton();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Form Confirmar = new VentanaConfirmarBorrFV();
            NumeroFactura = txtNumFactura.Text;
            Confirmar.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
