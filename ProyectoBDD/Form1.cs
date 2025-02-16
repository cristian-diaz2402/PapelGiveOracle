using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VenatanaPrincipal : Form
    {
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;
        public VenatanaPrincipal()
        {
            InitializeComponent();
        }

        private void conexion_Click(object sender, EventArgs e)
        {
            VentanaClientes Vclientes = new VentanaClientes();
            Vclientes.ShowDialog();
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSede_Click(object sender, EventArgs e)
        {
            
        }

        private void btnempleados_Click(object sender, EventArgs e)
        {
            VentanaEmpleados Vempleados = new VentanaEmpleados();
            Vempleados.ShowDialog();
        }

        private void btnproductos_Click(object sender, EventArgs e)
        {
            VentanaProductos Vproductos = new VentanaProductos();
            Vproductos.ShowDialog();
        }

        private void btnfacturasv_Click(object sender, EventArgs e)
        {
            
        }

        private void bntproveedores_Click(object sender, EventArgs e)
        {
            VentanaProveedores Vproveedores = new VentanaProveedores();
            Vproveedores.ShowDialog();
        }

        private void btnitemsv_Click(object sender, EventArgs e)
        {
            
        }

        private void btnnoc_Click(object sender, EventArgs e)
        {
           
        }

        private void btnitemsc_Click(object sender, EventArgs e)
        {
           
        }

        private void btncaja_Click(object sender, EventArgs e)
        {
            VentanaCaja Vcaja = new VentanaCaja();
            Vcaja.ShowDialog();
        }

        private void btntransacciones_Click(object sender, EventArgs e)
        {
            VentanaTransacciones Vclientes = new VentanaTransacciones();
            Vclientes.ShowDialog();
        }

        private void VenatanaPrincipal_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }

        private void btnAuditorias_Click(object sender, EventArgs e)
        {
            VentanaAuditorias VA = new VentanaAuditorias();
            VA.ShowDialog();
        }
    }
}
