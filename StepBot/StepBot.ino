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

// Initialisiert das Programm.
void setup ()
{
	// Schrittmotoren initialisieren
	_stepperLeft.Initialize ();
	_stepperRight.Initialize ();

	// Timer initialisieren
	Timer1.initialize ();
	Timer1.attachInterrupt (tick, INTERRUPT_INTERVAL);
}

// Handler der Hauptschleife.
void loop ()
{
	_stepperLeft.AccelerateToSpeed (2500);
	_stepperRight.AccelerateToSpeed (3000);

	delay (1500);

	_stepperLeft.AccelerateToSpeed (3000);
	_stepperRight.AccelerateToSpeed (2500);

	delay (1500);
}

// Handler für einen Tick des Interrupt-Timers
void tick ()
{
	// Geschwindigkeit alle 100ms aktualisieren
	if (_ticksSinceAccUpdate++ >= 1000)
	{
		// Geschwindigkeiten aktualisieren
		_stepperLeft.UpdateSpeed ();
		_stepperRight.UpdateSpeed ();

		// Zähler zurücksetzen
		_ticksSinceAccUpdate = 0;
	}

	// Motoren bei Bedarf drehen
	_stepperLeft.ProcessTick ();
	_stepperRight.ProcessTick ();
}