// Interruptbasierte Timer-Bibliothek
#include <TimerOne.h>

// Motor-Bibliothek
#include "StepperMotor.h"

// Intervall des Interrupt-Timers in Mikrosekunden
#define INTERRUPT_INTERVAL 100

// Linker Schrittmotor
StepperMotor _stepperLeft (7, 6, 800, 5000, INTERRUPT_INTERVAL, true);

// Rechter Schrittmotor
StepperMotor _stepperRight (12, 11, 800, 5000, INTERRUPT_INTERVAL, false);

// Zählt die Ticks des Timers seit dem letzten Geschwindigkeits-Update
unsigned int _ticksSinceAccUpdate = 0;

// Gibt an, welcher Teil des Befehles gerade empfangen wird
int commandPart = 0;

// Zwischenspeicher für eingehende Befehle
String command = "";
String arg0 = "";
String arg1 = "";

// Initialisiert das Programm
void setup ()
{
	// Serielle Schnittstelle initialisieren
	Serial.begin (115200);

	// Schrittmotoren initialisieren
	_stepperLeft.Initialize ();
	_stepperRight.Initialize ();

	// Timer initialisieren
	Timer1.initialize ();
	Timer1.attachInterrupt (tick, INTERRUPT_INTERVAL);

	// Statusmeldung
	Serial.println ("INIT");
}

// Handler der Hauptschleife
void loop ()
{
	// Nichts tun.
}

// Handler für eingehende Daten an der Seriellen Schnittstelle
void serialEvent ()
{
	// Lesen, solange Daten vorhanden sind
	while (Serial.available ())
	{
		// Nächstes Zeichen aus dem Stream lesen
		char c = (char)Serial.read ();

		// Zeilenende?
		if (c == '\n')
		{
			// Befehl verarbeiten
			computeCommand ();

			// Neuer Befehl beginnt
			commandPart = 0;
			command = "";
			arg0 = "";
			arg1 = "";

		}
		else if (c == ',')
		{
			// Nächster Teil wird eingeleitet
			commandPart++;
		}
		else if (c != '\r')
		{
			// Zeichen zwischenspeichern
			switch (commandPart)
			{
				case 0: command += c; break;
				case 1: arg0 += c; break;
				case 2: arg1 += c; break;
			}
		}
	}
}

void computeCommand ()
{
	// Befehl untersuchen
	if (command == "SACC")
	{
		// Geschwindigkeit beschleunigt ändern
		_stepperLeft.AccelerateToSpeed (arg0.toInt ());
		_stepperRight.AccelerateToSpeed (arg1.toInt ());
	}
}

// Handler für einen Tick des Interrupt-Timers
void tick ()
{
	// Geschwindigkeit alle 100ms aktualisieren
	if (_ticksSinceAccUpdate++ >= 1000)
	{
		// Geschwindigkeiten aktualisieren und protokollieren
		Serial.print ("SPED,");
		Serial.print (_stepperLeft.UpdateSpeed ());
		Serial.print (",");
		Serial.println (_stepperRight.UpdateSpeed ());

		// Zähler zurücksetzen
		_ticksSinceAccUpdate = 0;
	}

	// Motoren bei Bedarf drehen
	_stepperLeft.ProcessTick ();
	_stepperRight.ProcessTick ();
}