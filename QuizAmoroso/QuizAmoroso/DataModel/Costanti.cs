using System;
using Xamarin.Forms;

namespace QuizAmoroso.DataModel
{
    public class Costanti
    {
        /**
         * Link ai servizi esposti lato piattaforma
         * */
        public const string sitoAK12 = 
            "http://www.ak12srl.it";

        public const string paginaFacebook = 
            "https://www.facebook.com/centro.militari/?fref=ts";

        public const string urlLocation =
                "https://www.google.it/maps/place/Viale+Italia,+53,+81020+San+Nicola+La+Strada+CE/@41.0507431,14.3264049,17z/data=!3m1!4b1!4m5!3m4!1s0x133a551ccb708adb:0xb134a991e304809a!8m2!3d41.0507431!4d14.3285936";

        public const string urlBase = 
            "https://amorosoconcorsi.ak12srl.it/services/";

        public const string domandeNew = 
            "https://amorosoconcorsi.ak12srl.it/services/servizioapp/domandeNew";

       

        public const string logout = 
            "https://amorosoconcorsi.ak12srl.it/services/servizioapp/logout";

      
        public const string sessione = 
            "https://amorosoconcorsi.ak12srl.it/services/servizioapp/sessione";

        public const string sessionePerTuttiConcorsi = 
            "https://amorosoconcorsi.ak12srl.it/services/servizioapp/sessionepertutticoncorsi";

        public const string pianoformativo = 
            "https://amorosoconcorsi.ak12srl.it/services/servizioapp/pianoformativo";

        public const string setdomande = 
            "https://amorosoconcorsi.ak12srl.it/services/servizioapp/setdomande";


        

        public const string statisticheURL = 
            "https://amorosoconcorsi.ak12srl.it/services/statistiche/homes.html?username=";

        public const string cronologia = 
            "https://amorosoconcorsi.ak12srl.it/services/servizioapp/cronologia";

        public const string commenti = 
            "https://amorosoconcorsi.ak12srl.it/services/servizioapp/commenti";

        public const string materietotali = 
            "https://amorosoconcorsi.ak12srl.it/services/servizioapp/materietotali";

        public const string domconcorsorandomtotaliNew = 
            "https://amorosoconcorsi.ak12srl.it/services/servizioapp/domconcorsorandomtotaliNew";



        /**
         * Costanti di servizio
         * */
        public static char[] alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        /**
         * Costanti per il limite di domande ammesse per eseguire il test 
         * sulle domande contenute sull'intera banca dati amministratore
         * */
        public const int numeroMassimoDomandeAmmesso = 150;
        public const int numeroMassimoDomandeDelTestSuInteroDB = 100;

        public const string eseguiTestSuInteroDb = "Esercitazione su tutti i Concorsi";

        public const string login =
           "http://192.168.125.97/servizioPayApp/login.php";
        public const string salvaPagamento =
           "http://192.168.125.97/servizioPayApp/cronologiaAcquisti.php";
        public const string concorsiDisponibili =
           "http://192.168.125.97/servizioPayApp/concorsiAcquisti.php";
        public const string MaterieDisponibili =
   "http://192.168.125.97/servizioPayApp/materieAcquisti.php";
        public const string concorsi =
            "http://192.168.125.97/servizioPayApp/concorsi.php";
        public const string materieconcorso =
    "http://192.168.125.97/servizioPayApp/materieconcorso.php";
        public const string domconcorsorandomNew =
          "http://192.168.125.97/servizioPayApp/domandeconcorsorandomNew.php";
        public const string registrazione= "http://192.168.125.97/servizioPayApp/registrazione.php";
        public const string domconcorsosequenzaNew =
            "http://192.168.125.97/servizioPayApp/domandeconcorsosequenzaNew.php";
        public const string invioTempiGlobali =
            "http://192.168.125.97/servizioPayApp/tempoUtilizzo.php";
        public const string ConcorsiLezioni =
            "http://192.168.125.97/servizioPayApp/lezioniConcorso.php";
        public const string ListaLezioni =
            "http://192.168.125.97/servizioPayApp/listaLezioni.php";
        public const string DatiStatistica =
            "http://192.168.125.97/servizioPayApp/salvaSessioni.php";


    }
}