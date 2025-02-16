using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VentanaConfirmarAddProd : Form
    {
        public VentanaConfirmarAddProd()
        {
            InitializeComponent();
        }
        static OracleConnection conn = DataAccess.getConn();
        OracleCommand comm = null;

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                comm.ExecuteNonQuery();
                MessageBox.Show("El producto fue ingresado con éxito");
                this.btnConfirmar.Enabled = false;
            }
            catch (OracleException ex)
            {
                if (ex.Message.Contains("ORA-00001"))
                {
                    MessageBox.Show("¡¡ERROR!!, El producto ya existe");
                    this.btnConfirmar.Enabled = false;
                }
                else
                {
                    MessageBox.Show("OracleException con código de error: " + ex.ErrorCode + "\nDetalles: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excepción no manejada: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void VentanaConfirmarAddProd_Load(object sender, EventArgs e)
        {
            CenterToParent();
            conn.Open();
            string strComm = "sp_InsertarProductos";
            comm = new OracleCommand(strComm, conn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add(new OracleParameter("p_NombreProducto", OracleType.VarChar)).Value = VentanaProductos.NombreProducto;
            comm.Parameters.Add(new OracleParameter("p_PrecioPaquete", OracleType.Number)).Value = VentanaProductos.PrecioXpaquete;
            comm.Parameters.Add(new OracleParameter("p_CodigoBarra", OracleType.Number)).Value = Convert.ToInt32(VentanaProductos.CodigoBarra);

        }
    }
}
