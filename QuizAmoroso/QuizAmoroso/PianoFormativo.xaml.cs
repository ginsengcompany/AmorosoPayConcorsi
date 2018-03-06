using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using QuizAmoroso.DataModel;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using System.Net.Http.Headers;

/*
 * @Author: Alessio Calabrese
 * 
 * Questa classe effettua la connessione al database inviando l'username dell'utente e
 * restituendo i piani formativi associati a quest'ultimo. Successivamente è possibile 
 * cliccare su di essi tramite listview. Facendo questo il piano formativo verrà inviato
 * al server e si aprirà la pagina setDomande
 * */



namespace QuizAmoroso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PianoFormativo : TabbedPage
    {

        private List<ConcorsiAquistabili> ListaConcorsi = new List<ConcorsiAquistabili>();
        private string risultatojson;
        private bool flagConnessione = false;
        private List<ConcorsiAquistabili> ListaVideoLezioni = new List<ConcorsiAquistabili>();


        public PianoFormativo()
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
                await creaGriglia(Costanti.concorsiDisponibili);
                await creaGriglia(Costanti.MaterieDisponibili);
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

        public async Task<List<ConcorsiAquistabili>> ConnessioneConcorsi(string URL)
        {
            string username = Utente.Instance.getUserName;
            var client = new HttpClient();
            try
            {
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("username", username));
                var content = new FormUrlEncodedContent(values);
                var result = await client.PostAsync(URL, content);
                risultatojson = await result.Content.ReadAsStringAsync();
                
                if (risultatojson == "Impossibile connettersi al servizio")
                {
                    flagConnessione = true;
                    throw new Exception();
                    return new List<ConcorsiAquistabili>();
                }
                else
                {
                    flagConnessione = false;
                    ListaConcorsi = JsonConvert.DeserializeObject<List<ConcorsiAquistabili>>(risultatojson);
                    return ListaConcorsi;
                }
            }
            catch (Exception e)
            {
                return new List<ConcorsiAquistabili>();
            }
        }

        /*
         * Questo metodo invia il piano formativo selezionato dall'utente  
         * */
        public async Task creaGriglia(string URL)
        {
            Grid grigliaConcorsi = new Grid();

            grigliaConcorsi.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            List<ConcorsiAquistabili> ListaConcorsi = await ConnessioneConcorsi(URL);

            int righe = 0, colonne = 0;
            foreach (var i in ListaConcorsi)
            {
                grigliaConcorsi.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                Grid DettagliConcorso = new Grid();

                DettagliConcorso.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                DettagliConcorso.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                DettagliConcorso.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                DettagliConcorso.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                DettagliConcorso.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                DettagliConcorso.ColumnSpacing = 0;

                

                var stackDescrizione = new StackLayout
                {
                 HorizontalOptions = LayoutOptions.Center
                };
                var stackTitolo = new StackLayout
                {
                    BackgroundColor = Color.FromHex("#37637F"),
                    VerticalOptions = LayoutOptions.Center
                };
                var stackPrezzo= new StackLayout
                {
                    BackgroundColor = Color.FromHex("#37637F"),
                    VerticalOptions = LayoutOptions.Center
                };

                Image logoConcorso = new Image
                {
                    Source = "carabinieri.png",
                    WidthRequest = 50
                };
                Label titolo = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Color.White,
                    FontSize = 20
                };
                Label descrizione = new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 20
                };
                Label prezzo = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.White,
                    FontSize = 20
                };
                Button acquista = new Button
                {
                    Text = "",
                    BackgroundColor = Color.FromHex("#37637F"),
                    TextColor = Color.White

                };
                if (i.state.Equals("Purchased"))
                {
                    acquista.Text = "AQUISTATO";
                    acquista.IsEnabled = false;
                }
                else
                {
                    acquista.Text = "ACQUISTA";
                    acquista.Clicked += async delegate (object sender, EventArgs e)
                    {
                        try
                        {
                            var productId = i.ProductId;

                            var connected = await CrossInAppBilling.Current.ConnectAsync();

                            if (!connected)
                            {
                                //Couldn't connect to billing, could be offline, alert user
                                return;
                            }

                            //try to purchase item
                            var purchase = await CrossInAppBilling.Current.PurchaseAsync(productId, ItemType.InAppPurchase, i.codiceControllo);
                            if (purchase == null)
                            {
                                //Not purchased, alert the user

                            }
                            else
                            {
                                //Purchased, save this information
                                var id = purchase.Id;
                                var token = purchase.PurchaseToken;
                                var state = purchase.State;
                                var codControllo = purchase.Payload;
                                var data = purchase.TransactionDateUtc;

                                await invioDatiPagamento(productId, id, token, state.ToString(), codControllo, data.ToString());

                            }
                        }
                        catch (Exception ex)
                        {
                            //Something bad has occurred, alert user
                        }
                        finally
                        {
                            //Disconnect, it is okay if we never connected
                            await CrossInAppBilling.Current.DisconnectAsync();
                        }
                    };
                }

                titolo.Text = i.Titolo;
                descrizione.Text = i.Descrizione;
                prezzo.Text = i.Prezzo + "€";
                logoConcorso.Source= Xamarin.Forms.ImageSource.FromStream(
                    () => new MemoryStream(Convert.FromBase64String(i.logo)));
                stackDescrizione.Children.Add(descrizione);
                stackTitolo.Children.Add(titolo);
                stackPrezzo.Children.Add(prezzo);
                DettagliConcorso.Children.Add(stackTitolo, 0, 0);
                Grid.SetColumnSpan(stackTitolo, 2);
                DettagliConcorso.Children.Add(stackDescrizione,1,1);
                DettagliConcorso.Children.Add(stackPrezzo,2,0);
                DettagliConcorso.Children.Add(acquista,2,1);
                DettagliConcorso.Children.Add(logoConcorso,0,1);

                colonne++;
                grigliaConcorsi.Children.Add(DettagliConcorso, 0, righe);
                colonne = 0;
                righe++;

            }
            if (URL.Contains(Costanti.concorsiDisponibili))
            {
            steckGrigliaConcorsi.Children.Clear();
            steckGrigliaConcorsi.Children.Add(grigliaConcorsi);
            }
            else
            {
                steckGrigliaMateria.Children.Clear();
                steckGrigliaMateria.Children.Add(grigliaConcorsi);
            }
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