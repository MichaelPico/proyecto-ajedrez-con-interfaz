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
    /// Lógica de interacción para Juego.xaml
    /// </summary>
    public partial class Juego : Window
    {
        //Constantes:
        private const string EspacioVacio = "•";
        private const string FondoVerde = "#FF84A27D";
        private const string FondoAmarillo = "#FFD2D0B2";

        string casillaSeleccionadaAnterior = "";
        string casillaSeleccionada = "";
        char[,] tablero = IniciarTablero();

        public Juego()
        {
            InitializeComponent();
            InicializarTablero();
        }

        //NOTA: Cuando se accede a un array bidimensional
        //la primera dimension es la fila y la otra la columna

        /*
             * Casillas:
             *  Blancas:
             *      Rey: ♔
             *      Reina:♕
             *      Torre:♖
             *      Alfil: ♗
             *      Caballo:♘
             *      Peon: ♙
             *  Negras:
             *      Rey: ♚
             *      Reina:♛
             *      Torre:♜
             *      Alfil: ♝
             *      Caballo:♞
             *      Peon: ♟︎
             *  Casilla Vacia: "•" 
            */


        //Metodo que inicia el tablero
        public static char[,] IniciarTablero()
        {
            char[,] respuesta = new char[,]
            {

                { '♜','♞','♝','♛','♚','♝','♞','♜' },
                { '♟','♟','♟','♟','♟','♟','♟','♟' },
                { '•','•','•','•','•','•','•','•' },
                { '•','•','•','•','•','•','•','•' },
                { '•','•','•','•','•','•','•','•' },
                { '•','•','•','•','•','•','•','•' },
                { '♙','♙','♙','♙','♙','♙','♙','♙' },
                { '♖','♘','♗','♕','♔','♗','♘','♖' },
            };

            return respuesta;
        }

        //Metodo que devuelve true si la casilla posee un caracter vacio
        private Boolean EstaVacio(char casilla)
        {
            if (casilla == '•')
            {
                return true;
            }
            return false;
        }

        //Metodo que recibe un char array, fila y columna y devuelve la casilla de esa columna
        private char ObtenerCasilla(int fila, int columna, char[,] tablero)
        {
            return tablero[fila, columna];
        }

        //TESTEADO
        //Metodo que recibe una casilla y devuelve un int[] correspondientes a las coordenadas de un array
        //El metodo recibe una casilla en formato columna-fila, ejemplo: la casilla a2 pasaria a ser [6,0]
        //Las columnas van de (a-h) correspondiendo con coordenadas (0-7) siendo "a" la coordenada 0, "b" -> 1, "c" -> 2...
        //Las filas van de (1-8) correspondiendo con coordenadas (0-7) siendo la fila 1 la coordenada 7, la fila 2 -> coordenada 6...
        private int[] TraducirCasillaCoordenadas(string casilla)
        {
            char[] auxiliarColumna = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'
            };

            char columna = casilla[0];
            int fila = casilla[1] - '0'; //Asi se castea un char a int
            fila = 8 - fila;  //Cambio su valor al contrario del rango, si era 0 ahora es 7, 3->4...

            for (int i = 0; i < auxiliarColumna.Length; i++)
            {
                if (columna == auxiliarColumna[i])
                {
                    return new int[]
                    {
                        i, fila
                    };
                }
            }
            return null;
        }

        //TESTEADO
        //Metodo que recibe una coordenada del array y la traduce a casilla de tablero
        private string TraducirCoordenadaToCasilla(int[] coordenada)
        {
            char[] auxiliarColumna = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'
            };

            char letra = auxiliarColumna[coordenada[1]];
            int numero = 8 - coordenada[0];

            return "" + letra + numero;
        }

        List<Button> botones = new List<Button>();



        //Metodo que sincroniza la parte visual con el array del tablero
        public void InicializarTablero()
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int[] coordenada = { i, j };
                    string casilla = "casilla_" + TraducirCoordenadaToCasilla(coordenada);
                    Button boton = this.FindName(casilla) as Button;
                    boton.Content = tablero[i, j];

                }
            }
        }

        //Metodo que resibe una casilla objetivo y actualiza su contenido
        public void ActualizarCasilla(string Casilla, string NuevoContenido)
        {
            string Objetivo = "casilla_" + Casilla;
            Button Boton = this.FindName(Objetivo) as Button;
            Boton.Content = NuevoContenido;
        }

        public string getContenidoCasilla(string Casilla)
        {
            string Objetivo = "casilla_" + Casilla;
            Button Boton = this.FindName(Objetivo) as Button;
            return Boton.Content.ToString();
        }

        public void moverPieza()
        {
            if (this.casillaSeleccionadaAnterior.Equals(""))
            {
                return;
            }
            else if (getContenidoCasilla(this.casillaSeleccionadaAnterior).Equals(EspacioVacio))
            {
                this.casillaSeleccionadaAnterior = "";

            }
            else if (!this.casillaSeleccionadaAnterior.Equals(""))
            {
                ActualizarCasilla(this.casillaSeleccionada, getContenidoCasilla(this.casillaSeleccionadaAnterior));
                ActualizarCasilla(this.casillaSeleccionadaAnterior, EspacioVacio);
                this.casillaSeleccionadaAnterior = "";
                this.casillaSeleccionada = "";
            }
        }


        public void movimientoPeonBlanco()
        {

        }

        //Infierno de botones: 
        private void a1(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "a1";
            moverPieza();
        }

        private void b1(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "b1";
            moverPieza();
        }

        private void c1(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "c1";
            moverPieza();
        }

        private void d1(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "d1";
            moverPieza();
        }

        private void e1(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "e1";
            moverPieza();
        }

        private void f1(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "f1";
            moverPieza();
        }

        private void g1(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "g1";
            moverPieza();
        }

        private void h1(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "h1";
            moverPieza();
        }

        private void a2(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "a2";
            moverPieza();
        }

        private void b2(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "b2";
            moverPieza();
        }

        private void c2(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "c2";
            moverPieza();
        }

        private void d2(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "d2";
            moverPieza();
        }

        private void e2(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "e2";
            moverPieza();
        }

        private void f2(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "f2";
            moverPieza();
        }

        private void g2(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "g2";
            moverPieza();
        }

        private void h2(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "h2";
            moverPieza();
        }

        private void a3(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "a3";
            moverPieza();
        }

        private void b3(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "b3";
            moverPieza();
        }

        private void c3(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "c3";
            moverPieza();
        }

        private void d3(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "d3";
            moverPieza();
        }

        private void e3(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "e3";
            moverPieza();
        }

        private void f3(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "f3";
            moverPieza();
        }

        private void g3(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "g3";
            moverPieza();
        }

        private void h3(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "h3";
            moverPieza();
        }

        private void a4(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "a4";
            moverPieza();
        }

        private void b4(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "b4";
            moverPieza();
        }

        private void c4(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "c4";
            moverPieza();
        }

        private void d4(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "d4";
            moverPieza();
        }

        private void e4(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "e4";
            moverPieza();
        }

        private void f4(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "f4";
            moverPieza();
        }

        private void g4(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "g4";
            moverPieza();
        }

        private void h4(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "h4";
            moverPieza();
        }

        private void a5(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "a5";
            moverPieza();
        }

        private void b5(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "b5";
            moverPieza();
        }

        private void c5(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "c5";
            moverPieza();
        }

        private void d5(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "d5";
            moverPieza();
        }

        private void e5(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "e5";
            moverPieza();
        }

        private void f5(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "f5";
            moverPieza();
        }

        private void g5(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "g5";
            moverPieza();
        }

        private void h5(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "h5";
            moverPieza();
        }

        private void a6(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "a6";
            moverPieza();
        }

        private void b6(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "b6";
            moverPieza();
        }

        private void c6(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "c6";
            moverPieza();
        }

        private void d6(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "d6";
            moverPieza();
        }

        private void e6(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "e6";
            moverPieza();
        }

        private void f6(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "f6";
            moverPieza();
        }

        private void g6(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "g6";
            moverPieza();
        }

        private void h6(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "h6";
            moverPieza();
        }

        private void a7(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "a7";
            moverPieza();
        }

        private void b7(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "b7";
            moverPieza();
        }

        private void c7(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "c7";
            moverPieza();
        }

        private void d7(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "d7";
            moverPieza();
        }

        private void e7(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "e7";
            moverPieza();
        }

        private void f7(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "f7";
            moverPieza();
        }

        private void g7(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "g7";
            moverPieza();
        }

        private void h7(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "h7";
            moverPieza();
        }

        private void a8(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "a8";
            moverPieza();
        }

        private void b8(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "b8";
            moverPieza();
        }

        private void c8(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "c8";
            moverPieza();
        }

        private void d8(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "d8";
            moverPieza();
        }

        private void e8(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "e8";
            moverPieza();
        }

        private void f8(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "f8";
            moverPieza();
        }

        private void g8(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "g8";
            moverPieza();
        }

        private void h8(object sender, RoutedEventArgs e)
        {
            this.casillaSeleccionadaAnterior = this.casillaSeleccionada;
            this.casillaSeleccionada = "h8";
            moverPieza();
        }



        //
    }
}
