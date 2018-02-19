using System;
using System.Collections.Generic;
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
    public partial class PianoFormativo : ContentPage
    {

        private List<ConcorsiAquistabili> ListaConcorsi = new List<ConcorsiAquistabili>();
        private string risultatojson;
        private bool flagConnessione = false;


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

        public async Task<List<ConcorsiAquistabili>> ConnessioneConcorsi()
        {
            string username = Utente.Instance.getUserName;
            var client = new HttpClient();
            try
            {
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("username", username));
                var content = new FormUrlEncodedContent(values);
                var result = await client.PostAsync(Costanti.concorsiDisponibili, content);
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
        public async Task creaGriglia()
        {
            Grid grigliaConcorsi = new Grid();

            grigliaConcorsi.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            List<ConcorsiAquistabili> ListaConcorsi = await ConnessioneConcorsi();

            int righe = 0, colonne = 0;
            foreach (var i in ListaConcorsi)
            {
                grigliaConcorsi.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                Grid DettagliConcorso = new Grid();

                DettagliConcorso.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                DettagliConcorso.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                DettagliConcorso.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                DettagliConcorso.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                DettagliConcorso.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                StackLayout prova = new StackLayout//riga1
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.Center
                };
                StackLayout provabtn = new StackLayout//Acquista
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.Center
                };

                Image logoConcorso = new Image
                {
                    Source = "btn_Pause.png",
                    WidthRequest = 50
                };
                Label titolo = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 20
                };
                Label descrizione = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 15
                };
                Label prezzo = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 15
                };
                Button acquista = new Button
                {
                    Text = ""
                };
                if (i.state.Equals("Purchased"))
                {
                    acquista.Text = "extra";
                    acquista.Clicked += async delegate (object sender, EventArgs e)
                    {
                        await Navigation.PushAsync(new PianoFormativoExtra(i));
                    };
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

                DettagliConcorso.Children.Add(titolo, 0, 0);
                Grid.SetColumnSpan(titolo, 2);
                DettagliConcorso.Children.Add(descrizione,1,1);
                DettagliConcorso.Children.Add(prezzo,2,0);
                DettagliConcorso.Children.Add(acquista,2,1);
                DettagliConcorso.Children.Add(logoConcorso,0,1);

                colonne++;
                grigliaConcorsi.Children.Add(DettagliConcorso, 0, righe);
                colonne = 0;
                righe++;

            }
            steckGrigliaConcorsi.Children.Clear();
            steckGrigliaConcorsi.Children.Add(grigliaConcorsi);
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