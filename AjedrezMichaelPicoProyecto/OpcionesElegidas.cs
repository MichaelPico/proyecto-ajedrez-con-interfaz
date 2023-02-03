using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjedrezMichaelPicoProyecto
{
    public partial class OpcionesElegidas
    {
        public bool ModoDosJugadores { get; set; }
        public int Dificultad { get; set; }
        public int Color { get; set; }
        public int Pantalla { get; set; }

        public OpcionesElegidas()
        {
            ModoDosJugadores = true;
            Dificultad = 0;
            Color = 0; //0 = Aleatorio, 1 = Blancas, 2 = Negras
            Pantalla = 0; //0 = ModoVentana, 1 = ModoSinBordes, 2 = PantallaCompleta
        }

        
    }
}
