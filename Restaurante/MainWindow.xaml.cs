using System;
using System.Collections.Generic;
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
using System.Linq;
namespace Restaurante
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //entregamos los parametros de conexion
        OracleConnection cone = new OracleConnection("Data Source = localhost:1521 / xe; password=123456; User id = RESTAURANT;");

        public MainWindow()
        {
            InitializeComponent();
        }
        //funcion para realizar el login
        private void Entrar_Click(object sender, RoutedEventArgs e)
        {
            if (user.Text != "" && password.Password.ToString() != "")
            {
                //abrimos la conexion
                cone.Open();
                String login = "";
                //realizamos la consulta verificando que hayan datos correspondientes a los ingresados por el usuario
                String lector = "select id_tipo_usuario from usuario where nom_usuario = ('" + user.Text + "') and contraseña = '" + password.Password.ToString() + "'";
                OracleDataAdapter adaptador = new OracleDataAdapter(lector, cone);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                //si la consulta devuelve 1 fila esto significa que los datos son correctos y podemos iniciar el loign
                if (dt.Rows.Count > 0)
                {
                    login = "login exitoso";
                    //Estos if son para filtrar segun el tipo de usuario logeado al sistema y enviarlo a su vista correspondiente
                    if (dt.Rows[0]["id_tipo_usuario"].ToString().Equals("1"))
                    {
                        MessageBox.Show(login);
                        Vista.Administrador adm = new Vista.Administrador();
                        adm.ShowDialog();
                    }
                    else if (dt.Rows[0]["id_tipo_usuario"].ToString().Equals("2"))
                    {
                        MessageBox.Show("Nada que ver por aqui! prueba intentando a ingresar a nuestro sitio web");
                    }
                    else if (dt.Rows[0]["id_tipo_usuario"].ToString().Equals("3"))
                    {
                        MessageBox.Show(login);
                        Vista.Finanzas fin = new Vista.Finanzas();
                        fin.ShowDialog();
                    }
                    else if (dt.Rows[0]["id_tipo_usuario"].ToString().Equals("4"))
                    {
                        MessageBox.Show(login);
                        Vista.Cocina.MainCocina coc = new Vista.Cocina.MainCocina();
                        coc.ShowDialog();
                    }
                    else if (dt.Rows[0]["id_tipo_usuario"].ToString().Equals("5"))
                    {
                        MessageBox.Show("Nada que ver por aqui! prueba intentando a ingresar a nuestro sitio web");
                    }
                    else if (dt.Rows[0]["id_tipo_usuario"].ToString().Equals("6"))
                    {
                        MessageBox.Show(login);
                        Vista.Bodeguero bod = new Vista.Bodeguero();
                        bod.ShowDialog();
                    }
                    user.Text = "";
                    password.Password = "";
                }
                else
                {
                    login = "credenciales erroneas";
                    MessageBox.Show(login);
                }
                cone.Close();
            }else
            {
                MessageBox.Show("Rellene los campos necesarios");
            }
        }
    }
}
