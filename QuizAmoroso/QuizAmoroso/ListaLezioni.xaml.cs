using Newtonsoft.Json;
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
    public partial class ListaLezioni : ContentPage
    {
        private List<Lezioni> ListLezioni = new List<Lezioni>();
        private string risultatojson;
        private Concorsi concorso = new Concorsi();
        private bool flagConnessione = false;

        public ListaLezioni(Concorsi concorso)
        {
            this.concorso = concorso;
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
                await DisplayAlert("Errore", "Problema sulla connessione", "Ok");
                DisabilitaLayoutActivityIndicator.IsVisible = false;
                caricamentoPagina.IsVisible = false;
                caricamentoPagina.IsRunning = false;
            }
        }

        public async Task<List<Lezioni>> ConnessioneConcorsi()
        {

                string username = Utente.Instance.getUserName;
                var client = new HttpClient();
                try
                {
                    var values = new List<KeyValuePair<string, string>>();
                    values.Add(new KeyValuePair<string, string>("username", username));
                    values.Add(new KeyValuePair<string, string>("id_concorso", concorso.id_concorso));
                    var content = new FormUrlEncodedContent(values);
                    var result = await client.PostAsync(Costanti.ListaLezioni, content);
                risultatojson = await result.Content.ReadAsStringAsync();
                if (risultatojson == "Impossibile connettersi al servizio")
                {
                    flagConnessione = true;
                    throw new Exception();
                    return new List<Lezioni>();
                }
                else
                {
                    flagConnessione = false;
                    ListLezioni = JsonConvert.DeserializeObject<List<Lezioni>>(risultatojson);
                    return ListLezioni;
                }
            }
            catch (Exception e)
            {
                return new List<Lezioni>();
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
            List<Lezioni> ListaConcorsi = await ConnessioneConcorsi();

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
                    await Navigation.PushAsync(new Lezione(i,ListaConcorsi));
                };

                codice_concorso.Text = i.Nome;
                codice_concorso.FontAttributes = FontAttributes.Bold;
                descrizione.Text ="Piano di Studi: " + i.nomeSet;
                anno.Text ="lezione numero: " +  i.Lezione;

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

        private async Task invioDatiPagamento(string productId, string purchaseId, string token, string state, string codiceControllo, string data)
        {
            string username = Utente.Instance.getUserName;
            var client = new HttpClient();
            try
            {
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("username", username));
                values.Add(new KeyValuePair<string, string>("productId", productId));
                values.Add(new KeyValuePair<string, string>("purchaseId", purchaseId));
                values.Add(new KeyValuePair<string, string>("token", token));
                values.Add(new KeyValuePair<string, string>("state", state));
                values.Add(new KeyValuePair<string, string>("codiceControllo", codiceControllo));
                values.Add(new KeyValuePair<string, string>("data", data));
                var content = new FormUrlEncodedContent(values);
                var result = await client.PostAsync(Costanti.salvaPagamento, content);
                risultatojson = await result.Content.ReadAsStringAsync();
                ListLezioni = JsonConvert.DeserializeObject<List<Lezioni>>(risultatojson);
                if (risultatojson == "Impossibile connettersi al servizio")
                {
                    await invioDatiPagamento(productId, purchaseId, token, state, codiceControllo, data);
                }
            }
            catch (Exception e)
            {
                await invioDatiPagamento(productId, purchaseId, token, state, codiceControllo, data);
            }
        }

    }
}