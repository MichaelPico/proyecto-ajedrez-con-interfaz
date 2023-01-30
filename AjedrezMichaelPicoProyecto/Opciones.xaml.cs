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

namespace AjedrezMichaelPicoProyecto
{
    /// <summary>
    /// Lógica de interacción para Opciones.xaml
    /// </summary>
    public partial class Opciones : Window
    {
        public Opciones()
        {
            InitializeComponent();
        }
        //Si el boton "contra la maquina" no esta activado, no se podra cambiar la dificultad
        private void RadioBotonContraLaMaquina(object sender, RoutedEventArgs e)
        {
            bool ContraLaMaquina = (bool)((RadioButton)sender).IsChecked;
            ComboBoxOpcionDificultad.IsEnabled = ContraLaMaquina;
            
        }

        private void RadioBotonDosJugadores(object sender, RoutedEventArgs e)
        {

        }
    }
}
