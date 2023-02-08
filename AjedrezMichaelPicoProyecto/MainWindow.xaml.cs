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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AjedrezMichaelPicoProyecto
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int IndexPaletaDeColor { get; set; }
        public Opciones ventanaOpciones;
        public Juego ventanaJuego = null;
        public bool modoDebug = false; //En la version final tiene que ser false
        

        System.Media.SoundPlayer ReproductorDeSonidoDeBoton;

        public MainWindow()
        {
            InitializeComponent();
            ventanaOpciones = new Opciones(this);
            CargarSonidoBotones();
            IndexPaletaDeColor = 0;
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

        /// <summary>
        /// Metodo que habilita la visibilidad de el boton continuar
        /// </summary>
        public void MostrarBotonContinuar()
        {
            BotonContinuarJuego.Visibility = Visibility.Visible;
            BotonContinuarJuego.IsEnabled = true;
        }

        /// <summary>
        /// Metodo que carga el sonido de los botones
        /// </summary>
        public void CargarSonidoBotones()
        {
            System.IO.Stream recursoaudio = Properties.Resources.sonidoBoton;
            ReproductorDeSonidoDeBoton = new System.Media.SoundPlayer(recursoaudio);
            ReproductorDeSonidoDeBoton.Load();
        }

        /// <summary>
        /// Boton que crea la ventana de Juego, y le da el modo de ventana seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BotonNuevoJuego_Click(object sender, RoutedEventArgs e)
        {
            ventanaJuego = new Juego(this);
            ventanaJuego.Show();
            this.Hide();
            if (modoDebug)
            {
                ventanaJuego.MostrarMenuDebug();
            }

        }

        /// <summary>
        /// Boton que cambia a la ventana de opciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonOpciones_Click(object sender, RoutedEventArgs e)
        {
            ventanaOpciones.Show();
            this.Hide();
        }

        /// <summary>
        /// Boton que si el juego ha sido creado, este boton se habilitara automaticamente y al dar click abrira la ventana de juego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonContinuarJuego_Click(object sender, RoutedEventArgs e)
        {
            ventanaJuego.Show();
            this.Hide();
        }

        /// <summary>
        /// Boton que termina la aplicacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BotonSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Metodo que emite el sonido cargado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReproducirSonidoBotonSistema(object sender, MouseEventArgs e)
        {
            ReproductorDeSonidoDeBoton.Play();

        }

    }
}
