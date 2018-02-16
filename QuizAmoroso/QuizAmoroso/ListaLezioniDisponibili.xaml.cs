using Newtonsoft.Json;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using QuizAmoroso.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuizAmoroso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaLezioniDisponibili : ContentPage
    {

        private List<Concorsi> ListaConcorsi = new List<Concorsi>();
        private string risultatojson;
        private bool flagConnessione = false;
        public ListaLezioniDisponibili()
        {
            InitializeComponent();
        }
        /*
         * Questo metodo invia l'username dell'utente e vengono restituiti in formato json i piani formativi a lui associati
         * */
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                DisabilitaLayoutActivityIndicator.IsVisible = true;
                caricamentoPagina.IsRunning = true;
                caricamentoPagina.IsVisible = true;
                await creaGriglia();

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

        public async Task<List<Concorsi>> ConnessioneConcorsi()
        {
            string username = Utente.Instance.getUserName;
            var client = new HttpClient();
            try
            {
                var result = await client.GetAsync(Costanti.ConcorsiLezioni);
                risultatojson = await result.Content.ReadAsStringAsync();
                if (risultatojson == "Impossibile connettersi al servizio")
                {
                    flagConnessione = true;
                    throw new Exception();
                    return new List<Concorsi>();
                }
                else
                {
                    flagConnessione = false;
                    ListaConcorsi = JsonConvert.DeserializeObject<List<Concorsi>>(risultatojson);
                    return ListaConcorsi;
                }
            }
            catch (Exception e)
            {
                return new List<Concorsi>();
            }
        }

        /*
         * Questo metodo invia il piano formativo selezionato dall'utente  
         * */
        public async Task creaGriglia()
        {
            Grid grigliaConcorsitemp = new Grid();

            grigliaConcorsitemp.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            grigliaConcorsitemp.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            List<Concorsi> ListaConcorsi = await ConnessioneConcorsi();

            int righe = 0, colonne = 0;
            foreach (var i in ListaConcorsi)
            {
                grigliaConcorsitemp.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });


                StackLayout prova = new StackLayout
                {
                    Orientation = StackOrientation.Vertical
                };
                StackLayout provabtn = new StackLayout
                {
                    Orientation = StackOrientation.Vertical
                };
                Label codice_concorso = new Label();
                Label descrizione = new Label();
                Label anno = new Label();

                Button acquista = new Button
                {
                    Text = "Accedi"
                };
                acquista.Clicked += async delegate (object sender, EventArgs e)
                {
                    await Navigation.PushAsync(new ListaLezioni(i));
                };

                codice_concorso.Text = i.codice_concorso + " " + i.corpo;
                codice_concorso.FontAttributes = FontAttributes.Bold;
                descrizione.Text = i.descrizione;
                anno.Text = "ANNO:" + i.anno;

                prova.Children.Add(codice_concorso);
                prova.Children.Add(descrizione);
                prova.Children.Add(anno);


                grigliaConcorsitemp.Children.Add(prova, colonne, righe);
                provabtn.Children.Add(acquista);
                colonne++;
                grigliaConcorsitemp.Children.Add(provabtn, colonne, righe);
                colonne = 0;
                righe++;

            }
            steckGrigliaConcorsi.Children.Clear();
            steckGrigliaConcorsi.Children.Add(grigliaConcorsitemp);
        }

        private async Task connessioneLezioniAsync(string id_concorso)
        {

            string username = Utente.Instance.getUserName;
            var client = new HttpClient();
            try
            {
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("username", username));
                values.Add(new KeyValuePair<string, string>("id_concorso", id_concorso));
                var content = new FormUrlEncodedContent(values);
                var result = await client.PostAsync(Costanti.ListaLezioni, content);
                risultatojson = await result.Content.ReadAsStringAsync();
                
            }
            catch (Exception e)
            {
                 
            }
        }
    }
}