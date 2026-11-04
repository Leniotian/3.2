# Runda 4: trzy przykłady różniące się między sobą (Few-shot)

## Wysłana prośba
> Notatki zapisuj do jednego pliku notatki.md w folderze projektu.
> Najnowsza notatka ma zawsze trafiać na górę pliku.
> Polskie znaki mają pozostać polskimi znakami (kodowanie UTF-8).
> Format wpisu i struktura pliku mają wyglądać dokładnie tak jak w poniższych przykładach:
> 
> ## 2026-11-04 23:25 — Zakupy - na weekend
> mleko
> chleb
> jaja wiejskie
> 
> ## 2026-11-04 23:20 — Krótka myśl
> Sprawdzić działanie Avalonii.
> 
> ## 2026-11-04 23:15 — Żółć i gęśl — test polskich znaków
> Zażółć gęślą jaźń.
> Druga linia treści z polskimi znakami: ą, ć, ę, ł, ń, ó, ś, ź, ż.

## Zawartość pliku `notatki.md` po zapisaniu
```markdown
## 2026-11-04 23:25 — Zakupy - na weekend
mleko
chleb
jaja wiejskie

## 2026-11-04 23:20 — Krótka myśl
Sprawdzić działanie Avalonii.

## 2026-11-04 23:15 — Żółć i gęśl — test polskich znaków
Zażółć gęślą jaźń.
Druga linia treści z polskimi znakami: ą, ć, ę, ł, ń, ó, ś, ź, ż.
```

## Obserwacje
1. **Trzy zróżnicowane przykłady**: Pokazanie wpisu jednolinijkowego, wielolinijkowego oraz wpisu z myślnikami i polskimi znakami usunęło wszelkie wątpliwości modelu odnośnie elastyczności treści.
2. **Niezmiennik struktury**: Model jasno zidentyfikował stałe elementy: nagłówek `## RRRR-MM-DD HH:mm — Tytuł`, treść bezpośrednio pod spodem i dokładnie jedną pustą linię separatora między wpisami.
3. **Kolejność**: Wpisy są ułożone malejąco chronologicznie (najnowsze na samej górze).
