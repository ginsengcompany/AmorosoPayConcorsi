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
        private Grid prova;
        private List<Domande> list = new List<Domande>();
        private DatiConnessioneDomande datiConnessione = new DatiConnessioneDomande();
        private Label numeroDomande;
        private string statoPrecedente = "unselected_circle.png";
        private int indice, sign, count = 0;
        private int indiceVisualizzato = 1;
        private List<Image> countImage = new List<Image>();
        private double width, height;

        private Image arrowleft = new Image{
            Source= "arrowLeft.png",
            WidthRequest=15
            };
        private Image arrowright = new Image
        {
            Source = "arrowRight.png",
            WidthRequest = 15
        };
        protected override async void OnAppearing()
        {
            base.OnAppearing();
           list = await ConnessioneDomande();
           await Griglia();
        }
        public Quiz(DatiConnessioneDomande datiConnessione)
        {
            this.datiConnessione = datiConnessione;
            InitializeComponent();

            
          /*  for(int i = 0; i< Convert.ToInt32(datiConnessione.numeroDomande); i++)
            {
                Image prova = new Image
                {
                    Source = statoPrecedente,
                    WidthRequest = 15

                };
                if (i == 2  || i == 5 || i==7 || i==25 || i==27 || i==29)
                {
                    prova.Source = "sign_RispostaData.png";
                }
                countImage.Add(prova);
            }
            countImage[0].Source = "selected_circle.png";*/
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
              //  Prova.Children.Clear();

                 //   Prova.Children.Add(GetLayoutData(width, height));


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
            prova = new Grid();
            prova.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            prova.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            prova.Children.Add(numeroDomande, colonne, 0);
            colonne++;
            count = 0;
            double test = width-15.00;
            while (test >15.00 && countImage.Count>count)
            {
                prova.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                prova.Children.Add(countImage[count], colonne, 0);
                test = test - 15;
                colonne++;
                count++;
            }
            if (count < countImage.Count)
            {
                prova.Children.Add(arrowright, colonne--, 0);
            }
            return prova;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
           /* int count = this.count-1;
            if (indice+1 < countImage.Count)
            {
                indiceVisualizzato = indiceVisualizzato + 1;
                numeroDomande.Text = indiceVisualizzato + "/" + datiConnessione.numeroDomande;
                countImage[indice].Source = statoPrecedente;
                statoPrecedente = countImage[indice + 1].Source.ToString();
                statoPrecedente = statoPrecedente.Substring(6);
                countImage[indice+1].Source = "selected_circle.png";
                indice++;

                if(indice > count && indice>sign)
                {
                    prova.Children.Remove(countImage[0]);
                    prova.Children.Add(arrowleft, 1, 0);
                    if (indice == countImage.Count - 1)
                    {
                        int test = indice - (count);
                        int colonna = 2;
                        count++;
                        prova.Children.Remove(arrowright);
                        while (colonna <= count+1)
                        {
                            prova.Children.Add(countImage[test], colonna, 0);
                            colonna++;
                            test = test + 1;
                        }
                    }
                    else
                    {
                        int test = indice - (count-1);
                        int colonna = 2;
                        count++;
                        while (colonna <= count)
                        {
                            prova.Children.Add(countImage[test], colonna, 0);
                            colonna++;
                            test = test+ 1;
                        }
                    }
                    
                    
                 sign = indice;
                }
               
                   // prova.Children.Add(countImage[indice + 1- count], 1, 0);
                
            }*/

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
           /* int count = this.count - 1;
            
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
                if (sign > 0)
                {
                    int prova = sign - (count);
                     
                    if (indice == 0 && prova>0)
                    {
                        //prova.Children.Remove(countImage[count]);
                        
                        if (prova == 1)
                        {
                        this.prova.Children.Remove(arrowleft);
                            int index = this.count + 1;
                            this.prova.Children.Add(arrowright, index, 0);
                        }
                        else
                            this.prova.Children.Add(arrowright, this.count, 0);

                        int colonna;
                        if (prova == 0)
                            colonna = 1;
                        else
                            colonna = 2;
                        count++;
                        while (colonna <= count)
                        {
                            this.prova.Children.Add(countImage[prova], colonna, 0);
                            colonna++;
                            prova = prova + 1;
                        }
                        sign = sign-1;
                    }

                }

            }*/
        }

        public async Task<List<Domande>> ConnessioneDomande()
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
                    return new List<Domande>();
                }
                else
                {
                    var flag = false;
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
                await DisplayAlert("Errore","fff", "Ok");
                return new List<Domande>();
            }
        }
        public async Task Griglia()
        {
            GrigliaDomande.Children.Clear();
            GrigliaDomande.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            GrigliaDomande.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            GrigliaDomande.ColumnDefinitions = new ColumnDefinitionCollection();
            Frame frmDomanda = new Frame
            {
                HasShadow = true,
                BackgroundColor=Colori.ColoriSecondario,
                Padding=10,
                Margin=8,
                CornerRadius=5
            };
            
            

            Label domanda = new Label
            {
                Text = list[0].Domanda,
                FontAttributes=FontAttributes.Bold,
                TextColor=Color.White
            };
            frmDomanda.Content = domanda;
            GrigliaDomande.Children.Add(frmDomanda, 0, 0);

            Grid gridQuesiti = new Grid();
            gridQuesiti.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            gridQuesiti.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            char Alfabeto = 'A';
            int riga = 0;
            foreach (var i in list[0].Quesiti)
            {
                gridQuesiti.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                Button lettera = new Button
                {
                    Text = Alfabeto.ToString(),
                    BackgroundColor=Colori.Button
                };
                Alfabeto = (Char)(Convert.ToUInt16(Alfabeto) + 1);
                Label quesito = new Label
                {
                    Text = i
                };

                gridQuesiti.Children.Add(lettera, 0, riga);
                gridQuesiti.Children.Add(quesito, 1, riga);
                riga++;
            }

            GrigliaDomande.Children.Add(gridQuesiti, 0, 1);

        }
    }
}