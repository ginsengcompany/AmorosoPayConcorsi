using Xamarin.Forms;

namespace QuizAmoroso.DataModel
{
    public class DatiRisultati
    {
        public string Domanda { get; set; }
        public string rispostaEsattaYN { get; set; }
        public string letteraRisposta { get; set; }
        public string risposta { get; set; }
        public string tuaRisposta { get; set; }
        public string letteraSelezionata { get; set; }
        public Color color { get; set; }

    }
}