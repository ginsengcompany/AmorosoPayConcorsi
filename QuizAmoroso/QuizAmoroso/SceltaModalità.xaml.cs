using QuizAmoroso.Classi;
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
    public partial class SceltaModalità : TabbedPage
    {
        public SceltaModalità(Materie i,String idconcorso)
        {
            this.Title = i.materia;
            this.Children.Add(new ModalitàQuizRandom(i,idconcorso));
            this.Children.Add(new Info());
        }
    }
}