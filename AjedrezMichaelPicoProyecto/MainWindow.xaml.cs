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
        public Opciones opcionesJuego;
        bool cambiado = false;
        

        System.Media.SoundPlayer PlayerBoton;

        public MainWindow()
        {
            InitializeComponent();
            opcionesJuego = new Opciones(this);
            //CambiarFuente();
            CargarSonidoBotones();
            IndexPaletaDeColor = 0;
        }

        public void CambiarFuente()
        {
            if (!cambiado)
            {
                FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata
                {
                    DefaultValue = FindResource(typeof(Window))
                });
                cambiado = true;
            }
        }

        //Testeado
        public void mostrarBotonContinuar()
        {
            BotonContinuarJuego.Visibility = Visibility.Visible;
            BotonContinuarJuego.IsEnabled = true;
        }

        //Carga el sonido de el boton para asi evitar el retraso en la primera vez que se usa el sonido
        public void CargarSonidoBotones()
        {
            System.IO.Stream recursoaudio = Properties.Resources.sonidoBoton;
            PlayerBoton = new System.Media.SoundPlayer(recursoaudio);
            PlayerBoton.Load();
            PlayerBoton.Play();
        }

        private void BotonNuevoJuego_Click(object sender, RoutedEventArgs e)
        {
            Juego ventanaJuego = new Juego();
            ventanaJuego.Show();
            this.Close();
        }

        private void BotonOpciones_Click(object sender, RoutedEventArgs e)
        {
            opcionesJuego.Show();
            this.Hide();
        }

        private void BotonContinuarJuego_Click(object sender, RoutedEventArgs e)
        {
            //Para implementar este boton hay que hacer un constructor en la clase juego el cual reciba un booleano o algo asi para distinguir de un juego nuevo
        }

        private void BotonSalir_Click(object sender, RoutedEventArgs e)
        {
            opcionesJuego.Close();
            this.Close();
        }

        public void SonidoBoton_MouseEnter(object sender, MouseEventArgs e)
        {
            PlayerBoton.Play();

        }
    }
}
