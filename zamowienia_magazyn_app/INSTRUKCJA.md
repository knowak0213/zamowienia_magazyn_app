# Instrukcja Obsługi Aplikacji "Zamówienia Magazyn"

Dokumentacja zawiera kroki niezbędne do uruchomienia i obsługi systemu zamówień magazynowych.

## Wymagania Techniczne
*   Zainstalowany pakiet **.NET SDK** (wersja 8.0 lub nowsza).
*   Dostęp do lokalnej bazy danych (LocalDB) - standardowo instalowana z Visual Studio.

## Szybki Start (Konsola)

1.  **Otwórz terminal** (Wiersz poleceń, PowerShell lub Terminal w VS Code) w głównym folderze projektu (tam gdzie znajduje się plik `zamowienia_magazyn_app.csproj`).

2.  **Zaktualizuj bazę danych** (wymagane przy pierwszym uruchomieniu):
    Wpisz poniższą komendę, aby utworzyć bazę i tabele:
    ```powershell
    dotnet ef database update
    ```

3.  **Uruchom aplikację**:
    ```powershell
    dotnet run
    ```
    
4.  **Otwórz przeglądarkę**:
    Aplikacja będzie dostępna pod adresem: [http://localhost:5053](http://localhost:5053)

## Konta Użytkowników

System posiada wbudowany mechanizm ról. Przy pierwszym uruchomieniu automatycznie tworzone jest konto administratora.

### Administrator
*   **Login**: `admin@zm.pl`
*   **Hasło**: `Admin123!`
*   **Uprawnienia**:
    *   Zarządzanie produktami (Dodawanie, Edycja, Usuwanie).
    *   Podgląd wszystkich zamówień wszystkich klientów.
    *   Edycja statusów zamówień (np. zmiana na "Wysłane").
    *   Podgląd stanów magazynowych.

### Klient
*   **Rejestracja**: Nowe konto można założyć klikając przycisk **Register** w prawym górnym rogu.
*   **Login**: Adres e-mail podany przy rejestracji.
*   **Uprawnienia**:
    *   Składanie nowych zamówień.
    *   Przeglądanie historii **wyłącznie własnych** zamówień.
    *   Brak dostępu do edycji produktów.

## Główne Funkcje

### 1. Składanie Zamówienia (Jako Klient)
1.  Zaloguj się na konto klienta.
2.  Przejdź do zakładki **Zamówienia** i kliknij **Nowe Zamówienie**.
3.  Z listy wybierz klienta (swoje dane).
4.  Wpisz ilość sztuk przy produktach, które chcesz kupić.
    *   *System automatycznie sprawdzi dostępność towaru. Jeśli zamówisz więcej niż jest w magazynie, otrzymasz komunikat błędu.*
5.  Kliknij **Utwórz**. Ilość produktów w magazynie zostanie automatycznie pomniejszona.

### 2. Realizacja Zamówienia (Jako Administrator)
1.  Zaloguj się na konto `admin@zm.pl`.
2.  Przejdź do zakładki **Zamówienia**. Widzisz tu listę wszystkich zamówień.
3.  Kliknij przycisk **Edytuj** przy wybranym zamówieniu.
4.  Zmień **Status** (np. z *New* na *Shipped*) i zapisz zmiany.

### 3. Zarządzanie Magazynem (Jako Administrator)
1.  Przejdź do zakładki **Produkty**.
2.  Możesz dodać nowy towar (`Create New`) lub edytować istniejący (`Edit`), aby np. ręcznie zwiększyć stan magazynowy (`Stock Quantity`).

## Komendy Pomocnicze

W razie problemów z aplikacją:

*   **Wyczyszczenie projektu** (gdy wystąpią dziwne błędy kompilacji):
    ```powershell
    dotnet clean
    dotnet build
    ```
*   **Dodanie nowej migracji** (jeśli zmienisz model danych):
    ```powershell
    dotnet ef migrations add NazwaZmiany
    dotnet ef database update
    ```
