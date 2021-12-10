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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data;
using System.Data.OracleClient;
//using RESTAURANTE.CONTROLADOR;



namespace Restaurante.Vista.paginas_administrador
{
    /// <summary>
    /// Lógica de interacción para adm_usuario.xaml
    /// </summary>
    public partial class adm_usuario : Page
    {
        string correo; string estado; string rut; string nombre; string apellido;//SP_CREAR_EMPLEADOS
        
        static String Conexion = "Data Source=localhost:1521/xe; password=123456; User id=restaurant";
        static OracleConnection cone = new OracleConnection();


        public adm_usuario()
        {
            InitializeComponent();
        }
        //Esta funcion sirve para mostrar los controles necesarios para crear un nuevo usuario siendo administrador, solo cuando se desee crearlo
        private void mostrarCrear()
        {
            lblCorreo.Visibility = Visibility.Visible;
            lblestado.Visibility = Visibility.Visible;
            lblpass.Visibility = Visibility.Visible;
            lbltipo.Visibility = Visibility.Visible;
            lblnomusuario.Visibility = Visibility.Visible;
            lb_apellido.Visibility = Visibility.Visible;
            txtCorreo.Visibility = Visibility.Visible;
            txtEstado.Visibility = Visibility.Visible;
            txtnom.Visibility = Visibility.Visible;
            txt_rut.Visibility = Visibility.Visible;
            txt_apellido.Visibility = Visibility.Visible;
            cbtipo.Visibility = Visibility.Visible;
            Crear_confirmacion.Visibility = Visibility.Visible;
        }
        //listar
        private void listar()
        {
            cone.ConnectionString = Conexion;
            cone.Open();

            OracleCommand cmd = new OracleCommand("SP_LISTAR_USUARIOS", cone);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("registros", OracleType.Cursor).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            
            OracleDataAdapter adaptador = new OracleDataAdapter();
            adaptador.SelectCommand = cmd;
            
            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            dgUsuarios.ItemsSource = dt.AsDataView();
            dgUsuarios.Items.Refresh();



            cone.Close();
        }
        //Ocultamos los controles para crear un nuevo usuario en caso de no ser necesario su uso(al querer eliminar, listar o actualizar)
        private void ocultarCrear()
        {

            lblCorreo.Visibility = Visibility.Hidden;
            lblestado.Visibility = Visibility.Hidden;
            lblpass.Visibility = Visibility.Hidden;
            lbltipo.Visibility = Visibility.Hidden;
            lblnomusuario.Visibility = Visibility.Hidden;
            lb_apellido.Visibility = Visibility.Hidden;
            txtCorreo.Visibility = Visibility.Hidden;
            txtEstado.Visibility = Visibility.Hidden;
            txtnom.Visibility = Visibility.Hidden;
            txt_rut.Visibility = Visibility.Hidden;
            txt_apellido.Visibility = Visibility.Hidden;
            cbtipo.Visibility = Visibility.Hidden;
            Crear_confirmacion.Visibility = Visibility.Hidden;
        }
        //Lo mismo de arriba, en este caso mostrando los controles al querer eliminar un usuario
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
        //Segun el ComboItem elegido para el tipo de usuario, le entregaremos un valor entero para poder enviarlo a la base de datos
        private int tipo_usuario()
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

        //Cuando el administrador desee crear un nuevo usuario apareceran los controles necesarios
        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            mostrarCrear();
            ocultarEliminar();
        }
        //Funcion para Listar o mostrar los datos que hay actualmente en usuarios
        private void btnListar_Click(object sender, RoutedEventArgs e)
        {
            ocultarCrear();
            ocultarEliminar();
            listar();
        }
        //Cuando el administrador desee eliminar un usuario apareceran los controles necesarios
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            mostrarEliminar();
            ocultarCrear();
            eliminartxt.IsEnabled = true;
            confirmar.IsEnabled = true;

            listar();
        }
        //El administrador confirma que desea eliminar y se realiza el proceso junto a la base de datos
        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            
            if (eliminartxt.Text!="")
            {
                //Se abre la conexion
            
                cone.ConnectionString = Conexion;
                cone.Open();
                //Se envia el nombre del usuario a eliminar

                OracleCommand cmd = new OracleCommand("SP_DESCACTIVAR_USUARIO", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("nombre", OracleType.Char).Value = eliminartxt.Text;
                cmd.ExecuteNonQuery();
                cone.Close();
                listar();
                MessageBox.Show("usuario desactivado");

            }
            else
            {
                MessageBox.Show("Debe ingresar nombre de usuario a eliminar");
            }

        }
        //El administrador confirma que desea crear y se realiza el proceso junto a la base de datos
        private void Crear_confirmacion_Click(object sender, RoutedEventArgs e)
        {
            if (txtnom.Text != "" && txt_rut.Text != "" && txtCorreo.Text != ""
                && txtEstado.Text != "" && txt_apellido.Text != "")
            {
                //llamamos a la funcion antes mencionada para obtener que tipo de insumo es y le asignamos un valor local
                int tipo = tipo_usuario();
                //Abrimos la conexion con la bd

                 correo      = txtCorreo.Text;
                 estado      = txtEstado.Text; 
                 rut         = txt_rut.Text;
                 nombre      = txtnom.Text;
                 apellido    = txt_apellido.Text;

                cone.ConnectionString = Conexion;
               
                //Realizamos el insert con todos sus parametros necesarios
                OracleCommand cmd = new OracleCommand("SP_CREAR_EMPLEADO", cone);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("p_correo", OracleType.Char).Value = correo;
                cmd.Parameters.Add("p_estado", OracleType.Char).Value = estado;
                cmd.Parameters.Add("p_rut_empleado", OracleType.Char).Value = rut;
                cmd.Parameters.Add("p_nombre", OracleType.Char).Value = nombre;
                cmd.Parameters.Add("p_ap", OracleType.Char).Value = apellido;
                cmd.Parameters.Add("p_tipo_usuario", OracleType.Number).Value = tipo;
                cone.Open();
                cmd.ExecuteNonQuery();


                cone.Close();

                MessageBox.Show("creacion exitosa");
                listar();
            }
            else
            {
                MessageBox.Show("faltan campos por rellenar");
            }
        }
    }
}
