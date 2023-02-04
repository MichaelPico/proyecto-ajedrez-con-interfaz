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
        
        /// <summary>
        /// Al activarse este boton permitira el uso de de las combo box elegir color y elegir dificultad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioBotonContraLaMaquina(object sender, RoutedEventArgs e)
        {
            bool ContraLaMaquina = (bool)((RadioButton)sender).IsChecked;
            ComboBoxOpcionDificultad.IsEnabled = ContraLaMaquina;
            ComboBoxOpcionColor.IsEnabled = ContraLaMaquina;
        }

        /// <summary>
        /// Es la opcion defecto de modo de juego, no hace nada ya que contra la maquina no esta implementado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioBotonDosJugadores(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Al seleccionar cambia la paleta de colores a la numero 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Al seleccionar cambia la paleta de colores a la numero 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Al seleccionar cambia la paleta de colores a la numero 3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Al seleccionar cambia las fichas a el modelo numero 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioBotonFichas1(object sender, RoutedEventArgs e)
        {
            FontFamily fichas1 = (FontFamily)App.Current.Resources["Arial"]; //Get
            App.Current.Resources["fuentePiezas"] = fichas1; //Set
        }

        /// <summary>
        /// Al seleccionar cambia las fichas a el modelo numero 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioBotonFichas2(object sender, RoutedEventArgs e)
        {
            FontFamily fichas2 = (FontFamily)App.Current.Resources["Quivira"];
            App.Current.Resources["fuentePiezas"] = fichas2;
        }

        /// <summary>
        /// Al seleccionar cambia las fichas a el modelo numero 3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioBotonFichas3(object sender, RoutedEventArgs e)
        {
            FontFamily fichas3 = (FontFamily)App.Current.Resources["Pecita"];
            App.Current.Resources["fuentePiezas"] = fichas3;
        }

        /// <summary>
        /// Reproduce el sonido de los botones cuando el mouse entra a estos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReproducirSonido_MouseEnter(object sender, MouseEventArgs e)
        {
            Inicio.SonidoBoton_MouseEnter(sender, e);
        
        }

        /// <summary>
        /// Boton que oculta la ventana de opciones y muestra la ventana de inicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonVolverInicio_Click(object sender, RoutedEventArgs e)
        {
            Inicio.Show();
            this.Hide();
        }

        /// <summary>
        /// Caja que cambia el modo de pantalla dependiendo de la seleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Caja que cambiara la dificultad de la maquina en funcion de la opcion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxOpcionDificultad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// Caja que cambiara el color de el jugador en funcion de la opcion elegida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxOpcionColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
