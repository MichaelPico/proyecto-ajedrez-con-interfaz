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
    /// Lógica de interacción para Promocion.xaml
    /// </summary>
    public partial class Promocion : Window
    {
        Juego ventanaJuego = null;
        bool esBlanco;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="blanco"></param>
        public Promocion(Juego sender,bool blanco)
        {
            SetColores();
            InitializeComponent();
            esBlanco = blanco;
            ventanaJuego = sender;
        }

        /// <summary>
        /// Cierra esta ventana
        /// </summary>
        public void CerrarVentana()
        {
            ventanaJuego.IsEnabled = true;
            Close();
        }

        /// <summary>
        /// Cambia los colores de la ventana en funcion de si se promocionan piezas blancas o negras
        /// </summary>
        /// <param name="blanco"></param>
        public void SetColores()
        {
            App.Current.Resources["PromocionColorFondo"] = (SolidColorBrush)App.Current.Resources["colorOscuroBase"];
            App.Current.Resources["PromocionColorLetras"] = (SolidColorBrush)App.Current.Resources["PromocionColorNegro"];
        }

        

        /// <summary>
        /// Cambia la casilla a promocionar y cierra esta ventana
        /// </summary>
        /// <param name="caracter"></param>
        public void AccionBoton(string caracter)
        {
            ventanaJuego.ActualizarCaracterCasilla(ventanaJuego.CasillaSeleccionada, caracter);
            ventanaJuego.ChequearJaque(true);
            CerrarVentana();
        }

        private void ClickBotonReina(object sender, RoutedEventArgs e)
        {
            if (esBlanco)
            {
                AccionBoton("♕");
            } else
            {
                AccionBoton("♛");
            }
        }

        private void ClickBotonTorre(object sender, RoutedEventArgs e)
        {
            if (esBlanco)
            {
                AccionBoton("♖");
            }
            else
            {
                AccionBoton("♜");
            }
        }

        private void ClickBotonAlfil(object sender, RoutedEventArgs e)
        {
            if (esBlanco)
            {
                AccionBoton("♗");
            }
            else
            {
                AccionBoton("♝");
            }
        }

        private void ClickBotonCaballo(object sender, RoutedEventArgs e)
        {
            if (esBlanco)
            {
                AccionBoton("♘");
            }
            else
            {
                AccionBoton("♞");
            }
        }


    }
}
