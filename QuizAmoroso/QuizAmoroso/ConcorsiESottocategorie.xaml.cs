using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QuizAmoroso.DataModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuizAmoroso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConcorsiESottocategorie : ContentPage
    {
        public List<QuizVeloce.StrutturaPickerConcorso> listaConcorsi = new List<QuizVeloce.StrutturaPickerConcorso>();
        
        public ConcorsiESottocategorie()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                DisabilitaLayoutActivityIndicator.IsVisible = true;
                caricamentoPagina.IsRunning = true;
                caricamentoPagina.IsVisible = true;
                await IngressoPagina();

                caricamentoPagina.IsVisible = false;
                caricamentoPagina.IsRunning = false;
                DisabilitaLayoutActivityIndicator.IsVisible = false;
            }
            catch (Exception errore)
            {
                //await DisplayAlert("Errore", "Problema sulla connessione", "Ok");
                //DisabilitaLayoutActivityIndicator.IsVisible = false;
                //caricamentoPagina.IsVisible = false;
                //caricamentoPagina.IsRunning = false;
            }
           
        }

        public async Task IngressoPagina()
        {
           await CreazioneGriglia();
        }

        public async Task CreazioneGriglia()
        {
            await AttesaRicezioneConcorsi();

            Grid grigliaConcorsi = new Grid();
            
            grigliaConcorsi.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
            grigliaConcorsi.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            int righe = 0, colonne = 0;
            foreach (var i in listaConcorsi)
            {
                Grid grigliaConcorsiInDettaglio = new Grid();
                grigliaConcorsiInDettaglio.RowDefinitions.Add(new RowDefinition {Height = new GridLength(2, GridUnitType.Auto)});
                grigliaConcorsiInDettaglio.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.3, GridUnitType.Auto) });
                grigliaConcorsiInDettaglio.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                grigliaConcorsiInDettaglio.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                //connessione materie
                var listaMaterieDelConcorsoSelezionato = await AttesaRicezioneMaterie(i.id_concorso);
                Grid grigliaMaterie = await CreazioneGrigliaMaterie(listaMaterieDelConcorsoSelezionato);
                grigliaMaterie.IsVisible = false;

                var Titolo = new Label
                {
                    Text = i.Corpo,
                    FontSize = 30,
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Center

                };
                var Descrizione = new Label
                {
                    Text = i.codice_concorso,
                    TextColor = Color.Black
                };


                var tapGestureRecognizerCella = new TapGestureRecognizer();
                tapGestureRecognizerCella.Tapped += async (s, e) =>
                {
                    if (grigliaMaterie.IsVisible == false)
                    {
                        grigliaMaterie.IsVisible = true;
                        
                    }
                       
                    else
                        grigliaMaterie.IsVisible = false;


                };
                var stackTitolo= new StackLayout
                {
                    BackgroundColor = Colori.ColoriSecondario,
                };
                stackTitolo.GestureRecognizers.Add(tapGestureRecognizerCella);
                var stackSottotitolo = new StackLayout
                {
                    BackgroundColor = Color.White,

                };
                stackTitolo.Children.Add(Titolo);
                stackSottotitolo.Children.Add(Descrizione);
                grigliaConcorsiInDettaglio.Children.Add(stackTitolo,0,0);
                grigliaConcorsiInDettaglio.Children.Add(stackSottotitolo, 0, 1);
                grigliaConcorsiInDettaglio.Children.Add(grigliaMaterie,0,2);
                grigliaConcorsi.Children.Add(grigliaConcorsiInDettaglio,colonne,righe);
                righe++;
            }

            StackPrincipale.Children.Clear();
            StackPrincipale.Children.Add(grigliaConcorsi);
        }


        public async Task<Grid> CreazioneGrigliaMaterie(List<QuizVeloce.StrutturaPickerMaterie> listaMaterie)
        {
            Grid GrigliaMaterie = new Grid();
            GrigliaMaterie.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            int riga=0, colonna = 0;
            foreach (var i in listaMaterie)
            {
                GrigliaMaterie.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                var stackTitoloMateria = new StackLayout
                {
                    BackgroundColor = Colori.coloreTerziario
                };

                var TitoloMateria = new Label
                {
                    Text = i.materia,
                    FontSize = 20,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
           
                stackTitoloMateria.Children.Add(TitoloMateria);
                GrigliaMaterie.Children.Add(stackTitoloMateria,0,riga);
                riga++;
            }
            return GrigliaMaterie;
        }











        public async Task AttesaRicezioneConcorsi()
        {
            try
            {
                string username = Utente.Instance.getUserName;
                var client = new HttpClient();
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("username", username));
                var content = new FormUrlEncodedContent(values);
                try
                {
                    var result = await client.PostAsync(Costanti.concorsi, content);
                   var risultatoChiamataQuizVeloce = await result.Content.ReadAsStringAsync();
                    listaConcorsi = JsonConvert.DeserializeObject<List<QuizVeloce.StrutturaPickerConcorso>>(risultatoChiamataQuizVeloce);

                   
                }
                catch (Exception errore)
                {
                    //await DisplayAlert("Attenzione", "Non sono presenti concorsi. E' comunque possibile esercitarsi sull'intera banca dati.", "Ok");
                }
            }
            catch (Exception errore)
            {
                await DisplayAlert("Attenzione", "Connessione persa durante il caricamento dei concorsi.", "Ok");
            }
        }
        public async Task<List<QuizVeloce.StrutturaPickerMaterie>> AttesaRicezioneMaterie(string idConcorsoSelezionato)
        {

            List<QuizVeloce.StrutturaPickerMaterie> listaMaterie = new List<QuizVeloce.StrutturaPickerMaterie>();
            try
            { 
                var client = new HttpClient();
                var values = new List<KeyValuePair<string, string>>();
               values.Add(new KeyValuePair<string, string>("id_concorso",idConcorsoSelezionato));
                var content = new FormUrlEncodedContent(values);
                var result = await client.PostAsync(Costanti.materieconcorso, content);
                var risualtatoChiamataMaterie = await result.Content.ReadAsStringAsync();
                 listaMaterie = JsonConvert.DeserializeObject<List<QuizVeloce.StrutturaPickerMaterie>>(risualtatoChiamataMaterie);
            
            }
            catch (Exception e)
            {
                await DisplayAlert("Attenzione", "Connessione persa durante il caricamento delle materie.", "riprova");
            }


            return listaMaterie;
        }
    }
}