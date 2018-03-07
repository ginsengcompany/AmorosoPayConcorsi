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
	public partial class MenuHamburger : MasterDetailPage
	{
		public MenuHamburger ()
		{
			InitializeComponent ();
            inizializzazioneMenu();
		}

	    private void SelezioneCellaMenu(object sender, SelectedItemChangedEventArgs e)
	    {
	        var menu = e.SelectedItem as Menu; //La variabile menu contiene l'elemento selezionato
	        if (menu != null) //Controlla se l'elemento non è null
	        {
	            /*
                 * In base all'elemento che l'utente ha tappato si avvia la relativa pagina o si effettua il logout
                 */
	            if (menu.MenuTitle.Equals("Home"))
	            {
	                IsPresented = false;
	                Detail = new NavigationPage(new HomePage()); //Avvia la pagina principale
	            }
	            else if (menu.MenuTitle.Equals("Acquista"))
	            {
	                IsPresented = false;
	                Detail = new NavigationPage(new PianoFormativo()); 
	            }
	            else if (menu.MenuTitle.Equals("Info"))
	            {
	                IsPresented = false;
	                Detail = new NavigationPage(new Info());
	            }
                else if (menu.MenuTitle.Equals("Quiz"))
	            {
	                IsPresented = false;
	                Detail = new NavigationPage(new ConcorsiESottocategorie()); //Avvia la pagina per la scelta della struttura preferita
	            }
	        }
        }

        private void inizializzazioneMenu()
        {
            List<Menu> menuPrincipale = new List<Menu> //Lista contenente le pagine a cui si può accedere dalla MasterDetailPage
            {
                new Menu { MenuTitle = "Home"},
              //  new Menu { MenuTitle = "Scegli Struttura Preferita", ImageIcon = "modify.png"},
                new Menu { MenuTitle ="Acquista"},
                new Menu{MenuTitle ="Quiz"},
                new Menu{MenuTitle ="Info"}
            };
            ListaMenu.ItemsSource = menuPrincipale; //Assegna all'oggetto ListaMenu dello xaml della pagina la lista precedentemente inizializzata
            Detail = new NavigationPage(new HomePage()); //Avvia la pagina principale
        }


        public class Menu
        {
            public string MenuTitle
            {
                get;
                set;
            }

            public string ImageIcon
            {
                get;
                set;
            }
        }
    }
}