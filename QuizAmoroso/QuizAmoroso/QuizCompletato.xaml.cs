using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QuizAmoroso.DataModel;
using QuizAmoroso.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuizAmoroso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuizCompletato : ContentPage
    {
      private RisultatoQuiz risultato = new RisultatoQuiz();
        private List<Domande> lista = new List<Domande>();

        public List<DatiRisultati> Lista { get; set; }

        public QuizCompletato(RisultatoQuiz risultato, List<Domande> lista)
        {
            InitializeComponent();
            this.risultato = risultato;
            this.lista = lista;
            Lista = risultato.risultati;
            ListaRisultati.ItemsSource = Lista;
            foreach (var i in Lista)
            {
                if (i.rispostaEsattaYN == "true")
                {
                    i.rispostaEsattaYN = "corretto";
                }
                else if(i.rispostaEsattaYN=="false")
                {
                    i.rispostaEsattaYN = "sbagliato";
                }
                if (i.risposta == null)
                {
                    i.risposta = "non risposto";
                }
                if (i.tuaRisposta==null)
                {
                    i.tuaRisposta = "non risposto";
                }
            }
            

        }
     

    }


}