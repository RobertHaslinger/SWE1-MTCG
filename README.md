# SWE1-MTCG
Monster Trading Card Game with REST HTTP-based WebService and NUnit3 Unit Tests.

## Allgemeines

Dies ist das MTCG (Monster Trading Card Game) von Robert Haslinger.

Link zu GitHub: https://github.com/RobertHaslinger/SWE1-MTCG

## Setup

### Datenbank

Für dieses Projekt wurde PostgreSQL lokal installiert. Als GUI habe ich pgAdmin 4 verwendet.

Der Connection String ist im [DB Singleton](https://github.com/RobertHaslinger/SWE1-MTCG/blob/master/SWE1-MTCG/Database/PostgreSQLSingleton.cs) zu finden und kann bei Bedarf geändert werden.
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
  
Um diese Datenbank zu reproduzieren muss einfach die Datei [DB Create](https://github.com/RobertHaslinger/SWE1-MTCG/blob/master/db_create.sql) ausgeführt werden.

In diesem Projekt wurde weniger mit Relationen gearbeitet, dafür umso mehr mit serialisierten JSONs in einer Tabelle. Dies dient dem Zweck einer einfacheren Handhabung mit den Daten.

### Aufbau der Applikation

* Es gibt einen [Server](https://github.com/RobertHaslinger/SWE1-MTCG/blob/master/SWE1-MTCG/Server/WebServer.cs), welcher asynchron Verbindungen annimmt und diese in einem neuen Thread bearbeitet (mit einer Unterscheidung zwischen anonymous und known Clients)
* Die Klasse [RequestContext](https://github.com/RobertHaslinger/SWE1-MTCG/blob/master/SWE1-MTCG/Server/RequestContext.cs) hilft bei der Verarbeitung von eingehenden Requests
* Danach entscheidet der [ApiService](https://github.com/RobertHaslinger/SWE1-MTCG/blob/master/SWE1-MTCG/Services/ApiService.cs) mit welchem Endpunkt die Request bearbeitet wird
* Die entsprechende [Api](https://github.com/RobertHaslinger/SWE1-MTCG/tree/master/SWE1-MTCG/Api) bekommt dann die Request und den eingeloggten Client falls verfügbar, und ruft je nach [Http Methode](https://github.com/RobertHaslinger/SWE1-MTCG/blob/master/SWE1-MTCG/Enums/HttpMethod.cs) die entsprechende Methode eines [Controllers](https://github.com/RobertHaslinger/SWE1-MTCG/tree/master/SWE1-MTCG/Controller) auf
* Der Controller steht in Verbindung mit den [Services](https://github.com/RobertHaslinger/SWE1-MTCG/tree/master/SWE1-MTCG/Services), welche mit der DB kommunizieren. Außerdem werden in dem Controller etliche Exceptions abgefangen und mit wertvollen Informationen an die Api zurückgeschickt
* Die Api bildet dann aus dem Ergebnis (KeyValuePair<[StatusCode](https://github.com/RobertHaslinger/SWE1-MTCG/blob/master/SWE1-MTCG/Enums/StatusCode.cs), object>) eine [Response](https://github.com/RobertHaslinger/SWE1-MTCG/blob/master/SWE1-MTCG/Server/RequestContext.cs) und schickt diese über den Server zurück an den Client

### User Management

Der [Client Singleton](https://github.com/RobertHaslinger/SWE1-MTCG/blob/master/SWE1-MTCG/Server/ClientMapSingleton.cs) ist threadsafe und speichert eingeloggte Clients in memory.

Bei der Verarbeitung einer Request wird mithilfe dieses Singletons entschieden, ob es sich um einen anonymen oder eingeloggten Client handelt. Dafür haben die Apis die Eigenschaft `AllowAnonymous`, welche die Zugänglichkeit beschreibt.

## Tests

### Unit Tests

Im [Test Projekt](https://github.com/RobertHaslinger/SWE1-MTCG/tree/master/SWE1-MTCG.Test) sind über 50 Unit Tests vorhanden. Ein paar wenige wurden auskommentiert, da sie mit Fortschreitung des Projekts deprecated wurden.

Im Fokus der Unit Tests lagen für mich vor allem das Zusammenspiel der Elemente und Kartentypen, welche in den Testklassen reichlich ausgetestet wurden.

Ebenso wurden Tests für das komplexe deserialisieren von Karten erstellt und auch für die Funktionalität der Datenbank.

### Integration Tests

Im Verlauf des Projekts wurden die Funktionalität der Apis mit Postman getestet ([Vollständige Postman Dokumentation](https://documenter.getpostman.com/view/13224957/TVzREcYQ)).

Einzelne Apis und Funktionalitäten wurden auch mit curl getestet.

Unterschiede zu dem vorgegebenen Skript:

* In diesem Projekt werden zuerst einzelne Karten erstellt (Name, CardType, Damage, Element) und als Ergebnis bekommt man die Id der erstellten Karte
* Beim Erstellen von Packages werden nur die erzeugten Card Ids mitgegeben
* Mit /transactions/packages kann man Packages erwerben. Mit /packages/open kann man ein Package öffnen
* Mit POST /deck kann man sein Deck das 1. Mal konfigurieren. Danach sollte man PUT verwenden.
* Die User Data wird mit einem Query Parameter (?username=) statt mit einer Resource erreicht, somit kann man sich auch Profile anderer Nutzer ansehen
* Beim Editieren des Profils muss man keine Resource angeben, da man nur das eigene Profil bearbeiten kann
* Bei /tradings kann man ebenso mit einem Query Parameter (?username=) offene Deals nach Benutzer filtern
* Beim Erstellen eines Trades gibt man zusätzlich das Element der Karte, welche man erhalten will, mit

## Pitfalls

Wirklich enorme Pitfalls gab es nicht. Ein paar komplexere Themen waren:

* das Erstellen des Request Flows (Server -> Api -> Controller -> Service und wieder retour)
* das Speichern von etlichen Daten in der Datenbank (z.B.: User Statistiken)
* die Battle Logik und Synchronisation von zwei Clients
* das Traden und somit das Tauschen zweier Karten

## Lessions Learned

* TDD ist sehr gut und wichtig, um einen "roten Faden" für das Projekt zu erstellen, wenn man ganz an Anfang steht. Jedoch werden manche Tests deprecated sobald eine gewisse Komplexität erreicht wird, bzw. manche Klassen geändert werden müssen.

* Für dieses Projekt und meinen individuellen Stil hätte eine dokumentenbasierte NoSql Datenbank (z.B.: MongoDB) vieles erleichert/übersichtlicher gemacht.

* Mit Branches zu arbeiten erleichtert die Aufgabenteilung in mehrere abgrenzbare Bereiche und Bugs/neue Features lassen sich leichter ausbessern und implementieren.

* **Last but not least**: Ein vollautomatisches curl-Skript für Integration Tests lässt sich nicht leicht erzeugen, wenn man Ids einzigartig zur Runtime erzeugt :) 
