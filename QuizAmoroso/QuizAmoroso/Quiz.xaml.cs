﻿using Newtonsoft.Json;
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
        //timer
        private Timer tempoQuiz = new Timer();
        private bool scelta = true;
        //Variabili dati
        private List<DatiRisultati> risultato = new List<DatiRisultati>();
        private List<Domande> list = new List<Domande>();
        private string statoPrecedente = "unselected_circle.png";
        private string statoSelected = "selected_circle.png";
        private string statoRisposta = "sign_RispostaData.png";
        private int indice, sign, count = 0;
        private int indiceVisualizzato = 1;
        //Griglie
        private Grid GridGrande;
        private List<Grid> grid_domande = new List<Grid>();
        //connessione
        private DatiConnessioneDomande datiConnessione = new DatiConnessioneDomande();
        //Form
        private Label numeroDomande;

        private List<Image> countImage = new List<Image>();
        private double width, height;

        private Image arrowleft = new Image
        {
            Source = "arrowLeft.png",
            WidthRequest = 15
        };
        private Image arrowright = new Image
        {
            Source = "arrowRight.png",
            WidthRequest = 15
        };
        protected override async void OnAppearing()
        {

            base.OnAppearing();
            btnAvanti.BackgroundColor = Colori.Button;
            btnIndietro.BackgroundColor = Colori.Button;
            DisabilitaLayoutActivityIndicator.IsVisible = true;
            caricamentoPagina.IsRunning = true;
            stackPagina.IsVisible = false;
            list = await ConnessioneDomande();
            listRisultato();
            await Griglia();
            DisabilitaLayoutActivityIndicator.IsVisible = false;
            caricamentoPagina.IsRunning = false;
            stackPagina.IsVisible = true;
            scelta = true;
            tempoQuiz.AvvioTempo(scelta, lblTimer);
        }

        private async void listRisultato()
        {
            foreach (var i in list)
            {
                DatiRisultati risposte = new DatiRisultati();
                risposte.Domanda = i.Domanda;
                risposte.risposta = i.Risposta;
                risultato.Add(risposte);
            }
        }

        private void Indietro()
        {
            if (indice != 0)
            {
                countImage[indice].Source = statoPrecedente;
                btnAvanti.Text = "AVANTI";
                indice--;
                indiceVisualizzato--;
                statoPrecedente = countImage[indice].Source.ToString();
                statoPrecedente = statoPrecedente.Substring(6);
                countImage[indice].Source = statoSelected;
                grigliaDomande.Children.Clear();
                grigliaDomande.Children.Add(grid_domande[indice]);
                if (indice == 0)
                    btnIndietro.IsVisible = false;
                numeroDomande.Text = indiceVisualizzato + "/" + datiConnessione.numeroDomande;
            }
        }
        private async Task Avanti()
        {
            if (indice != grid_domande.Count() - 1)
            {
                btnIndietro.IsVisible = true;
                countImage[indice].Source = statoPrecedente;
                indiceVisualizzato++;
                indice++;
                statoPrecedente = countImage[indice].Source.ToString();
                statoPrecedente = statoPrecedente.Substring(6);
                countImage[indice].Source = statoSelected;
                grigliaDomande.Children.Clear();
                grigliaDomande.Children.Add(grid_domande[indice]);
                if (indice == grid_domande.Count() - 1)
                    btnAvanti.Text = "FINE";
                else
                    btnAvanti.Text = "AVANTI";
                numeroDomande.Text = indiceVisualizzato + "/" + datiConnessione.numeroDomande;
                
            }
            else
            {
                if(await App.Current.MainPage.DisplayAlert("ATTENZIONE", "Sei sicuro di voler concludere la simulazione?", "SI", "NO"))
                {
                    scelta = false;
                    tempoQuiz.FermaTempo();
                    RisultatoQuiz risultati = RisultatiQuiz();
                    await Navigation.PushAsync(new RisultatiQuiz(risultati));
                    Navigation.RemovePage(this);
                    
                }
            }

        }
        private void riempicountImage()
        {

            for (int i = 0; i < Convert.ToInt32(datiConnessione.numeroDomande); i++)
            {
                Image cerchietto = new Image
                {
                    Source = statoPrecedente,
                    WidthRequest = 15

                };
                countImage.Add(cerchietto);
            }
            countImage[0].Source = statoSelected;
            indice = 0;
            
        }
        public Quiz(DatiConnessioneDomande datiConnessione)
        {
            this.datiConnessione = datiConnessione;
            riempicountImage();
            InitializeComponent();
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                StackGridContatore.Children.Clear();
                StackGridContatore.Children.Add(GetLayoutData(width, height));


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
            GridGrande = new Grid();
            GridGrande.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            GridGrande.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            GridGrande.Children.Add(numeroDomande, colonne, 0);
            colonne++;
            count = 0;
            double test = width - 15.00;
            while (test > 15.00)
            {
                GridGrande.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                if(countImage.Count > count)
                    GridGrande.Children.Add(countImage[count], colonne, 0);
                test = test - 15;
                colonne++;
                count++;
            }
            if (count < countImage.Count)
            {
                GridGrande.Children.Add(arrowright, colonne--, 0);
            }
            return GridGrande;
        }

        private async void ButtonClickedAvanti(object sender, EventArgs e)
        {
            await Avanti();

            

        }

        private void ButtonClickedIndietro(object sender, EventArgs e)
        {
            Indietro();

          

        }

        public async Task<List<Domande>> ConnessioneDomande()
        {
            var client = new HttpClient();
            var result = new HttpResponseMessage();
            try
            {
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("id_concorso", datiConnessione.idConcorso));

                if(datiConnessione.materia!="SIMULAZIONE")
                    values.Add(new KeyValuePair<string, string>("materia", datiConnessione.materia));
                else
                {
                    values.Add(new KeyValuePair<string, string>("materia","null"));
                }
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
                    return new List<Domande>();
                }
                else
                {
                    List<Domande> struttura = JsonConvert.DeserializeObject<List<Domande>>(resultcontent);
                    if (datiConnessione.modalitaSelezionata == "Modalità Casuale")
                    {
                        struttura = ShuffleList.Shuffle<Domande>(struttura);
                    }
                    var numeroTotaleDelSetDiDomande = struttura.Count;
                    return struttura;
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Errore", "Connessione non avvenuta", "Ok");
                return new List<Domande>();
            }
        }
        public async Task Griglia()
        {/*
              GrigliaDomande.Children.Clear();
              GrigliaDomande.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
              GrigliaDomande.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
              GrigliaDomande.ColumnDefinitions = new ColumnDefinitionCollection();*/
            foreach (var list in this.list)
            {
                Grid gridDom = new Grid();
                gridDom.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                gridDom.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                gridDom.ColumnDefinitions = new ColumnDefinitionCollection();
                Frame frmDomanda = new Frame
                {
                    HasShadow = true,
                    BackgroundColor = Colori.ColoriSecondario,
                    Padding = 10,
                    Margin = 8,
                    CornerRadius = 5
                };
                
                Label domanda = new Label
                {
                    Text = list.Domanda,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.White
                };
                frmDomanda.Content = domanda;

                gridDom.Children.Add(frmDomanda, 0, 0);

                Grid gridQuesiti = new Grid();
                gridQuesiti.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                gridQuesiti.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                char Alfabeto = 'A';
                int riga = 0;
                foreach (var i in list.Quesiti)
                {
                    gridQuesiti.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    Button lettera = new Button
                    {
                        Text = Alfabeto.ToString(),
                        BackgroundColor = Colori.Button,
                        TextColor=Color.White
                    };
                    lettera.Clicked += async delegate (object sender, EventArgs e)
                    {
                        lettera.BackgroundColor = Colori.coloreTerziario;
                        foreach (var y in gridQuesiti.Children)
                        {
                            if (y.GetType() == lettera.GetType())
                            {
                                y.IsEnabled = false;
                                if (y == lettera)
                                    await Risposta(i, lettera.Text.ToString(), list);
                            }
                        }
                        statoPrecedente = statoRisposta;
                        await Avanti();
                    };
                    Alfabeto = (Char)(Convert.ToUInt16(Alfabeto) + 1);
                    Label quesito = new Label
                    {
                        Text = i,
                        VerticalOptions=LayoutOptions.Center
                    };

                    gridQuesiti.Children.Add(lettera, 0, riga);
                    gridQuesiti.Children.Add(quesito, 1, riga);
                    riga++;
                }
                gridDom.Children.Add(gridQuesiti, 0, 1);
                if (list.link != null)
                {
                    string urlRisorsa = "";
                    if (list.tipo == "img")
                    {
                        urlRisorsa = Costanti.urlBase + list.link;
                        var urlProva = new System.Uri(urlRisorsa);
                        Task<ImageSource> result = Task<ImageSource>.Factory.StartNew(() => ImageSource.FromUri(urlProva));
                        Image img = new Image();
                        img.Source = await result;
                        img.HorizontalOptions = LayoutOptions.Center;
                        gridDom.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                        gridDom.Children.Add(img, 0, 2);

                    }
                    else if (list.tipo == "pdf")
                    {
                        urlRisorsa = Costanti.urlBase + list.link;
                        Button pdf = new Button
                        {
                            Text = "PDF",
                            BackgroundColor = Colori.Button
                        };
                        pdf.Clicked += async delegate (object sender, EventArgs e)
                        {
                            Device.OpenUri(new Uri(urlRisorsa));
                        };
                        gridDom.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                        gridDom.Children.Add(pdf, 0, 2);
                    }
                }
                grid_domande.Add(gridDom);
            }
            grigliaDomande.Children.Add(grid_domande[0]);

        }

        private async Task Risposta(string i, string alfabeto, Domande list)
        {
            foreach (var k in risultato)
            {
                if (k.Domanda == list.Domanda && k.risposta == list.Risposta)
                {
                    k.tuaRisposta = i;
                    k.letteraSelezionata = alfabeto;
                    if (k.letteraSelezionata == k.risposta)
                    {
                        k.rispostaEsattaYN = "true";
                    }
                    else
                    {
                        k.rispostaEsattaYN = "false";
                    }
                }
            }
        }
        private RisultatoQuiz RisultatiQuiz()
        {
            RisultatoQuiz risultati = new RisultatoQuiz();
            int contEsatteTot = 0,
                contNonRisposteTot = 0,
                contSbagliateTot = 0;
                //lstdatirisultati = 0;
            foreach (var i in risultato)
            {
                if(i.rispostaEsattaYN=="true")
                    contEsatteTot= contEsatteTot + 1;
                else if(i.rispostaEsattaYN=="false")
                    contSbagliateTot = contSbagliateTot + 1;
                else
                    contNonRisposteTot = contNonRisposteTot+1;
            }
            risultati.contEsatteTot = contEsatteTot.ToString();
            risultati.contSbagliateTot = contSbagliateTot.ToString();
            risultati.contNonRisposteTot = contNonRisposteTot.ToString();
            risultati.TmpTotale = lblTimer.Text;
            risultati.numeroDomande = risultato.Count.ToString();
            return risultati;
        }
    }
}