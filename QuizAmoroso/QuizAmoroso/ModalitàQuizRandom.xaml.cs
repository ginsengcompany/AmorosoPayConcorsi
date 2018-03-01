using QuizAmoroso.Classi;
using QuizAmoroso.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuizAmoroso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModalitàQuizRandom : ContentPage
    {
        Materie materia;
        DatiConnessioneDomande datiConnessione = new DatiConnessioneDomande();
        public ModalitàQuizRandom(Materie i, String idConcorso)
        {
            materia = i;
            datiConnessione.idConcorso = idConcorso;
            InitializeComponent();
            btnAvvio.BackgroundColor = Colori.Button;
            SliderSelezioneNumeroDomande.Maximum =Convert.ToInt32(i.domandemateriamax);
            if (Convert.ToInt32(i.domandemateriamax) > 10)
            {

                SliderSelezioneNumeroDomande.Minimum = 10;
                if (Convert.ToInt32(i.domandemateriamax) > 30)
                    SliderSelezioneNumeroDomande.Value = 30;
                else
                    SliderSelezioneNumeroDomande.Value = 10;
            }
            else
            {
                SliderSelezioneNumeroDomande.Minimum = 1;
                SliderSelezioneNumeroDomande.Value = 1;
            }

            LabelValoriSelezionatiConSlider.Text = "Max: " + SliderSelezioneNumeroDomande.Value.ToString();
            LabelMinimoSlider.Text = "Min: " + SliderSelezioneNumeroDomande.Minimum.ToString();
            LabelMassimoSlider.Text = i.domandemateriamax;
        }

        private void SliderSelezioneNumeroDomande_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int ke = Convert.ToInt32(e.NewValue);
            LabelValoriSelezionatiConSlider.Text = ke.ToString();

        }

        private async Task Button_Clicked(object sender, EventArgs e)
        {/*
                caricamentoPagina.IsRunning = true;
                caricamentoPagina.IsVisible = true;
          */
            datiConnessione.materia = materia.materia;
            datiConnessione.numeroDomande = Convert.ToInt32(SliderSelezioneNumeroDomande.Value).ToString();
            datiConnessione.modalitaSelezionata = "Libera";

               await Navigation.PushAsync(new Quiz(datiConnessione));
                      
                   
             /*   caricamentoPagina.IsRunning = false;
                caricamentoPagina.IsVisible = false;
                */
        }
    }
}