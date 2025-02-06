using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ConsoleApplication.Models;
using ConsoleApplication.Services;

class Program
{
    static void Main(string[] args)
    {
        string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data");

        List<DzienPracy> dniPracy = new List<DzienPracy>();

        foreach (string file in Directory.GetFiles(folderPath, "*.csv"))
        {
            if (file.Contains("rcp1"))
                dniPracy.AddRange(CsvParser.ParsujRcp1(file));
            else if (file.Contains("rcp2"))
                dniPracy.AddRange(CsvParser.ParsujRcp2(file));
        }

        if (dniPracy.Count == 0)
        {
            Console.WriteLine("Nie znaleziono żadnych danych w plikach CSV.");
            return;
        }

        foreach (var dzien in dniPracy)
        {
            Console.WriteLine(dzien);
        }
    }
}
