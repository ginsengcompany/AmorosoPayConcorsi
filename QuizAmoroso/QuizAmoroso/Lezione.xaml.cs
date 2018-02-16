using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizAmoroso.DataModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuizAmoroso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lezione : ContentPage
    {
        private double width;
        private double height;
        private Lezioni lezione = new Lezioni();
        private List<Lezioni> lista = new List<Lezioni>();
        public Lezione(Lezioni lezione, List<Lezioni> lista)
        {
            this.lista = lista;
            this.lezione = lezione;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblConcorso.Text = lezione.Nome;
            lblDescrizione.Text = lezione.Descrizione;
            lblMateria.Text = lezione.nomeSet;
            lblNumero.Text = "Lezione Numero: " + lezione.Lezione;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                    innerGrid.RowDefinitions.Clear();
                    innerGrid.ColumnDefinitions.Clear();
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    innerGrid.Children.Remove(controlsGrid);
                    innerGrid.Children.Add(controlsGrid, 1, 0);
                }
                else
                {
                    innerGrid.ColumnDefinitions.Clear();
                    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    innerGrid.Children.Remove(controlsGrid);
                    innerGrid.Children.Add(controlsGrid, 0, 1);
                }
            }
        }

        private async Task Button_Clicked_Video(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VideoLezioni(lezione.VideoSource));
        }

        private async Task Button_Clicked_LezioneSuccessiva(object sender, EventArgs e)
        {
            bool flag = false;
            foreach(var i in lista)
            {
                if (flag == true)
                {
                    flag = false;
                    await Navigation.PushAsync(new Lezione(i,lista));
                    break;
                }
                if(i.Nome == lezione.Nome)
                {
                    flag = true;
                }
            }
            if (flag == true)
            {
                await DisplayAlert("CONGRATULAZIONI", "SEI ARRIVATO ALL'ULTIMA LEZIONE, PREMI OK PER TORNARE ALLA HOME", "OK");
                await Navigation.PopToRootAsync();
            }

        }
    }
}