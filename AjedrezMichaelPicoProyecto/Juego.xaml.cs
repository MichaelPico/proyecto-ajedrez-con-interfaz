using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AjedrezMichaelPicoProyecto
{
    /// <summary>
    /// Lógica de interacción para Juego.xaml
    /// </summary>
    public partial class Juego : Window
    {
        //Constantes:
        private const string EspacioVacio = " ";

        //Variables para mover piezas
        public string CasillaSeleccionadaAnterior = "";
        public string CasillaSeleccionada = "";

        //Booleanos usados para el funcionamiento de el juego
        bool EstaElCaminoDibujado = false;
        bool ChequeandoJaque = false;
        public bool Promocionando = false;

        //Booleanos que definen la partida
        bool EsTurnoDeBlancas = true;
        bool SePuedeEnroqueBlancoDerecha = true;
        bool SePuedeEnroqueBlancoIzquierda = true;
        bool SePuedeEnroqueNegroDerecha = true;
        bool SePuedeEnroqueNegroIzquierda = true;
        bool JaqueReyBlanco = false;
        bool JaqueReyNegro = false;
        int ContadorNotacion = 0;
        string casillaEnPassant;

        //Componentes de el programa
        readonly MainWindow ventanaInicio;

        //Sonidos
        System.Media.SoundPlayer ReproductorDeSonidoMoverPieza;
        System.Media.SoundPlayer ReproductorDeSonidoInicioPartida;

        /// <summary>
        /// Constructor de la ventana juego
        /// </summary>
        /// <param name="ventanaInicioRecibida"></param>
        public Juego(MainWindow ventanaInicioRecibida)
        {
            ReproducirSonidoInicioPartida();
            CargarSonidoMoverPieza();
            char[,] tablero = DevolverTableroNuevo();
            InitializeComponent();
            ventanaInicio = ventanaInicioRecibida;
            RellenarTablero(tablero);
        }

        /// <summary>
        /// Metodo que devuelve un tablero de partida nueva
        /// </summary>
        /// <returns>Un char[,] que representa el tablero de una partida recien comenzada</returns>
        public static char[,] DevolverTableroNuevo()
        {
            char[,] respuesta = new char[,]
            {
                { '♜','♞','♝','♛','♚','♝','♞','♜'},
                { '♟','♟','♟','♟','♟','♟','♟','♟'},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { '♙','♙','♙','♙','♙','♙','♙','♙'}, //Blancas = ♔♕♖♗♘♙
                { '♖','♘','♗','♕','♔','♗','♘','♖'}, //Negras  = ♚♛♜♝♞♟
            };

            return respuesta;
        }

        /// <summary>
        /// Metodo que le da a cada boton de el tablero su caracter 
        /// correspondiente y reproduce un sonido que se asemeja a la colocacion de piezas
        /// </summary>
        /// <param name="Tablero">Tablero el cual sera usado para la partida</param>
        public void RellenarTablero(char[,] Tablero)
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int[] coordenada = { j, i }; //Las invierto por que lo que es la a8 para el tablero en el array es {0,0}
                    string Casilla = "casilla_" + TraducirCoordenadaToCasilla(coordenada);
                    Button boton = FindName(Casilla) as Button;
                    boton.Content = Tablero[i, j];

                }
            }
        }

        //Metodos para cambiar el fondo//

        /// <summary>
        /// Metodo que cambia el fondo de la casilla a un fondo de "base"
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le quiere cambiar el fondo</param>
        public void PintarBase(string Casilla)
        {
            SetFondo(Casilla, 0);
        }

        /// <summary>
        /// Metodo que cambia el fondo de la casilla a un fondo de rastro
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le quiere cambiar el fondo</param>
        public void PintarRastro(string Casilla)
        {
            SetFondo(Casilla, 1);
        }

        /// <summary>
        /// Metodo que cambia el fondo de la casilla a un fondo de camino y actualiza el booleano
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le quiere cambiar el fondo</param>
        public void PintarCamino(string casilla)
        {
            if (!ChequeandoJaque)
            {
                SetFondo(casilla, 2);
                BorrarRastro();
                EstaElCaminoDibujado = true;
            }
            else
            {
                if (GetContenidoCasilla(casilla).Equals("♚"))
                {
                    JaqueReyNegro = true;
                }
                if (GetContenidoCasilla(casilla).Equals("♔"))
                {
                    JaqueReyBlanco = true;
                }
            }
        }

        /// <summary>
        /// Metodo que cambia el fondo de la casilla a un color especial que indica enroque
        /// </summary>
        /// <param name="Casilla"></param>
        public void PintarEnroque(string Casilla)
        {
            EstaElCaminoDibujado = true;
            SetFondo(Casilla, 3);
        }

        /// <summary>
        /// Metodo que cambia el fondo de la casilla a un color especial que indica En Passant
        /// </summary>
        /// <param name="Casilla"></param>
        public void PintarEnPassant(string Casilla)
        {
            EstaElCaminoDibujado = true;
            SetFondo(Casilla, 4);
        }

        /// <summary>
        /// Metodo que borra todo el camino y cambia el booleano
        /// </summary>
        public void BorrarCamino()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string casilla = TraducirCoordenadaToCasilla(i, j);
                    if (EsUnaCasillaDeCamino(casilla) || EsUnaCasillaDeEnroque(casilla) || EsUnaCasillaDeEnPassant(casilla))
                    {
                        PintarBase(casilla);
                    }
                }
            }

            EstaElCaminoDibujado = false;
        }

        /// <summary>
        /// Metodo que borra el rastro
        /// </summary>
        public void BorrarRastro()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string casilla = TraducirCoordenadaToCasilla(i, j);
                    if (EsUnaCasillaDeRastro(casilla))
                    {
                        PintarBase(casilla);
                    }
                }
            }
        }

        /// <summary>
        /// Metodo que borra el rastro y el camino
        /// </summary>
        public void RestaurarTablero()
        {
            BorrarCamino(); //Borro el camino
            BorrarRastro();
        }

        /// <summary>
        /// Metodo que pinta cada casilla para asegurarse que cambian de color
        /// </summary>
        public void RepintarTablero()
        {
            List<string> rastros = GetListaRastro();

            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string casilla = TraducirCoordenadaToCasilla(i, j);
                    PintarBase(casilla);
                }
            }

            SetRastro(rastros);
        }


        //METODOS DE DIBUJAR//

        /// <summary>
        /// Metodo que dibuja camino de ser posible
        /// </summary>
        public void IntentarDibujarCamino()
        {
            //Si la casilla no esta vacia
            if (!EstaLaCasillaVacia(CasillaSeleccionada))
            {
                //Si la casilla es de el color que toca
                if (EsPiezaBlanca(CasillaSeleccionada) == EsTurnoDeBlancas)
                {
                    switch (GetTipoDeFicha(CasillaSeleccionada))
                    {
                        case 0:
                            DibujarCaminoPeon();
                            break;
                        case 1:
                            DibujarCaminoAlfil();
                            break;
                        case 2:
                            DibujarCaminoCaballo();
                            break;
                        case 3:
                            DibujarCaminoTorre();
                            break;
                        case 4:
                            DibujarCaminoReina();
                            break;
                        case 5:
                            DibujarCaminoRey();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Metodo que dibuja el camino de la pieza peon
        /// </summary>
        public void DibujarCaminoPeon()
        {

            int[] coordenadasDibujar = TraducirCasillaCoordenadas(CasillaSeleccionada);
            int fila = 8 - coordenadasDibujar[1];
            string casillaPasoDoble;


            //Si la pieza es un peon a el que le toca promocionar, no dibujare su camino ya que no tiene
            if ((EsPiezaBlanca(CasillaSeleccionada) && GetFila(CasillaSeleccionada) == 0) || (!EsPiezaBlanca(CasillaSeleccionada) && GetFila(CasillaSeleccionada) == 7))
            {
                return;
            }

            //Si la pieza es blanca
            if (EsPiezaBlanca(CasillaSeleccionada))
            {
                //Tengo que mirar 1 hacia arriba y dos hacia los lados
                //La segunda coordenada de el array representa la "y" y como voy hacia arriba hago y = y-1
                coordenadasDibujar[1] = coordenadasDibujar[1] - 1;

                //Para el paso doble tendre que mirar 2 hacia adelante
                casillaPasoDoble = TraducirCoordenadaToCasilla(coordenadasDibujar[1] - 1, coordenadasDibujar[0]);

                //Si la pieza es negra
            }
            else
            {
                //Tendre que mirar 1 hacia abajo y dos hacia los lados
                //La segunda coordenada de el array representa la "y" y como voy hacia arriba hago y = y + 1
                coordenadasDibujar[1] = coordenadasDibujar[1] + 1;

                //Para el paso doble tendre que mirar 2 hacia adelante
                casillaPasoDoble = TraducirCoordenadaToCasilla(coordenadasDibujar[1] + 1, coordenadasDibujar[0]);
            }


            //Ahora que la coordenada y esta en la posicion donde quiero dibujar, voy a intentar dibujar
            //camino en los 3 lados posibles 

            //Aqui se dan cuatro situaciones:
            //Situacion 1: El peon se podra mover hacia un lado solo cuando haya una pieza de color distinto
            //Situacion 2: El peon se podra mover hacia adelante siempre que el espacio este vacio
            //Situacion 3: El peon podra avanzar dos casillas hacia adelante siempre que se encuentre en la primera fila
            //Situacion 4: El peon podra capturar enpassant
            for (int i = coordenadasDibujar[0] - 1; i <= coordenadasDibujar[0] + 1; i++)
            {
                if (0 <= i && i <= 7)
                {
                    //Cuando se esta chequeando por jaque, es posible que estas coordenadas lleguen a salir de los limites, con este metodo evitamos el bug
                    if (coordenadasDibujar[1] == -1)
                    {
                        coordenadasDibujar[1] = 0;
                    }
                    else if (coordenadasDibujar[1] == 8)
                    {
                        coordenadasDibujar[1] = 7;
                    }
                    string casillaObjetivo = TraducirCoordenadaToCasilla(coordenadasDibujar[1], i); //Guardo la casilla en un string

                    //Si la casilla esta vacia y es un posible enpassant pinto enPassant
                    if (EstaLaCasillaVacia(casillaObjetivo) && casillaObjetivo.Equals(casillaEnPassant))
                    {
                        PintarEnPassant(casillaEnPassant);
                    }
                    //Si la casilla objetivo tiene una pieza de distinto color que la que intento probar, no esta vacia, y no es la casilla de enfrente de el peon dibujare camino
                    else if (EsUnaPiezaEnemiga(casillaObjetivo) && !EstaLaCasillaVacia(casillaObjetivo) && i != coordenadasDibujar[0])
                    {
                        PintarCamino(casillaObjetivo);
                    }
                    //Si la casilla esta vacia y es justo la casilla de enfrente dibujo el camino en la posicion de adelante
                    else if (EstaLaCasillaVacia(casillaObjetivo) && i == coordenadasDibujar[0])
                    {
                        PintarCamino(casillaObjetivo);

                        //Si el peon esta en la fila inicial y no hay ninguna casilla en la poscion pasoDoble dibujjo camino
                        if (EstaPeonEnFilaInicial(EsPiezaBlanca(CasillaSeleccionada), fila) && EstaLaCasillaVacia(casillaPasoDoble))
                        {
                            PintarCamino(casillaPasoDoble);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Metodo que verifica si un peon ha llegado a la ultima fila de el tablero y tiene que ser promocionado
        /// </summary>
        public void IntentarPromocion()
        {
            string casilla = CasillaSeleccionada;
            //Si el ultimo movimiento realizado es un peon
            if (GetTipoDeFicha(casilla) == 0)
            {
                //Miro si es un peon llegando al final 
                if ((EsPiezaBlanca(casilla) && GetFila(casilla) == 0) || (!EsPiezaBlanca(casilla) && GetFila(casilla) == 7))
                {
                    Promocion ventanaPromocion = new Promocion(this, EsTurnoDeBlancas);
                    this.IsEnabled = false;
                    Promocionando = true;
                    ventanaPromocion.Show();
                }
            }
        }

        /// <summary>
        /// Metodo que dibuja camino en lineas diagonales
        /// </summary>
        public void DibujarCaminoAlfil()
        {
            DibujarLineaDiagonal(true, true, CasillaSeleccionada);
            DibujarLineaDiagonal(true, false, CasillaSeleccionada);
            DibujarLineaDiagonal(false, true, CasillaSeleccionada);
            DibujarLineaDiagonal(false, false, CasillaSeleccionada);
        }

        /// <summary>
        /// Metodo que dibuja casillas de camino cada al final de una L desde el origen
        /// </summary>
        public void DibujarCaminoCaballo()
        {
            int[] coordenadasDibujar = TraducirCasillaCoordenadas(CasillaSeleccionada);
            int filaCaballo = coordenadasDibujar[1];
            int columnaCaballo = coordenadasDibujar[0];

            //coordenadasDibujar[1] es la fila

            //Primero dibujare los caminos horizaontales
            for (int i = coordenadasDibujar[1] - 1; i <= coordenadasDibujar[1] + 1; i += 2)
            {

                //Miro a la derecha
                //Si no me estoy saliendo de los limites
                if (0 <= i && i <= 7 && columnaCaballo + 2 < 8)
                {
                    string casillaObjetivoDerecha = TraducirCoordenadaToCasilla(i, columnaCaballo + 2);

                    //Si la casilla es valida para mover el caballo
                    if (EstaLaCasillaVacia(casillaObjetivoDerecha) || EsUnaPiezaEnemiga(casillaObjetivoDerecha))
                    {
                        //Pinto camino
                        PintarCamino(casillaObjetivoDerecha);
                    }
                }

                //Miro a la izquierda
                //Si no me estoy saliendo de los limites
                if (0 <= i && i <= 7 && (0 <= (columnaCaballo - 2)))
                {
                    string casillaObjetivoIzquierda = TraducirCoordenadaToCasilla(i, columnaCaballo - 2);
                    //Si la casilla es valida para mover el caballo
                    if (EstaLaCasillaVacia(casillaObjetivoIzquierda) || EsUnaPiezaEnemiga(casillaObjetivoIzquierda))
                    {
                        //Pinto camino
                        PintarCamino(casillaObjetivoIzquierda);
                    }
                }
            }

            //Ahora los caminos verticales
            for (int i = coordenadasDibujar[0] - 1; i <= coordenadasDibujar[0] + 1; i += 2)
            {

                //Miro a la derecha
                //Si no me estoy saliendo de los limites
                if (0 <= i && i <= 7 && filaCaballo + 2 < 8)
                {
                    string casillaObjetivoAbajo = TraducirCoordenadaToCasilla(filaCaballo + 2, i);

                    //Si la casilla es valida para mover el caballo
                    if (EstaLaCasillaVacia(casillaObjetivoAbajo) || EsUnaPiezaEnemiga(casillaObjetivoAbajo))
                    {
                        //Pinto camino
                        PintarCamino(casillaObjetivoAbajo);
                    }
                }

                //Miro a la izquierda
                //Si no me estoy saliendo de los limites
                if (0 <= i && i <= 7 && (filaCaballo - 2 >= 0))
                {
                    string casillaObjetivoArriba = TraducirCoordenadaToCasilla(filaCaballo - 2, i);

                    //Si la casilla es valida para mover el caballo
                    if (EstaLaCasillaVacia(casillaObjetivoArriba) || EsUnaPiezaEnemiga(casillaObjetivoArriba))
                    {
                        //Pinto camino
                        PintarCamino(casillaObjetivoArriba);
                    }
                }
            }

        }

        /// <summary>
        /// Metodo que dibuja el camino de una torre
        /// (linea recta hasta antes de encontrar una pieza amiga o hasta encontrar una pieza enemiga)
        /// </summary>
        public void DibujarCaminoTorre()
        {
            DibujarLineaRecta(true, true, CasillaSeleccionada);
            DibujarLineaRecta(true, false, CasillaSeleccionada);
            DibujarLineaRecta(false, true, CasillaSeleccionada);
            DibujarLineaRecta(false, false, CasillaSeleccionada);
        }

        /// <summary>
        /// Metodo que dibuja el camino de la reina el cual es el camino de un alfil y de una torre juntos
        /// </summary>
        public void DibujarCaminoReina()
        {
            DibujarCaminoAlfil();
            DibujarCaminoTorre();
        }

        /// <summary>
        /// Metodo que dibuja el camino de la pieza de rey (una casilla en cada direccion siempre que no haya una pieza amiga
        /// </summary>
        public void DibujarCaminoRey()
        {
            int[] coordenadasDibujar = TraducirCasillaCoordenadas(CasillaSeleccionada);

            //Bucle que recorre desde la columna de la izquierda de el rey hasta la columna de la derecha
            for (int i = coordenadasDibujar[0] - 1; i <= coordenadasDibujar[0] + 1; i++)
            {
                //Bucle que recorre desde la fila de arriba de el rey hasta la fila de abajo
                for (int j = coordenadasDibujar[1] - 1; j <= coordenadasDibujar[1] + 1; j++)
                {
                    if (0 <= i && i <= 7 && 0 <= j && j <= 7)
                    {
                        string casillaObjetivo = TraducirCoordenadaToCasilla(j, i);
                        //Si la casilla donde me intento mover esta vacia o tiene una pieza del color opuesto pinto camino
                        if (EstaLaCasillaVacia(casillaObjetivo) || EsUnaPiezaEnemiga(casillaObjetivo))
                        {
                            PintarCamino(casillaObjetivo);
                        }
                        //Si se puede enrocar y no hay ninguna pieza en medio pinto camino para enroque
                        if (EsPiezaBlanca(CasillaSeleccionada))
                        {
                            if (SePuedeEnrocar(true, true))
                            {
                                PintarEnroque("g1");
                            }
                            if (SePuedeEnrocar(true, false))
                            {
                                PintarEnroque("c1");
                            }
                        }
                        else
                        {
                            if (SePuedeEnrocar(false, true))
                            {
                                PintarEnroque("g8");
                            }
                            if (SePuedeEnrocar(false, false))
                            {
                                PintarEnroque("c8");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Metodo que dibuja una linea de camino recta partiendo de la casilla pasada por parametro
        /// </summary>
        /// <param name="SeAvanzaEnPositivo">Define el sentido de la linea recta</param>
        /// <param name="DibujoHorizontal">Define la horientacion de la linea recta</param>
        /// <param name="Casilla">Define el punto de partida de la linea recta</param>
        public void DibujarLineaRecta(bool SeAvanzaEnPositivo, bool DibujoHorizontal, string Casilla)
        {
            int[] coordenadasDibujar = TraducirCasillaCoordenadas(Casilla);
            int coordenadaAvance;

            if (DibujoHorizontal)
            {
                coordenadaAvance = coordenadasDibujar[0]; //Avanzare en la columna
            }
            else
            {
                coordenadaAvance = coordenadasDibujar[1]; //Avanzare en la fila
            }

            for (int i = coordenadaAvance + 1; i <= coordenadaAvance + 7; i++)
            {
                bool sePuedeDibujar;

                //Defino el booleano para ser true hasta salirme de el limite
                if (SeAvanzaEnPositivo)
                {
                    sePuedeDibujar = i <= 7;
                }
                else
                {
                    sePuedeDibujar = (coordenadaAvance - (i - coordenadaAvance)) >= 0;
                }

                //Si no estoy fuera de el tablero
                if (sePuedeDibujar)
                {
                    string casillaObjetivo = "";

                    //La formula varia dependiendo de la direccion y sentido
                    if (DibujoHorizontal && SeAvanzaEnPositivo) //Si voy a la derecha
                    {
                        casillaObjetivo = TraducirCoordenadaToCasilla(coordenadasDibujar[1], i);

                    }
                    else if (DibujoHorizontal && !SeAvanzaEnPositivo) //Si voy a la izquierda
                    {
                        casillaObjetivo = TraducirCoordenadaToCasilla(coordenadasDibujar[1], coordenadasDibujar[0] - (i - coordenadasDibujar[0]));

                    }
                    else if (!DibujoHorizontal && SeAvanzaEnPositivo) //Si voy hacia abajo
                    {
                        casillaObjetivo = TraducirCoordenadaToCasilla(i, coordenadasDibujar[0]);

                    }
                    else if (!DibujoHorizontal && !SeAvanzaEnPositivo) //Si voy hacia arriba
                    {
                        casillaObjetivo = TraducirCoordenadaToCasilla(coordenadasDibujar[1] - (i - coordenadasDibujar[1]), coordenadasDibujar[0]);
                    }

                    //Empiezo a mirar casillas
                    //Si encuentro una casilla aliada paro de dibujar
                    if (!EsUnaPiezaEnemiga(casillaObjetivo) && !EstaLaCasillaVacia(casillaObjetivo))
                    {
                        return;

                        //Si encuentro una pieza enemiga dibujo y paro
                    }
                    else if (EsUnaPiezaEnemiga(casillaObjetivo) && !EstaLaCasillaVacia(casillaObjetivo))
                    {
                        PintarCamino(casillaObjetivo);
                        return;

                        //Si no me han dicho de parar, no he econtrado pieza y no me he salido dibujo
                    }
                    else
                    {
                        PintarCamino(casillaObjetivo);
                    }
                }
                else
                {
                    return; //Cuando llego al limite de el tablero no intento dibujar mas
                }
            }
        }

        /// <summary>
        /// Metodo que dibuja una linea de camino diagonal partiendo de la casilla pasado por parametros 
        /// El sentido de esta linea depende de los booleanos
        /// </summary>
        /// <param name="seAvanzaHaciaArriba"></param>
        /// <param name="seAvanzaHaciaDerecha"></param>
        /// <param name="casilla"></param>
        public void DibujarLineaDiagonal(bool seAvanzaHaciaArriba, bool seAvanzaHaciaDerecha, string casilla)
        {

            int[] coordenadasOrigen = TraducirCasillaCoordenadas(casilla);
            int i = coordenadasOrigen[1];
            int j = coordenadasOrigen[0];

            //La coondicion varia de la direccion de la linea
            while (true)
            {
                i++;
                j++;

                int coordenadaHaciaArriba = coordenadasOrigen[1] - (i - coordenadasOrigen[1]);
                int coordenadaHaciaAbajo = i;
                int coordenadaHaciaDerecha = j;
                int coordenadaHaciaIzquierda = coordenadasOrigen[0] - (j - coordenadasOrigen[0]);


                //Si me salgo en alguna direccion en la cual estoy dibujando
                if ((seAvanzaHaciaArriba && coordenadaHaciaArriba < 0) ||
                    ((!seAvanzaHaciaArriba) && coordenadaHaciaAbajo > 7) ||
                    (seAvanzaHaciaDerecha && coordenadaHaciaDerecha > 7) ||
                    ((!seAvanzaHaciaDerecha) && coordenadaHaciaIzquierda < 0))
                {
                    return; //Dejo de divbjar la linea
                }
                else
                {
                    string casillaObjetivo = "";

                    //La formula varia dependiendo de la direccion y sentido
                    if (seAvanzaHaciaArriba && seAvanzaHaciaDerecha)
                    {
                        casillaObjetivo = TraducirCoordenadaToCasilla(coordenadaHaciaArriba, coordenadaHaciaDerecha);
                    }
                    else if (seAvanzaHaciaArriba && !seAvanzaHaciaDerecha)
                    {
                        casillaObjetivo = TraducirCoordenadaToCasilla(coordenadaHaciaArriba, coordenadaHaciaIzquierda);
                    }
                    else if (!seAvanzaHaciaArriba && seAvanzaHaciaDerecha)
                    {
                        casillaObjetivo = TraducirCoordenadaToCasilla(coordenadaHaciaAbajo, coordenadaHaciaDerecha);
                    }
                    else if (!seAvanzaHaciaArriba && !seAvanzaHaciaDerecha)
                    {
                        casillaObjetivo = TraducirCoordenadaToCasilla(coordenadaHaciaAbajo, coordenadaHaciaIzquierda);
                    }

                    //Empiezo a mirar casillas
                    //Si encuentro una casilla aliada paro de dibujar
                    if (!EsUnaPiezaEnemiga(casillaObjetivo) && !EstaLaCasillaVacia(casillaObjetivo))
                    {
                        return;

                        //Si encuentro una pieza enemiga dibujo y paro
                    }
                    else if (EsUnaPiezaEnemiga(casillaObjetivo) && !EstaLaCasillaVacia(casillaObjetivo))
                    {
                        PintarCamino(casillaObjetivo);
                        return;

                        //Si no me han dicho de parar, no he econtrado pieza y no me he salido dibujo
                    }
                    else
                    {
                        PintarCamino(casillaObjetivo);
                    }
                }
            }
        }

        /// <summary>
        /// Metodo que cambia el fondo de las dos ultmias casillas seleccionadas a rastro
        /// </summary>
        public void DibujarRastro()
        {
            PintarRastro(CasillaSeleccionada);
            PintarRastro(CasillaSeleccionadaAnterior);
        }


        //METODOS ENCARGADOS DE ACTUALIZAR LA INFORMACION EN LA INTERFAZ O DE EL JUEGO//

        /// <summary>
        /// Metodo que cambia el caracter de la casilla a el caracter pasado por parametros
        /// </summary>
        /// <param name="Casilla">Casilla cuyo caracter se quiere cambiar</param>
        /// <param name="NuevoContenido">Caracter nuevo</param>
        public void ActualizarCaracterCasilla(string Casilla, string NuevoContenido)
        {
            string Objetivo = "casilla_" + Casilla;
            Button Boton = FindName(Objetivo) as Button;
            Boton.Content = NuevoContenido;
        }
        /// <summary>
        /// Metodo que en caso de captura de una pieza actualiza la puntuacion de el jugador correspondiente
        /// </summary>
        public void ActualizarLabelPuntuacion()
        {
            //Si la casilla no esta vacia significa que hubo una captura
            if (!EstaLaCasillaVacia(CasillaSeleccionada))
            {
                string piezaCapturada = GetContenidoCasilla(CasillaSeleccionada);
                //Depende de el turno la pieza capturada
                if (EsTurnoDeBlancas)
                {
                    //Añado la pieza capturado y organizo todas las piezas
                    string capturas = CapturasBlancas.Content + piezaCapturada;
                    char[] piezasCapturadasArray = capturas.ToCharArray();
                    Array.Sort(piezasCapturadasArray);
                    Array.Reverse(piezasCapturadasArray);
                    capturas = new string(piezasCapturadasArray);
                    CapturasBlancas.Content = capturas;
                }
                else
                {
                    //Añado la pieza capturado y organizo todas las piezas
                    string capturas = CapturasNegras.Content + piezaCapturada;
                    char[] piezasCapturadasArray = capturas.ToCharArray();
                    Array.Sort(piezasCapturadasArray);
                    Array.Reverse(piezasCapturadasArray);
                    capturas = new string(piezasCapturadasArray);
                    CapturasNegras.Content = capturas;
                }
            }
        }

        /// <summary>
        /// Metodo que actualiza el label turno en funcion de a quien le toca jugar
        /// </summary>
        public void ActualizarLabelTurno()
        {
            LabelTurno.Content = EsTurnoDeBlancas ? "Turno: Blancas" : "Turno: Negras";
        }

        /// <summary>
        /// Metodo que en caso de moverse por primera vez una torre o un rey elimina la posibilidad de enroque correspondiente
        /// </summary>
        public void ActualizarBooleanosEnroque()
        {
            //La primera vez que el rey se mueve elimina toda posibilidad de enrocar esa partida
            if (CasillaSeleccionadaAnterior.Equals("e1"))
            {
                SePuedeEnroqueBlancoDerecha = false;
                SePuedeEnroqueBlancoIzquierda = false;
            }
            if (CasillaSeleccionadaAnterior.Equals("e8"))
            {
                SePuedeEnroqueNegroDerecha = false;
                SePuedeEnroqueNegroIzquierda = false;
            }

            //La primera vez que una torre se mueve elimina toda posibilidad de enrocar en ese lado
            if (CasillaSeleccionada.Equals("a1"))
            {
                SePuedeEnroqueBlancoIzquierda = false;
            }
            else if (CasillaSeleccionada.Equals("h1"))
            {
                SePuedeEnroqueBlancoDerecha = false;
            }
            else if (CasillaSeleccionada.Equals("a8"))
            {
                SePuedeEnroqueNegroIzquierda = false;
            }
            else if (CasillaSeleccionada.Equals("h8"))
            {
                SePuedeEnroqueNegroDerecha = false;
            }
        }

        /// <summary>
        /// Metodo que añade el string recivido a la notacion
        /// </summary>
        public void ActualizarNotacion(string nuevaNotacion)
        {
            labelNotacion.Text += nuevaNotacion;
        }

        /// <summary>
        /// Metodo que acutaliza el string de casilla en passant
        /// </summary>
        /// <param name="casilla"></param>
        public void ActualizarEnPassantPosible(string casilla)
        {
            int[] coordenadas = TraducirCasillaCoordenadas(casilla);
            //Si el peon movido es blanco la casilla de detras sera un posible en passant
            if (EsTurnoDeBlancas)
            {
                coordenadas[1] -= 1;
            }
            else
            {

                coordenadas[1] += 1;
            }
            casillaEnPassant = TraducirCoordenadaToCasilla(coordenadas);
        }


        //METODOS ENCARGADOS DE MOVER LAS PIEZAS//

        /// <summary>
        /// Metodo llamado por cada boton de el tablero el cual gestiona el juego
        /// </summary>
        /// <param name="Casilla"></param>
        public void SeleccionCasilla(string Casilla)
        {
            CasillaSeleccionadaAnterior = CasillaSeleccionada;
            CasillaSeleccionada = Casilla;
            IntentarMoverPieza();
        }

        /// <summary>
        /// Metodo el cual comprueba que la casilla seleccionada es una pieza que se puede mover
        /// y dibuja las posibles casillas a donde esa pieza se puede mover
        /// </summary>
        public void IntentarMoverPieza()
        {
            //Si hay camino significa que se puede mover una pieza
            if (!EstaElCaminoDibujado)
            {
                //Si no hay camino intento dibujarlo
                IntentarDibujarCamino();
            }
            else
            {
                //Si la casilla seleccionada es una de camino
                if (EsUnaCasillaDeCamino(CasillaSeleccionada))
                {
                    IniciarMovimiento();
                }
                else if (EsUnaCasillaDeEnroque(CasillaSeleccionada))
                {
                    IniciarEnroque();
                }
                else if (EsUnaCasillaDeEnPassant(CasillaSeleccionada))
                {
                    IniciarEnPassant();
                }
                else //Si la casilla seleccionada no es de camino ni enroque borro el camino anterior e intento dibujar su camino 
                {
                    BorrarCamino();
                    IntentarDibujarCamino();
                }
            }
        }

        /// <summary>
        /// Metodo que realiza el enroque haciendo que la torre salte el rey
        /// </summary>
        public void IniciarEnroque()
        {
            //Blancas = ♔♕♖♗♘♙
            //Negras  = ♚♛♜♝♞♟
            if (CasillaSeleccionada.Equals("g1"))
            {
                ActualizarCaracterCasilla("h1", EspacioVacio);
                ActualizarCaracterCasilla("f1", "♖");

            }
            else if (CasillaSeleccionada.Equals("c1"))
            {
                ActualizarCaracterCasilla("a1", EspacioVacio);
                ActualizarCaracterCasilla("d1", "♖");

            }
            else if (CasillaSeleccionada.Equals("g8"))
            {
                ActualizarCaracterCasilla("h8", EspacioVacio);
                ActualizarCaracterCasilla("f8", "♜");

            }
            else if (CasillaSeleccionada.Equals("c8"))
            {
                ActualizarCaracterCasilla("a8", EspacioVacio);
                ActualizarCaracterCasilla("d8", "♜");

            }

            IniciarMovimiento();
        }

        /// <summary>
        /// Metodo que realiza el enpassant
        /// </summary>
        public void IniciarEnPassant()
        {
            int[] coordenadas = TraducirCasillaCoordenadas(casillaEnPassant);

            //Muevo la pieza a ser capturada una casilla hacia atras para asi aprovecharme de el resto de metodos creados 
            if (EsTurnoDeBlancas)
            {
                coordenadas[1] += 1;
                ActualizarCaracterCasilla(casillaEnPassant, GetContenidoCasilla(TraducirCoordenadaToCasilla(coordenadas)));
                ActualizarCaracterCasilla(TraducirCoordenadaToCasilla(coordenadas), EspacioVacio);
            }
            else
            {
                coordenadas[1] -= 1;
                ActualizarCaracterCasilla(casillaEnPassant, GetContenidoCasilla(TraducirCoordenadaToCasilla(coordenadas)));
                ActualizarCaracterCasilla(TraducirCoordenadaToCasilla(coordenadas), EspacioVacio);
            }

            IniciarMovimiento();
        }

        /// <summary>
        /// Metodo que ueve la pieza de casillaSeleccionadaAnterior a la casillaSeleccionada y gestiona todo lo que mover una pieza conlleva
        /// </summary>
        public void IniciarMovimiento()
        {
            if (!SigueSiendoJaque()) //Si el nuevo movimiento no deja al rey de el color que le toca mover en jaque se puede seguir jugando, si es jaque no se realiza el movimiento
            {
                ActualizarLabelPuntuacion(); //Actualiza si es posible los label de puntuacion
                CalcularNotacion(); //Calcula la notacion y la actualiza
                RealizarMovimiento(); //Mueve la pieza
                IntentarPromocion(); //Verifica si la pieza es un peon que tiene que promocionar
                CambiarTurno();//Cambio el turno
                RestaurarTablero(); //Metodo que elimina todos los caminos y rastros
                DibujarRastro(); //Metod que dibuja el nuevo rastro
                ReproducirMoverPiezaSonido(); //Metodo que reproduce el sonido de mover pieza

                //La ventana de promocion se encarga de llamar estos metodos, esto con el fin de chequarjaque y final de partida despues de actualizar la casilla con la nueva pieza
                if (!Promocionando)
                {
                    ChequearJaque(true); //Metodo que actualiza los booleanos de jaque para asi controlar los movimientos en el siguiente turno
                    ChequaFinalPartida();
                }
            }
        }

        /// <summary>
        /// Metodo que traduce el movimiento realizado a notacion y actuazlia el label de la notacion
        /// </summary>
        public void CalcularNotacion()
        {
            string notacion = "";

            //Si es el turno de blancas coloco el numero de jugada
            if (EsTurnoDeBlancas)
            {
                ContadorNotacion++;
                notacion += ContadorNotacion + ". ";
            }

            //Si hay enroque la notacion es especial
            if (EsUnaCasillaDeEnroque(CasillaSeleccionada))
            {
                //Si la notacion es a la izquierda
                if (CasillaSeleccionada.Equals("c8") || CasillaSeleccionada.Equals("c1"))
                {
                    notacion += GetContenidoCasilla(CasillaSeleccionadaAnterior) + "0-0-0 ";
                }
                else
                {

                    notacion += GetContenidoCasilla(CasillaSeleccionadaAnterior) + "0-0 ";
                }
            } //En el caso de una captura
            else if (!EstaLaCasillaVacia(CasillaSeleccionada))
            {
                notacion += GetContenidoCasilla(CasillaSeleccionadaAnterior) + "x" + CasillaSeleccionada + " ";
            } //En caso de movimietno 
            else
            {
                notacion += GetContenidoCasilla(CasillaSeleccionadaAnterior) + CasillaSeleccionada + " ";

            }

            if (!EsTurnoDeBlancas)
            {
                notacion += "  ";
                if (ContadorNotacion % 2 == 0)
                {
                    notacion += "\n";
                }
            }

            ActualizarNotacion(notacion);

        }

        /// <summary>
        /// Metodo que acutaliza las casillas para mover la pieza
        /// </summary>
        public void RealizarMovimiento()
        {
            ActualizarBooleanosEnroque();
            CalcularCasillaEnPassant();
            ActualizarCaracterCasilla(CasillaSeleccionada, GetContenidoCasilla(CasillaSeleccionadaAnterior));
            ActualizarCaracterCasilla(CasillaSeleccionadaAnterior, EspacioVacio);
        }

        /// <summary>
        /// Metodo que no deja que el jugador mueva una pieza si despues de dicho movimiento sigue estando en jaque
        /// </summary>
        /// <returns></returns>
        public bool SigueSiendoJaque()
        {
            //En caso de jaque, muevo las piezas y miro si hay jaque, en caso de haberlo cancelo el movimiento e informo, sino hay jaque, 

            //Primero guardo el estado de el tablero para poder restaurarlo luego y tambien el de las variables que se puedan ver alteradas
            char[,] tableroGuardado = GetTablero();
            bool jaqueBlanco = JaqueReyBlanco;
            bool jaqueNegro = JaqueReyNegro;
            string LaCasillaSeleccionada = CasillaSeleccionada;

            //Hago el movimiento y chequeo si el rey sigue en jaque
            ActualizarCaracterCasilla(CasillaSeleccionada, GetContenidoCasilla(CasillaSeleccionadaAnterior));
            ActualizarCaracterCasilla(CasillaSeleccionadaAnterior, EspacioVacio);
            ChequearJaque(true);

            if (JaqueReyBlanco && EsTurnoDeBlancas)
            {
                //Restauro el estado de la partida
                JaqueReyNegro = jaqueNegro;
                JaqueReyBlanco = jaqueBlanco;
                RellenarTablero(tableroGuardado);
                CasillaSeleccionada = LaCasillaSeleccionada;

                //Mensaje de movimiento ilegal
                string messageBoxText = "Este movimiento es ilegal ya que dejaria a tu rey en jaque";
                string caption = "Movimiento ilegal";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                return true;
            }
            if (JaqueReyNegro && !EsTurnoDeBlancas)
            {
                //Restauro el estado de la partida
                JaqueReyBlanco = jaqueBlanco;
                JaqueReyNegro = jaqueNegro;
                RellenarTablero(tableroGuardado);
                CasillaSeleccionada = LaCasillaSeleccionada;

                //Mensaje de movimiento ilegal
                string messageBoxText = "Este movimiento es ilegal ya que dejaria a tu rey en jaque";
                string caption = "Movimiento ilegal";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                return true;

            }
            //Si no habia jaque devuelvo false

            //Restauro el estado de la partida
            JaqueReyNegro = jaqueNegro;
            CasillaSeleccionada = LaCasillaSeleccionada;
            JaqueReyBlanco = jaqueBlanco;
            RellenarTablero(tableroGuardado);

            return false;
        }

        /// <summary>
        /// Metodo que verifica que un peon dio un paso doble y actualiza la informacion para que el juego sepa que se puede dar un enpassant
        /// </summary>
        public void CalcularCasillaEnPassant()
        {
            //Si se mueve un peon que estaba en su casilla inicial dos casillas hacia adelante ese peon podra ser comido por enpassant
            if (GetContenidoCasilla(CasillaSeleccionadaAnterior).Equals("♙") && GetFila(CasillaSeleccionadaAnterior) == 6 && (GetFila(CasillaSeleccionadaAnterior) - 2) == GetFila(CasillaSeleccionada))
            {
                ActualizarEnPassantPosible(CasillaSeleccionadaAnterior);
            }
            else if (GetContenidoCasilla(CasillaSeleccionadaAnterior).Equals("♟") && GetFila(CasillaSeleccionadaAnterior) == 1 && (GetFila(CasillaSeleccionadaAnterior) + 2) == GetFila(CasillaSeleccionada))
            {
                ActualizarEnPassantPosible(CasillaSeleccionadaAnterior);
            }
            else
            {
                //Limpia la posible casilla anterior
                casillaEnPassant = "";
            }
        }

        /// <summary>
        /// Metodo que cambia el turno de el color que toca
        /// </summary>
        public void CambiarTurno()
        {
            EsTurnoDeBlancas = !EsTurnoDeBlancas;
            ActualizarLabelTurno();
        }

        /// <summary>
        /// Metodo que cambiara el booleano de true o false en funcion de si alguna pieza esta mirando a el rey enemigo
        /// tiene dos modos, cuando se llama despues de un movimiento ("ChequearJaque(true)") verifica si el ultimo movimiento
        /// ha dejado en jaque al rey enemigo y el otro modo 
        /// </summary>
        public void ChequearJaque(bool movimientoHecho)
        {
            if (movimientoHecho)
            {
                JaqueReyBlanco = false;
                JaqueReyNegro = false;
            }
            if (JaqueReyNegro && !EsTurnoDeBlancas && !movimientoHecho)
            {
                JaqueReyNegro = false;
            }
            else if (JaqueReyBlanco && !EsTurnoDeBlancas && !movimientoHecho)
            {
                JaqueReyBlanco = false;
            }

            string guardarCasilla = CasillaSeleccionada; //Guardo la casilla seleccionado
            EsTurnoDeBlancas = !EsTurnoDeBlancas; //Cambio el turno para poder chequear las piezas de el color opuesto
            ChequeandoJaque = true; //booleano que cambia el funcionamiento de el metodo dibujarcamino

            string blancas = "♔♕♖♗♘♙";
            string negras = "♚♛♜♝♞♟";

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string casilla = TraducirCoordenadaToCasilla(i, j);

                    //Si acaban de jugar las blancas mirare si alguna de las piezas negras tiene vision de el rey blanco
                    if (!EsTurnoDeBlancas)
                    {
                        //Llamo al metodo dibujar camino de todas las piezas de un color que al estar el booleano chequear jaque
                        //activado no dibujaara camino sino que mirara si el rey opuesto esta en el camino de alguna pieza
                        if (negras.Contains(GetContenidoCasilla(casilla)))
                        {
                            CasillaSeleccionada = casilla;
                            IntentarDibujarCamino();
                        }
                    }
                    else
                    {
                        if (blancas.Contains(GetContenidoCasilla(casilla)))
                        {
                            CasillaSeleccionada = casilla;
                            IntentarDibujarCamino();
                        }
                    }
                }
            }

            CasillaSeleccionada = guardarCasilla; //Restauro la casilla
            ChequeandoJaque = false;
            BorrarCamino();
            EsTurnoDeBlancas = !EsTurnoDeBlancas;

        }

        /// <summary>
        /// Metodo que en caso de mate informa y para la partida
        /// </summary>
        public void ChequaFinalPartida()
        {
            if (EsMate() || EsEmpate())
            {
                FinalPartida();
            }
        }

        /// <summary>
        /// Metodo que informa quien gano la partida
        /// </summary>
        public void FinalPartida()
        {
            string ganador;
            if (EsEmpate())
            {
                string messageBoxText = "Ha habido un empate, ninguno ha ganado";
                string caption = "Juego acabado";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                GridTablero.IsEnabled = false;
            }
            else
            {
                if (EsTurnoDeBlancas)
                {
                    ganador = "Negras";
                }
                else
                {
                    ganador = "Blancas";
                }
                GridTablero.IsEnabled = false;
                string messageBoxText = "EL jugador de las piezas " + ganador + " ha ganado la partida.";
                string caption = "Juego acabado";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }

            GridDebug.Visibility = Visibility.Visible;
        }

        //METODOS PARA TRADUCIR COORDENADAS//

        /// <summary>
        /// Metodo que recibe una array de coordenadas y la traduce a Casilla de tablero
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
        /// </summary>
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

        /// <summary>
        /// Metodo que recibe una coordenada del array y la traduce a Casilla de tablero
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
        /// </summary>
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

        /// <summary>
        /// Metodo que recibe una coordenada "X" y otra "Y" y las traduce a su casilla en notacion.
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
        /// <param name="fila">Coordenada "Y" (numero)</param>
        /// <param name="letra">Coordenada "X" (letra)</param>
        /// <returns></returns>
        public static string TraducirCoordenadaToCasilla(int fila, int letra)
        {
            char[] auxiliarColumna = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'
            };

            char columna = auxiliarColumna[letra];
            int numero = 8 - fila;

            return "" + columna + numero;
        }


        //METODOS GET://

        /// <summary>
        /// Metodo que devuelve el caracter de la casilla
        /// </summary>
        /// <param name="Casilla"></param>
        /// <returns></returns>
        public string GetContenidoCasilla(string Casilla)
        {
            string Objetivo = "casilla_" + Casilla;
            Button Boton = FindName(Objetivo) as Button;
            return Boton.Content.ToString();
        }

        /// <summary>
        /// Metodo que devuelve el color de fondo de la casilla
        /// </summary>
        /// <param name="casilla">casilla de la cual se quiere obtener el color</param>
        /// <returns></returns>
        public string GetColorFondo(string casilla)
        {

            string Objetivo = "casilla_" + casilla;
            Button Boton = FindName(Objetivo) as Button;
            return Boton.Background.ToString();
        }

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
        private int GetTipoDeFicha(string Casilla)
        {
            switch (GetContenidoCasilla(Casilla))
            {
                case "♙":
                case "♟":
                    return 0;
                case "♗":
                case "♝":
                    return 1;
                case "♘":
                case "♞":
                    return 2;
                case "♖":
                case "♜":
                    return 3;
                case "♕":
                case "♛":
                    return 4;
                case "♔":
                case "♚":
                    return 5;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// Metodo que devuelve la coordenada fila de la casilla pasada por parametros
        /// </summary>
        /// <param name="casilla"></param>
        /// <returns></returns>
        public int GetFila(string casilla)
        {
            int[] coordenadas = TraducirCasillaCoordenadas(casilla);
            return coordenadas[1];
        }

        /// <summary>
        /// Metodo que devuelve el tablero de la partida actual
        /// </summary>
        /// <returns></returns>
        public char[,] GetTablero()
        {
            char[,] tableroRespuesta = new char[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string casilla = TraducirCoordenadaToCasilla(i, j);
                    tableroRespuesta[i, j] = GetContenidoCasilla(casilla)[0];
                }
            }

            return tableroRespuesta;
        }

        /// <summary>
        /// Metodo que devuelve las casillas que tienen un rastro dibujado
        /// </summary>
        /// <returns></returns>
        public List<string> GetListaRastro()
        {
            List<string> lista = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string casilla = TraducirCoordenadaToCasilla(i, j);
                    if (EsUnaCasillaDeRastro(casilla))
                    {
                        lista.Add(casilla);
                    }
                }
            }
            return lista;
        }

        /// <summary>
        /// Metodo que recorre el tablero contando la cantidad de casillas de camino que hay
        /// </summary>
        /// <returns></returns>
        public int GetCantidadCamino()
        {
            int cantidad = 0;
            List<string> listaRastro = GetListaRastro();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string casilla = TraducirCoordenadaToCasilla(i, j);
                    if (EsUnaCasillaDeCamino(casilla) || EsUnaCasillaDeEnPassant(casilla) || EsUnaCasillaDeEnroque(casilla))
                    {
                        cantidad++;
                    }

                }
            }
            BorrarCamino();
            SetRastro(listaRastro);
            return cantidad;
        }


        //METODOS SET://

        /// <summary>
        /// Metodo que cambia el fondo de la casilla a uno de los 3 fondos dependiendo de el modo:
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
        /// <item>
        /// <description>Modo 3 = colorEnroque</description>
        /// </item>
        /// <item>
        /// <description>Modo 4 = colorEnPassant</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le quiere cambiar el fondo</param>
        /// <param name="modo">
        /// </param>
        public void SetFondo(string Casilla, int modo)
        {
            SolidColorBrush colorClaroBaseAux = (SolidColorBrush)Application.Current.Resources["colorClaroBase"];
            SolidColorBrush colorClaroCaminoAux = (SolidColorBrush)Application.Current.Resources["colorClaroCamino"];
            SolidColorBrush colorClaroRastroAux = (SolidColorBrush)Application.Current.Resources["colorClaroRastro"];
            SolidColorBrush colorOscuroBaseAux = (SolidColorBrush)Application.Current.Resources["colorOscuroBase"];
            SolidColorBrush colorOscuroCaminoAux = (SolidColorBrush)Application.Current.Resources["colorOscuroCamino"];
            SolidColorBrush colorOscuroRastroAux = (SolidColorBrush)Application.Current.Resources["colorOscuroRastro"];


            string Objetivo = "casilla_" + Casilla;
            Button Boton = FindName(Objetivo) as Button;


            //Si la suma de las coordenadas es par, la casilla es de color claro, sino sera de color oscuro
            int[] coordenadas = TraducirCasillaCoordenadas(Casilla);
            bool esClara = (coordenadas[0] + coordenadas[1]) % 2 == 0;
            if (!esClara)
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
                    case 3:
                        Boton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#D31A38");
                        break;
                    case 4:
                        Boton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E6B");
                        break;
                    default:
                        break;
                }
            }
            else //si la Casilla es de color claro
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
                    case 3:
                        Boton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#D31A38");
                        break;
                    case 4:
                        Boton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E6B");
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Mejtodo que establece los parametros de el juego en funcion de los pasados y actualiza el label turno y limpia la notacion
        /// </summary>
        /// <param name="turnBlanc"></param>
        /// <param name="enroqueBlancDere"></param>
        /// <param name="enroqueBlancIzq"></param>
        /// <param name="enroqueNegrDere"></param>
        /// <param name="enroqueNegrIzq"></param>
        public void SetParametrosPartida(bool turnBlanc, bool enroqueBlancDere, bool enroqueBlancIzq, bool enroqueNegrDere, bool enroqueNegrIzq)
        {
            EsTurnoDeBlancas = turnBlanc;
            SePuedeEnroqueBlancoDerecha = enroqueBlancDere;
            SePuedeEnroqueBlancoIzquierda = enroqueBlancIzq;
            SePuedeEnroqueNegroDerecha = enroqueNegrDere;
            SePuedeEnroqueNegroIzquierda = enroqueNegrIzq;
            ActualizarLabelTurno();
            CambiarDebugText("... ");
        }

        /// <summary>
        /// Metodo que hace "set De una partida entera con todos sus parametros"
        /// </summary>
        /// <param name="tablero">Tablero de la partida</param>
        /// <param name="turnoBlanc">A quien le toca jugar</param>
        /// <param name="enroqueBlancDere">El blanco puede enrocar hacia la derecha</param>
        /// <param name="enroqueBlancIzq">El blanco puede enrocar hacia la izquierda</param>
        /// <param name="enroqueNegrDere">El negro puede enrocar hacia la derecha</param>
        /// <param name="enroqueNegrIzq">El negro puede enrocar hacia la izquierda</param>
        /// <param name="jaqueBlanc">El rey blanco esta en jaque</param>
        /// <param name="jaqueNegr">El rey negro esta en jaque</param>
        public void SetParametrosPartida(char[,] tablero, bool turnoBlanc, bool enroqueBlancDere, bool enroqueBlancIzq, bool enroqueNegrDere, bool enroqueNegrIzq, bool jaqueBlanc, bool jaqueNegr, List<string> listaRastro, string casiSelec, string casiSelecAnt)
        {
            RellenarTablero(tablero);
            EsTurnoDeBlancas = turnoBlanc;
            SePuedeEnroqueBlancoDerecha = enroqueBlancDere;
            SePuedeEnroqueBlancoIzquierda = enroqueBlancIzq;
            SePuedeEnroqueNegroDerecha = enroqueNegrDere;
            SePuedeEnroqueNegroIzquierda = enroqueNegrIzq;
            JaqueReyBlanco = jaqueBlanc;
            JaqueReyNegro = jaqueNegr;
            CasillaSeleccionada = casiSelec;
            CasillaSeleccionadaAnterior = casiSelecAnt;

            RestaurarTablero(); //Borro rastro y caminos viejos

            SetRastro(listaRastro);

            
        }

        /// <summary>
        /// Metodo que hace set de el rastro recibido
        /// </summary>
        public void SetRastro(List<string> listaRastro)
        {
            //Restauro el rastro
            if (listaRastro != null && listaRastro.Count > 0)
            {
                foreach (string a in listaRastro)
                {
                    PintarRastro(a);
                }
            }
        }


        //METODOS BOOL://

        /// <summary>
        /// Metodo que devuelve true si el caracter correspondiente a la casilla es una pieza blanca
        /// </summary>
        /// <param name="Casilla">Casilla la cual se comprobara</param>
        /// <returns></returns>
        private bool EsPiezaBlanca(string Casilla)
        {
            string blancas = "♔♕♖♗♘♙";

            return blancas.Contains(GetContenidoCasilla(Casilla));
        }

        /// <summary>
        /// Metodo que devuelve true si el caracter de la casilla corresponde con el caracter usado en las casillas vacias
        /// </summary>
        /// <param name="Casilla">Casilla la cual se comprobara</param>
        /// <returns></returns>
        private bool EstaLaCasillaVacia(string Casilla)
        {
            return EspacioVacio.Equals(GetContenidoCasilla(Casilla));
        }

        /// <summary>
        /// Metodo que devuelve true si el fondo de la casilla pasada por parametros
        /// es uno de los colores de camino
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le mirara el fondo</param>
        /// <returns></returns>
        private bool EsUnaCasillaDeCamino(string Casilla)
        {
            SolidColorBrush colorClaroCaminoAux = (SolidColorBrush)App.Current.Resources["colorClaroCamino"];
            SolidColorBrush colorOscuroCaminoAux = (SolidColorBrush)App.Current.Resources["colorOscuroCamino"];

            if (GetColorFondo(Casilla).Equals(colorClaroCaminoAux.ToString()) || GetColorFondo(Casilla).Equals(colorOscuroCaminoAux.ToString()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo que devuelve true si el fondo de la casilla pasada por parametros
        /// es uno de los colores de camino
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le mirara el fondo</param>
        /// <returns></returns>
        private bool EsUnaCasillaDeRastro(string Casilla)
        {
            SolidColorBrush colorClaroCaminoAux = (SolidColorBrush)App.Current.Resources["colorClaroRastro"];
            SolidColorBrush colorOscuroCaminoAux = (SolidColorBrush)App.Current.Resources["colorOscuroRastro"];

            if (GetColorFondo(Casilla).Equals(colorClaroCaminoAux.ToString()) || GetColorFondo(Casilla).Equals(colorOscuroCaminoAux.ToString()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo que devuelve true si el fondo de la casilla pasada por parametros
        /// es un color de enroque
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le mirara el fondo</param>
        /// <returns></returns>
        private bool EsUnaCasillaDeEnroque(string Casilla)
        {
            if (GetColorFondo(Casilla).Equals("#FFD31A38"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo que devuelve true si el fondo de la casilla pasada por parametros
        /// es un color de EnPassant
        /// </summary>
        /// <param name="Casilla">Casilla a la cual se le mirara el fondo</param>
        /// <returns></returns>
        private bool EsUnaCasillaDeEnPassant(string casilla)
        {
            if (GetColorFondo(casilla).Equals("#FFFF7E6B"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo que devuelve true si la fila corresponde a la fila inicial de el color definido por el booleano
        /// </summary>
        /// <param name="esBlanco">Define el color de la pieza, true para piezas blancas y false para negras</param>
        /// <param name="fila">Fila donde la pieza se encuentra</param>
        /// <returns></returns>
        private bool EstaPeonEnFilaInicial(bool esBlanco, int fila)
        {
            if (esBlanco && fila == 2)
            {
                return true;
            }
            else if (!esBlanco && fila == 7)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo que verifica si hay un enroque disponible en funcion de los parametros pasados
        /// </summary>
        /// <param name="esBlanco"></param>
        /// <param name="derecha"></param>
        /// <returns></returns>
        private bool SePuedeEnrocar(bool esBlanco, bool derecha)
        {
            if (esBlanco && derecha && SePuedeEnroqueBlancoDerecha && EstaLaCasillaVacia("f1") && EstaLaCasillaVacia("g1"))
            {
                return true;
            }
            else if (esBlanco && !derecha && SePuedeEnroqueBlancoIzquierda && EstaLaCasillaVacia("b1") && EstaLaCasillaVacia("c1") && EstaLaCasillaVacia("d1"))
            {
                return true;
            }
            else if ((!esBlanco) && derecha && SePuedeEnroqueNegroDerecha && EstaLaCasillaVacia("f8") && EstaLaCasillaVacia("g8"))
            {
                return true;
            }
            else if ((!esBlanco) && !derecha && SePuedeEnroqueNegroIzquierda && EstaLaCasillaVacia("b8") && EstaLaCasillaVacia("c8") && EstaLaCasillaVacia("d8"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo que devuelve true si la pieza en la casilla no es de el color al que le toca jugar
        /// </summary>
        /// <param name="Casilla"></param>
        /// <returns></returns>
        private bool EsUnaPiezaEnemiga(string Casilla)
        {
            return EsTurnoDeBlancas != EsPiezaBlanca(Casilla);
        }

        /// <summary>
        /// Metodo que en funcion de la partida detecta si hay un mate
        /// </summary>
        /// <returns></returns>
        private bool EsMate()
        {
            if (JaqueReyBlanco && EsTurnoDeBlancas)
            {
                if (!HayAlgunMovimientoPosible(true))
                {
                    return true;
                }
            }
            if (JaqueReyNegro && !EsTurnoDeBlancas)
            {
                if (!HayAlgunMovimientoPosible(false))
                {
                    return true;
                }
            }


            return false;
        }

        /// <summary>
        /// Este metodo intentara realizar cada movimiento posible para las piezas de el color pasado por parametro
        /// y devolvera true si hay algun movimiento posible
        /// </summary>
        /// <param name="chequearBlancas"></param>
        /// <returns></returns>
        private bool HayAlgunMovimientoPosible(bool chequearBlancas)
        {
            //Primero guardare el estado de la partida completamente para restaurarlo al final
            char[,] tablero = GetTablero();
            bool turnoBlanc = EsTurnoDeBlancas;
            bool enroqueBlancDere = SePuedeEnroqueBlancoDerecha;
            bool enroqueBlancIzq = SePuedeEnroqueBlancoIzquierda;
            bool enroqueNegrDere = SePuedeEnroqueNegroDerecha;
            bool enroqueNegrIzq = SePuedeEnroqueNegroIzquierda;
            bool jaqueBlanc = JaqueReyBlanco;
            bool jaqueNegr = JaqueReyNegro;
            string casiSelec = CasillaSeleccionada;
            string casiSelecAnt = CasillaSeleccionadaAnterior;
            List<string> listaRastro = GetListaRastro();

            //
            string blancas = "♔♕♖♗♘♙";
            string negras = "♚♛♜♝♞♟";

            //Recorro todo el tablero hasta encontrar una pieza de el color que estoy intentado chequear
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    SetParametrosPartida(tablero, turnoBlanc, enroqueBlancDere, enroqueBlancIzq, enroqueNegrDere, 
                        enroqueNegrIzq, jaqueBlanc, jaqueNegr, listaRastro, casiSelec, casiSelecAnt); //Cada rotacion restauro el tablero
                    CasillaSeleccionada = TraducirCoordenadaToCasilla(i, j);
                    
                    //Ahora intentare realizar todo movimiento posible y si en algun movimiento el jaque es false significa que si hay un movimiento posible
                    if (chequearBlancas)
                    {
                        JaqueReyBlanco = true; //Hago que el jaque sea true, movere una pieza y mirare si el jaque sigue siendo true, si en algun momento es false significa que si hay un movimiento posible
                        EsTurnoDeBlancas = true;

                        //Si encuentro una pieza de el color que intento chequear
                        if (blancas.Contains(GetContenidoCasilla(CasillaSeleccionada)))
                        {
                            EstaElCaminoDibujado = false;
                            IntentarDibujarCamino(); //Intento dibujar su camino

                            //Si no hay ningun camino no hay nada que intentar
                            if (EstaElCaminoDibujado)
                            {
                                for (int k = 0; k < 8; k++)
                                {
                                    for (int l = 0; l < 8; l++)
                                    {
                                        CasillaSeleccionadaAnterior = CasillaSeleccionada;
                                        CasillaSeleccionada = TraducirCoordenadaToCasilla(k, l);

                                        //Si la casilla seleccionada es una casilla de camino
                                        if (EsUnaCasillaDeCamino(CasillaSeleccionada))
                                        {
                                            //Realizo el movimiento y miro si hay jaque
                                            ActualizarCaracterCasilla(CasillaSeleccionada, GetContenidoCasilla(CasillaSeleccionadaAnterior));
                                            ActualizarCaracterCasilla(CasillaSeleccionadaAnterior, EspacioVacio);
                                            ChequearJaque(true);

                                            //Si despues de el movimiento simulado no es jaque significa que hay un movimiento posible por lo que restaurare la partida y devolvere true 
                                            if (!JaqueReyBlanco)
                                            {
                                                SetParametrosPartida(tablero, turnoBlanc, enroqueBlancDere, enroqueBlancIzq, enroqueNegrDere, enroqueNegrIzq,
                                                    jaqueBlanc, jaqueNegr, listaRastro, casiSelec, casiSelecAnt);
                                                return true;
                                            }
                                        }
                                        //Si la casilla no es de camino, o se simulo el movimiento y sigue siendo jaque
                                        //Restauro la partida y sigo buscando una casilla de camino
                                        SetParametrosPartida(tablero, turnoBlanc, enroqueBlancDere, enroqueBlancIzq, enroqueNegrDere,
                                        enroqueNegrIzq, jaqueBlanc, jaqueNegr, listaRastro, casiSelec, casiSelecAnt);
                                        JaqueReyBlanco = true;
                                        EsTurnoDeBlancas = true;
                                        CasillaSeleccionada = TraducirCoordenadaToCasilla(i, j);
                                        IntentarDibujarCamino(); //Dibujo su camino otra vez para seguir intentando

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        JaqueReyNegro = true; //Hago que el jaque sea true, movere una pieza y mirare si el jaque sigue siendo true, si en algun momento es false significa que si hay un movimiento posible
                        EsTurnoDeBlancas = false;

                        //Si encuentro una pieza de el color que intento chequear
                        if (negras.Contains(GetContenidoCasilla(CasillaSeleccionada)))
                        {
                            EstaElCaminoDibujado = false;
                            IntentarDibujarCamino(); //Intento dibujar su camino

                            if (EstaElCaminoDibujado)
                            {
                                for (int k = 0; k < 8; k++)
                                {
                                    for (int l = 0; l < 8; l++)
                                    {
                                        CasillaSeleccionadaAnterior = CasillaSeleccionada;
                                        CasillaSeleccionada = TraducirCoordenadaToCasilla(k, l);

                                        //Si la casilla seleccionada es una casilla de camino
                                        if (EsUnaCasillaDeCamino(CasillaSeleccionada))
                                        {
                                            //Realizo el movimiento y miro si hay jaque
                                            ActualizarCaracterCasilla(CasillaSeleccionada, GetContenidoCasilla(CasillaSeleccionadaAnterior));
                                            ActualizarCaracterCasilla(CasillaSeleccionadaAnterior, EspacioVacio);
                                            ChequearJaque(true);

                                            //Si despues de el movimiento simulado no es jaque significa que hay un movimiento posible por lo que restaurare la partida y devolvere true 
                                            if (!JaqueReyNegro)
                                            {
                                                SetParametrosPartida(tablero, turnoBlanc, enroqueBlancDere, enroqueBlancIzq, enroqueNegrDere, enroqueNegrIzq,
                                                    jaqueBlanc, jaqueNegr, listaRastro, casiSelec, casiSelecAnt);
                                                return true;
                                            }
                                        }
                                        //Si la casilla no es de camino, o se simulo el movimiento y sigue siendo jaque
                                        //Restauro la partida y sigo buscando una casilla de camino
                                        SetParametrosPartida(tablero, turnoBlanc, enroqueBlancDere, enroqueBlancIzq, enroqueNegrDere,
                                        enroqueNegrIzq, jaqueBlanc, jaqueNegr, listaRastro, casiSelec, casiSelecAnt);
                                        JaqueReyNegro = true;
                                        EsTurnoDeBlancas = false;
                                        CasillaSeleccionada = TraducirCoordenadaToCasilla(i, j);
                                        IntentarDibujarCamino(); //Dibujo su camino otra vez para seguir intentando
                                    }
                                }
                            }
                        }
                    }
                }
            }


            //Al final de el metodo restaurare la partida y al no haber encontrado un movimiento posible devolvere false
            SetParametrosPartida(tablero, turnoBlanc, enroqueBlancDere, enroqueBlancIzq, enroqueNegrDere, enroqueNegrIzq, jaqueBlanc, jaqueNegr, listaRastro, casiSelec, casiSelecAnt);
            return false;



        }

        /// <summary>
        /// Metodo que en funcion de la partida detecta si hay un empate
        /// </summary>
        /// <returns></returns>
        private bool EsEmpate()
        {
            if (EsReyAhogado())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Metodo que comprueba si el jugador a el que le toca jugar tiene algun movimiento posible o esta ahogado
        /// </summary>
        /// <returns></returns>
        private bool EsReyAhogado()
        {


            if (EsTurnoDeBlancas && !JaqueReyBlanco)
            {
                if (!HayAlgunMovimientoPosible(true))
                {
                    return true;
                }
            } 
            else if(!EsTurnoDeBlancas && !JaqueReyNegro)
            {
                if (!HayAlgunMovimientoPosible(false))
                {
                    return true;
                }
            }
            return false;
        }

        //Metodos encargados de los sonidos//

        /// <summary>
        /// Metodo que prepara el sonido de mover pieza para cuando se quiera usar
        /// </summary>
        public void CargarSonidoMoverPieza()
        {
            System.IO.Stream recursoaudio = Properties.Resources.movimientoPieza;
            ReproductorDeSonidoMoverPieza = new System.Media.SoundPlayer(recursoaudio);
            ReproductorDeSonidoMoverPieza.Load();
        }

        /// <summary>
        /// Metodo que reproduce el sonido de moverPieza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReproducirMoverPiezaSonido()
        {
            ReproductorDeSonidoMoverPieza.Play();
        }

        /// <summary>
        /// Metodo que carga y reproduce el sonido de InicioPartida
        /// </summary>
        public void ReproducirSonidoInicioPartida()
        {
            System.IO.Stream recursoaudio = Properties.Resources.InicioPartida;
            ReproductorDeSonidoInicioPartida = new System.Media.SoundPlayer(recursoaudio);
            ReproductorDeSonidoInicioPartida.Load();
            ReproductorDeSonidoInicioPartida.Play();
        }

        /// <summary>
        /// Boton el cual reproduce el sonido de los botones de el programa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReproducirSonidoBotonAplicacion(object sender, MouseEventArgs e)
        {
            ventanaInicio.ReproducirSonidoBotonSistema(sender, e);

        }


        //BOTONES DE LA INTERFAZ//

        /// <summary>
        /// Boton que oculta la ventana de juego y muestra la ventana de el inicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonvolverInicio_Click(object sender, RoutedEventArgs e)
        {
            BorrarCamino();
            CasillaSeleccionada = "";
            CasillaSeleccionadaAnterior = "";

            Hide();
            ventanaInicio.MostrarBotonContinuar();
            ventanaInicio.Show();

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
        /// Boton el cual cierra el programa y todas sus ventanas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonrSalir_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


        //METODOS DE DEBUG//

        /// <summary>
        /// Metodo que cambia la visibilida de el menu oculto a visible
        /// </summary>
        public void MostrarMenuDebug()
        {
            if (ventanaInicio.modoDebug)
            {
                GridDebug.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Metodo que cambia el contenido de el label debug
        /// </summary>
        /// <param name="laString"></param>
        public void CambiarDebugText(string laString)
        {
            labelNotacion.Text = laString;
        }

        /// <summary>
        /// Boton que carga el tablero recibido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CargarTableroDebug(object sender, RoutedEventArgs e)
        {
            GridTablero.IsEnabled = true;
            RestaurarTablero();
            ReproducirSonidoInicioPartida();
            ContadorNotacion = 0;

            //Puzzle
            char[,] tableroMateEnUno = new char[,]
            {
                { ' ','♗','♝',' ',' ',' ','♗','♘'},
                { '♖',' ',' ','♙','♚',' ',' ','♜'},
                { ' ','♕',' ',' ',' ',' ',' ','♗'},
                { ' ',' ',' ',' ','♛',' ',' ',' '},
                { ' ',' ','♝','♘',' ',' ',' ',' '},
                { ' ',' ',' ',' ','♕',' ','♗','♔'},
                { ' ','♟',' ',' ',' ',' ',' ',' '},
                { ' ','♝','♛',' ','♖',' ','♜','♝'},
            };

            //julitoflo (1613) vs. Loefwing (1666) - New Years Challenge 2015 - 26 Jan 2015
            char[,] tableroPromocionMateEnUno = new char[,]
            {
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { '♟',' ',' ',' ',' ',' ',' ',' '},
                { '♙',' ',' ',' ',' ',' ',' ','♟'},
                { '♔',' ','♞',' ',' ',' ',' ','♙'},
                { ' ','♟','♚',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
            };

            //Edward Lasker vs. Sir George Thomas - Londres - 1911
            char[,] tableroEnroqueParaMate = new char[,]
            {
                { '♜','♞',' ',' ',' ','♜',' ',' '},
                { '♟','♝','♟','♟','♛',' ','♟',' '},
                { ' ','♟',' ',' ','♟','♘',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ','♙',' ',' ','♘','♙'},
                { ' ',' ',' ',' ',' ',' ','♙',' '},
                { '♙','♙','♙',' ','♗','♙',' ','♖'},
                { '♖',' ',' ',' ','♔',' ','♚',' '},
            };

            //Jon Ludvig Hammer vs Magnus Carlsen (top 1 de el mundo) - Live Chess - 2023
            char[,] tableroEnPassantMate = new char[,]
            {
                { ' ',' ',' ','♖',' ',' ',' ','♜'},
                { ' ',' ',' ',' ',' ',' ','♝','♚'},
                { ' ','♟',' ',' ','♟',' ',' ',' '},
                { ' ',' ',' ',' ','♙','♟','♙','♟'},
                { ' ',' ',' ',' ','♗',' ',' ',' '},
                { '♜',' ','♟',' ',' ',' ',' ',' '},
                { '♙',' ',' ',' ',' ','♙','♙',' '},
                { ' ',' ','♔',' ',' ',' ',' ',' '},
            };
            //Solucion Kxc4
            char[,] tableroPuezzle2 = new char[,]
            {
                { '♜',' ','♝',' ',' ','♝',' ','♜'},
                { '♟','♟',' ',' ',' ','♕','♟',' '},
                { ' ',' ','♞','♚','♞',' ',' ','♟'},
                { ' ',' ',' ','♟','♟','♙',' ','♟'},
                { '♙',' ','♟',' ',' ',' ',' ',' '},
                { ' ','♘','♙',' ','♘','♗',' ',' '},
                { ' ','♙','♙',' ','♙','♙','♖',' '},
                { ' ','♔',' ','♖',' ',' ',' ','♛'},
            };
            //Socuion Qh5
            char[,] tableroPuezzle3 = new char[,]
            {
                { ' ',' ',' ','♖',' ','♗',' ',' '},
                { ' ',' ',' ',' ',' ',' ','♟',' '},
                { '♝','♟','♟','♞',' ',' ','♙','♟'},
                { '♟',' ','♚',' ',' ',' ',' ',' '},
                { '♛',' ',' ',' ','♟','♟',' ','♙'},
                { ' ',' ','♙',' ','♙',' ','♟',' '},
                { '♙','♙',' ','♙',' ',' ','♙','♜'},
                { ' ',' ',' ','♕','♔','♗',' ',' '},
            };
            //Solucion  kd4
            char[,] tableroPuezzle4 = new char[,]
            {
                { ' ',' ',' ',' ',' ',' ',' ','♔'},
                { ' ',' ',' ','♙',' ','♕',' ',' '},
                { '♖',' ',' ',' ','♘',' ',' ','♚'},
                { ' ','♙','♙',' ',' ','♙','♟','♙'},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { '♙',' ',' ',' ',' ',' ',' ',' '},
                { ' ','♝','♙',' ',' ','♘',' ','♕'},
                { ' ','♖','♗',' ',' ','♗',' ',' '},
            };
            //Solucion Alfilc5
            char[,] tableroPuezzle5 = new char[,]
            {
                { ' ',' ',' ',' ',' ','♗',' ',' '},
                { ' ','♛',' ','♕',' ','♘',' ','♜'},
                { ' ','♚',' ',' ',' ',' ',' ','♜'},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ','♔',' ','♞',' ',' '},
                { ' ',' ',' ','♘',' ',' ',' ',' '},
                { ' ',' ',' ','♙',' ','♕',' ',' '},
                { '♖','♝',' ',' ',' ',' ','♝',' '},
            };

            char[,] tableroReyAhogado = new char[,]
            {
                { '♚',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ','♕',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ','♔',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
            };

            char[,] tableroMateAnastasia = new char[,]
            {
                { ' ',' ',' ',' ',' ','♜','♚',' '},
                { ' ',' ',' ',' ',' ','♟','♟',' '},
                { ' ',' ','♘',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ','♔',' ',' ',' ','♖'},
            };

            char[,] tableroMateAlfilCaballo = new char[,]
            {
                { ' ',' ',' ',' ',' ',' ',' ','♚'},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ','♔','♘'},
                { ' ',' ',' ',' ',' ',' ','♗',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
            };

            char[,] tableroMateEsquina = new char[,]
            {
                { ' ',' ',' ',' ',' ',' ','♜','♚'},
                { ' ',' ',' ',' ',' ',' ','♟','♟'},
                { ' ',' ',' ','♘',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ','♔',' '},
            };

            char[,] tableroMateDosTorres= new char[,]
            {
                { ' ',' ',' ','♚',' ',' ',' ',' '},
                { '♖',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ','♔','♖'},
            };
            
            char[,] tableroMatePasillo = new char[,]
            {
                { ' ',' ',' ',' ',' ',' ','♚',' '},
                { ' ',' ',' ',' ',' ','♟','♟','♟'},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ','♖',' ',' ','♔',' ',' ',' '},
            };

            char[,] tableroMateBoden = new char[,]
            {
                { ' ',' ','♚','♜',' ',' ',' ',' '},
                { ' ',' ',' ','♟',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ','♗',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ','♗',' ','♔'},
            };

            char[,] tableroMateDavidGoliath = new char[,]
            {
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ','♖'},
                { '♟','♚','♟',' ',' ',' ',' ',' '},
                { ' ','♞',' ',' ',' ',' ',' ',' '},
                { '♙','♙',' ',' ',' ',' ',' ',' '},
                { ' ',' ',' ',' ',' ',' ',' ','♔'},
            };

            if (RadioDebug1.IsChecked == true)
            {
                SetParametrosPartida(DevolverTableroNuevo(), true, true, true, true, true, false, false, null, "", "");
                CambiarDebugText("");

            }
            else if (RadioDebug2.IsChecked == true)
            {
                SetParametrosPartida(tableroMateEnUno, true, false, false, false, false, false, false, null, "", "");
                CambiarDebugText("Puzzle en el que solo hay un movimiento que termine en jaque mate, ¿podras encontrarlo?  ");
            }
            else if (RadioDebug3.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                rastros.Add("a3");
                rastros.Add("a2");
                SetParametrosPartida(tableroPromocionMateEnUno, false, false, false, false, false, false, false, rastros, "", "");
                CambiarDebugText("julitoflo (1613) vs. Loefwing (1666) - New Years Challenge 2015 - 26 Jan 2015, partida " +
                    "en la cual se dio la ocasion de un mate por promoción.  ");
            }
            else if (RadioDebug4.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                rastros.Add("g1");
                rastros.Add("g2");
                SetParametrosPartida(tableroEnroqueParaMate, true, false, true, false, false, false, false, rastros, "", "");
                CambiarDebugText("Edward Lasker vs. Sir George Thomas - Londres - 1911, juego historico el cual tiene dos " +
                    "posibilidades para acabar, una de ellas el bellisimo jaque por enroque. ");

            }
            else if (RadioDebug5.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                rastros.Add("f7");
                rastros.Add("f5");
                SetParametrosPartida(tableroEnPassantMate, true, false, false, false, false, false, false, rastros, "", "");
                casillaEnPassant = "f6";
                CambiarDebugText("Jon Ludvig Hammer vs Magnus Carlsen (top 1 de el mundo) - Live Chess - 2023, juego bastante " +
                    "polemico de comienzos de el año 2023 donde el campeon de el mundo fue derrotado por un bellisimo mate por En Passant.  ");
            }
            else if (RadioDebug6.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                SetParametrosPartida(tableroPuezzle2, true, false, false, false, false, false, false, null, "", "");
                CambiarDebugText("Puzzle en el que solo hay un movimiento que termine en jaque mate, ¿podras encontrarlo?  ");
            }
            else if (RadioDebug7.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                SetParametrosPartida(tableroPuezzle3, true, false, false, false, false, false, false, null, "", "");
                CambiarDebugText("Puzzle en el que solo hay un movimiento que termine en jaque mate, ¿podras encontrarlo?  ");
            }
            else if (RadioDebug8.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                SetParametrosPartida(tableroPuezzle4, true, false, false, false, false, true, false, null, "", "");
                CambiarDebugText("Puzzle en el que solo hay un movimiento que termine en jaque mate, ¿podras encontrarlo?  ");
            }
            else if (RadioDebug9.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                SetParametrosPartida(tableroPuezzle5, true, false, false, false, false, false, false, null, "", "");
                CambiarDebugText("Puzzle en el que solo hay un movimiento que termine en jaque mate, ¿podras encontrarlo?  ");
            }
            else if (RadioDebug14.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                SetParametrosPartida(tableroReyAhogado, true, false, false, false, false, false, false, null, "", "");
                CambiarDebugText("Mueve la reina a la casilla c7 para probar el empate por rey ahogado");
                PintarEnroque("c7");
            }
            else if (RadioDebug10.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                SetParametrosPartida(tableroMateAnastasia, true, false, false, false, false, false, false, null, "", "");
                CambiarDebugText("El nombre de este mate es más bien enigmático. Proviene de la novela " +
                    "\"Anastasia y el juego de ajedrez, carta desde Italia\" obra del poeta alemán Wilhelm " +
                    "Heinse, publicada en 1803.");
                PintarEnroque("e7");
            }
            else if (RadioDebug11.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                SetParametrosPartida(tableroMateAlfilCaballo, true, false, false, false, false, false, false, null, "", "");
                CambiarDebugText("El mate de rey, alfil y caballo contra rey es el más complejo de los mates " +
                    "básicos y de los finales sin peones en ajedrez. Con el turno y un juego perfecto, el " +
                    "bando superior en material puede forzar el mate en unas treinta y cinco jugadas.");
                PintarEnroque("f6");
            }
            else if (RadioDebug12.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                SetParametrosPartida(tableroMateEsquina, true, false, false, false, false, false, false, null, "", "");
                CambiarDebugText("En ajedrez, el mate de la coz es un jaque mate en el que el caballo ataca al rey " +
                    "contrario, que no puede escapar de la amenaza al encontrarse rodeado por sus propias piezas.");
                PintarEnroque("f7");
            }
            else if (RadioDebug13.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                SetParametrosPartida(tableroMateDosTorres, true, false, false, false, false, false, false, null, "", "");
                CambiarDebugText("Este es el primer mate que debe practicar el principiante, ya que es el más sencillo " +
                    "y fácil de comprender. Se debe llevar al rey enemigo a la banda con dos torres usando el método conocido como \"la escalera\".");
                PintarEnroque("h8");
            }
            else if (RadioDebug15.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                SetParametrosPartida(tableroMatePasillo, true, false, false, false, false, false, false, null, "", "");
                CambiarDebugText("En ajedrez, el mate del pasillo, Callejón de la Muerte, del tubo o de casita es un " +
                    "jaque mate que se produce cuando una torre o dama amenaza al rey enemigo en la octava fila.");
                PintarEnroque("b8");
            }
            else if (RadioDebug16.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                SetParametrosPartida(tableroMateBoden, true, false, false, false, false, false, false, null, "", "");
                CambiarDebugText("En el mate de Boden, dos alfiles atacan por diagonales cruzadas al rey contrario, " +
                    "obstruido por piezas de su bando; usualmente una torre y un peón.");
                PintarEnroque("a6");
            }
            else if (RadioDebug16.IsChecked == true)
            {
                List<string> rastros = new List<string>();
                SetParametrosPartida(tableroMateDavidGoliath, true, false, false, false, false, false, false, null, "", "");
                CambiarDebugText("El Mate de David y Goliath puede tomar muchas formas, aunque generalmente se " +
                    "caracteriza por ser ejecutado por un peón y en el cual los peones enemigos están cerca.");
                PintarEnroque("a3");
            }

        }


        //BOTONES DE EL TABLERO//

        private void A1(object sender, RoutedEventArgs e)
        {
            SeleccionCasilla("a1");
        }

        private void B1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b1");

        }

        private void C1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c1");

        }

        private void D1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d1");

        }

        private void E1(object sender, RoutedEventArgs e)
        {
            SeleccionCasilla("e1");
        }

        private void F1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f1");

        }

        private void G1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g1");

        }

        private void H1(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h1");

        }

        private void A2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a2");

        }

        private void B2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b2");

        }

        private void C2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c2");

        }

        private void D2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d2");

        }

        private void E2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e2");

        }

        private void F2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f2");

        }

        private void G2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g2");

        }

        private void H2(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h2");

        }

        private void A3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a3");

        }

        private void B3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b3");

        }

        private void C3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c3");

        }

        private void D3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d3");

        }

        private void E3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e3");

        }

        private void F3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f3");

        }

        private void G3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g3");

        }

        private void H3(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h3");

        }

        private void A4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a4");

        }

        private void B4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b4");

        }

        private void C4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c4");

        }

        private void D4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d4");

        }

        private void E4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e4");

        }

        private void F4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f4");

        }

        private void G4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g4");

        }

        private void H4(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h4");

        }

        private void A5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a5");

        }

        private void B5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b5");

        }

        private void C5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c5");

        }

        private void D5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d5");

        }

        private void E5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e5");

        }

        private void F5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f5");

        }

        private void G5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g5");

        }

        private void H5(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h5");

        }

        private void A6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a6");

        }

        private void B6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b6");

        }

        private void C6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c6");

        }

        private void D6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d6");

        }

        private void E6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e6");

        }

        private void F6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f6");

        }

        private void G6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g6");

        }

        private void H6(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h6");

        }

        private void A7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a7");

        }

        private void B7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b7");

        }

        private void C7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c7");

        }

        private void D7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d7");

        }

        private void E7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e7");

        }

        private void F7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f7");

        }

        private void G7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g7");

        }

        private void H7(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h7");

        }

        private void A8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("a8");

        }

        private void B8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("b8");

        }

        private void C8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("c8");

        }

        private void D8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("d8");

        }

        private void E8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("e8");

        }

        private void F8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("f8");

        }

        private void G8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("g8");

        }

        private void H8(object sender, RoutedEventArgs e)
        {

            SeleccionCasilla("h8");

        }
    }
}
