﻿using System;
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
    /// Lógica de interacción para Juego.xaml
    /// </summary>
    public partial class Juego : Window
    {
        //Constantes:
        private const string EspacioVacio = "•";

        //Variables para mover piezas
        string CasillaSeleccionadaAnterior = "";
        string CasillaSeleccionada = "";
        char[,] tablero = IniciarTablero();
        bool EsTurnoDeBlancas = true;

        //Componentes de el programa
        MainWindow ventanaInicio;

        //Sonidos
        System.Media.SoundPlayer ReproductorDeSonidoMoverPieza;
        System.Media.SoundPlayer ReproductorDeSonidoInicioPartida;


        public Juego(MainWindow ventanaInicioRecibida)
        {
            InitializeComponent();
            ventanaInicio = ventanaInicioRecibida;
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
                //♟♟♟♟♟ ♝♝ ♞ ♜
                //♙♙♙ ♖♖ ♕
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

        //TESTEADO
        //Metodo que recibe una Casilla y devuelve un int[] correspondientes a las coordenadas de un array
        //El metodo recibe una Casilla en formato columna-fila, ejemplo: la Casilla a2 pasaria a ser [6,0]
        //Las columnas van de (a-h) correspondiendo con coordenadas (0-7) siendo "a" la coordenada 0, "b" -> 1, "c" -> 2...
        //Las filas van de (1-8) correspondiendo con coordenadas (0-7) siendo la fila 1 la coordenada 7, la fila 2 -> coordenada 6...
        private int[] TraducirCasillaCoordenadas(string Casilla)
        {
            char[] auxiliarColumna = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'
            };

            char columna = Casilla[0];
            int fila = Casilla[1] - '0'; //Asi se castea un char a int
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
        //Metodo que recibe una coordenada del array y la traduce a Casilla de tablero
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

        //Sonidos
            //TESTEADO
            //  Mover Pieza
        public void CargarSonidoMoverPieza()
        {
            System.IO.Stream recursoaudio = Properties.Resources.movimientoPieza;
            ReproductorDeSonidoMoverPieza = new System.Media.SoundPlayer(recursoaudio);
            ReproductorDeSonidoMoverPieza.Load();
        }

        public void SonidoMoverPieza_MouseEnter(object sender, MouseEventArgs e)
        {
            ReproductorDeSonidoMoverPieza.Play();

        }

        //TESTEADO
        //   Carga y reproduce el sonido de InicioPartida
        public void ReproducirSonidoInicioPartida()
        {
            System.IO.Stream recursoaudio = Properties.Resources.InicioPartida;
            ReproductorDeSonidoInicioPartida = new System.Media.SoundPlayer(recursoaudio);
            ReproductorDeSonidoInicioPartida.Load();
            ReproductorDeSonidoInicioPartida.Play();
        }


        //TESTEADO
        //Metodo que sincroniza la parte visual con el array del tablero y reproduce el sonido de IniciarTablero/colocar las piezas ademas carga el
        public void InicializarTablero()
        {

            ReproducirSonidoInicioPartida();
            CargarSonidoMoverPieza();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int[] coordenada = { i, j };
                    string Casilla = "casilla_" + TraducirCoordenadaToCasilla(coordenada);
                    Button boton = this.FindName(Casilla) as Button;
                    boton.Content = tablero[i, j];

                }
            }
        }

        /////////////////////////////////
        //Metodos para cambiar el fondo//
        /////////////////////////////////
        public void pintarBase(String Casilla)
        {
            cambiarFondo(Casilla, 0);
        }

        public void pintarRastro(String Casilla)
        {
            cambiarFondo(Casilla, 1);
        }

        public void pintarCamino(String Casilla)
        {
            cambiarFondo(Casilla, 2);
        }

        //TESTEADO
        //Cambia el color de la Casilla a otro color dependiendo de el modo
        //Modo 0 = colorBase
        //Modo 1 = colorRastro
        //Modo 2 = colorCamino
        public void cambiarFondo(String Casilla, int modo)
        {
            SolidColorBrush colorClaroBaseAux = (SolidColorBrush)App.Current.Resources["colorClaroBase"];
            SolidColorBrush colorClaroCaminoAux = (SolidColorBrush)App.Current.Resources["colorClaroCamino"];
            SolidColorBrush colorClaroRastroAux = (SolidColorBrush)App.Current.Resources["colorClaroRastro"];
            SolidColorBrush colorOscuroBaseAux = (SolidColorBrush)App.Current.Resources["colorOscuroBase"];
            SolidColorBrush colorOscuroCaminoAux = (SolidColorBrush)App.Current.Resources["colorOscuroCamino"];
            SolidColorBrush colorOscuroRastroAux = (SolidColorBrush)App.Current.Resources["colorOscuroRastro"];


            string Objetivo = "casilla_" + Casilla;
            string fondo = getColorFondo(Casilla);
            Button Boton = this.FindName(Objetivo) as Button;


            //Si la Casilla es de color oscuro
            if (fondo == colorOscuroBaseAux.Color.ToString() || fondo == colorOscuroCaminoAux.Color.ToString() || fondo == colorOscuroRastroAux.Color.ToString())
            {
                switch (modo)
                {
                    case 0:
                        Boton.Background = colorOscuroBaseAux;
                        break;
                    case 1:
                        Boton.Background = colorOscuroRastroAux;
                        break;
                    case 2:
                        Boton.Background = colorOscuroCaminoAux;
                        break;
                }
            } else //si la Casilla es de color claro
            {
                switch (modo)
                {
                    case 0:
                        Boton.Background = colorClaroBaseAux;
                        break;
                    case 1:
                        Boton.Background = colorClaroRastroAux;
                        break;
                    case 2:
                        Boton.Background = colorClaroCaminoAux;
                        break;
                }
            }
        }

        //TESTEADO
        //Metodo que resibe una Casilla objetivo y actualiza su contenido
        public void ActualizarCasilla(string Casilla, string NuevoContenido)
        {
            string Objetivo = "casilla_" + Casilla;
            Button Boton = this.FindName(Objetivo) as Button;
            Boton.Content = NuevoContenido;
        }

        ////////////////
        //Metodos get://
        ////////////////
        //TESTEADO
        public string getContenidoCasilla(string Casilla)
        {
            string Objetivo = "casilla_" + Casilla;
            Button Boton = this.FindName(Objetivo) as Button;
            return Boton.Content.ToString();
        }

        //TESTEADO
        public string getColorFondo(String Casilla)
        {

            string Objetivo = "casilla_" + Casilla;
            Button Boton = this.FindName(Objetivo) as Button;
            return Boton.Background.ToString();
        }

        //TESTEADO
        private char getCaracterCasilla(int fila, int columna, char[,] tablero)
        {
            return tablero[fila, columna];
        }

        /////////////////
        //Metodos bool://
        /////////////////
        //TESTEADO
        private bool EsPiezaBlanca(string Casilla)
        {
            string blancas = "♔:♕♖♗♘♙";

            if (blancas.Contains(getContenidoCasilla(Casilla)))
            {
                return true;
            }

            return false;
        }

        //TESTEADO
        private bool EstaLaCasillaVacia(string Casilla)
        {

            if (EspacioVacio.Equals(getContenidoCasilla(Casilla)))
            {
                return true;
            }

            return false;
        }

        //TESTEADO
        private bool EsUnaCasillaDeCamino(string Casilla)
        {
            SolidColorBrush colorClaroCaminoAux = (SolidColorBrush)App.Current.Resources["colorClaroCamino"];
            SolidColorBrush colorOscuroCaminoAux = (SolidColorBrush)App.Current.Resources["colorOscuroCamino"];

            if (getColorFondo(Casilla).Equals(colorClaroCaminoAux.ToString()) || getColorFondo(Casilla).Equals(colorOscuroCaminoAux.ToString()))
            {
                return true;
            }
            return false;
        }

        //Recibe una coordenada y intenta dibujar hay un camino
        public void dibujarCamino(int[] coordenadas)
        {

        }

        //////////////////////
        //METODOS DE DIBUJAR//
        //////////////////////
        //El peon es la unica pieza donde el color importa debido a que dependiendo de el color se mueve hacia arriba o hacia abajo
        //Si la pieza es blanca mirare hacia arriba (-1 en la primera coordenada)
        public void dibujarCaminoPeon()
        {
            //Si la pieza es blanca
            if (EsPiezaBlanca(CasillaSeleccionada){
                //Tengo que mirar 1 hacia arriba y dos hacia los lados
            }
        }

        public void dibujarCaminoRey()
        {
        }

        public void dibujarCaminoReina()
        {
        }

        public void dibujarCaminoTorre()
        {
        }

        public void dibujarCaminoCaballo()
        {
        }

        public void dibujarCaminoAlfil()
        {
        }


        //TESTEADO
        //Metodo auxiliar que devuelve un numero dependiendo de el tipo de pieza
        //Develve 0 para peones
        //Develve 1 para Alfiles
        //Develve 2 para Caballos
        //Develve 3 para Torres
        //Develve 4 para Reina
        //Develve 5 para Rey
        //Devuelve -1 para todo lo demas
        private int getTipoDeFicha(string Casilla)
        {
            string LaCasilla = this.getContenidoCasilla(Casilla);
            string Peones = "♙♟︎";
            string Alfiles = "♗♝";
            string Caballos = "♘♞";
            string Torres = "♖♜";
            string Reina = "♕♛";
            string Rey = "♔♚";

            if (Peones.Contains(LaCasilla))
            {
                return 0;
            } else if (Alfiles.Contains(LaCasilla))
            {
                return 1;
            }
            else if (Caballos.Contains(LaCasilla))
            {
                return 2;
            }
            else if (Torres.Contains(LaCasilla))
            {
                return 3;
            }
            else if (Reina.Contains(LaCasilla))
            {
                return 4;
            }
            else if (Rey.Contains(LaCasilla))
            {
                return 5;
            }

            return -1;

        }


        //Metodo para mover la pieza, sera refactorizado en el futuro
        public void moverPieza()
        {
            if (this.CasillaSeleccionada.Equals(this.CasillaSeleccionadaAnterior))
            {

            }
            else if (this.CasillaSeleccionadaAnterior.Equals(EspacioVacio) || this.CasillaSeleccionadaAnterior.Equals("") || getContenidoCasilla(this.CasillaSeleccionadaAnterior).Equals(EspacioVacio))
            {
                this.CasillaSeleccionadaAnterior = "";
            }
            else if (!this.CasillaSeleccionadaAnterior.Equals(""))
            {
                ActualizarCasilla(this.CasillaSeleccionada, getContenidoCasilla(this.CasillaSeleccionadaAnterior));
                ActualizarCasilla(this.CasillaSeleccionadaAnterior, EspacioVacio);
                this.CasillaSeleccionadaAnterior = "";
                this.CasillaSeleccionada = "";
            }
        }

        public void moverPieza2()
        {
            //Si la casilla no esta vacia
            if (!EstaLaCasillaVacia(CasillaSeleccionada))
            {
                //Si la casilla es de el color que toca
                if(EsPiezaBlanca(CasillaSeleccionada) == EsTurnoDeBlancas)
                {
                    switch (getTipoDeFicha(CasillaSeleccionada))
                    {
                        case 0:
                            dibujarCaminoPeon();
                            break;
                        case 1:
                            dibujarCaminoAlfil();
                            break;
                        case 2:
                            dibujarCaminoCaballo();
                            break;
                        case 3:
                            dibujarCaminoTorre();
                            break;
                        case 4:
                            dibujarCaminoReina();
                            break;
                        case 5:
                            dibujarCaminoRey();
                            break;
                    }
                }
            }
        }

        


        //TESTEADO
        //Metodo para debuguear
        public void cambiarDebugText(string laString)
        {
            labelDebug.Text = laString;
        }

        //TESTEADO
        //Metodo llamado por todas las Casillas
        public void SeleccionCasilla(string Casilla)
        {
            this.CasillaSeleccionadaAnterior = this.CasillaSeleccionada;
            this.CasillaSeleccionada = Casilla;
            moverPieza();
        }


        //Botones de la interfaz
        //TESTEADO
        private void BotonvolverInicio_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ventanaInicio.mostrarBotonContinuar();
            ventanaInicio.Show();

        }

        //TESTEADO
        private void ReproducirSonido_MouseEnter(object sender, MouseEventArgs e)
        {
            ventanaInicio.SonidoBoton_MouseEnter(sender, e);

        }

        //TESTEADO
        private void BotonrSalir_Click(object sender, RoutedEventArgs e)
        {
            ventanaInicio.BotonSalir_Click(sender, e);
        }


        //Botones de el tablero
        private void a1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a1");

        }

        private void b1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b1");

        }

        private void c1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c1");

        }

        private void d1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d1");

        }

        private void e1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e1");

        }

        private void f1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f1");

        }

        private void g1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g1");

        }

        private void h1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h1");

        }

        private void a2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a2");

        }

        private void b2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b2");

        }

        private void c2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c2");

        }

        private void d2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d2");

        }

        private void e2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e2");

        }

        private void f2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f2");

        }

        private void g2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g2");

        }

        private void h2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h2");

        }

        private void a3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a3");

        }

        private void b3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b3");

        }

        private void c3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c3");

        }

        private void d3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d3");

        }

        private void e3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e3");

        }

        private void f3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f3");

        }

        private void g3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g3");

        }

        private void h3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h3");

        }

        private void a4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a4");

        }

        private void b4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b4");

        }

        private void c4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c4");

        }

        private void d4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d4");

        }

        private void e4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e4");

        }

        private void f4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f4");

        }

        private void g4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g4");

        }

        private void h4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h4");

        }

        private void a5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a5");

        }

        private void b5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b5");

        }

        private void c5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c5");

        }

        private void d5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d5");

        }

        private void e5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e5");

        }

        private void f5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f5");

        }

        private void g5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g5");

        }

        private void h5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h5");

        }

        private void a6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a6");

        }

        private void b6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b6");

        }

        private void c6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c6");

        }

        private void d6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d6");

        }

        private void e6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e6");

        }

        private void f6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f6");

        }

        private void g6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g6");

        }

        private void h6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h6");

        }

        private void a7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a7");

        }

        private void b7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b7");

        }

        private void c7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c7");

        }

        private void d7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d7");

        }

        private void e7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e7");

        }

        private void f7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f7");

        }

        private void g7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g7");

        }

        private void h7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h7");

        }

        private void a8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a8");

        }

        private void b8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b8");

        }

        private void c8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c8");

        }

        private void d8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d8");

        }

        private void e8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e8");

        }

        private void f8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f8");

        }

        private void g8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g8");

        }

        private void h8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h8");

        }
    }
}
