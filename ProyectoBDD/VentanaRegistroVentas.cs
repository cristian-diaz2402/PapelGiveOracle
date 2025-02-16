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
    public partial class VentanaRegistroVentas : Form
    {
        public static string NumeroFactura;
        public static string cicliente = "";
        public static string iva = "";
        public static string montototal = "";
        public static string ModoPago = "";
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;
        public VentanaRegistroVentas()
        {
            InitializeComponent();
        }

        private void VentanaRegistroVentas_Load(object sender, EventArgs e)
        {
            strComm = "select*from facturasv";
            OracleCommand cmd = new OracleCommand(strComm, conn);
            OracleDataAdapter adaptador = new OracleDataAdapter(cmd);
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);
            dataGridViewfacturasv.DataSource = tabla;
            dataGridViewfacturasv.ReadOnly = true;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Form Confirmar = new VentanaConfirmarBorrFV();
            NumeroFactura = txtNumFactura.Text;
            Confirmar.ShowDialog();
        }

        private void dataGridViewfacturasv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNumFactura.Text = dataGridViewfacturasv.SelectedCells[0].Value.ToString();
            txttotal.Text = dataGridViewfacturasv.SelectedCells[1].Value.ToString();
            txtiva.Text = dataGridViewfacturasv.SelectedCells[2].Value.ToString();
            txtfecha.Text = Convert.ToDateTime(dataGridViewfacturasv.SelectedCells[3].Value).ToShortDateString();
            txtCI.Text = dataGridViewfacturasv.SelectedCells[4].Value.ToString();
            if (dataGridViewfacturasv.SelectedCells[5].Value.ToString()=="Efectivo")
            {
                rdbEfectivo.Checked = true;
            }
            if (dataGridViewfacturasv.SelectedCells[5].Value.ToString() == "Transferencia")
            {
                rdbTransferencia.Checked = true;
            }

        }

        private void btnmodificar_Click(object sender, EventArgs e)
        {
            Form Confirmar = new VentanaConfirmarModFv();
            NumeroFactura = txtNumFactura.Text;
            cicliente = txtCI.Text;
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
    }
}
