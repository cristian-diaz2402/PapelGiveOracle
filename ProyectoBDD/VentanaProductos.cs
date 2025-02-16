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
    public partial class VentanaProductos : Form
    {
        public VentanaProductos()
        {
            InitializeComponent();
        }
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;
        OracleCommand comma = null;
        private void EnableUpdateButton()
        {
            bool allFieldsFilled = !string.IsNullOrWhiteSpace(txtNomProd.Text) &&
                                   !string.IsNullOrWhiteSpace(txtCodBarra.Text) &&
                                   !string.IsNullOrWhiteSpace(txtPrecio.Text);
            btnAgregar.Enabled = allFieldsFilled;
        }
        private void btnsalir_Click(object sender, EventArgs e)
        {
     
        }
        public static string CodigoBarra = "";
        public static string NombreProducto = "";
        public static decimal PrecioXpaquete;
        private void VentanaProductos_Load(object sender, EventArgs e)
        {
            CenterToParent();
            Random random = new Random();
            int count = 1;
            while (count != 0)
            {
                int numeroAleatorio = random.Next(0, 100000);
                string numeroFormateado = numeroAleatorio.ToString("D5");
                string strC = "SELECT COUNT(*) FROM Productos WHERE id_producto = '" + numeroFormateado + "'";
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
                    this.txtCodBarra.Text = numeroFormateado;
                }
            }
            // Utiliza la sintaxis de System.Data.OracleClient
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
                dataGridViewProductos.DataSource = tabla;
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
            dataGridViewProductos.ReadOnly = true;
        }


        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNomProd.Text = dataGridViewProductos.SelectedCells[1].Value.ToString();
            txtCodBarra.Text = dataGridViewProductos.SelectedCells[0].Value.ToString();
            txtPrecio.Text = dataGridViewProductos.SelectedCells[2].Value.ToString();
        }

        private void txtNomProd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '\b')
            {
                // Si no es una letra ni una tecla de borrar, se ignora la entrada del usuario
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

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
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

        private void dataGridViewProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewProductos.SelectedRows.Count > 0)
            {
                // Habilitar el botón si se ha seleccionado una fila
                btnModificar.Enabled = true;
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
                string strC = "SELECT COUNT(*) FROM Productos WHERE id_producto = '" + numeroFormateado + "'";
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
                    this.txtCodBarra.Text = numeroFormateado;
                    this.txtNomProd.Clear();
                    this.txtPrecio.Clear();
                }

            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal PrecioXPaquete = decimal.Parse(txtPrecio.Text);
            Form Confirmar = new VentanaConfirmarAddProd();
            NombreProducto = txtNomProd.Text;
            PrecioXpaquete = PrecioXPaquete;
            CodigoBarra = txtCodBarra.Text; 
            Confirmar.ShowDialog();
        }

        private void txtCodBarra_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtNomProd_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
        }

        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
            EnableUpdateButton();
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
                dataGridViewProductos.DataSource = tabla;
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

        private void btnModificar_Click(object sender, EventArgs e)
        {
            bool errorEnId2 = false;
            string commando = "SELECT id_producto FROM productos WHERE id_producto = '" + this.txtCodBarra.Text + "' AND ROWNUM <= 1";
            comm = new OracleCommand(commando, conn); // Asignar la conexión a comm
            conn.Open(); // Abrir la conexión
            object respuesta = comm.ExecuteScalar();
            conn.Close(); // Cerrar la conexión después de usarla
            if (respuesta == null)
            {
                MessageBox.Show(" ¡¡ERROR!!, No puedes modificar el codigo de barras");
                errorEnId2 = true;
            }
            if (!errorEnId2)
            {
                decimal PrecioXPaquete = decimal.Parse(txtPrecio.Text);
                Form Confirmar = new VentanaConfirmarModProd();
                NombreProducto = txtNomProd.Text;
                PrecioXpaquete = PrecioXPaquete;
                CodigoBarra = txtCodBarra.Text;
                Confirmar.ShowDialog();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Form Confirmar = new VentanaConfirmarBorrProd();
            CodigoBarra = txtCodBarra.Text;
            Confirmar.ShowDialog();
        }
    }
    
}
