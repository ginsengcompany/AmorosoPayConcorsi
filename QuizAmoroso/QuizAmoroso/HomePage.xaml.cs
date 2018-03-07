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
            await icQuiz.TranslateTo(-100, -100, 1000);
            App.Current.MainPage = new NavigationPage(new ConcorsiESottocategorie());
        }

        public void paginaInfo(object sender, EventArgs args)
        {
            App.Current.MainPage = new NavigationPage(new Info());
        }

        public void paginaPurchase(object sender, EventArgs args)
        {
            App.Current.MainPage = new NavigationPage(new PianoFormativo());
        }
    }
}