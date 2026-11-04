# Runda 1: sama prośba (Zero-shot)

## Wysłana prośba
> Zrób notatnik: pole na tytuł, pole na treść, przycisk zapisu i lista zapisanych notatek. Notatki mają być zapisywane do pliku.

## Wynik na dysku
Model zinterpretował polecenie „zapisywane do pliku” tworząc osobny plik tekstowy dla każdego zapisanego wpisu, z nazwą zawierającą znacznik czasu (`notatka_RRRRMMDD_HHMMSS.txt`).

### Plik: `notatka_20261104_120000.txt`
```
Spotkanie organizacyjne
Omówienie harmonogramu projektu.
```

### Plik: `notatka_20261104_120500.txt`
```
Lista zakupów
Mąka, jajka, mleko.
```

## Obserwacje
1. **Brak formatu**: Tytuł i treść zostały sklejone ze sobą zwykłym znakiem nowej linii bez żadnego nagłówka Markdown ani metadanych.
2. **Wiele plików**: Powstały dwa osobne pliki zamiast jednego wspólnego pliku z notatkami.
3. **Nieokreślone kodowanie**: Zapis używał domyślnego kodowania systemowego.
