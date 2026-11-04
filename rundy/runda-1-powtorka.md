# Runda 1 (powtórka): to samo polecenie bez żadnych zmian

## Wysłana prośba (identyczna jak w Rundzie 1)
> Zrób notatnik: pole na tytuł, pole na treść, przycisk zapisu i lista zapisanych notatek. Notatki mają być zapisywane do pliku.

## Wynik na dysku w powtórzonym uruchomieniu
Przy ponownym wysłaniu dokładnie tej samej prośby model zinterpretował zapis zupełnie inaczej: zamiast tworzyć osobne pliki `.txt` dla każdej notatki, wygenerował jeden plik `notes.json` zawierający tablicę obiektów:

### Plik: `notes.json`
```json
[
  {
    "id": 1,
    "title": "Spotkanie organizacyjne",
    "body": "Omówienie harmonogramu projektu.",
    "createdAt": "2026-11-04T12:00:00Z"
  },
  {
    "id": 2,
    "title": "Lista zakupów",
    "body": "Mąka, jajka, mleko.",
    "createdAt": "2026-11-04T12:05:00Z"
  }
]
```

## Porównanie z pierwszym uruchomieniem (runda-1.md)
| Cecha | Runda 1 (pierwsze uruchomienie) | Runda 1 (powtórka) |
| :--- | :--- | :--- |
| **Liczba plików** | Osobny plik na każdą notatkę (`notatka_*.txt`) | Jeden plik zbiorczy (`notes.json`) |
| **Format danych** | Zwykły tekst, tytuł w 1. linii, treść w 2. | Format JSON z kluczami (`id`, `title`, `body`, `createdAt`) |
| **Nazwa pliku** | Losowa/czasowa nazwa pliku | Stała nazwa `notes.json` |
| **Wymuszenie z prośby** | Żadne – model sam zadecydował | Żadne – model sam zadecydował |

## Wniosek
Brak formatu i ograniczeń w prośbie (zero-shot) sprawia, że model losuje jedną z wielu prawdopodobnych interpretacji z przestrzeni wag. Ta sama prośba daje dwa fundamentalnie różne systemy plików na dysku.
