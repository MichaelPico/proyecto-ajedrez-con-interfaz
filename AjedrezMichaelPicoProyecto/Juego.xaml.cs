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
    /// Lógica de interacción para Juego.xaml
    /// </summary>
    public partial class Juego : Window
    {
        //Constantes:
        private const string EspacioVacio = "•";

        //Variables para mover piezas
        string CasillaSeleccionadaAnterior = "";
        string CasillaSeleccionada = "";
        bool EsTurnoDeBlancas = true;

        //Componentes de el programa
        MainWindow ventanaInicio;

        //Sonidos
        System.Media.SoundPlayer ReproductorDeSonidoMoverPieza;
        System.Media.SoundPlayer ReproductorDeSonidoInicioPartida;

        /// <summary>
        /// Constructor de la ventana juego
        /// </summary>
        /// <param name="ventanaInicioRecibida"></param>
        public Juego(MainWindow ventanaInicioRecibida)
        {
            char[,] tablero = IniciarTablero();
            InitializeComponent();
            ventanaInicio = ventanaInicioRecibida;
            RellenarTablero(tablero);

        }

        /// <summary>
        /// Inicia la variable tablero con una paratida nueva
        /// </summary>
        /// <returns>Un char[,] que representa el tablero de una partida recien comenzada</returns>
        public static char[,] IniciarTablero()
        {
            char[,] respuesta = new char[,]
            {
                { '♜','♞','♝','♛','♚','♝','♞','♜'},
                { '♟','♟','♟','♟','♟','♟','♟','♟'},
                { '•','•','•','•','•','•','•','•'},
                { '•','•','•','•','•','♖','•','•'},
                { '•','•','♖','•','♖','•','♟','•'},
                { '•','•','•','•','•','•','•','•'},
                { '♙','♙','♙','♙','♙','♙','♙','♙'},
                { '♖','♘','♗','♕','♔','♗','♘','♖'},
            };

            return respuesta;
        }

        //TESTEADO
        /// <summary>
        /// Recibe una array de coordenadas y la traduce a Casilla de tablero
        /// </summary>
        /// <example>
        /// <list type="bullet">
        /// <item>
        /// <description>Si recibe "a2" devolvera {0,6}</description>
        /// </item>                           
        /// <item>                            
        /// <description>Si recibe "a8" devolvera {0,0}</description>
        /// </item>                           
        /// <item>                            
        /// <description>Si recibe "c8" devolvera {3,0}</description>
        /// </item>
        /// </list>
        /// </example>
        /// <param name="coordenada"></param>
        /// <returns></returns>
        private int[] TraducirCasillaCoordenadas(string Casilla)
        {
            char[] auxiliarColumna = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'
            };

            char columna = Casilla[0];
            int fila = Casilla[1] - '0'; //Asi se castea un char a int
            fila = 8 - fila;  //Cambio su valor al contrario del rango, si era 0 ahora es 8, 3->5...

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
        /// <summary>
        /// Recibe una coordenada del array y la traduce a Casilla de tablero
        /// </summary>
        /// <example>
        /// <list type="bullet">
        /// <item>
        /// <description>Si recibe {4,7} devolvera h4</description>
        /// </item>
        /// <item>
        /// <description>Si recibe {6,7} devolvera h2</description>
        /// </item>
        /// <item>
        /// <description>Si recibe {4,0} devolvera a4</description>
        /// </item>
        /// </list>
        /// </example>
        /// <param name="coordenada"></param>
        /// <returns></returns>
        private string TraducirCoordenadaToCasilla(int[] coordenada)
        {
            char[] auxiliarColumna = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'
            };

            char letra = auxiliarColumna[coordenada[0]];
            int numero = 8 - coordenada[1];

            return "" + letra + numero;
        }

        //TESTEADO
        /// <summary>
        /// Recibe una coordenada "X" y otra "Y" y las traduce a su casilla en notacion.
        /// <example>
        /// <list type="bullet">
        /// <item>
        /// <description>Si recibe (4,7) devolvera h4</description>
        /// </item>
        /// <item>
        /// <description>Si recibe (6,7) devolvera h2</description>
        /// </item>
        /// <item>
        /// <description>Si recibe (4,0) devolvera a4</description>
        /// </item>
        /// </list>
        /// </example>
        /// </summary>
        /// <param name="y">Coordenada "Y" (numero)</param>
        /// <param name="x">Coordenada "X" (letra)</param>
        /// <returns></returns>
        public static string TraducirCoordenadaToCasilla(int y, int x)
        {
            char[] auxiliarColumna = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'
            };

            char letra = auxiliarColumna[x];
            int numero = 8 - y;

            return "" + letra + numero;
        }

        //Sonidos
        /////////
        
        //TESTEADO
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SonidoMoverPieza_MouseEnter(object sender, MouseEventArgs e)
        {
            ReproductorDeSonidoMoverPieza.Play();
        }

        //TESTEADO
        /// <summary>
        /// Carga y reproduce el sonido de InicioPartida
        /// </summary>
        public void ReproducirSonidoInicioPartida()
        {
            System.IO.Stream recursoaudio = Properties.Resources.InicioPartida;
            ReproductorDeSonidoInicioPartida = new System.Media.SoundPlayer(recursoaudio);
            ReproductorDeSonidoInicioPartida.Load();
            ReproductorDeSonidoInicioPartida.Play();
        }

        //TESTEADO
        /// <summary>
        /// Metodo que le da a cada boton de el tablero su caracter 
        /// correspondiente y reproduce un sonido que se asemeja a la colocacion de piezas
        /// </summary>
        /// <param name="Tablero">Tablero el cual sera usado para la partida</param>
        public void RellenarTablero(char[,] Tablero)
        {

            ReproducirSonidoInicioPartida();
            CargarSonidoMoverPieza();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int[] coordenada = { j, i }; //Las invierto por que lo que es la a8 para el tablero en el array es {0,0}
                    string Casilla = "casilla_" + TraducirCoordenadaToCasilla(coordenada);
                    Button boton = this.FindName(Casilla) as Button;
                    boton.Content = Tablero[i, j];

                }
            }
        }

        //Metodos para cambiar el fondo//
        /////////////////////////////////

        /// <summary>
        /// Cambia el fondo de la casilla a un fondo de "base"
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le quiere cambiar el fondo</param>
        public void pintarBase(String Casilla)
        {
            cambiarFondo(Casilla, 0);
        }

        /// <summary>
        /// Cambia el fondo de la casilla a un fondo de rastro
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le quiere cambiar el fondo</param>
        public void pintarRastro(String Casilla)
        {
            cambiarFondo(Casilla, 1);
        }

        /// <summary>
        /// Cambia el fondo de la casilla a un fondo de camino
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le quiere cambiar el fondo</param>
        public void pintarCamino(String Casilla)
        {
            cambiarFondo(Casilla, 2);
        }

        //TESTEADO
        /// <summary>
        /// Cambia el fondo de la casilla a uno de los 3 fondos dependiendo de el modo:
        /// <list type="bullet">
        /// <item>
        /// <description>Modo 0 = colorBase</description>
        /// </item>
        /// <item>
        /// <description>Modo 1 = colorRastro</description>
        /// </item>
        /// <item>
        /// <description>Modo 2 = colorCamino</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le quiere cambiar el fondo</param>
        /// <param name="modo">
        /// </param>
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
        /// <summary>
        /// Cambia el caracter de la casilla a el caracter pasado por parametros
        /// </summary>
        /// <param name="Casilla">Casilla cuyo caracter se quiere cambiar</param>
        /// <param name="NuevoContenido">Caracter nuevo</param>
        public void ActualizarCasilla(string Casilla, string NuevoContenido)
        {
            string Objetivo = "casilla_" + Casilla;
            Button Boton = this.FindName(Objetivo) as Button;
            Boton.Content = NuevoContenido;
        }

        //METODOS GET://
        ////////////////

        //TESTEADO
        /// <summary>
        /// Devuelve el caracter de la casilla
        /// </summary>
        /// <param name="Casilla"></param>
        /// <returns></returns>
        public string getContenidoCasilla(string Casilla)
        {
            string Objetivo = "casilla_" + Casilla;
            Button Boton = this.FindName(Objetivo) as Button;
            return Boton.Content.ToString();
        }

        //TESTEADO
        /// <summary>
        /// Devuelve el color de fondo de la casilla
        /// </summary>
        /// <param name="Casilla">Casilla de la cual se quiere obtener el color</param>
        /// <returns></returns>
        public string getColorFondo(String Casilla)
        {

            string Objetivo = "casilla_" + Casilla;
            Button Boton = this.FindName(Objetivo) as Button;
            return Boton.Background.ToString();
        }

        //TESTEADO
        /// <summary>
        /// Metodo que devuelve un numero dependiendo de el tipo de ficha que tiene la Casilla
        /// <list type="bullet">
        /// <item>
        /// <description>Develve 0 para peones</description>
        /// </item>
        /// <item>
        /// <description>Develve 1 para Alfiles</description>
        /// </item>
        /// <item>
        /// <description>Develve 2 para Caballos</description>
        /// </item>
        /// <item>
        /// <description>Develve 3 para Torres</description>
        /// </item>
        /// <item>
        /// <description>Develve 4 para Reina</description>
        /// </item>
        /// <item>
        /// <description>Develve 5 para Rey</description>
        /// </item>
        /// <item>
        /// <description>Devuelve -1 para todo lo demas</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="Casilla"></param>
        /// <returns></returns>
        private int getTipoDeFicha(string Casilla)
        {
            switch (this.getContenidoCasilla(Casilla))
            {
                case "♙": case "♟︎":
                    return 0;
                case "♗": case "♝":
                    return 1;
                case "♘": case "♞":
                    return 2;
                case "♖": case "♜":
                    return 3;
                case "♕": case "♛":
                    return 4;
                case "♔": case "♚":
                    return 5;
                default:
                    return -1;
            }
        }

        //METODOS BOOL://
        /////////////////

        //TESTEADO

        //TESTEADO
        /// <summary>
        /// Devuelve true si el caracter correspondiente a la casilla es una pieza blanca
        /// </summary>
        /// <param name="Casilla">Casilla la cual se comprobara</param>
        /// <returns></returns>
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
        /// <summary>
        /// Devuelve true si el caracter de la casilla corresponde con el caracter usado en las casillas vacias
        /// </summary>
        /// <param name="Casilla">Casilla la cual se comprobara</param>
        /// <returns></returns>
        private bool EstaLaCasillaVacia(string Casilla)
        {

            if (EspacioVacio.Equals(getContenidoCasilla(Casilla)))
            {
                return true;
            }

            return false;
        }

        //TESTEADO
        /// <summary>
        /// Devuelve true si el fondo de la casilla pasada por parametros
        /// es uno de los colores de camino
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le mirara el fondo</param>
        /// <returns></returns>
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

        //TESTEADO
        /// <summary>
        /// Devuelve true si la fila corresponde a la fila inicial de el color definido por el booleano
        /// </summary>
        /// <param name="esBlanco">Define el color de la pieza, true para piezas blancas y false para negras</param>
        /// <param name="fila">Fila donde la pieza se encuentra</param>
        /// <returns></returns>
        private bool EstaPeonEnFilaInicial(bool esBlanco, int fila)
        {
            cambiarDebugText(labelDebug.Text + " FILA: " + fila.ToString());
            if(esBlanco && fila == 2)
            {
                return true;
            } else if (!esBlanco && fila == 7)
            {
                return true;
            }
            return false;
        }

        //TESTEADO
        /// <summary>
        /// Devuelve true si la pieza en la casilla no es de el color al que le toca jugar
        /// </summary>
        /// <param name="Casilla"></param>
        /// <returns></returns>
        private bool EsUnaPiezaEnemiga(string Casilla)
        {
            return EsTurnoDeBlancas != EsPiezaBlanca(Casilla);
        }

        //METODOS DE DIBUJAR//
        //////////////////////
        
        //TESTEADO
        /// <summary>
        /// Dibuja el camino de la pieza peon
        /// </summary>
        public void dibujarCaminoPeon()
        {

            int[] coordenadasDibujar = TraducirCasillaCoordenadas(CasillaSeleccionada);
            int fila = 8 - coordenadasDibujar[1];
            string casillaPasoDoble = "";

            //Si la pieza es blanca
            if (EsPiezaBlanca(CasillaSeleccionada))
                {
                //Tengo que mirar 1 hacia arriba y dos hacia los lados
                //La segunda coordenada de el array representa la "y" y como voy hacia arriba hago y = y-1
                coordenadasDibujar[1] = coordenadasDibujar[1] - 1;

                //Para el paso doble tendre que mirar 2 hacia adelante
                casillaPasoDoble = TraducirCoordenadaToCasilla(coordenadasDibujar[1] - 1, coordenadasDibujar[0]);

                //Si la pieza es negra
            } else
            {
                //Tendre que mirar 1 hacia abajo y dos hacia los lados
                //La segunda coordenada de el array representa la "y" y como voy hacia arriba hago y = y + 1
                coordenadasDibujar[1] = coordenadasDibujar[1] + 1;

                //Para el paso doble tendre que mirar 2 hacia adelante
                casillaPasoDoble = TraducirCoordenadaToCasilla(coordenadasDibujar[1] + 1, coordenadasDibujar[0]);
            }


            //Ahora que la coordenada y esta en la posicion donde quiero dibujar, voy a intentar dibujar
            //camino en los 3 lados posibles 

            //Aqui se dan tres situaciones:
            //Situacion 1: El peon se podra mover hacia un lado solo cuando haya una pieza de color distinto
            //Situacion 2: El peon se podra mover hacia adelante siempre que el espacio este vacio
            //Situacion 3: El peon podra avanzar dos casillas hacia adelante siempre que se encuentre en la primera fila
            for (int i = coordenadasDibujar[0] - 1; i <= coordenadasDibujar[0] + 1; i++)
            {
                if (0 <= i && i <= 7)
                {
                    string casillaObjetivo = TraducirCoordenadaToCasilla(coordenadasDibujar[1], i); //Guardo la casilla en un string
                    
                    //Si la casilla objetivo tiene una pieza de distinto color que la que intento probar, no esta vacia, y no es la casilla de enfrente de el peon dibujare camino
                    if (EsUnaPiezaEnemiga(casillaObjetivo) && !EstaLaCasillaVacia(casillaObjetivo) && i != coordenadasDibujar[0])
                    {
                        pintarCamino(casillaObjetivo);
                    //Si la casilla esta vacia y es justo la casilla de enfrente dibujo el camino en la posicion de adelante
                    } else if(EstaLaCasillaVacia(casillaObjetivo) && i == coordenadasDibujar[0])
                    {
                        cambiarDebugText(casillaPasoDoble + " " + EstaPeonEnFilaInicial(EsPiezaBlanca(CasillaSeleccionada), fila).ToString() + "   " + EstaLaCasillaVacia(casillaPasoDoble).ToString());
                        pintarCamino(casillaObjetivo);

                        //Si el peon esta en la fila inicial y no hay ninguna casilla en la poscion pasoDoble dibujjo camino
                        if (EstaPeonEnFilaInicial(EsPiezaBlanca(CasillaSeleccionada), fila) && EstaLaCasillaVacia(casillaPasoDoble))
                        {
                            pintarCamino(casillaPasoDoble);
                        }
                    }
                }
                
            }
        }

        //TESTEADO
        /// <summary>
        /// Dibuja el camino de la pieza de rey (una casilla en cada direccion siempre que no haya una pieza amiga
        /// </summary>
        public void dibujarCaminoRey()
        {
            int[] coordenadasDibujar = TraducirCasillaCoordenadas(CasillaSeleccionada);

            //Bucle que recorre desde la columna de la izquierda de el rey hasta la columna de la derecha
            for(int i = coordenadasDibujar[0] - 1; i <= coordenadasDibujar[0] + 1; i++)
            {
                //Bucle que recorre desde la fila de arriba de el rey hasta la fila de abajo
                for (int j = coordenadasDibujar[1] - 1; j <= coordenadasDibujar[1] + 1; j++)
                {
                    if ((0 <= i && i <= 7) && (0 <= j && j <= 7))
                    {
                        string casillaObjetivo = TraducirCoordenadaToCasilla(j, i);
                        //Si la casilla donde me intento mover esta vacia o tiene una pieza del color opuesto pinto camino
                        if ((EstaLaCasillaVacia(casillaObjetivo) || EsUnaPiezaEnemiga(casillaObjetivo)))
                        {
                            pintarCamino(casillaObjetivo);
                        }
                    }
                }
            }
        }

        //TODO
        public void dibujarCaminoReina()
        {
        }

        //
        /// <summary>
        /// Dibuja el camino de una torre
        /// (linea recta hasta antes de encontrar una pieza amiga o hasta encontrar una pieza enemiga)
        /// </summary>
        public void dibujarCaminoTorre()
        {
            int[] coordenadasDibujar = TraducirCasillaCoordenadas(CasillaSeleccionada);
            bool pararDerecha = false;
            bool pararIzquierda = false;
            bool pararArriba = false;
            bool pararAbajo = false;

            //Primero dibujare las lineas horizontale
            for(int i = coordenadasDibujar[0] + 1; i <= coordenadasDibujar[0] + 7; i++)
            {
                bool sePuedeDibujarDerecha = i <= 7;
                bool sePuedeDibujarIzquierda =  (coordenadasDibujar[0] - (i - coordenadasDibujar[0])) >= 0;

                //Si me salgo de el limite por los dos lados o ya no tengo permitido dibujar paro el bucle
                if((!sePuedeDibujarDerecha && !sePuedeDibujarIzquierda) ||
                    (pararDerecha && pararIzquierda))
                {
                    break;
                }

                //DibujoDerecha
                //Si no estoy fuera de el tablero y no he parado
                if (sePuedeDibujarDerecha && pararDerecha == false)
                {
                    string casillaObjetivoDerecha = TraducirCoordenadaToCasilla(coordenadasDibujar[1], i);

                    //Si encuentro una casilla aliada paro de dibujar
                    if (!EsUnaPiezaEnemiga(casillaObjetivoDerecha) && !EstaLaCasillaVacia(casillaObjetivoDerecha))
                    {
                        pararDerecha = true;

                        //Si encuentro una pieza enemiga dibujo y paro
                    }else if (EsUnaPiezaEnemiga(casillaObjetivoDerecha) && !EstaLaCasillaVacia(casillaObjetivoDerecha))
                    {
                        pintarCamino(casillaObjetivoDerecha);
                        pararDerecha = true;

                        //Si no me han dicho de parar, no he econtrado pieza y no me he salido dibujo
                    } else
                    {
                        pintarCamino(casillaObjetivoDerecha);
                    }
                }

                //DibujoIzquierda
                //Si no estoy fuera de el tablero y no he parado
                if (sePuedeDibujarIzquierda && pararIzquierda == false)
                {
                    string casillaObjetivoIzquierda = TraducirCoordenadaToCasilla(coordenadasDibujar[1], coordenadasDibujar[0] - (i - coordenadasDibujar[0]));

                    //Si encuentro una casilla aliada paro de dibujar
                    if (!EsUnaPiezaEnemiga(casillaObjetivoIzquierda) && !EstaLaCasillaVacia(casillaObjetivoIzquierda))
                    {
                        pararIzquierda = true;

                        //Si encuentro una casilla no vacia y es una pieza enemiga dibujo y paro
                    }
                    else if (EsUnaPiezaEnemiga(casillaObjetivoIzquierda) && !EstaLaCasillaVacia(casillaObjetivoIzquierda))
                    {
                        pintarCamino(casillaObjetivoIzquierda);
                        pararIzquierda = true;

                        //Si no me han dicho de parar, no he econtrado pieza y no me he salido dibujo
                    }
                    else
                    {
                        pintarCamino(casillaObjetivoIzquierda);
                    }
                }
            }

            //Ahora dibujare las verticales
            for(int i = coordenadasDibujar[1] + 1; i <= coordenadasDibujar[0] + 7; i++)
            {
                bool sePuedeDibujarArriba = i <= 7;
                bool sePuedeDibujarIzquierda = (coordenadasDibujar[0] - (i - coordenadasDibujar[0])) >= 0;

                //Si me salgo de el limite por los dos lados o ya no tengo permitido dibujar paro el bucle
                if ((!sePuedeDibujarArriba && !sePuedeDibujarIzquierda) ||
                    (pararArriba && pararIzquierda))
                {
                    break;
                }

                //DibujoArriba
                //Si no estoy fuera de el tablero y no he parado
                if (sePuedeDibujarArriba && pararArriba == false)
                {
                    string casillaObjetivoArriba = TraducirCoordenadaToCasilla(coordenadasDibujar[1], i);

                    //Si encuentro una casilla aliada paro de dibujar
                    if (!EsUnaPiezaEnemiga(casillaObjetivoArriba) && !EstaLaCasillaVacia(casillaObjetivoArriba))
                    {
                        pararArriba = true;

                        //Si encuentro una pieza enemiga dibujo y paro
                    }
                    else if (EsUnaPiezaEnemiga(casillaObjetivoArriba) && !EstaLaCasillaVacia(casillaObjetivoArriba))
                    {
                        pintarCamino(casillaObjetivoArriba);
                        pararArriba = true;

                        //Si no me han dicho de parar, no he econtrado pieza y no me he salido dibujo
                    }
                    else
                    {
                        pintarCamino(casillaObjetivoArriba);
                    }
                }

                //DibujoIzquierda
                //Si no estoy fuera de el tablero y no he parado
                if (sePuedeDibujarIzquierda && pararIzquierda == false)
                {
                    string casillaObjetivoIzquierda = TraducirCoordenadaToCasilla(coordenadasDibujar[1], coordenadasDibujar[0] - (i - coordenadasDibujar[0]));

                    //Si encuentro una casilla aliada paro de dibujar
                    if (!EsUnaPiezaEnemiga(casillaObjetivoIzquierda) && !EstaLaCasillaVacia(casillaObjetivoIzquierda))
                    {
                        pararIzquierda = true;

                        //Si encuentro una casilla no vacia y es una pieza enemiga dibujo y paro
                    }
                    else if (EsUnaPiezaEnemiga(casillaObjetivoIzquierda) && !EstaLaCasillaVacia(casillaObjetivoIzquierda))
                    {
                        pintarCamino(casillaObjetivoIzquierda);
                        pararIzquierda = true;

                        //Si no me han dicho de parar, no he econtrado pieza y no me he salido dibujo
                    }
                    else
                    {
                        pintarCamino(casillaObjetivoIzquierda);
                    }
                }
            }
        }

        //TESTEADO
        /// <summary>
        /// Dibuja casillas de camino cada al final de una L desde el origen
        /// </summary>
        public void dibujarCaminoCaballo()
        {
            int[] coordenadasDibujar = TraducirCasillaCoordenadas(CasillaSeleccionada);
            int filaCaballo = coordenadasDibujar[1];
            int columnaCaballo = coordenadasDibujar[0];

            //coordenadasDibujar[1] es la fila

            //Primero dibujare los caminos horizaontales
            for (int i = coordenadasDibujar[1] - 1; i <= coordenadasDibujar[1] + 1; i+=2)
            {

                //Miro a la derecha
                //Si no me estoy saliendo de los limites
                if ((0 <= i && i <= 7) && columnaCaballo + 2 < 8)
                {
                    string casillaObjetivoDerecha = TraducirCoordenadaToCasilla(i, columnaCaballo + 2);

                    //Si la casilla es valida para mover el caballo
                    if ((EstaLaCasillaVacia(casillaObjetivoDerecha) || EsUnaPiezaEnemiga(casillaObjetivoDerecha)))
                    {
                        //Pinto camino
                        pintarCamino(casillaObjetivoDerecha);
                    }
                }

                //Miro a la izquierda
                //Si no me estoy saliendo de los limites
                if ((0 <= i && i <= 7) && (0 <= (columnaCaballo - 2)))
                {
                    string casillaObjetivoIzquierda = TraducirCoordenadaToCasilla(i, columnaCaballo - 2);
                    //Si la casilla es valida para mover el caballo
                    if ((EstaLaCasillaVacia(casillaObjetivoIzquierda) || EsUnaPiezaEnemiga(casillaObjetivoIzquierda)))
                    {
                        //Pinto camino
                        pintarCamino(casillaObjetivoIzquierda);
                    }
                }
            }

            //Ahora los caminos verticales
            for (int i = coordenadasDibujar[0] - 1; i <= coordenadasDibujar[0] + 1; i += 2)
            {

                //Miro a la derecha
                //Si no me estoy saliendo de los limites
                if ((0 <= i && i <= 7) && filaCaballo + 2 < 8)
                {
                    string casillaObjetivoAbajo = TraducirCoordenadaToCasilla(filaCaballo + 2, i);

                    //Si la casilla es valida para mover el caballo
                    if ((EstaLaCasillaVacia(casillaObjetivoAbajo) || EsUnaPiezaEnemiga(casillaObjetivoAbajo)))
                    {
                        //Pinto camino
                        pintarCamino(casillaObjetivoAbajo);
                    }
                }

                //Miro a la izquierda
                //Si no me estoy saliendo de los limites
                if ((0 <= i && i <= 7) && filaCaballo - 2 >= 0)
                {
                    string casillaObjetivoArriba = TraducirCoordenadaToCasilla(filaCaballo - 2, i);

                    //Si la casilla es valida para mover el caballo
                    if ((EstaLaCasillaVacia(casillaObjetivoArriba) || EsUnaPiezaEnemiga(casillaObjetivoArriba)))
                    {
                        //Pinto camino
                        pintarCamino(casillaObjetivoArriba);
                    }
                }
            }

        }

        //TODO
        /// <summary>
        /// 
        /// </summary>
        public void dibujarCaminoAlfil()
        {
        }





        [Obsolete("moverPieza esta deprecated, por favor usa moverPieza2.")]
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

        /// <summary>
        /// Metodo el cual comprueba que la casilla seleccionada es una pieza que se puede mover
        /// y dibuja las posibles casillas a donde esa pieza se puede mover
        /// </summary>
        public void moverPieza2()
        {
            //Si la casilla no esta vacia
            if (!EstaLaCasillaVacia(CasillaSeleccionada))
            {
                //Si la casilla es de el color que toca
                if(EsPiezaBlanca(CasillaSeleccionada) == EsTurnoDeBlancas)
                {

                    cambiarDebugText(labelDebug.Text + " " + CasillaSeleccionada);
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
        /// <summary>
        /// Metodo que cambia el contenido de el label debug
        /// </summary>
        /// <param name="laString"></param>
        public void cambiarDebugText(string laString)
        {
            labelDebug.Text = laString;
        }

        //TESTEADO
        /// <summary>
        /// Metodo llamado por cada boton de el tablero el cual gestiona el juego
        /// </summary>
        /// <param name="Casilla"></param>
        public void SeleccionCasilla(string Casilla)
        {
            this.CasillaSeleccionadaAnterior = this.CasillaSeleccionada;
            this.CasillaSeleccionada = Casilla;
            //moverPieza();
            cambiarDebugText(Casilla);
            moverPieza2();




            //Lineas para debuguear los metodoos de coordenada
            //int[] coor = TraducirCasillaCoordenadas(Casilla);
            //cambiarDebugText(Casilla + " coorx" + coor[1].ToString() + " coory" + coor[0] + " OTRO " + TraducirCoordenadaToCasilla(coor) + " OTROOO " + TraducirCoordenadaToCasilla(coor[1],coor[0])); 
        }

        //Botones de la interfaz//
        //////////////////////////

        //TESTEADO
        /// <summary>
        /// Boton que oculta la ventana de juego y muestra la ventana de el inicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonvolverInicio_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ventanaInicio.mostrarBotonContinuar();
            ventanaInicio.Show();

        }

        //TESTEADO
        /// <summary>
        /// Boton el cual reproduce el sonido de los botones de el programa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReproducirSonido_MouseEnter(object sender, MouseEventArgs e)
        {
            ventanaInicio.SonidoBoton_MouseEnter(sender, e);

        }

        //TESTEADO
        /// <summary>
        /// Boton el cual cierra el programa y todas sus ventanas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
