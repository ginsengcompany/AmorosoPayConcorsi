using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizAmoroso.Classi
{
    public class Materie
    {
        // denominazione della materia (es. GEOGRAFIA, materia json)
        public string materia { get; set; }
        // numero delle domande totali contenute nella materia (domandemateriamax json)
        public string domandemateriamax { get; set; }
    }
}
