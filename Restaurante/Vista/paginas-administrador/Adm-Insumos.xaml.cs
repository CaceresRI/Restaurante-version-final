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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurante.Vista.paginas_administrador
{
    /// <summary>
    /// Lógica de interacción para Adm_Insumos.xaml
    /// </summary>
    public partial class Adm_Insumos : Page
    { 
        //abrimos la conexion nuevamente
        String Conexion = "Data Source=localhost:1521/xe; password=123456; User id=RESTAURANT";
        OracleConnection cone = new OracleConnection();
        public Adm_Insumos()
        {
            InitializeComponent();
        }
        //Esta funcion sirve para mostrar los controles necesarios para crear un nuevo insumo siendo administrador, solo cuando se desee crearlo
        private void mostrarCrear()
        {
            lblDescripcion.Visibility = Visibility.Visible;
            lblinsumo.Visibility = Visibility.Visible;
            lblprecio.Visibility = Visibility.Visible;
            lblStock.Visibility = Visibility.Visible;
            lbltipo.Visibility = Visibility.Visible;
            txtDescripcion.Visibility = Visibility.Visible;
            txtinsumos.Visibility = Visibility.Visible;
            txtPrecio.Visibility = Visibility.Visible;
            txtStock.Visibility = Visibility.Visible;
            cbtipo.Visibility = Visibility.Visible;
            Crear_confirmacion.Visibility = Visibility.Visible;
        }
        //Ocultamos los controles para crear un nuevo insumos en caso de no ser necesario su uso(al querer eliminar, listar o actualizar)
        private void ocultarCrear()
        {

            lblDescripcion.Visibility = Visibility.Hidden;
            lblinsumo.Visibility = Visibility.Hidden;
            lblprecio.Visibility = Visibility.Hidden;
            lbltipo.Visibility = Visibility.Hidden;
            lblStock.Visibility = Visibility.Hidden;
            txtDescripcion.Visibility = Visibility.Hidden;
            txtinsumos.Visibility = Visibility.Hidden;
            txtPrecio.Visibility = Visibility.Hidden;
            txtStock.Visibility = Visibility.Hidden;
            cbtipo.Visibility = Visibility.Hidden;
            Crear_confirmacion.Visibility = Visibility.Hidden;
        }
        //Lo mismo de arriba, en este caso mostrando los controles al querer eliminar un insumo
        private void mostrarEliminar()
        {
            eliminartxt.Visibility = Visibility.Visible;
            confirmar.Visibility = Visibility.Visible;
        }
        //Otra vez ocultamos los controles cuando seran utilizados 
        private void ocultarEliminar()
        {
            eliminartxt.Visibility = Visibility.Hidden;
            confirmar.Visibility = Visibility.Hidden;
        }
        //Segun el ComboItem elegido para el tipo de insumo, le entregaremos un valor entero para poder enviarlo a la base de datos
        private int tipo_Insumo()
        {
            int tipo = 0;
            if (_1.IsSelected)
                tipo = 1;
            else if (_2.IsSelected)
                tipo = 2;
            else if (_3.IsSelected)
                tipo = 3;
            else if (_4.IsSelected)
                tipo = 4;
            else if (_5.IsSelected)
                tipo = 5;
            else if (_6.IsSelected)
                tipo = 6;
            return tipo;
        }
        //Cuando el administrador desee crear un nuevo insumo apareceran los controles necesarios
        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            mostrarCrear();
            ocultarEliminar();
        }
        //Funcion para Listar o mostrar los datos que hay actualmente en insumos
        private void btnListar_Click(object sender, RoutedEventArgs e)
        {
            ocultarCrear();
            ocultarEliminar();
            cone.ConnectionString = Conexion;
            cone.Open();
            DataTable dt = new DataTable();
            String lector = "select * from Insumos";
            OracleDataAdapter adaptador = new OracleDataAdapter(lector, Conexion);
            adaptador.Fill(dt);
            dgInsumos.ItemsSource = dt.AsDataView();
            dgInsumos.Items.Refresh();
            cone.Close();
        }
        //Cuando el administrador desee eliminar un insumo apareceran los controles necesarios
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            mostrarEliminar();
            ocultarCrear();
            eliminartxt.IsEnabled = true;
            confirmar.IsEnabled = true;
        }
        //El administrador confirma que desea eliminar y se realiza el proceso junto a la base de datos
        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (eliminartxt.Text != "")
            {
                //Se abre la conexion
                cone.ConnectionString = Conexion;
                cone.Open();
                //Se envia el nombre del insumo a eliminar
                String lector = "delete from insumos where nom_insumo ='" + eliminartxt.Text + "'";
                OracleDataAdapter adaptador = new OracleDataAdapter(lector, Conexion);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                dgInsumos.ItemsSource = dt.AsDataView();
                dgInsumos.Items.Refresh();
                cone.Close();
            }else
            {
                MessageBox.Show("ingrese insumo a eliminar");
            }
        }
        //El administrador confirma que desea crear y se realiza el proceso junto a la base de datos
        private void Crear_confirmacion_Click(object sender, RoutedEventArgs e)
        {
            if (txtinsumos.Text != "" && txtPrecio.Text != "" && txtDescripcion.Text != "" && txtStock.Text != ""
                && int.TryParse(txtPrecio.Text, out int result) == true
                && int.TryParse(txtStock.Text, out int result2) == true)
            {
                //llamamos a la funcion antes mencionada para obtener que tipo de insumo es y le asignamos un valor local
                int tipo = tipo_Insumo();
                //Abrimos la conexion con la bd
                cone.ConnectionString = Conexion;
                cone.Open();
                //Realizamos el insert con todos sus parametros necesarios
                String lector = "insert into insumos (nom_insumo, precio_unitario, descripcion, stock, id_tipoinsumo) values ('" + txtinsumos.Text + "'," + txtPrecio.Text + ",'" + txtDescripcion.Text + "', " + txtStock.Text + "," + tipo + ")";
                OracleDataAdapter adaptador = new OracleDataAdapter(lector, Conexion);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                dgInsumos.ItemsSource = dt.AsDataView();
                dgInsumos.Items.Refresh();
                cone.Close();
                MessageBox.Show("Ingreso correcto");
            }else
            {
                if (int.TryParse(txtPrecio.Text, out int result3) == true ||
                    int.TryParse(txtStock.Text, out int result4) == true)
                    MessageBox.Show("Stock o precio invalido, solo debe ingresar numeros");
                else
                    MessageBox.Show("faltan campos por rellenar");
            }
        }
    }
}
