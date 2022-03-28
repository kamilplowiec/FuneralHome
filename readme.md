# Zakład pogrzebowy

## Podstawowe informacje

Rozbudowany program, pozwalający na zarządzanie zakłądem pogrzebowym. Program zawiera edytowalne słowniki działań, listę pojazdów, możliwość wyłączenia z dnia pracy wybranego pojazdu, dodawania osób (pracownik zmarły, ksiądz itp), listę cmentarzy oraz kalendarz.

![Zakład pogrzebowy](https://kamilplowiec.tk/img/portfolio/csdesktop7.jpg)

## Funkcjonalności

1. Dziennik
- Program po uruchomieniu wyświetla dziennik działań zaplanowanych pracowników firmy
- Dwuklik na rekordzie działania pozwala na edycję działania

2. Plan firmy
- Pojazdy
  - Program pozwala na dodawanie i edycję floty pojazdów firmy
  - Dwuklik na rekordzie pojazdu przechodzi do jego edycji

- Pracownicy
  - Program pozwala na dodawanie i edycję pracowników firmy
  - Dwuklik na rekordzie pracownika przechodzi do edycji pracownika
  - Typ pracownika (osoby) jest możliwy do wybrania tylko podczas tworzenia nowej osoby w systemie. Ma to na celu zapobiec przypisywania różnych działan do danej osoby, podczas gdy dana osoba zmieni swój typ według systemu
  - System rozróznia typy osób: Zmarły, Pracownik, Kierowca, Ksiądz i sa to wiodące typy, potrzebne do działania systemu. Możliwe jest dodawanie nowych typów, ale zalecy się, aby nie usuwać już istniejących typów, a zwałaszcza wyszczegołnionych

- Cmentarze
  - Program umożliwia wyświetlanie i edycję cmentarzy, powiązanych i współpracujących z firmą
  - Z cmentarzem ściśle powiązany jest ksiądz, który odpowaida za pochowanie zmarłego
  - Księdza można wybrac podczas dodawanie.edycji cmentarza lub można dodać nowego, jeżeli tego nie będzie na liście

3. Ewidencja zmarłych
- Aplikacja poazwala na wyświetlanie infromacji o popchowanych osobach i miejscach ich pochówku
- Dwuklik na rekordzie pochowanej osoby wyświetla nekrolog danej osoby
  - Nekrolog utworzony przez program może byc zapisany jako obraz i wydrukowany w póxniejszym czasie

4. Kalendarz pogrzebów
- Główne okno programu przedstawia kalendarz do wyboru dni, do szybkiego przeglądu zaplanowanych pogtrzebów na dany dzień
- Po kliknięciu na dany dzień, pogram wyświetli w tabeli pod kalendarzem zaplanowane pogrzeby na dany dzień

5. Menu: Funkcje i Zarządzanie systemem
- Zarządzanie systemem
  - Edycja słownika (typy osób, typy działań)
    - Program przechowuje słowniki w jednym miejscu, więc sa rozróżnione na typy
    - Typy słowników moga być dodawane.edytowane
    - Słowniki mogą być edytowane z tego poziomu, ale nie zaleca się edycji rekordów już istniejących
- Firma
  - Dodaj pojazd - umozliwia dodawanie nowego pojazdu do floty pojazdów firmy
  - Dodaj osobę - możliwośc dodawa nowej osoby, powiązanej z firmą, według typu ze słownika
  - Dodaj cmentarz - dodawanie cmentarza, wraz z wyborem lub  dodaniem księdza w systemie
- Funkcje
  - Funkcja zamknij pozwala na opuszczenie programu
