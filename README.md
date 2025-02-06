# CSVParser

# Opis projektu

RCP Parser to aplikacja konsolowa w języku C#, której celem jest wczytanie plików CSV zawierających dane o rejestracji czasu pracy (RCP) pracowników w dwóch różnych formatach, a następnie ich przetworzenie i zapis do wynikowych plików CSV w ujednoliconym formacie.

# Funkcjonalności

Parsowanie plików CSV

Obsługuje dwa różne formaty plików RCP:

Firma 1: Kod_pracownika;data;godzina_wejścia;godzina_wyjścia

Firma 2: Kod_pracownika;data;godzina;WE/WY

Konsolidacja danych

Zapewnia, że każdy pracownik ma maksymalnie jeden rekord na dzień.

W przypadku wielu wejść/wyjść wybiera pierwsze wejście i ostatnie wyjście.

Obsługa błędów i walidacja danych

Pomija uszkodzone lub niekompletne wpisy i raportuje je do logów.

Eksport wyników do CSV

Wynikowe pliki CSV są zapisywane w formacie: KodPracownika;Data;GodzinaWejscia;GodzinaWyjscia

# Struktura projektu

📁 CSV_Parser
   📂 ConsoleApplication  (główny projekt aplikacji konsolowej)
       📂 Data        (przechowywanie i obsługa plików CSV)
       📂 Models      (klasy biznesowe, np. DzienPracy)
       📂 Services    (logika przetwarzania CSV)
       📜 Program.cs  (główna aplikacja)

# Instalacja i uruchomienie

Wymagania

.NET 8.0+ (do sprawdzenia wersji: dotnet --version)

Uruchomienie aplikacji

Pobierz repozytorium:

git clone https://github.com/user/repo.git
cd repo

Zbuduj projekt:

dotnet build

Uruchom aplikację (pliki CSV powinny znajdować się w katalogu Data):

dotnet run -- "Data"

Alternatywnie, uruchom aplikację podając własny katalog z plikami CSV:

dotnet run -- "C:\sciezka\do\folderu"

# Przykładowe dane wejściowe

Plik rcp1.csv

Kod_pracownika;data;godzina_wejścia;godzina_wyjścia
1234;2024-02-05;08:00;16:00
5678;2024-02-05;09:15;17:30

Plik rcp2.csv

Kod_pracownika;data;godzina;WE/WY
1234;2024-02-05;08:00;WE
1234;2024-02-05;16:00;WY
5678;2024-02-05;09:15;WE
5678;2024-02-05;17:30;WY

# Wynikowy plik CSV (wynikFirma1.csv lub wynikFirma2.csv)

KodPracownika;Data;GodzinaWejscia;GodzinaWyjscia
1234;2024-02-05;08:00;16:00
5678;2024-02-05;09:15;17:30

# Architektura kodu

Program.cs – Punkt wejściowy aplikacji, odpowiedzialny za iterację po plikach CSV i ich przetwarzanie.

DzienPracy.cs – Model danych reprezentujący pojedynczy dzień pracy pracownika.

CsvParser.cs – Logika przetwarzania plików CSV dla firm 1 i 2.

ZapiszDoCsv.cs – Obsługa eksportu danych do plików wynikowych.

# Obsługa błędów

Niepoprawne formaty danych → Pominięcie wiersza i zapis ostrzeżenia do logów.

Brakujące dane wejściowe → Informacja w konsoli.

Niepoprawna ścieżka do folderu CSV → Wyświetlenie komunikatu błędu i zakończenie programu.
