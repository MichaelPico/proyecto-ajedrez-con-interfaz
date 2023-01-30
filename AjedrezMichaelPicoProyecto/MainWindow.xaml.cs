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
        System.Media.SoundPlayer PlayerBoton;

        public MainWindow()
        {
            InitializeComponent();
            CargarSonidoBotones();
        }

        public void CargarSonidoBotones()
        {
            System.IO.Stream recursoaudio = Properties.Resources.sonidoBoton;
            PlayerBoton = new System.Media.SoundPlayer(recursoaudio);
            PlayerBoton.Load();
        }

        private void BotonNuevoJuego_Click(object sender, RoutedEventArgs e)
        {
            Juego ventanaJuego = new Juego();
            ventanaJuego.Show();
            this.Close();
        }

        private void BotonOpciones_Click(object sender, RoutedEventArgs e)
        {

            Opciones ventanaOpciones = new Opciones();
            ventanaOpciones.Show();
            this.Close();
        }

        private void BotonContinuarJuego_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BotonSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SonidoBoton_MouseEnter(object sender, MouseEventArgs e)
        {
            PlayerBoton.Play();

        }
    }
}
