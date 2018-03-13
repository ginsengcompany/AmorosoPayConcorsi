using QuizAmoroso.ModelView;
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
    public partial class Statistiche : ContentPage
    {
        private StatisticheModelView modelView;

        public Statistiche()
        {
            InitializeComponent();
            modelView = new StatisticheModelView();
            BindingContext = modelView;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            if (picker.SelectedIndex > 0)
            {
                string materia = picker.SelectedItem as string;
                modelView.getStatistiche(materia);
            }
            else if(picker.SelectedIndex == 0)
            {
                modelView.getStatistiche("");
            }
        }
    }
}