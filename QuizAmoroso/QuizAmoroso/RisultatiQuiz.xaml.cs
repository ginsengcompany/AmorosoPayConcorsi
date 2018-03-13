using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizAmoroso.DataModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuizAmoroso
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RisultatiQuiz : ContentPage
	{
        private RisultatoQuiz risultato = new RisultatoQuiz();
        private List<Domande>lista = new List<Domande>();
		public RisultatiQuiz (RisultatoQuiz risultati,List<Domande> listaDomande)
		{
			InitializeComponent ();
		    this.lista = listaDomande;
		    this.risultato = risultati;
		    LabelEsatte.Text = risultati.contEsatteTot;
            LabelEsatteSopra.Text= risultati.contEsatteTot;
		    LabelSbagliate.Text = risultati.contSbagliateTot;
		    LabelSbagliateSopra.Text = risultati.contSbagliateTot;
		    LabelNonRisposte.Text = risultati.contNonRisposteTot;
		    LabelNumeroDomande.Text = risultati.numeroDomande;
		    LabelTempo.Text = risultati.TmpTotale;
		}

	    private void RivediQuizMethod(object sender, EventArgs e)
	    {
	        Navigation.PushAsync(new QuizCompletato(risultato,lista));
	    }
	}
}