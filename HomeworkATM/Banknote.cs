using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkATM
{
    public class Banknote
    {
        public string Nomimal { get; private set; }
        public string Series { get; private set; }
        public Banknote(string nominal, string series)
        {
            this.Nomimal = nominal;
            this.Series = series;
        }

        public string ToString()
        {
            return $"Номинал: {Nomimal}, Серия: {Series}";
        }
    }
}
