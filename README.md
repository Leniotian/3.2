# Notatnik — Zadanie 3.2

Aplikacja notatnika zbudowana w oparciu o framework **Avalonia UI (.NET 10)**, powstała w ramach lekcji *Moduł 3b: Budowa 2 – jak prosić, żeby dostać*.

---

## 1. Działanie aplikacji i spełnienie wymagań

Aplikacja realizuje pełną specyfikację z Rund 1–4 oraz bloku „Własną ręką: obsługa błędu”:
- **Format zapisu**: Notatki zapisywane są do pojedynczego pliku `notatki.md` w katalogu projektu zgodnie ze ścisłym formatem:
  ```markdown
  ## RRRR-MM-DD HH:mm — Tytuł notatki
  treść notatki
  ```
  Każdy wpis oddzielony jest dokładnie jedną pustą linią.
- **Kolejność wpisów**: Najnowsza notatka zawsze trafia na samą górę pliku (`prepend`).
- **Bezpieczeństwo danych**: Aplikacja nie traci dotychczasowych wpisów – po piątym (i każdym kolejnym) zapisie wszystkie wcześniejsze notatki pozostają nienaruszone.
- **Kodowanie znaków**: Wszystkie operacje wejścia/wyjścia korzystają z `UTF-8`, dzięki czemu polskie znaki diakrytyczne (`ą`, `ć`, `ę`, `ł`, `ń`, `ó`, `ś`, `ź`, `ż`) zachowują poprawność.
- **Obsługa błędów (`try-catch`)**: Blok zapisu do pliku ujęty jest w klauzulę `try-catch (Exception ex)`. W przypadku problemów (np. próba zapisu do pliku z atrybutem „tylko do odczytu” lub brak uprawnień) okno aplikacji nie zamyka się awaryjnie, a kontrolka `StatusText` wyświetla czytelny komunikat: `Nie udało się zapisać: {ex.Message}`. Po udanym zapisie pojawia się komunikat `Zapisano`.

---

## 2. Analiza powtórzonego uruchomienia rundy (`runda-1-powtorka.md`)

Jedną rundę (Rundę 1) uruchomiono ponownie z **dokładnie tą samą, jednozdaniową prośbą**:
> *„Zrób notatnik: pole na tytuł, pole na treść, przycisk zapisu i lista zapisanych notatek. Notatki mają być zapisywane do pliku.”*

### Co się różni w powtórzonym wyniku?
- W pierwszym podejściu (`rundy/runda-1.md`) model wygenerował logikę tworzenia **wielu osobnych plików `.txt`** o nazwach ze stemplem czasu (`notatka_20261104_120000.txt`), w których tytuł i treść były połączone zwykłym znakiem nowej linii.
- W powtórzonym podejściu (`rundy/runda-1-powtorka.md`) ten sam model zadecydował o utworzeniu **jednego pliku `notes.json`**, w którym notatki gromadzone były w formie tablicy obiektów JSON z kluczami `id`, `title`, `body` oraz `createdAt`.

### Ocena wpływu niedeterminizmu:
Znaczna część różnic obserwowanych na początku między rundami 1–4 (np. wybór między wieloma plikami a jednym plikiem czy wybór między formatem tekstowym a JSON) wynikała wyłącznie z losowości i niedeterminizmu modelu wypełniającego niedopowiedzenia w prośbie, a dopiero dodanie sztywnego formatu i przykładów (few-shot) trwale zredukowało tę wariancję do zera.

---

## 3. Zalecenie producenta modelu (Google Gemini Prompting Guide)

- **Źródło**: Google Gemini API Documentation — Prompting Strategies  
  [https://ai.google.dev/gemini-api/docs/prompting-strategies](https://ai.google.dev/gemini-api/docs/prompting-strategies)
- **Data odczytania**: 04.11.2026
- **Zalecenie producenta**: Stosowanie strukturalnych znaczników i separatorów (np. tagów XML `<instructions>`, `<format>`, `<examples>`) do jednoznacznego rozdzielenia poleceń systemowych od przykładowych danych wejściowych i treści użytkownika.
- **Wpływ na notatnik**: W notatniku technika ta zagwarantowała, że wpisywana przez użytkownika treść notatek (nawet zawierająca słowa kluczowe typu „Format:” czy składnię Markdown) nigdy nie koliduje z instrukcjami parsowania i zapisu, co trwale zabezpieczyło strukturę pliku `notatki.md` przed uszkodzeniem formatowania.

---

## 4. Spis rund i struktura repozytorium

```
3.1 zadanie/
├── MainWindow.axaml          # Interfejs z polami, przyciskiem i kontrolką StatusText
├── MainWindow.axaml.cs       # Logika zapisu na górę pliku, kodowanie UTF-8, try-catch
├── notatki.md                # Plik wynikowy z notatkami w wymaganym formacie
├── notatnik.csproj           # Plik projektu Avalonia (.NET 10)
├── README.md                 # Wnioski, analiza powtórki i zalecenie Google
└── rundy/
    ├── runda-1.md            # Sama prośba (Zero-shot)
    ├── runda-1-powtorka.md   # Powtórzenie rundy 1 bez zmian w prośbie
    ├── runda-2.md            # Zadanie, format i ograniczenia (Zero-shot z formatem)
    ├── runda-3.md            # Jeden przykład (One-shot)
    ├── runda-4.md            # Trzy zróżnicowane przykłady (Few-shot)
    └── runda-5.md            # Prośba wygenerowana przez model (Meta-prompt)
```
