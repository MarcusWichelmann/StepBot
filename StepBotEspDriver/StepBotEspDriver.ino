// WLAN-Bibliothek für den ESP8266
#include <ESP8266WiFi.h>

// SSID des zu erstellenen AccessPoints
#define SSID "StepBot"

// Zu setzendes Passwort
#define KEY "StepBot123"

// Der Port, auf dem der Steuerungsserver läuft
#define CONTROL_SERVER_PORT 8888

// Steuerungs-Server
WiFiServer _controlServer (CONTROL_SERVER_PORT);

// Der aktuell verbundene Steuerungs-Server Client
WiFiClient _controlClient;

// Initialisiert das Programm
void setup ()
{
	// Serielle Schnittstelle initialisieren
	Serial.begin (115200);

	// Eigene IP und Subnet-Maske festlegen
	IPAddress localIp (192, 168, 50, 1);
	IPAddress subnetMask (255, 255, 255, 0);

	// AccessPoint einrichten
	WiFi.softAPConfig (localIp, localIp, subnetMask);
	WiFi.softAP (SSID, KEY);

	// TCP-Server starten
	_controlServer.begin ();
}

// Handler der Hauptschleife
void loop ()
{
	// Gibt es neue Verbindungsanfragen?
	if (_controlServer.hasClient ())
	{
		// Akutelle Verbindung ggf. beenden
		if (_controlClient)
			_controlClient.stop ();

		// Neue Verbindung annehmen
		_controlClient = _controlServer.available ();
	}

	// Ist ein Client verbunden?
	if (!_controlClient || !_controlClient.connected ())
		return;

	// Alle am Serial-Port anliegende Daten weiterleiten
	int serialBytesAvailable = Serial.available ();
	if (serialBytesAvailable > 0)
	{
		// Array für die empangenen Daten
		uint8_t* data = new uint8_t[serialBytesAvailable];

		// Alle Daten aus Stream lesen
		Serial.readBytes (data, serialBytesAvailable);

		// Daten an den Clienten weiterleiten
		_controlClient.write (const_cast<uint8_t*>(data), serialBytesAvailable);

		// Speicher freigeben
		delete[] data;
	}

	// Alle am Steuerungs-Server anliegende Daten weiterleiten
	int clientBytesAvailable = _controlClient.available ();
	if (clientBytesAvailable > 0)
	{
		// Array für die empangenen Daten
		uint8_t* data = new uint8_t[clientBytesAvailable];

		// Alle Daten aus Stream lesen
		_controlClient.read (data, clientBytesAvailable);

		// Daten an die serielle Schnittstelle weiterleiten
		Serial.write (const_cast<uint8_t*>(data), clientBytesAvailable);

		// Speicher freigeben
		delete[] data;
	}
}