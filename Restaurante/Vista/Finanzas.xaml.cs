using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Restaurante.Vista
{
    /// <summary>
    /// Lógica de interacción para Finanzas.xaml
    /// </summary>
    public partial class Finanzas : Window
    {
        OracleConnection cone = new OracleConnection("Data Source = localhost:1521 / xe; password=123456; User id = RESTAURANT;");
        public Finanzas()
        {
            InitializeComponent();
            listardatos();
            CargarcomboMetodo();
            CargarcomboPedido();
        }
        public void listardatos()
        {
            OracleCommand comando = new OracleCommand("select nro_pedido, cant_pedidos, estado_pedido from detalle_pedido "
                + "where estado_pedido = 'pendiente de pago'", cone);
            cone.Open();
            DataTable dt = new DataTable();
            OracleDataAdapter adaptador = new OracleDataAdapter();
            adaptador.SelectCommand = comando;
            adaptador.Fill(dt);
            dgPedidos.ItemsSource = dt.AsDataView();
            dgPedidos.Items.Refresh();
            cone.Close();
        }
        public void CargarcomboPedido()
        {
            OracleCommand comando = new OracleCommand("select nro_pedido, id_detalle_pedido from detalle_pedido  where estado_pedido = 'pendiente de pago'", cone);
            cone.Open();
            OracleDataReader registro = comando.ExecuteReader();
            while (registro.Read())
            {
                cbPedido.Items.Add(registro["nro_pedido"].ToString());
            }
            registro.Close();
            cone.Close();

        }
        public void CargarcomboMetodo()
        {
            OracleCommand comando = new OracleCommand("select metodo_pago from metodo_pago", cone);
            cone.Open();
            OracleDataReader registro = comando.ExecuteReader();
            while (registro.Read())
            {
                cbmetodo.Items.Add(registro["metodo_pago"].ToString());
            }
            registro.Close();
            cone.Close();

        }
        private void Finalizar_Click(object sender, RoutedEventArgs e)
        {
            if (cbPedido.SelectedIndex > -1 && cbmetodo.SelectedIndex > -1)
            {
                //Obtenemos el id del detalle del pedido
                string indexPedido = cbPedido.Text;
                int indexMetodo = 1 + cbmetodo.SelectedIndex;
                string id = "";
                cone.Open();
                OracleCommand comando = new OracleCommand("select id_detalle_pedido from detalle_pedido where nro_pedido = " + indexPedido, cone);
                OracleDataReader registro = comando.ExecuteReader();
                while (registro.Read())
                {
                    id = registro["id_detalle_pedido"].ToString();
                }
                cone.Close();
                



                //Se envia el numero de pedido a finalizar  a la vez que este es pagado
                String lector = "update detalle_pedido set estado_pedido = 'finalizado' where id_detalle_pedido = " + indexPedido;
                cone.Open();
                OracleDataAdapter adaptador = new OracleDataAdapter(lector, cone);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                cone.Close();

                string hora = DateTime.Now.ToString("hh:mm:ss tt");
                string dia = DateTime.Today.ToString("dd/MM/yyyy");
                //Se envia el numero de pedido a finalizar
                String lector2 = "insert into boleta_de_pago (hora_pago, fecha_pago, id_metodo, id_detalle_pedido) values ('" + hora + "', '" + dia + "' , " + indexMetodo + ", " + id + ")";
                cone.Open();
                OracleDataAdapter adaptador2 = new OracleDataAdapter(lector2, cone);
                DataTable dt2 = new DataTable();
                adaptador2.Fill(dt2);
                cone.Close();
                listardatos();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un pedido o metodo de pago");

            }
        }

        private void actualizar_Click(object sender, RoutedEventArgs e)
        {
            listardatos();
        }
    }
}
