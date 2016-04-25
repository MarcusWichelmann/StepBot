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

// Z�hlt die Ticks des Timers seit dem letzten Geschwindigkeits-Update
unsigned int _ticksSinceAccUpdate = 0;

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
	_stepperLeft.AccelerateToSpeed (1000);
	_stepperRight.AccelerateToSpeed (1000);

	delay (2000);

	_stepperLeft.AccelerateToSpeed (3000);
	_stepperRight.AccelerateToSpeed (3000);

	delay (2000);
}

// Handler f�r einen Tick des Interrupt-Timers
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

		// Z�hler zur�cksetzen
		_ticksSinceAccUpdate = 0;
	}

	// Motoren bei Bedarf drehen
	_stepperLeft.ProcessTick ();
	_stepperRight.ProcessTick ();
}