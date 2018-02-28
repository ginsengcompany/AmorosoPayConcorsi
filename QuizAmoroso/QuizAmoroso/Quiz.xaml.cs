using Newtonsoft.Json;
using QuizAmoroso.DataModel;
using QuizAmoroso.Model;
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
    public partial class Quiz : ContentPage
    {
        private DatiConnessioneDomande datiConnessione = new DatiConnessioneDomande();
        private Label numeroDomande;
        private string statoPrecedente = "unselected_circle.png";
        private int indice = 0;
        private int indiceVisualizzato = 1;
        private List<Image> countImage = new List<Image>();
        private double width, height;
        public Quiz(DatiConnessioneDomande datiConnessione)
        {
            this.datiConnessione = datiConnessione;
            InitializeComponent();
            for(int i = 0; i< Convert.ToInt32(datiConnessione.numeroDomande); i++)
            {
                Image prova = new Image
                {
                    Source = statoPrecedente,
                    WidthRequest = 15

                };
                countImage.Add(prova);
            }
            countImage[0].Source = "selected_circle.png";
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                Prova.Children.Clear();

                    Prova.Children.Add(GetLayoutData(width, height));


            }

        }
        private Grid GetLayoutData(double width, double height)
        {

            // Check if cached information is available.
            int colonne = 0;
            numeroDomande = new Label
            {
                Text = indiceVisualizzato + "/" + datiConnessione.numeroDomande,
                FontSize = 15
            };
            Grid prova = new Grid();
            prova.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            prova.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            prova.Children.Add(numeroDomande, colonne, 0);
            colonne++;
            int count = 0;
            double test = width-15.00;
            while (test >15.00 && countImage.Count>count)
            {
                prova.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                prova.Children.Add(countImage[count], colonne, 0);
                test = test - 15;
                colonne++;
                count++;
            }
            return prova;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (indice+1 < countImage.Count)
            {
            indiceVisualizzato = indiceVisualizzato + 1;
            numeroDomande.Text = indiceVisualizzato + "/" + datiConnessione.numeroDomande;
            countImage[indice].Source = statoPrecedente;
            statoPrecedente = countImage[indice + 1].Source.ToString();
            statoPrecedente = statoPrecedente.Substring(6);
            countImage[indice+1].Source = "selected_circle.png";
            indice++;
            }

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (indice != 0)
            {
                if (indiceVisualizzato != 1)
                {
                    indiceVisualizzato = indiceVisualizzato - 1;
                    numeroDomande.Text = indiceVisualizzato + "/" + datiConnessione.numeroDomande;
                }
                countImage[indice].Source = statoPrecedente;
                statoPrecedente = countImage[indice - 1].Source.ToString();
                statoPrecedente = statoPrecedente.Substring(6);
                countImage[indice - 1].Source = "selected_circle.png";
                indice--;
            }
        }

        public async Task ConnessioneDomande()
        {
            var client = new HttpClient();
            var result = new HttpResponseMessage();
            try
            {
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("id_concorso", datiConnessione.idConcorso));
                values.Add(new KeyValuePair<string, string>("materia", datiConnessione.materia));
                values.Add(new KeyValuePair<string, string>("numerodomande", datiConnessione.numeroDomande));

                var content = new FormUrlEncodedContent(values);
                if (datiConnessione.modalitaSelezionata == "Libera")
                {
                    content = new FormUrlEncodedContent(values);
                    result = await client.PostAsync(Costanti.domconcorsorandomNew, content);
                }
                else
                {
                    values.Add(new KeyValuePair<string, string>("domandainiziale", QuizVeloce.numeroselezionato.ToString()));
                    content = new FormUrlEncodedContent(values);
                    result = await client.PostAsync(Costanti.domconcorsosequenzaNew, content);
                }

                var resultcontent = await result.Content.ReadAsStringAsync();

                if (resultcontent.ToString() == "errore nella get")
                {
                    var flag = true;
                }
                else
                {
                    var flag = false;
                   var struttura = JsonConvert.DeserializeObject<List<Domande>>(resultcontent);
                    if (datiConnessione.modalitaSelezionata == "Modalità Casuale")
                    {
                        struttura = ShuffleList.Shuffle<Domande>(struttura);
                    }
                   var numeroTotaleDelSetDiDomande = struttura.Count;
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Errore","fff", "Ok");
            }
        }
    }
}