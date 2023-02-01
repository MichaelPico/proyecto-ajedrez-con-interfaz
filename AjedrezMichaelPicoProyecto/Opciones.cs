using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjedrezMichaelPicoProyecto
{
    partial class Opciones
    {
        public bool ModoDosJugadores { get; set; }
        int dificultad { get; set; }
        int color { get; set; }
        int pantalla { get; set; }
        int paleta { get; set; }
        int Ficha { get; set; }

        public Opciones()
        {
            ModoDosJugadores = true;
            dificultad = 0;
            color = 0; //0 = Aleatorio, 1 = Blancas, 2 = Negras
            pantalla = 0; //0 = ModoVentana, 1 = ModoSinBordes, 2 = PantallaCompleta
            paleta = 0; //0 = paleta1, 1 = paleta2...
            Ficha = 0; //0 = Fichas1, 1 = Fichas2...
        }

        
    }
}
