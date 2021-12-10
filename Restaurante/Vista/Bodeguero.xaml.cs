using System;
using System.Collections.Generic;
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
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data;
using System.Data.OracleClient;

namespace Restaurante.Vista
{
    /// <summary>
    /// Lógica de interacción para Bodeguero.xaml
    /// </summary>
    public partial class Bodeguero : Window
    {

        OracleConnection cone = new OracleConnection("Data Source = localhost:1521 / xe; password=123456; User id = RESTAURANT;");
        public Bodeguero()
        {
            InitializeComponent();
        }
        public void mostrarIngresar()
        {
            lblDescripcion.Visibility = Visibility.Visible;
            lblinsumo.Visibility = Visibility.Visible;
            lblPrecio.Visibility = Visibility.Visible;
            lblStock.Visibility = Visibility.Visible;
            lblTipo.Visibility = Visibility.Visible;
            txtDescripcion.Visibility = Visibility.Visible;
            txtInsumo.Visibility = Visibility.Visible;
            txtPrecio.Visibility = Visibility.Visible;
            txtStock.Visibility = Visibility.Visible;
            cbTipo.Visibility = Visibility.Visible;
            btnConfirmar.Visibility = Visibility.Visible;
        }
        public void ocultarIngresar()
        {
            lblDescripcion.Visibility = Visibility.Hidden;
            lblinsumo.Visibility = Visibility.Hidden;
            lblPrecio.Visibility = Visibility.Hidden;
            lblStock.Visibility = Visibility.Hidden;
            lblTipo.Visibility = Visibility.Hidden;
            txtDescripcion.Visibility = Visibility.Hidden;
            txtInsumo.Visibility = Visibility.Hidden;
            txtPrecio.Visibility = Visibility.Hidden;
            txtStock.Visibility = Visibility.Hidden;
            cbTipo.Visibility = Visibility.Hidden;
            btnConfirmar.Visibility = Visibility.Hidden;
        }
        public void mostrarSolicitar()
        {
            lblCant.Visibility = Visibility.Visible;
            lblnom2.Visibility = Visibility.Visible;
            txtNom2.Visibility = Visibility.Visible;
            txtCant.Visibility = Visibility.Visible;
            btnsolicitar.Visibility = Visibility.Visible;
        }
        public void ocultarSolicitar()
        {
            lblCant.Visibility = Visibility.Hidden;
            lblnom2.Visibility = Visibility.Hidden;
            txtCant.Visibility = Visibility.Hidden;
            txtNom2.Visibility = Visibility.Hidden;
            btnsolicitar.Visibility = Visibility.Hidden;
        }
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
        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            mostrarIngresar();
            ocultarSolicitar();
        }

        private void controlar_Click(object sender, RoutedEventArgs e)
        {
            ocultarIngresar();
            mostrarSolicitar();
        }
        public void listar()
        {
            cone.Open();
            OracleCommand comando = new OracleCommand("listarStock", cone);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.Parameters.Add("registro", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataAdapter adaptador = new OracleDataAdapter();
            adaptador.SelectCommand = comando;
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);
            dgInsumos.ItemsSource = tabla.DefaultView;

            cone.Close();

        }

        private void Listar_Click(object sender, RoutedEventArgs e)
        {
            ocultarIngresar();
            listar();
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (txtInsumo.Text != "" && txtPrecio.Text != "" && txtDescripcion.Text != "" && txtStock.Text != ""
              && int.TryParse(txtPrecio.Text, out int result) == true
              && int.TryParse(txtStock.Text, out int result2) == true)
            {
                OracleCommand comando = new OracleCommand("sp_ingresar_insumos", cone);
                comando.CommandType = System.Data.CommandType.StoredProcedure;



                comando.Parameters.Add("nom", OracleType.VarChar).Value = txtInsumo.Text;
                comando.Parameters.Add("precio", OracleType.Int32).Value = int.Parse(txtPrecio.Text);
                comando.Parameters.Add("descripcion", OracleType.VarChar).Value = txtDescripcion.Text;
                comando.Parameters.Add("stock", OracleType.Int32).Value = int.Parse(txtStock.Text);
                comando.Parameters.Add("tipo", OracleType.Int32).Value = cbTipo.SelectedIndex + 1;
                cone.Open();
                comando.ExecuteNonQuery();
                cone.Close();
                listar();
                MessageBox.Show("Insumo agregado con exito");
            }
            else
            {
                if (int.TryParse(txtPrecio.Text, out int result3) == true ||
                    int.TryParse(txtStock.Text, out int result4) == true)
                    MessageBox.Show("Stock o precio invalido, solo debe ingresar numeros");
                else
                    MessageBox.Show("faltan campos por rellenar");
            }
        }

        private void btnsolicitar_Click(object sender, RoutedEventArgs e)
        {
            if(txtCant.Text != "" && txtNom2.Text != "" && int.TryParse(txtCant.Text, out int result) == true)
            {
                string id = "";
                cone.Open();
                OracleCommand comando = new OracleCommand("select id_insumo from insumos where nom_insumo = '" + txtNom2.Text + "'", cone);
                OracleDataAdapter adaptador = new OracleDataAdapter();
                OracleDataReader registro = comando.ExecuteReader();
                while (registro.Read())
                {
                    id = registro["id_insumo"].ToString();
                }
                string desc = txtNom2.Text + ", se necesitan  " + txtCant.Text + ", " + DateTime.Today.ToShortDateString();
                String lector = "insert into solicitud_reposicion (descripcion, estado, id_insumo) values ('"+desc + "','en espera de aprobacion', " + id + ")";
                OracleDataAdapter adaptador2 = new OracleDataAdapter(lector, cone);
                DataTable dt = new DataTable();
                adaptador2.Fill(dt);
                MessageBox.Show("Solicitud enviada correctamente");
                cone.Close();
            }
            else
            {
                if (int.TryParse(txtCant.Text, out int result2) == false)
                    MessageBox.Show("Cantidad ingresada invalida, recuerde solo ingresar numeros");
                else
                    MessageBox.Show("Faltan datos por ingresar");
            }
        }
    }
}
