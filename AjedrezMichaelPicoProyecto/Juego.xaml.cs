using System;
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
        private const string EspacioVacio = "•";

        //Variables para mover piezas
        public string CasillaSeleccionadaAnterior = "";
        public string CasillaSeleccionada = "";

        //Booleanos usados para el funcionamiento de el juego
        bool EstaElCaminoDibujado = false;

        //Booleanos que definen la partida
        bool PartidaAcabada = false;
        bool EsTurnoDeBlancas = true;
        bool SePuedeEnroqueBlancoDerecha = true;
        bool SePuedeEnroqueBlancoIzquierda = true;
        bool SePuedeEnroqueNegroDerecha = true;
        bool SePuedeEnroqueNegroIzquierda = true;
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
                { '•','•','•','•','•','•','•','•'},
                { '•','•','•','•','•','•','•','•'},
                { '•','•','•','•','•','•','•','•'},
                { '•','•','•','•','•','•','•','•'},
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

            ReproducirSonidoInicioPartida();
            CargarSonidoMoverPieza();

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
            if (!EsUnaCasillaDeRastro(casilla))
            { 
                EstaElCaminoDibujado = true;
                SetFondo(casilla, 2);
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

        //TODO: ONPASANT
        /// <summary>
        /// Metodo que dibuja el camino de la pieza peon
        /// </summary>
        public void DibujarCaminoPeon()
        {

            int[] coordenadasDibujar = TraducirCasillaCoordenadas(CasillaSeleccionada);
            int fila = 8 - coordenadasDibujar[1];
            string casillaPasoDoble;

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
                    string casillaObjetivo = TraducirCoordenadaToCasilla(coordenadasDibujar[1], i); //Guardo la casilla en un string

                    //Si la casilla esta vacia y es un posible enpassant pinto enPassant
                    if(EstaLaCasillaVacia(casillaObjetivo) && casillaObjetivo.Equals(casillaEnPassant))
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

        //TODO jaque, jaquemate y enroque
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
                        if (EsPiezaBlanca(CasillaSeleccionada)){
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
            labelNotacion.Text = labelNotacion.Text + nuevaNotacion;
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
            } else
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
                ActualizarCaracterCasilla(casillaEnPassant ,GetContenidoCasilla(TraducirCoordenadaToCasilla(coordenadas))); 
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
            ActualizarLabelPuntuacion(); //Actualiza si es posible los label de puntuacion
            CalcularNotacion(); //Calcula la notacion y la actualiza
            RealizarMovimiento(); //Mueve la pieza
            IntentarPromocion(); //Verifica si la pieza es un peon que tiene que promocionar
            CambiarTurno();//Cambio el turno
            RestaurarTablero(); //Metodo que elimina todos los caminos y rastros
            DibujarRastro(); //Metod que dibuja el nuevo rastro
            ReproducirMoverPiezaSonido(); //Metodo que reproduce el sonido de mover pieza
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
                } else
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
            }

            ActualizarNotacion(notacion);
        }

        /// <summary>
        /// Metodo quecutaliza las casillas para mover la pieza
        /// </summary>
        public void RealizarMovimiento()
        {
            ActualizarBooleanosEnroque();
            CalcularCasillaEnPassant();
            ActualizarCaracterCasilla(CasillaSeleccionada, GetContenidoCasilla(CasillaSeleccionadaAnterior));
            ActualizarCaracterCasilla(CasillaSeleccionadaAnterior, EspacioVacio);
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
            else if(GetContenidoCasilla(CasillaSeleccionadaAnterior).Equals("♟") && GetFila(CasillaSeleccionadaAnterior) == 1 && (GetFila(CasillaSeleccionadaAnterior) + 2) == GetFila(CasillaSeleccionada))
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
            ventanaInicio.SonidoBoton_MouseEnter(sender, e);

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
        /// Boton el cual cierra el programa y todas sus ventanas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonrSalir_Click(object sender, RoutedEventArgs e)
        {
            ventanaInicio.BotonSalir_Click(sender, e);
        }


        /// <summary>
        /// Boton usado en el debug a la hora de testear funciones de el programa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonBorrarCamino(object sender, RoutedEventArgs e)
        {
            BorrarCamino();
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
            RestaurarTablero();

            //Puzzle
            char[,] tableroMateEnUno = new char[,]
            {
                { '•','♗','♝','•','•','•','♗','♘'},
                { '♖','•','•','♙','♚','•','•','♜'},
                { '•','♕','•','•','•','•','•','♗'},
                { '•','•','•','•','♛','•','•','•'},
                { '•','•','♝','♘','•','•','•','•'},
                { '•','•','•','•','♕','•','♗','♔'},
                { '•','♟','•','•','•','•','•','•'},
                { '•','♝','♛','•','♖','•','♜','♝'},
            };

            //julitoflo (1613) vs. Loefwing (1666) - New Years Challenge 2015 - 26 Jan 2015
            char[,] tableroPromocionMateEnUno = new char[,]
            {
                { '•','•','•','•','•','•','•','•'},
                { '•','•','•','•','•','•','•','•'},
                { '•','•','•','•','•','•','•','•'},
                { '♟','•','•','•','•','•','•','•'},
                { '♙','•','•','•','•','•','•','♟'},
                { '♔','•','♞','•','•','•','•','♙'},
                { '•','♟','♚','•','•','•','•','•'},
                { '•','•','•','•','•','•','•','•'},
            };

            //Edward Lasker vs. Sir George Thomas - Londres - 1911
            char[,] tableroEnroqueParaMate = new char[,]
            {
                { '♜','♞','•','•','•','♜','•','•'},
                { '♟','♝','♟','♟','♛','•','♟','•'},
                { '•','♟','•','•','♟','♘','•','•'},
                { '•','•','•','•','•','•','•','•'},
                { '•','•','•','♙','•','•','♘','♙'},
                { '•','•','•','•','•','•','♙','•'},
                { '♙','♙','♙','•','♗','♙','•','♖'},
                { '♖','•','•','•','♔','•','♚','•'},
            };

            //Jon Ludvig Hammer vs Magnus Carlsen (top 1 de el mundo) - Live Chess - 2023
            //Blancas = ♔♕♖♗♘♙
            //Negras  = ♚♛♜♝♞♟
            char[,] tableroEnPassantMate = new char[,]
            {
                { '•','•','•','♖','•','•','•','♜'},
                { '•','•','•','•','•','•','♝','♚'},
                { '•','♟','•','•','♟','•','•','•'},
                { '•','•','•','•','♙','♟','♙','♟'},
                { '•','•','•','•','♗','•','•','•'},
                { '♜','•','♟','•','•','•','•','•'},
                { '♙','•','•','•','•','♙','♙','•'},
                { '•','•','♔','•','•','•','•','•'},
            };

            if (RadioDebug1.IsChecked == true)
            {
                RellenarTablero(DevolverTableroNuevo());
                SetParametrosPartida(true, true, true, true, true);
                
            }
            else if (RadioDebug2.IsChecked == true)
            {
                RellenarTablero(tableroMateEnUno);
                SetParametrosPartida(true, false, false, false, false);
            }
            else if (RadioDebug3.IsChecked == true)
            {
                PintarRastro("a3");
                PintarRastro("a2");
                SetParametrosPartida(false, false, false, false, false);
                RellenarTablero(tableroPromocionMateEnUno);
            }
            else if (RadioDebug4.IsChecked == true)
            {
                SePuedeEnroqueBlancoIzquierda = true;
                PintarRastro("g1");
                PintarRastro("g2");
                SetParametrosPartida(true, false, true, false, false);
                ActualizarLabelTurno();
                RellenarTablero(tableroEnroqueParaMate);
            }
            else if (RadioDebug5.IsChecked == true)
            {
                PintarRastro("f7");
                PintarRastro("f5");
                casillaEnPassant = "f6";
                SetParametrosPartida(true, false, false, false, false);
                RellenarTablero(tableroEnPassantMate);
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
