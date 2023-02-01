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
        MainWindow Inicio;

        public Opciones(MainWindow ventanaIncio)
        {
            Inicio = ventanaIncio;
            InitializeComponent();
        }
        //Si el boton "contra la maquina" no esta activado, no se podra cambiar la dificultad ni elegir color
        private void RadioBotonContraLaMaquina(object sender, RoutedEventArgs e)
        {
            bool ContraLaMaquina = (bool)((RadioButton)sender).IsChecked;
            Inicio.opcionesJuego.ModoDosJugadores = (bool)((RadioButton)sender).IsChecked;
            ComboBoxOpcionDificultad.IsEnabled = ContraLaMaquina;
            ComboBoxOpcionColor.IsEnabled = ContraLaMaquina;
        }

        private void RadioBotonDosJugadores(object sender, RoutedEventArgs e)
        {
        }

        private void RadioBotonPaleta1(object sender, RoutedEventArgs e)
        {

        }

        private void RadioBotonPaleta2(object sender, RoutedEventArgs e)
        {

        }

        private void RadioBotonPaleta3(object sender, RoutedEventArgs e)
        {

        }

        private void RadioBotonFichas1(object sender, RoutedEventArgs e)
        {
            FontFamily fichas1 = (FontFamily)App.Current.Resources["Arial"];
            App.Current.Resources["recursoPiezas"] = fichas1;
        }

        private void RadioBotonFichas2(object sender, RoutedEventArgs e)
        {
            FontFamily fichas2 = (FontFamily)App.Current.Resources["Quivira"];
            App.Current.Resources["recursoPiezas"] = fichas2;
        }

        private void RadioBotonFichas3(object sender, RoutedEventArgs e)
        {
            FontFamily fichas3 = (FontFamily)App.Current.Resources["Pecita"];
            App.Current.Resources["recursoPiezas"] = fichas3;
        }

        private void BotonAplicarCambios_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BotonAplicarCambios_MouseEnter(object sender, MouseEventArgs e)
        {
            Inicio.SonidoBoton_MouseEnter(sender, e);
        
        }

        private void BotonVolverInicio_Click(object sender, RoutedEventArgs e)
        {
            guardarOpciones();
            if (Inicio.opcionesJuego.pantalla == 2)
            {
                Inicio.WindowStyle = WindowStyle.None;
                Inicio.WindowState = WindowState.Maximized;
            }
            Inicio.Show();
            this.Hide();
        }

        private void ComboBoxModoPantalla_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int modoPantalla = ((ComboBox)sender).SelectedIndex;
            switch (modoPantalla)
            {
                case 0:
                    Inicio.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    break;
                case 1:
                    Inicio.WindowStyle = WindowStyle.None;
                    this.WindowStyle = WindowStyle.None;
                    break;
                case 2:
                    Inicio.WindowStyle = WindowStyle.None;
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Maximized;
                    break;
            }
        }

        public void guardarOpciones()
        {

            ComboBox BoxDificultad = (ComboBox)this.FindName("ComboBoxOpcionDificultad");
            ComboBox BoxColor = (ComboBox)this.FindName("ComboBoxOpcionColor");
            ComboBox BoxPantalla = (ComboBox)this.FindName("ComboBoxModoPantalla");

            Inicio.opcionesJuego.dificultad = BoxDificultad.SelectedIndex;
            Inicio.opcionesJuego.color = BoxColor.SelectedIndex;
            Inicio.opcionesJuego.pantalla = BoxPantalla.SelectedIndex;

        }

        private void ComboBoxOpcionDificultad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxOpcionColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
