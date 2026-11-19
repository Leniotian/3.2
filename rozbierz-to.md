# Rozbierz to — analiza fragmentu kodu agenta (Zadanie 3.4)

Poniższa analiza dotyczy fragmentu kodu notatnika wygenerowanego przez asystenta AI z lekcji *3d: Halucynacje, weryfikacja i pierwszy Rozbierz to*:

```csharp
private System.Timers.Timer autosaveTimer;

public NotesWindow()
{
    InitializeComponent();
    autosaveTimer = new System.Timers.Timer(60000);
    autosaveTimer.Elapsed += (s, e) => StatusText.Text = "Zapisano";
    autosaveTimer.Start();
}

// Zapisuje notatkę do pliku wybranego przez użytkownika.
// Obsługa błędów zapisu jest w środku.
private async void SaveButton_Click(object sender, RoutedEventArgs e)
{
    var dialog = new SaveFileDialog();
    dialog.InitialFileName = "notatki.md";
    var path = await dialog.ShowAsync(this);
    File.WriteAllText(path, NoteBox.Text);
}
```

---

## Błąd rozebrany w lekcji (punkt wyjścia)
* **Klasa `SaveFileDialog`**: istniała w starszych wersjach biblioteki Avalonia, w wersji 11 została oznaczona jako przestarzała, a w wersji 12 została całkowicie usunięta (zastąpiona przez `IStorageProvider.SaveFilePickerAsync`).
* **Kształt halucynacji**: *Coś, czego nie ma*.
* **Kiedy się ujawni**: Przy budowaniu projektu (`dotnet build`).

---

## Trzy pozostałe błędy fragmentu

### Błąd 1: Modyfikacja interfejsu użytkownika z pobocznego wątku zegara (`System.Timers.Timer`)

* **Na czym polega**: Zegar `System.Timers.Timer` jest zegarem wielowątkowym z biblioteki standardowej .NET. Jego zdarzenie `Elapsed` jest wywoływane na wątku pobocznym z puli wątków (`ThreadPool`), a nie na głównym wątku interfejsu użytkownika (UI Thread). Próba bezpośredniej modyfikacji kontrolki interfejsu (`StatusText.Text = "Zapisano"`) narusza podstawową zasadę bibliotek okienkowych (w tym Avalonia UI), w których interfejs może być modyfikowany wyłącznie przez wątek, który go utworzył. Skutkuje to natychmiastowym wyrzuceniem wyjątku wielowątkowości `InvalidOperationException` z komunikatem *„Call from invalid thread”*. W bibliotece Avalonia do odmierzania czasu powiązanego z interfejsem służy `Avalonia.Threading.DispatcherTimer` (który wykonuje zdarzenia bezpośrednio na wątku UI) lub zrzutowanie operacji przez `Dispatcher.UIThread.Post(...)`.
* **Kształt halucynacji**: **Coś, co istnieje, użyte źle**. Klasa `System.Timers.Timer` to w pełni poprawna klasa .NET, ale użyta w niewłaściwym kontekście architektonicznym. Kompilator nie zgłasza żadnego błędu ani ostrzeżenia.
* **Kiedy się ujawni**: **Przy uruchomieniu** — dokładnie po upływie 60 sekund od włączenia okna, gdy zegar po raz pierwszy wywoła zdarzenie `Elapsed`, powodując niespodziewane wyłożenie się całej aplikacji.
* **Czym bym to sprawdził**:
  1. Uruchomieniem aplikacji (`dotnet run`) i odczekaniem 60 sekund (lub testowym skróceniem interwału do 1 sekundy), obserwując zachowanie okna i logi w konsoli.
  2. Sprawdzeniem dokumentacji Avalonii w rozdziale *Threading*, która wprost przestrzega przed modyfikacją UI z wątków roboczych i wskazuje `DispatcherTimer` jako właściwe rozwiązanie.
* **Mój notatnik**: Mój notatnik nie ma tego błędu — sprawdziłem to poprzez inspekcję kodu `MainWindow.axaml.cs`, gdzie nie ma żadnego zegara działającego w tle, a cała modyfikacja tekstu statusu `StatusText.Text` oraz kolekcji notatek odbywa się synchronicznie wewnątrz procedury `SaveButton_Click` wywoływanej bezpośrednio na wątku UI Avalonii w odpowiedzi na kliknięcie przycisku przez użytkownika.

---

### Błąd 2: Ciche nadpisywanie pliku i bezpowrotna utrata historii notatek (`File.WriteAllText`)

* **Na czym polega**: Metoda `File.WriteAllText(path, NoteBox.Text)` tworzy nowy plik lub bezwarunkowo nadpisuje cały dotychczasowy plik pod wskazaną ścieżką. Jeżeli w pliku `notatki.md` znajdowały się już wcześniej zapisane notatki, to każde kolejne kliknięcie przycisku zapisu usuwa całą wcześniejszą zawartość, pozostawiając na dysku wyłącznie aktualnie wpisany tekst. Dodatkowo kod nie sprawdza, czy użytkownik nie kliknął przycisku „Anuluj” w oknie dialogowym — w takim wypadku `path` ma wartość `null`, co przy przekazaniu do `File.WriteAllText` rzuca `ArgumentNullException`. Kod działa destrukcyjnie, łamiąc fundamentalne założenie notatnika, w którym notatki mają być trwale gromadzone w jednym pliku z zachowaniem dotychczasowych wpisów.
* **Kształt halucynacji**: **Coś, co istnieje, użyte źle**. Metoda `File.WriteAllText` istnieje i z technicznego punktu widzenia działa bez zarzutu, jednak została użyta niezgodnie z wymaganiami zadania (zamiast dopisywania nowego wpisu z zachowaniem dotychczasowej zawartości pliku).
* **Kiedy się ujawni**: **Dopiero po kilku zapisach** (już przy drugim zapisie) — przy pierwszym uruchomieniu i pierwszym zapisie wszystko wygląda na sprawne, ale dopiero po zapisaniu drugiej notatki użytkownik zauważa, że pierwsza z nich bezpowrotnie zniknęła.
* **Czym bym to sprawdził**:
  1. Testem funkcjonalnym: zapisaniem notatki nr 1, a następnie notatki nr 2, po czym otwarciem pliku `notatki.md` w edytorze tekstu w celu weryfikacji liczby wpisów.
  2. Weryfikacją kodu operacji wejścia/wyjścia pod kątem odczytu istniejącej zawartości przed zapisem lub zastosowania metody dopisywania (`File.AppendAllText`).
* **Mój notatnik**: Mój notatnik nie ma tego błędu — ustaliłem to za pomocą testu wielokrotnego zapisu (zapisano 3 kolejne notatki, a plik `notatki.md` zachował wszystkie wpisy) oraz analizy kodu w `MainWindow.axaml.cs` (linie 61–66), gdzie przed zapisem sprawdzane jest istnienie pliku (`File.Exists`), odczytywana jest cała dotychczasowa treść (`File.ReadAllText`), a nowy wpis dołączany jest na samą górę (`updatedContent = newEntry + "\n" + existingContent"`), dzięki czemu historia notatek nigdy nie ulega skasowaniu.

---

### Błąd 3: Kłamliwy komentarz o obecności obsługi błędów (`// Obsługa błędów zapisu jest w środku.`)

* **Na czym polega**: Komentarz bezpośrednio nad metodą `SaveButton_Click` deklaruje: `// Obsługa błędów zapisu jest w środku.`. W rzeczywistości w ciele metody nie ma ani jednego bloku `try-catch`, ani żadnego mechanizmu wyłapywania wyjątków wejścia/wyjścia (np. plik z atrybutem „tylko do odczytu”, brak uprawnień dyskowych, zablokowanie pliku przez inny program czy wartość `path == null` po anulowaniu okna dialogowego). Komentarz wprowadza programistę w błąd, uśypiając jego czujność i sugerując odporność kodu na błędy, która w rzeczywistości nie istnieje.
* **Kształt halucynacji**: **Zdanie o kodzie, które nie jest prawdą**. Model wygenerował w języku naturalnym obietnicę funkcjonalności, której wygenerowany kod w ogóle nie implementuje. Żaden kompilator ani linter nie analizuje prawdziwości komentarzy.
* **Kiedy się ujawni**: W momencie wystąpienia pierwszej awarii środowiskowej (np. próba zapisu do zablokowanego pliku, brak uprawnień, plik read-only), co zakończy się nieobsłużonym wyjątkiem i natychmiastowym crashem aplikacji; dla człowieka — podczas uważnego czytania kodu źródłowego.
* **Czym bym to sprawdził**:
  1. Inspekcją wzrokową kodu źródłowego — przeczytaniem metody i sprawdzeniem, czy faktycznie zawiera konstrukcję `try { ... } catch { ... }`.
  2. Testem negatywnym: nadaniem plikowi atrybutu tylko do odczytu (`attrib +r notatki.md`) i próbą wykonania zapisu w aplikacji — brak obsługi błędów skutkuje natychmiastowym crashem programu z błędem `UnauthorizedAccessException`.
* **Mój notatnik**: Mój notatnik nie ma tego błędu — ustaliłem to badając kod `MainWindow.axaml.cs` (linie 59–77 oraz 28–42), w którym operacje zapisu i odczytu są w pełni otoczone blokami `try ... catch (Exception ex)` z prezentacją czytelnego komunikatu w kontrolce `StatusText.Text`, a także przeprowadzając test z plikiem tylko do odczytu, w którym aplikacja nie zamknęła się awaryjnie, lecz poprawnie wyświetliła komunikat: *„Nie udało się zapisać: Access to the path ... is denied.”*.
