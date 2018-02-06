using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizAmoroso.DataModel
{
    public class ConcorsiAquistabili
    {
        /// <summary>
        /// Name of the product
        /// </summary>
        public string Titolo { get; set; }

        /// <summary>
        /// Description of the product
        /// </summary>
        public string Descrizione { get; set; }

        /// <summary>
        /// Product ID or sku
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Localized Price (not including tax)
        /// </summary>
        public string Prezzo { get; set; }

        public string codiceControllo { get; set; }

        public string state { get;  set; }
    }
}
