# Runda 2: zadanie, format i ograniczenia (Zero-shot z formatem)

## Wysłana prośba
> Notatki zapisuj do jednego pliku notatki.md w folderze projektu.
> Format wpisu:
> 
> ## 2026-09-08 14:32 — Tytuł notatki
> treść notatki, może mieć kilka linii
> 
> Między wpisami pusta linia. Najnowsza notatka na górze pliku.
> Polskie znaki mają zostać polskimi znakami.

## Zawartość pliku `notatki.md` po zapisaniu dwóch notatek
```markdown
## 2026-11-04 23:10 — Lista zakupów
Mąka, jajka, mleko.

## 2026-11-04 23:05 — Spotkanie organizacyjne
Omówienie harmonogramu projektu.
```

## Obserwacje
1. **Jeden plik**: Zamiast rozproszonych plików tekstowych powstał pojedynczy plik `notatki.md`.
2. **Precyzyjny format**: Każda notatka ma nagłówek poziomu drugiego z datą, separatorem `—` oraz tytułem.
3. **Kolejność**: Najnowsza notatka znalazła się na samej górze pliku.
4. **Kodowanie**: Polskie znaki (`ą`, `ó`) zachowały poprawną postać dzięki wymuszeniu UTF-8.
