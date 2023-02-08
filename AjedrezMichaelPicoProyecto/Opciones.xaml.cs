using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        int contadorDebug = 0;
        System.Media.SoundPlayer ReproductorDeSonidoMoverPieza;

        public Opciones(MainWindow ventanaIncio)
        {
            Inicio = ventanaIncio;
            InitializeComponent();
            CargarSonidoMoverPieza();
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
            FontFamily fichas1 = (FontFamily)App.Current.Resources["Gothic"]; //Get
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
            FontFamily fichas3 = (FontFamily)App.Current.Resources["Code2000"];
            App.Current.Resources["fuentePiezas"] = fichas3;
        }

        /// <summary>
        /// Reproduce el sonido de los botones cuando el mouse entra a estos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReproducirSonido_MouseEnter(object sender, MouseEventArgs e)
        {
            Inicio.ReproducirSonidoBotonSistema(sender, e);
        
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
                    App.Current.Resources["estiloVentana"] = WindowStyle.SingleBorderWindow;
                    App.Current.Resources["estadoVentana"] = WindowState.Normal;
                    break;
                case 1:
                    App.Current.Resources["estiloVentana"] = WindowStyle.None;
                    App.Current.Resources["estadoVentana"] = WindowState.Normal;
                    break;
                case 2:
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

        /// <summary>
        /// Metodo que se encarga de un easteregg para activar la ventana de debug, cuando se le de 5 veces esta ventana estara activada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonDeElRey(object sender, RoutedEventArgs e)
        {
            contadorDebug++;
            if (contadorDebug < 5)
            {
                MoverPiezaSonido();
            }
            if (contadorDebug == 5)
            {
                string messageBoxText = "Has encontrado el boton secreto que habilita el menu de tableros! \n" +
                    "Disfruta de este menu en la ventana de juego";
                string caption = "Secreto encontrado";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                Inicio.modoDebug = true;
                if(Inicio.ventanaJuego != null)
                {
                    Inicio.ventanaJuego.MostrarMenuDebug();
                }
            }
        }

        /// <summary>
        /// Prepara el sonido de mover pieza para cuando se quiera usar
        /// </summary>
        public void CargarSonidoMoverPieza()
        {
            System.IO.Stream recursoaudio = Properties.Resources.movimientoPieza;
            ReproductorDeSonidoMoverPieza = new System.Media.SoundPlayer(recursoaudio);
            ReproductorDeSonidoMoverPieza.Load();
        }

        /// <summary>
        /// Reproduce el sonido de moverPieza
        /// </summary>
        public void MoverPiezaSonido()
        {
            ReproductorDeSonidoMoverPieza.Play();
        }

        /// <summary>
        /// Metodo que cierra todas las ventanas cuando se da a la x de la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
