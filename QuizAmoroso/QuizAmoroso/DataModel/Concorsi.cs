using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizAmoroso.DataModel
{
   public class Concorsi
    {
        public string codice_concorso { get; set; }
        public string anno { get; set; }
        public string descrizione { get; set; }
        public string corpo { get; set; }
        public string id_concorso { get; set; }
        public string attivo { get; set; }
        public string numero_domande { get; set; }
        public string correzione_esatta { get; set; }
        public string correzione_sbagliata { get; set; }
    }
}
