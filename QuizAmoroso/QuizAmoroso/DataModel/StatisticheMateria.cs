namespace QuizAmoroso.DataModel
{
    public class StatisticheMateria
    {
        public string username { get; set; }
        public string materia { get; set; }
        public int numerodomande { get; set; }
        public double risposteesatte { get; set; }
        public double rispostesbagliate { get; set; }
        public double domandesenzarisposta { get; set; }
        public double tempototale { get; set; } //Secondi

        public StatisticheMateria()
        {
            username = "";
            materia = "";
            numerodomande = 0;
            risposteesatte = 0;
            tempototale = 0;
            rispostesbagliate = 0;
            domandesenzarisposta = 0;
        }

        public StatisticheMateria(StatisticheMateria stat, string username)
        {
            this.username = username;
            materia = "";
            numerodomande = stat.numerodomande;
            risposteesatte = stat.risposteesatte;
            tempototale = stat.tempototale;
            rispostesbagliate = stat.rispostesbagliate;
            domandesenzarisposta = stat.domandesenzarisposta;
        }

        public StatisticheMateria(StatisticheMateria stat)
        {
            username = stat.username;
            materia = stat.materia;
            numerodomande = stat.numerodomande;
            risposteesatte = stat.risposteesatte;
            tempototale = System.Math.Round(stat.tempototale/60, 2);
            rispostesbagliate = stat.rispostesbagliate;
            domandesenzarisposta = stat.numerodomande - stat.risposteesatte - stat.rispostesbagliate;
        }

    }
}
