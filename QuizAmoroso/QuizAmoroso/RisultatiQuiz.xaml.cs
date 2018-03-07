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
		public RisultatiQuiz (RisultatoQuiz risultati)
		{
			InitializeComponent ();
		    LabelEsatte.Text = risultati.contEsatteTot;
            LabelEsatteSopra.Text= risultati.contEsatteTot;
		    LabelSbagliate.Text = risultati.contSbagliateTot;
		    LabelSbagliateSopra.Text = risultati.contSbagliateTot;
		    LabelNonRisposte.Text = risultati.contNonRisposteTot;
		    LabelTempo.Text = risultati.TmpTotale;
		}
	}
}