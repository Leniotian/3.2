# Dziennik weryfikacji — Notatnik

## 2026-11-04 — Budowa 2: Notatnik (wpis wsteczny i uczciwy)

- Sprawdzone: `dotnet build` w środowisku z domyślnym szablonem → kompilacja zakończona sukcesem, przyjęcie na wiarę wersji docelowej `<TargetFramework>net10.0</TargetFramework>` w pliku `notatnik.csproj` bez sprawdzenia, czy docelowy zestaw .NET SDK zainstalowany na docelowej maszynie to faktycznie wersja 10, czy 9 (co później skutkowało błędem NETSDK1045 na maszynie z .NET 9 SDK).
- Sprawdzone: filtr nagłówków `line.StartsWith("## ")` w metodzie `LoadNotes()` → przyjęto na wiarę, że każda linia zaczynająca się od `## ` jest tytułem notatki, bez sprawdzenia scenariusza, w którym użytkownik w samej treści wieloliniowej notatki użyje nagłówka drugiego poziomu Markdown (np. `## Krok 1`).
- Sprawdzone: deklaracja agenta o bezpiecznym kodowaniu UTF-8 i odporności na brak pliku `notatki.md` przy pierwszym starcie → sprawdzono pobieżnie przy obecnym pliku, przyjęto na wiarę bez wykonania testu na czystym środowisku z całkowicie usuniętym plikiem `notatki.md`.
- Nie wiadomo: co się stanie, gdy plik `notatki.md` zostanie otwarty i zablokowany do wyłącznego zapisu przez inny program w systemie (np. edytor blokujący uchwyt pliku) w momencie kliknięcia przycisku „Zapisz notatkę”.

---

## 2026-11-19 — Weryfikacja i rozbierz to (Zadanie 3.4)

- Sprawdzone: `dotnet build` → 0 błędów, 0 ostrzeżeń (po usunięciu błędu TargetFramework w `notatnik.csproj` i zmianie z `net10.0` na zainstalowaną wersję SDK `net9.0`).
- Sprawdzone: uruchomienie aplikacji (`dotnet run`) → okno notatnika o wymiarach 700×500 pikseli uruchamia się natychmiast bez wyjątków, poprawnie inicjalizuje interfejs użytkownika i wczytuje istniejącą listę notatek z pliku `notatki.md` do kontrolki `NotesList`.
- Sprawdzone: zapis notatek dokładnie według opisu → po wpisaniu tytułu i treści zawierającej polskie znaki diakrytyczne (`Zażółć gęślą jaźń`) i kliknięciu „Zapisz notatkę”:
  1. nowy wpis trafia na samą górę pliku `notatki.md` ze znacznikiem czasu w formacie `## RRRR-MM-DD HH:mm — Tytuł`,
  2. wcześniejsza zawartość pliku zostaje zachowana w nienaruszonym stanie (brak cichego kasowania danych),
  3. pola tekstowe tytułu i treści zostają wyczyszczone,
  4. kontrolka statusu wyświetla komunikat „Zapisano”,
  5. nowo dodana notatka pojawia się jako pierwszy element na liście `NotesList`.
- Nie wiadomo: jak zachowa się aplikacja przy jednoczesnym dopisywaniu notatek przez dwa równolegle uruchomione procesy lub w przypadku całkowitego zapełnienia pamięci dyskowej.

---

## Sprawdzenie w dokumentacji: nazwa z mojego notatnika

- **Nazwa**: `ObservableCollection<T>` (klasa `ObservableCollection<string>` z przestrzeni nazw `System.Collections.ObjectModel`, użyta w `MainWindow.axaml.cs`: `private readonly ObservableCollection<string> _notes = new();` oraz przypisana do `NotesList.ItemsSource = _notes;`)
- **Link do dokumentacji**: [https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1)
- **Data odczytania**: 19.11.2026
- **Jedno zdanie o tym, co robi**: Reprezentuje dynamiczną kolekcję danych, która implementuje interfejs `INotifyCollectionChanged`, dzięki czemu przy każdym dodaniu, usunięciu lub wstawieniu elementu (np. `_notes.Insert(0, ...)`) automatycznie powiadamia powiązane kontrolki interfejsu (takie jak `ListBox`), co powoduje natychmiastowe odświeżenie widoku na ekranie bez potrzeby ręcznego przeładowywania całej listy.
