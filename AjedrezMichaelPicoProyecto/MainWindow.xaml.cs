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

namespace AjedrezMichaelPicoProyecto
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int IndexPaletaDeColor { get; set; }
        public OpcionesElegidas opcionesJuego;
        public Opciones ventanaOpciones;
        public Juego ventanaJuego = null;
        

        System.Media.SoundPlayer ReproductorDeSonidoDeBoton;

        public MainWindow()
        {
            InitializeComponent();
            opcionesJuego = new OpcionesElegidas();
            ventanaOpciones = new Opciones(this);
            CargarSonidoBotones();
            IndexPaletaDeColor = 0;
        }

        //Testeado
        public void mostrarBotonContinuar()
        {
            BotonContinuarJuego.Visibility = Visibility.Visible;
            BotonContinuarJuego.IsEnabled = true;
        }

        //Carga el sonido de el boton 
        public void CargarSonidoBotones()
        {
            System.IO.Stream recursoaudio = Properties.Resources.sonidoBoton;
            ReproductorDeSonidoDeBoton = new System.Media.SoundPlayer(recursoaudio);
            ReproductorDeSonidoDeBoton.Load();
        }

        //Crea la ventana de Juego, y le da el modo de ventana seleccionado
        public void BotonNuevoJuego_Click(object sender, RoutedEventArgs e)
        {
            ventanaJuego = new Juego(this);
            ventanaJuego.Show();
            this.Hide();
        }

        //Muestra la ventana de opciones
        private void BotonOpciones_Click(object sender, RoutedEventArgs e)
        {
            ventanaOpciones.Show();
            this.Hide();
        }

        //Si el juego ha sido creado, este boton se habilitara automaticamente y al dar click abrira la ventana de juego
        private void BotonContinuarJuego_Click(object sender, RoutedEventArgs e)
        {
            ventanaJuego.Show();
            this.Hide();
        }

        //Cierra todas las ventanas
        public void BotonSalir_Click(object sender, RoutedEventArgs e)
        {
            CerrarTodasLasVentanas();
        }

        //Cierra todas las ventanas
        public void CerrarTodasLasVentanas()
        {
            if(ventanaJuego != null)
            {
                ventanaJuego.Close();
            }
            ventanaOpciones.Close();
            this.Close();
        }

        //Emite el sonido cargado
        public void SonidoBoton_MouseEnter(object sender, MouseEventArgs e)
        {
            ReproductorDeSonidoDeBoton.Play();

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            App.Current.Resources["altura"] = this.ActualHeight;
        }
    }
}
