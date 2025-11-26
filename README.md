# Enkel Felrapportering / Ärendehantering

## Beskrivning
Detta projekt är ett enkelt system för felrapportering och ärendehantering. Användare kan skapa ärenden (t.ex. "Knappen fungerar inte") och administratörer kan se och hantera dessa ärenden genom att ändra deras status (Öppen, Pågående, Löst, Stängd).

Systemet är byggt med C# och använder en SQL Server-databas med tre huvudtabeller: `Users`, `Tickets` och `TicketStatus`.

---

## Flödesschema (beskrivning med ord)

### 1. Start
- Programmet startar.
- Användaren loggar in (om inloggning implementeras).

### 2. Kontrollera användarroll
- Systemet avgör om den inloggade användaren är **vanlig användare** eller **administratör**.

### 3A. User-flöde
1. Visa meny med alternativ:
   - Skapa nytt ärende
   - Visa egna ärenden
   - Logga ut
2. **Skapa nytt ärende**
   - Användaren anger titel och beskrivning.
   - Systemet sparar ärendet med status "Öppen" och kopplar det till användarens ID.
3. **Visa egna ärenden**
   - Systemet hämtar alla ärenden som skapats av användaren.
   - Visar titel, beskrivning, status och datum.
4. **Logga ut**
   - Programmet avslutas eller återgår till inloggningsmeny.

### 3B. Admin-flöde
1. Visa meny med alternativ:
   - Se alla ärenden
   - Ändra ärendestatus
   - Logga ut
2. **Se alla ärenden**
   - Systemet visar alla ärenden med titel, beskrivning, status och skapare.
3. **Ändra ärendestatus**
   - Admin väljer ett ärende.
   - Systemet visar möjliga statusar: Öppen, Pågående, Löst, Stängd.
   - Admin väljer ny status, och systemet uppdaterar databasen.
4. **Logga ut**
   - Programmet avslutas eller återgår till inloggningsmeny.

---

## Databasstruktur

### Users
| Kolumn | Typ | Beskrivning |
|--------|-----|-------------|
| UserID | INT | Primärnyckel, identity |
| Name | NVARCHAR(100) | Användarnamn |
| Email | NVARCHAR(100) | Unik e-postadress |
| IsAdmin | BIT | Anger om användaren är admin (1) eller vanlig användare (0) |

### TicketStatus
| Kolumn | Typ | Beskrivning |
|--------|-----|-------------|
| id | INT | Primärnyckel, identity |
| Name | NVARCHAR(50) | Statusnamn (t.ex. Öppen, Pågående, Löst, Stängd) |

### Tickets
| Kolumn | Typ | Beskrivning |
|--------|-----|-------------|
| id | INT | Primärnyckel, identity |
| Title | NVARCHAR(200) | Titel på ärendet |
| Description | NVARCHAR(MAX) | Beskrivning av problemet |
| UserId | INT | Främmande nyckel till Users(UserID) |
| StatusId | INT | Främmande nyckel till TicketStatus(id) |
| CreatedAt | DATETIME | Skapelsedatum, default = GETDATE() |

---

## Funktioner

### Vanlig användare
- Skapa ärende
- Visa egna ärenden

### Administratör
- Se alla ärenden
- Ändra status på ärende

---

## Flödesschema i korthet (ordform)
1. Start → Logga in  
2. Kontrollera roll (User / Admin)  
3A. User → Meny → Skapa / Visa / Logga ut  
3B. Admin → Meny → Se alla / Ändra status / Logga ut  
4. Slut

---

## Syfte
Detta projekt visar god praxis med:
- **Normalisering**: TicketStatus som uppslagstabell för bättre underhåll.  
- **Tydliga roller**: Olika funktionalitet för användare och administratör.  
- **OOP-struktur**: C#-klasser för användare, ärenden och statusar.  

---

## Grunddata för TicketStatus
```sql
INSERT INTO TicketStatus (Name) VALUES 
('Öppen'),
('Pågående'),
('Löst'),
('Stängd');
