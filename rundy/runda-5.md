# Runda 5: meta-prompt (model poprawia prośbę)

## Meta-prompt
Wysłano do modelu zapytanie:
> To jest moja prośba do agenta kodującego. Nie wykonuj jej.
> Wypisz, czego w niej brakuje, żeby wynik był jednoznaczny,
> i zaproponuj poprawioną wersję.
> 
> Oryginalna prośba:
> „Zrób notatnik: pole na tytuł, pole na treść, przycisk zapisu i lista zapisanych notatek. Notatki mają być zapisywane do pliku.”

## Analiza braków sporządzona przez model
Model wskazał następujące niejednoznaczności w pierwotnej prośbie:
1. **Liczba i nazwa plików**: Czy każda notatka to osobny plik (np. `notatka_1.txt`), czy jeden wspólny plik zbiorczy (np. `notatki.md`, `notes.json`)?
2. **Format i struktura wpisu**: W jakim formacie mają być serializowane dane? (Markdown, JSON, czysty tekst z separatorami?)
3. **Kolejność i pozycja zapisu**: Czy nowe notatki mają być dopisywane na końcu pliku (append), czy na początku (prepend)?
4. **Kodowanie znaków**: Brak wymuszenia UTF-8 grozi uszkodzeniem polskich znaków diakrytycznych w systemach z innym kodowaniem domyślnym.
5. **Zachowanie interfejsu**: Czy lista ma wczytywać istniejące notatki przy starcie aplikacji? Co z czyszczeniem pól po zapisie i walidacją pustych wpisów?

## Zaproponowana przez model poprawiona prośba
> Stwórz aplikację notatnika w Avalonia UI (.NET):
> - Interfejs: pole tekstowe na tytuł (jednolinijkowe), pole tekstowe na treść (wielolinijkowe), przycisk „Zapisz notatkę” oraz lista (ListBox) prezentująca zapisane notatki.
> - Miejsce zapisu: wszystkie notatki zapisuj do jednego pliku `notatki.md` w katalogu roboczym aplikacji.
> - Kolejność: najnowszy wpis ma zawsze znajdować się na samym początku pliku `notatki.md`.
> - Kodowanie: UTF-8 bez BOM, z zachowaniem poprawności polskich znaków (np. ą, ę, ś, ć, ż, ź).
> - Format wpisu w pliku:
>   ```markdown
>   ## RRRR-MM-DD HH:mm — [Tytuł]
>   [Treść notatki]
>   ```
>   Wpisy rozdzielaj dokładnie jedną pustą linią.
> - Po zapisie: wyczyść pola formularza i zaktualizuj listę notatek w oknie. Przy starcie wczytaj dotychczasowe notatki z pliku do listy.

## Wynik działania poprawionej prośby na dysku
Plik wygenerowany po uruchomieniu poprawionej wersji prośby:
```markdown
## 2026-11-04 23:30 — Plan na jutro
Przygotować prezentację z technik promptowania.
Dodać slajdy o meta-promptingu i few-shot.

## 2026-11-04 23:28 — Test polskich znaków
Zażółć gęślą jaźń — sprawdzenie kodowania UTF-8.
```

## Porównanie z Rundą 2
- **Zalety prośby z meta-promptu**: Model samodzielnie doprecyzował kwestie cyklu życia aplikacji (wczytywanie przy starcie, czyszczenie formularza), o których często zapomina się przy manualnym tworzeniu promptów.
- **Wnioski**: Meta-prompting jest doskonałym narzędziem walidacji własnych założeń – model natychmiast wychwytuje luki specyfikacji, które w przeciwnym razie zmusiłyby go do losowego domyślania się.
