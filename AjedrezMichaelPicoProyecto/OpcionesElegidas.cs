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
        public int dificultad { get; set; }
        public int color { get; set; }
        public int pantalla { get; set; }

        public OpcionesElegidas()
        {
            ModoDosJugadores = true;
            dificultad = 0;
            color = 0; //0 = Aleatorio, 1 = Blancas, 2 = Negras
            pantalla = 0; //0 = ModoVentana, 1 = ModoSinBordes, 2 = PantallaCompleta
        }

        
    }
}
