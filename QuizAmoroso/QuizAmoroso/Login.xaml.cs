using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System.Net.Http.Headers;
using QuizAmoroso.DataModel;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;

namespace QuizAmoroso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    /**
     *@Authors: Antonio Fabrizio Fiume, Alessio Calabrese, Antonio Saverio Valente
     * Questa page gestisce la login al sistema, tramite l'invio dell'username e della password 
     * il sistema controllerà se corrispondono a quelli salvati nel database, se il controllo avrà successo 
     * l'utente sarà reinderizzato alla pagina di informazioni;
     **/
    public partial class Login : ContentPage

    {
        // DICHIARAZIONI variabili
        //json account, che utilizzeremo per la restituzione e invio dei campi richiesti dal server.
        public JsonAccount jsonAccount = new JsonAccount();
        //Stringa di risposta dal server.
        public string rispostaRichiestaLoginIniziale;
        //variabile booleana che ci permette di capire se la connessione è stata eseguita con successo o meno.
        private bool flagConnessioneAccettata = false;

        ImageSource image;


        //Costruttore
        public Login()
        {
            //inizializza i componenti, ad esempio label, entry ecc..
            InitializeComponent();
            //La classe cross connectivity avverte che la connessione sta cambiando da online a offline, o viceversa
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            //In questo controllo, si ricerca se il metodo recuperaUserName ha salvato l'username, se non l'ha salvato allora settiamo la entry a null, cioè ci farà vedere la entry vuota.
            if(Utente.Instance.recuperaUserName() == null)
            {
                inputUsername.Text = "";
            } else
            {
                inputUsername.Text = Utente.Instance.recuperaUserName();
            }
        }
        //Il metodo seguente ci sarà richiamato solo nel momento in cui la connessione cambierà di stato.
        private async void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (!e.IsConnected)
            {
                await DisplayAlert("Errore", "Controlla la connessione!", "Ok");
            }
        }
        //il metodo onAppearing ci visualizzerà a schermo un display alert del cambiamento di stato della connessione;
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Errore", "Controlla la connessione!", "Ok");
            }
        }

        /**
         * in questo metodo  si controlla la lunghezza della parola inserita nelle entry username e password,
         * si controlla anche se le entry sono vuote o meno, in caso di non rispetto dei controlli apparirà un displayalert 
         * che avvertirà l'utente dei propri errori.
         * Nel frammento di codice fra try/catch si controllerà se l'username è nuovo 
         * quindi il sistema controllerà l'username che ha salvato in precedenza, se non corrispondono 
         * allora aggiornerà l'username da salvare. Fatto ciò disabiliterà le entry e i bottoni, così da evitare spiacevoli crash dell'app 
         * Dopodichè si richiamerà il metodo di connessione, se andrà tutto a buon fine, reindirizzerà verso la pagina di info. 
         * in caso contrario, apparirà un display alert di errore;*/
        private async void loginButton_Clicked(object sender, EventArgs e)
        {

            if ((string.IsNullOrEmpty(inputUsername.Text) || string.IsNullOrEmpty(inputPassword.Text)) || (string.IsNullOrWhiteSpace(inputUsername.Text) || (string.IsNullOrWhiteSpace(inputPassword.Text))))
            {
                await DisplayAlert("Errore", "Inserire tutti i campi!", "Ok");
            }            else
            {
                //se i controlli sono stati superati, allora apparirà un' activity indicator.
                caricamentoPagina.IsRunning = true;
                caricamentoPagina.IsVisible = true;
            
                try
                {
                    if (Utente.Instance.recuperaUserName() != inputUsername.Text) {
                        Utente.Instance.cancellaEdAggiornaUsername(inputUsername.Text);
                    }
                    loginButton.IsEnabled = false;
                    inputUsername.IsEnabled = false;
                    inputPassword.IsEnabled = false;
                    await Task.Run(() => ConnessioneLogin());
                    caricamentoPagina.IsRunning = false;
                    caricamentoPagina.IsVisible = false;
                    if (flagConnessioneAccettata == false)
                    {
                        var p = new NavigationPage(new MainPage());
                        await Navigation.PushModalAsync(p);
                        
                        loginButton.IsEnabled = true;
                        inputUsername.IsEnabled = true;
                        inputPassword.IsEnabled = true;
                        inputUsername.Text = Utente.Instance.getUserName;
                        inputPassword.Text = string.Empty;
                    }
                    else
                    {
                        await DisplayAlert("Errore", rispostaRichiestaLoginIniziale, "OK");
                        loginButton.IsEnabled = true;
                        inputUsername.IsEnabled = true;
                        inputPassword.IsEnabled = true;
                    }
                }
                catch (Exception ee)
                {
                    await DisplayAlert("Errore", rispostaRichiestaLoginIniziale, "Ok");
                    loginButton.IsEnabled = true;
                    inputUsername.IsEnabled = true;
                    inputPassword.IsEnabled = true;
                }
                caricamentoPagina.IsRunning = false;
                caricamentoPagina.IsVisible = false;
            }
        }
        /* il metodo di connessione servirà per creare un collegamento fra device e server. Tramite una connessione ad una
         pagina Php ove verranno inviati tramite tecnologia JSON i dati richiesti dal server(Username,password,deviceinfo,descrizione del device)*/
        public async Task ConnessioneLogin()
        {
            string username = inputUsername.Text.ToUpper();
            string password = inputPassword.Text;
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(20);

            try
            {
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("username", username));
                values.Add(new KeyValuePair<string, string>("password", password));
                var content = new FormUrlEncodedContent(values);
                var result = await client.PostAsync(Costanti.login, content);
                rispostaRichiestaLoginIniziale = await result.Content.ReadAsStringAsync();

                if (rispostaRichiestaLoginIniziale.ToString() == "Utente non registrato" )
                {
                    flagConnessioneAccettata = true;
                }    
                else if (rispostaRichiestaLoginIniziale.Contains("login fallita"))
                {
                    flagConnessioneAccettata = true;
                    throw new Exception();
                }else 
                {
                    flagConnessioneAccettata = false;
                    jsonAccount = JsonConvert.DeserializeObject<JsonAccount>(rispostaRichiestaLoginIniziale);
                    Utente.Instance.getPassword = jsonAccount.password;
                    Utente.Instance.getUserName = jsonAccount.username;
                }
            }
            catch (Exception e)
            {
                flagConnessioneAccettata = true;
                rispostaRichiestaLoginIniziale = "Errore di connessione in fase di accesso al sistema.";
            }
        }

        //La classe seguente, definisce tutti i campi che avrà il JSON
        public class JsonAccount
        {
            public string username { get; set; }
            public string password { get; set; }
        }
        //Il seguente metodo ha lo scopo di mostrare o meno la password all'utente, tramite una tapgesture dell'immagine.

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
           if(inputPassword.IsPassword==true)
            {
                inputPassword.IsPassword = false;
                ShowPassw.Source = "shwpsswblueSegnato.png";
            }
            else
            {
                inputPassword.IsPassword = true;
                ShowPassw.Source = "shwpsswblue.png";
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registrazione());
        }
    }
}