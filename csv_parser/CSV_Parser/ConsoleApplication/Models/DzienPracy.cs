using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Models
{
    public class DzienPracy
    {
        public String? KodPracownika { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan GodzinaWejscia { get; set; }
        public TimeSpan GodzinaWyjscia { get; set; }

        public override string ToString()
        {
            return $"Pracownik {KodPracownika}: dnia {Data:yyyy-MM-dd} godziny: {GodzinaWejscia} - {GodzinaWyjscia}\n";
        }
    }
}
