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
    public partial class VentanaRegistroCajas : Form
    {
        public static string CodigoCaja;
        public static string MontoInincial=" ";
        public static string MontoCierre = " ";
        public static string TranferenciaG = " ";
        public static string EfectivoG = " ";
        public static string TranferenciaI = " ";
        public static string EfectivoI = " ";
        public static string GastosTotales = " ";
        public static string IngresosTotales = " ";
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;
        public VentanaRegistroCajas()
        {
            InitializeComponent();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            conn.Close();
            bool errorbus = false;
            bool errorfecha = false;
            string formatoFecha = "yyyy-MM-dd";
            // Validación del número de factura
            if (!DateTime.TryParseExact(txtCajaBuscar.Text, formatoFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                // La fecha ingresada tiene el formato correcto
                MessageBox.Show("Fecha invalida, debe tener el formato yyyy-MM-dd");
                errorfecha = true;
            }
            if (!errorbus && !errorfecha && DateTime.TryParseExact(txtCajaBuscar.Text, formatoFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaValidada))
            {
                string strComm = "BEGIN sp_BuscarCaja(:p_fecha, :cursorMemoria); END;";

                OracleCommand cmd = new OracleCommand(strComm, conn);
                cmd.CommandType = CommandType.Text;

                // Crear parámetro de entrada
                OracleParameter paramFecha = new OracleParameter(":p_fecha", OracleType.DateTime); // Usa OracleType.DateTime en lugar de OracleType.Date
                paramFecha.Value = fechaValidada;  // Pasa directamente el valor de fechaValidada
                cmd.Parameters.Add(paramFecha);

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
                    dataGridViewCajas.DataSource = tabla;
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
        }

        private void VentanaRegistroCajas_Load(object sender, EventArgs e)
        {
            CenterToParent();
            // Utiliza la sintaxis de System.Data.OracleClient
            string strCommMostrar = "BEGIN sp_MostrarCajas(:cursorCaja); END;";

            OracleCommand cmdMostrar = new OracleCommand(strCommMostrar, conn);
            cmdMostrar.CommandType = CommandType.Text;

            // Crear parámetro de salida para el cursor
            OracleParameter cursorParamMostrar = new OracleParameter(":cursorCaja", OracleType.Cursor);
            cursorParamMostrar.Direction = ParameterDirection.Output;
            cmdMostrar.Parameters.Add(cursorParamMostrar);

            OracleDataAdapter adaptadorMostrar = new OracleDataAdapter(cmdMostrar);
            DataTable tablaMostrar = new DataTable();

            try
            {
                conn.Open();
                adaptadorMostrar.Fill(tablaMostrar);
                dataGridViewCajas.DataSource = tablaMostrar;
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Form Confirmar = new VentanaConfirmarBorrCaja();
            CodigoCaja = txtcodigocaja.Text;
            Confirmar.ShowDialog();
        }

        private void dataGridViewCajas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtcodigocaja.Text = dataGridViewCajas.SelectedCells[0].Value.ToString();
            txtfecha.Text = Convert.ToDateTime(dataGridViewCajas.SelectedCells[1].Value).ToShortDateString();
            txtnombreusuario.Text = dataGridViewCajas.SelectedCells[2].Value.ToString();
            txtciusuario.Text = dataGridViewCajas.SelectedCells[3].Value.ToString();
            txttransferenciasg.Text = dataGridViewCajas.SelectedCells[4].Value.ToString();
            txtefectivog.Text = dataGridViewCajas.SelectedCells[5].Value.ToString();
            txttranferenciasi.Text = dataGridViewCajas.SelectedCells[6].Value.ToString();
            txtefectivoi.Text = dataGridViewCajas.SelectedCells[7].Value.ToString();
            txtgastost.Text = dataGridViewCajas.SelectedCells[8].Value.ToString();
            txtingresost.Text = dataGridViewCajas.SelectedCells[9].Value.ToString();
            txtmontoi.Text = dataGridViewCajas.SelectedCells[10].Value.ToString();
            txtmontoc.Text = dataGridViewCajas.SelectedCells[11].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form Confirmar = new VentanaConfirmarModCaja();
            CodigoCaja = txtcodigocaja.Text;
            MontoInincial = txtmontoi.Text;
            MontoCierre = txtmontoc.Text;
            TranferenciaG = txttransferenciasg.Text;
            EfectivoG = txtefectivog.Text;
            TranferenciaI = txttranferenciasi.Text;
            EfectivoI = txtefectivoi.Text;
            GastosTotales = txtgastost.Text;
            IngresosTotales = txtingresost.Text;
        Confirmar.ShowDialog();
        }
    }
}
