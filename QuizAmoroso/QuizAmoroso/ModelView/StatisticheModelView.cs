using Newtonsoft.Json;
using QuizAmoroso.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuizAmoroso.ModelView
{
    public class StatisticheModelView : INotifyPropertyChanged
    {

        private List<string> listamaterie;
        private StatisticheMateria statistiche;
        private List<BindingStatistica> data = new List<BindingStatistica>();
        private bool visible;

        public bool _visible
        {
            get
            {
                return visible;
            }
            set
            {
                OnPropertyChanged();
                visible = value;
            }
        }

        public List<BindingStatistica> Data
        {
            get
            {
                return data;
            }
            set
            {
                OnPropertyChanged();
                data = new List<BindingStatistica>(value);
            }
        }

        public List<string> _listamaterie
        {
            get
            {
                return listamaterie;
            }

            set
            {
                OnPropertyChanged();
                listamaterie = value;
            }
        }

        public StatisticheMateria _statistiche
        {
            get
            {
                return statistiche;
            }

            set
            {
                OnPropertyChanged();
                statistiche = new StatisticheMateria(value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public StatisticheModelView()
        {
            statistiche = new StatisticheMateria();
            _visible = false;
            getMaterie();
        }

        private async void getMaterie()
        {
            _statistiche.username = Utente.Instance.getUserName;
            await _getMaterie();
        }

        public async void getStatistiche(string materia)
        {
            _statistiche.materia = materia;
            await _getStatistiche();
            if (visible == false)
                _visible = true;
        }

        private async Task _getStatistiche()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(20);
            try
            {
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("username", statistiche.username));
                values.Add(new KeyValuePair<string, string>("materia", statistiche.materia));
                var content = new FormUrlEncodedContent(values);
                var result = await client.PostAsync(Costanti.statisticheMateria, content);
                string risposta = await result.Content.ReadAsStringAsync();
                if (risposta.ToString() == "Utente non registrato")
                {
                    await App.Current.MainPage.DisplayAlert("ATTENZIONE", risposta.ToString(), "OK");
                }
                else if (risposta.Contains("Nessuna materia trovata"))
                {
                    await App.Current.MainPage.DisplayAlert("ATTENZIONE", risposta.ToString(), "OK");
                }
                else
                {
                    StatisticheMateria jsonContent = JsonConvert.DeserializeObject<StatisticheMateria>(risposta);
                    _statistiche = new StatisticheMateria(jsonContent, _statistiche.username);
                    Data = new List<BindingStatistica>()
                    {
                        new BindingStatistica("Risposte esatte", statistiche.risposteesatte),
                        new BindingStatistica("Risposte errate", statistiche.rispostesbagliate),
                        new BindingStatistica("Domande senza risposta", _statistiche.domandesenzarisposta)
                    };
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ATTENZIONE", "Errore di connessione con il servizio", "OK");
            }
        }

        private async Task _getMaterie()
        {
            var client = new HttpClient();
            List<string> temp = new List<string>();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(20);
            try
            {
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("username", statistiche.username));
                var content = new FormUrlEncodedContent(values);
                var result = await client.PostAsync(Costanti.listaMaterie, content);
                string risposta = await result.Content.ReadAsStringAsync();
                if (risposta.ToString() == "Utente non registrato")
                {
                    await App.Current.MainPage.DisplayAlert("ATTENZIONE", risposta.ToString(), "OK");
                }
                else if (risposta.Contains("Nessuna materia trovata"))
                {
                    await App.Current.MainPage.DisplayAlert("ATTENZIONE", risposta.ToString(), "OK");
                }
                else
                {
                    List<string> jsonContent = JsonConvert.DeserializeObject<List<string>>(risposta);
                    temp.Add("TUTTE LE MATERIE");
                    for(int i=0;i< jsonContent.Count;i++)
                    {
                        temp.Add(jsonContent[i]);
                    }
                    _listamaterie = new List<string>(temp);
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ATTENZIONE", "Errore di connessione con il servizio", "OK");
            }
        }

        public class BindingStatistica
        {
            public string Name { get; set; }
            public double Value { get; set; }
            public BindingStatistica(string name, double value)
            {
                Name = name;
                Value = value;
            }
        }
    }
}
