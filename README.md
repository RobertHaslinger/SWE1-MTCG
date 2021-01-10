# SWE1-MTCG
Monster Trading Card Game with REST HTTP-based WebService and NUnit3 Unit Tests.

## Allgemeines

Dies ist das MTCG (Monster Trading Card Game) von Robert Haslinger.

Link zu GitHub: https://github.com/RobertHaslinger/SWE1-MTCG

## Setup

### Datenbank

Für dieses Projekt wurde PostgreSQL lokal installiert. Als GUI habe ich pgAdmin 4 verwendet.

Der Connection String ist unter SWE1_MTCG/Database/PostgreSQLSingleton.cs zu finden und kann bei Bedarf geändert werden.
Diese Klasse enthält die Verbindung zur Datenbank. 

Die Datenbank für dieses Projekt hat folgende Eigenschaften:

* Host: localhost

* Database: mtcg

* Schema: mtcg

* User: swe1_mtcg

* Tables:
  * User
  * Card
  * Package
  * PackageType
  * Transactions
  * TradingDeal
