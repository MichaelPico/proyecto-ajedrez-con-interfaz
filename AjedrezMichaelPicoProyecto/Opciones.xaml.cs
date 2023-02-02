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
    public partial class Opciones: Window
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
            //Get
            SolidColorBrush colorClaroBaseAux = (SolidColorBrush)App.Current.Resources["colorClaroBase1"];
            SolidColorBrush colorClaroCaminoAux = (SolidColorBrush)App.Current.Resources["colorClaroCamino1"];
            SolidColorBrush colorClaroRastroAux = (SolidColorBrush)App.Current.Resources["colorClaroRastro1"];
            SolidColorBrush colorOscuroBaseAux = (SolidColorBrush)App.Current.Resources["colorOscuroBase1"];
            SolidColorBrush colorOscuroCaminoAux = (SolidColorBrush)App.Current.Resources["colorOscuroCamino1"];
            SolidColorBrush colorOscuroRastroAux = (SolidColorBrush)App.Current.Resources["colorOscuroRastro1"];

            //Set
            App.Current.Resources["colorClaroBase"] = colorClaroBaseAux;
            App.Current.Resources["colorClaroCamino"] = colorClaroCaminoAux;
            App.Current.Resources["colorClaroRastro"] = colorClaroRastroAux;
            App.Current.Resources["colorOscuroBase"] = colorOscuroBaseAux;
            App.Current.Resources["colorOscuroCamino"] = colorOscuroCaminoAux;
            App.Current.Resources["colorOscuroRastro"] = colorOscuroRastroAux;
        }

        private void RadioBotonPaleta2(object sender, RoutedEventArgs e)
        {
            //Get
            SolidColorBrush colorClaroBaseAux = (SolidColorBrush)App.Current.Resources["colorClaroBase2"];
            SolidColorBrush colorClaroCaminoAux = (SolidColorBrush)App.Current.Resources["colorClaroCamino2"];
            SolidColorBrush colorClaroRastroAux = (SolidColorBrush)App.Current.Resources["colorClaroRastro2"];
            SolidColorBrush colorOscuroBaseAux = (SolidColorBrush)App.Current.Resources["colorOscuroBase2"];
            SolidColorBrush colorOscuroCaminoAux = (SolidColorBrush)App.Current.Resources["colorOscuroCamino2"];
            SolidColorBrush colorOscuroRastroAux = (SolidColorBrush)App.Current.Resources["colorOscuroRastro2"];

            //Set
            Application.Current.Resources["colorClaroBase"] = colorClaroBaseAux;
            App.Current.Resources["colorClaroCamino"] = colorClaroCaminoAux;
            App.Current.Resources["colorClaroRastro"] = colorClaroRastroAux;
            App.Current.Resources["colorOscuroBase"] = colorOscuroBaseAux;
            App.Current.Resources["colorOscuroCamino"] = colorOscuroCaminoAux;
            App.Current.Resources["colorOscuroRastro"] = colorOscuroRastroAux;
        }

        private void RadioBotonPaleta3(object sender, RoutedEventArgs e)
        {
            //Get
            SolidColorBrush colorClaroBaseAux = (SolidColorBrush)App.Current.Resources["colorClaroBase3"];
            SolidColorBrush colorClaroCaminoAux = (SolidColorBrush)App.Current.Resources["colorClaroCamino3"];
            SolidColorBrush colorClaroRastroAux = (SolidColorBrush)App.Current.Resources["colorClaroRastro3"];
            SolidColorBrush colorOscuroBaseAux = (SolidColorBrush)App.Current.Resources["colorOscuroBase3"];
            SolidColorBrush colorOscuroCaminoAux = (SolidColorBrush)App.Current.Resources["colorOscuroCamino3"];
            SolidColorBrush colorOscuroRastroAux = (SolidColorBrush)App.Current.Resources["colorOscuroRastro3"];

            //Set
            App.Current.Resources["colorClaroBase"] = colorClaroBaseAux;
            App.Current.Resources["colorClaroCamino"] = colorClaroCaminoAux;
            App.Current.Resources["colorClaroRastro"] = colorClaroRastroAux;
            App.Current.Resources["colorOscuroBase"] = colorOscuroBaseAux;
            App.Current.Resources["colorOscuroCamino"] = colorOscuroCaminoAux;
            App.Current.Resources["colorOscuroRastro"] = colorOscuroRastroAux;

        }

        private void RadioBotonFichas1(object sender, RoutedEventArgs e)
        {
            FontFamily fichas1 = (FontFamily)App.Current.Resources["Arial"]; //Get
            App.Current.Resources["fuentePiezas"] = fichas1; //Set
        }

        private void RadioBotonFichas2(object sender, RoutedEventArgs e)
        {
            FontFamily fichas2 = (FontFamily)App.Current.Resources["Quivira"];
            App.Current.Resources["fuentePiezas"] = fichas2;
        }

        private void RadioBotonFichas3(object sender, RoutedEventArgs e)
        {
            FontFamily fichas3 = (FontFamily)App.Current.Resources["Pecita"];
            App.Current.Resources["fuentePiezas"] = fichas3;
        }

        private void ReproducirSonido_MouseEnter(object sender, MouseEventArgs e)
        {
            Inicio.SonidoBoton_MouseEnter(sender, e);
        
        }

        private void BotonVolverInicio_Click(object sender, RoutedEventArgs e)
        {
            guardarOpciones();
            Inicio.Show();
            this.Hide();
        }

        private void ComboBoxModoPantalla_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int modoPantalla = ((ComboBox)sender).SelectedIndex;

            switch (modoPantalla)
            {
                case 0:
                    //if (Inicio.ventanaJuego != null)
                    //{
                    //    Inicio.ventanaJuego.WindowStyle = WindowStyle.SingleBorderWindow;
                    //}
                    //Inicio.WindowStyle = WindowStyle.SingleBorderWindow;
                    //this.WindowStyle = WindowStyle.SingleBorderWindow;
                    App.Current.Resources["estiloVentana"] = WindowStyle.SingleBorderWindow;
                    App.Current.Resources["estadoVentana"] = WindowState.Normal;
                    break;
                case 1:
                    //if (Inicio.ventanaJuego != null)
                    //{
                    //    Inicio.ventanaJuego.WindowStyle = WindowStyle.None;
                    //}
                    //Inicio.WindowStyle = WindowStyle.None;
                    //this.WindowStyle = WindowStyle.None;
                    App.Current.Resources["estiloVentana"] = WindowStyle.None;
                    App.Current.Resources["estadoVentana"] = WindowState.Normal;
                    break;
                case 2:
                    //if (Inicio.ventanaJuego != null)
                    //{
                    //    Inicio.ventanaJuego.WindowStyle = WindowStyle.None;
                    //    Inicio.ventanaJuego.WindowState = WindowState.Maximized;
                    //}
                    //Inicio.WindowStyle = WindowStyle.None;
                    //this.WindowStyle = WindowStyle.None;
                    //this.WindowState = WindowState.Maximized;
                    App.Current.Resources["estiloVentana"] = WindowStyle.None;
                    App.Current.Resources["estadoVentana"] = WindowState.Maximized;
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
