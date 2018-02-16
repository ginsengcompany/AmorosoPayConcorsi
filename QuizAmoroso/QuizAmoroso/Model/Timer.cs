using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace QuizAmoroso.Model
{
    public class Timer
    {
        // Timer tempo totale simulazione
        private Stopwatch tempoGlobale = new Stopwatch();
        private TimeSpan tempo;
        public string tempoTotale;
        private bool avviaTempo;
        private string nomeCampo = "";


        /**
         * TEMPO TRASCORSO IN MODALITA SIMULAZIONE
         * */
        public void AvvioTempo (bool start , string campo)
        {
            nomeCampo = campo;
            avviaTempo = start;
            Device.StartTimer(TimeSpan.FromSeconds(0), () =>
            {
                tempoGlobale.Start();
                tempo = tempoGlobale.Elapsed;
                tempoTotale = string.Format("{0:00}:{1:00}:{2:00}", tempo.Hours, tempo.Minutes, tempo.Seconds);
                return avviaTempo;
            });
        }

        public void FermaTempo()
        {
            tempoGlobale.Stop();
        }

        public void ResetTempo()
        {
            avviaTempo = false;
            tempoGlobale.Reset();
        }

        public void RestartTempo()
        {
            avviaTempo = true;
            tempoGlobale.Restart();
        }
    }
}
