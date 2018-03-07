using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuizAmoroso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        public async void paginaQuiz(object sender, EventArgs args)
        {
            var icon = sender as Image;
            await animateIcon(icon);
            await Navigation.PushAsync(new ConcorsiESottocategorie());
        }

        public async void paginaInfo(object sender, EventArgs args)
        {
            var icon = sender as Image;
            await animateIcon(icon);
            await Navigation.PushAsync(new Info());
        }

        public async void paginaPurchase(object sender, EventArgs args)
        {
            var icon = sender as Image;
            await animateIcon(icon);
            await Navigation.PushAsync(new PianoFormativo());
        }

        private async Task animateIcon(Image a)
        {
            /*
            await a.TranslateTo(0, -5, 200);
            await a.TranslateTo(0, 5, 200);
            await a.TranslateTo(0, -5, 200);
            await a.TranslateTo(0, 5, 200);
            */
            await a.RotateYTo(360, 2000);
        }

        protected override void OnDisappearing()
        {
            icQuiz.RotationY = 0;
            icPurchase.RotationY = 0;
            icInfo.RotationY = 0;
        }
    }
}