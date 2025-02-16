using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VentanaRegistroCompras : Form
    {
        public static string NumeroFactura;
        public static string cicliente = "";
        public static string iva = "";
        public static string montototal = "";
        public static string ModoPago = "";
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;
        public VentanaRegistroCompras()
        {
            InitializeComponent();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Form Confirmar = new VentanaConfirmarBorrFC();
            NumeroFactura = txtNumFactura.Text;
            Confirmar.ShowDialog();
        }

        private void btnmodificar_Click(object sender, EventArgs e)
        {
            Form Confirmar = new VentanaConfirmarModFC();
            NumeroFactura = txtNumFactura.Text;
            iva = txtiva.Text;
            montototal = txttotal.Text;
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

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VentanaRegistroCompras_Load(object sender, EventArgs e)
        {
            strComm = "select*from numeroordencompra";
            OracleCommand cmd = new OracleCommand(strComm, conn);
            OracleDataAdapter adaptador = new OracleDataAdapter(cmd);
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);
            dataGridViewcompras.DataSource = tabla;
            dataGridViewcompras.ReadOnly = true;
        }

        private void dataGridViewcompras_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNumFactura.Text = dataGridViewcompras.SelectedCells[0].Value.ToString();
            txttotal.Text = dataGridViewcompras.SelectedCells[1].Value.ToString();
            txtiva.Text = dataGridViewcompras.SelectedCells[2].Value.ToString();
            txtfecha.Text = Convert.ToDateTime(dataGridViewcompras.SelectedCells[3].Value).ToShortDateString();
            if (dataGridViewcompras.SelectedCells[4].Value.ToString() == "Efectivo")
            {
                rdbEfectivo.Checked = true;
            }
            if (dataGridViewcompras.SelectedCells[4].Value.ToString() == "Transferencia")
            {
                rdbTransferencia.Checked = true;
            }
        }
    }
}
