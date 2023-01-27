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

        String casillaSelecionada = "";

        public MainWindow()
        {
            InitializeComponent();
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
        private char[,] inciarTablero()
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
        private Boolean estaVacio(char casilla)
        {
            if (casilla == '•')
            {
                return true;
            }
            return false;
        }

        //Metodo que recibe un char array, fila y columna y devuelve la casilla de esa columna
        private char obtenerCasilla(int fila, int columna, char[,] tablero)
        {
            return tablero[fila, columna];
        }

        private void a1(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "a1";
        }

        private void b1(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "b1";
        }

        private void c1(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "c1";
        }

        private void d1(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "d1";
        }

        private void e1(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "e1";
        }

        private void f1(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "f1";
        }

        private void g1(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "g1";
        }

        private void h1(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "h1";
        }

        private void a2(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "a2";
        }

        private void b2(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "b2";
        }

        private void c2(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "c2";
        }

        private void d2(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "d2";
        }

        private void e2(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "e2";
        }

        private void f2(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "f2";
        }

        private void g2(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "g2";
        }

        private void h2(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "h2";
        }

        private void a3(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "a3";
        }

        private void b3(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "b3";
        }

        private void c3(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "c3";
        }

        private void d3(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "d3";
        }

        private void e3(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "e3";
        }

        private void f3(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "f3";
        }

        private void g3(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "g3";
        }

        private void h3(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "h3";
        }

        private void a4(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "a4";
        }

        private void b4(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "b4";
        }

        private void c4(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "c4";
        }

        private void d4(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "d4";
        }

        private void e4(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "e4";
        }

        private void f4(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "f4";
        }

        private void g4(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "g4";
        }

        private void h4(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "h4";
        }

        private void a5(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "a5";
        }

        private void b5(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "b5";
        }

        private void c5(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "c5";
        }

        private void d5(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "d5";
        }

        private void e5(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "e5";
        }

        private void f5(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "f5";
        }

        private void g5(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "g5";
        }

        private void h5(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "h5";
        }

        private void a6(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "a6";
        }

        private void b6(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "b6";
        }

        private void c6(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "c6";
        }

        private void d6(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "d6";
        }

        private void e6(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "e6";
        }

        private void f6(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "f6";
        }

        private void g6(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "g6";
        }

        private void h6(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "h6";
        }

        private void a7(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "a7";
        }

        private void b7(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "b7";
        }

        private void c7(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "c7";
        }

        private void d7(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "d7";
        }

        private void e7(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "e7";
        }

        private void f7(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "f7";
        }

        private void g7(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "g7";
        }

        private void h7(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "h7";
        }

        private void a8(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "a8";
        }

        private void b8(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "b8";
        }

        private void c8(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "c8";
        }

        private void d8(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "d8";
        }

        private void e8(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "e8";
        }

        private void f8(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "f8";
        }

        private void g8(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "g8";
        }

        private void h8(object sender, RoutedEventArgs e)
        {
            this.casillaSelecionada = "h8";
        }



        //
    }
}
