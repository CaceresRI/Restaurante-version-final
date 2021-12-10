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
    /// Lógica de interacción para Administrador.xaml
    /// </summary>
    public partial class Administrador : Window
    {
        public Administrador()
        {
            InitializeComponent();
        }
        //En la vista del administrador cada boton abre la pestaña correspondiente
        //Este boton abre la pestaña de administrar los usuarios
        private void AdmUsuario_Click(object sender, RoutedEventArgs e)
        {
           VistaAdm.Content = new Vista.paginas_administrador.adm_usuario();
        }
        //Este boton abre la pestaña de administrar los insumos
        private void AdmInsumos_Click(object sender, RoutedEventArgs e)
        {
            VistaAdm.Content = new Vista.paginas_administrador.Adm_Insumos();
        }

        private void AdmRecetas_Click(object sender, RoutedEventArgs e)
        {
            VistaAdm.Content = new Vista.paginas_administrador.Adm_Recetas();
        }
    }
}
