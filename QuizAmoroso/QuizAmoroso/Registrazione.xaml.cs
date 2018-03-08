using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QuizAmoroso.DataModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuizAmoroso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registrazione : ContentPage
    {
        private UtenteRegistrazione utente;
        public Registrazione()
        {
            InitializeComponent();
        }

        private async Task InvioDati()
        {
            utente= new UtenteRegistrazione();
            utente.Nome = LabelNome.Text;
            utente.Cognome = LabelCognome.Text;
            utente.Email = LabelEmail.Text;
            utente.password = LabelPassword.Text;
            await ConnessioneRegistrazione();


        }

        private async void Registrati(object sender, EventArgs e)
        {
            bool pass = true;
            if (string.IsNullOrEmpty(LabelNome.Text))
            {
                LabelNome.ErrorText = "Campo obbligatorio";
                pass = false;
            }
            if (string.IsNullOrEmpty(LabelCognome.Text))
            {
                LabelCognome.ErrorText = "Campo obbligatorio";
                pass = false;
            }
            {
                if (string.IsNullOrEmpty(LabelEmail.Text)|| (LabelEmail.Text.Contains("@") == false))
            {
                LabelEmail.ErrorText = "Campo obbligatorio";
                pass = false;
            }
               
            }

            if (string.IsNullOrEmpty(LabelPassword.Text))
            {
                LabelPassword.ErrorText = "Campo obbligatorio";
                pass = false;
            }

            if(pass)
            await InvioDati();
            
        }

        public async Task ConnessioneRegistrazione()
        {

            var client = new HttpClient();

            try
            {
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("email", utente.Email));
                values.Add(new KeyValuePair<string, string>("password", utente.password));
                values.Add(new KeyValuePair<string, string>("nome", utente.Nome));
                values.Add(new KeyValuePair<string, string>("cognome", utente.Cognome));


                var content = new FormUrlEncodedContent(values);
                var result = await client.PostAsync(Costanti.registrazione, content);
               var rispostaRichiestaLoginIniziale = await result.Content.ReadAsStringAsync();
                if (rispostaRichiestaLoginIniziale == "Utente Registrato")
                {
                    await DisplayAlert("Attenzione", "Registrazione avvenuta con successo", "OK");
                    App.Current.MainPage=new NavigationPage(new Login());
                }
                else
                {
                    await DisplayAlert("Attenzione", rispostaRichiestaLoginIniziale, "ok");
                }
               
            }
            catch (Exception e)
            {
              await  DisplayAlert("Attenzione", "errore con la comunicazione con il server", "ok");
            }
        }
    }
}