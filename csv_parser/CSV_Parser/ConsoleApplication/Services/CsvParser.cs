using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApplication.Services
{
    public static class CsvParser
    {
        public static List<DzienPracy> ParsujRcp1(string filePath)
        {
            var dniPracyDict = new Dictionary<(string, DateTime), DzienPracy>();

            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split(';');
                string kod = parts[0];
                DateTime data = DateTime.ParseExact(parts[1], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                TimeSpan godzinaWejscia = TimeSpan.Parse(parts[2]);
                TimeSpan godzinaWyjscia = TimeSpan.Parse(parts[3]);

                var key = (kod, data);

                if (!dniPracyDict.ContainsKey(key))
                {
                    dniPracyDict[key] = new DzienPracy
                    {
                        KodPracownika = kod,
                        Data = data,
                        GodzinaWejscia = godzinaWejscia,
                        GodzinaWyjscia = godzinaWyjscia
                    };
                }
                else
                {
                    Console.WriteLine($"[UWAGA] Pominięto duplikat dla pracownika {kod} w dniu {data:yyyy-MM-dd}");
                }
            }

            var dniPracy = dniPracyDict.Values.ToList();
            var outputPath = "wynikFirma1.csv";
            string firstLine = "Firma 1\n\nKodPracownika;Data;GodzinaWejscia;GodzinaWyjscia\n";
            ZapiszDoCsv(dniPracy, outputPath, firstLine);

            return dniPracy;
        }

        public static List<DzienPracy> ParsujRcp2(string filePath)
        {
            var entries = new Dictionary<(string, DateTime), (TimeSpan?, TimeSpan?)>();

            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split(';');
                if (parts.Length < 4 || string.IsNullOrWhiteSpace(parts[2]))
                {
                    Console.WriteLine($"Pominięto niepoprawny wiersz: {line}");
                    continue;
                }

                string kod = parts[0];
                if (!DateTime.TryParseExact(parts[1], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data))
                {
                    Console.WriteLine($"Niepoprawna data w wierszu: {line}");
                    continue;
                }

                if (!TimeSpan.TryParse(parts[2], out TimeSpan godzina))
                {
                    Console.WriteLine($"Niepoprawny format godziny w wierszu: {line}");
                    continue;
                }

                string weWy = parts[3];
                var key = (kod, data);

                if (!entries.ContainsKey(key))
                {
                    entries[key] = (null, null);
                }

                if (weWy == "WE" && entries[key].Item1 == null)
                {
                    entries[key] = (godzina, entries[key].Item2);
                }
                else if (weWy == "WY")
                {
                    entries[key] = (entries[key].Item1, godzina);
                }
            }

            var dniPracy = entries
            .Where(e => e.Value.Item1 != null && e.Value.Item2 != null)
            .Select(e => new DzienPracy
            {
                KodPracownika = e.Key.Item1,
                Data = e.Key.Item2,
                GodzinaWejscia = e.Value.Item1.Value,
                GodzinaWyjscia = e.Value.Item2.Value
            })
            .ToList();

            var outputPath = "wynikFirma2.csv";
            string firstLine = "Firma 2\n\nKodPracownika;Data;GodzinaWejscia;GodzinaWyjscia\n";
            ZapiszDoCsv(dniPracy, outputPath, firstLine);

            return dniPracy;
        }

        public static void ZapiszDoCsv(List<DzienPracy> dniPracy, string outputPath, string firstLine)
        {
            try
            {
                using StreamWriter writer = new(outputPath);
                writer.WriteLine(firstLine);
                foreach (var dzien in dniPracy)
                {
                    writer.WriteLine($"Pracownik {dzien.KodPracownika}: dnia {dzien.Data:yyyy-MM-dd} godziny: {dzien.GodzinaWejscia} - {dzien.GodzinaWyjscia}\n");
                }

                Console.WriteLine($"Plik zapisano poprawnie: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd zapisu do pliku {outputPath}: {ex.Message}");
            }
        }


    }
}
