using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizAmoroso.DataModel
{
    public class InvioDatiStatistica
    {
        public string username { get; set; }
        public string materia { get; set; }
        public int domande { get; set; }
       public string  risposteY { get; set; }
       public string  risposteN { get; set; }
        public string data { get; set; }
        public string ora { get; set; }
        public string tempoQuiz { get; set; }
        
    }
}

