// Headerdatei
#include "StepperMotor.h"

// Arduino-Bibliothek
#include <Arduino.h>

StepperMotor::StepperMotor (unsigned char directionPin, unsigned char stepperPin, unsigned short stepsPerRound, unsigned short maxStepsPerSecond, unsigned short triggerInterval, bool inverted)
{
	// Konfiguration speichern
	_directionPin = directionPin;
	_stepperPin = stepperPin;
	_stepsPerRound = stepsPerRound;
	_maxStepsPerSecond = maxStepsPerSecond;
	_triggerInterval = triggerInterval;
	_inverted = inverted;

	// Geschwindigkeitswerte initialisieren
	_currentStepsPerSecond = 0;
	_wantedStepsPerSecond = 0;
	_stepsPerTick = 0;
	_stepsDone = 0;
}

void StepperMotor::Initialize ()
{
	// Motor-Pins als Ausg�nge setzen
	pinMode (_directionPin, OUTPUT);
	pinMode (_stepperPin, OUTPUT);
}

void StepperMotor::Step ()
{
	// Einen Impuls auf dem Stepper-Pin ausl�sen
	digitalWrite (_stepperPin, HIGH);
	digitalWrite (_stepperPin, LOW);
}

void StepperMotor::SetDirection (bool direction)
{
	// Richtungs-Pin setzen
	digitalWrite (_directionPin, direction == _inverted);
}

void StepperMotor::SetSpeed (short stepsPerSecond)
{
	// Richtung festlegen
	SetDirection (stepsPerSecond >= 0);

	// Wirkliche Geschwindigkeit bestimmen
	unsigned short realStepsPerSecond = abs (stepsPerSecond);

	// Angabe an Maximalgeschwindigkeit anpassen
	if (realStepsPerSecond > _maxStepsPerSecond)
	{
		realStepsPerSecond = _maxStepsPerSecond;
	}

	// Die Gr��e der "virtuellen" Schritte bestimmen
	_stepsPerTick = ((float)realStepsPerSecond * (float)_triggerInterval) / 1000000;

	// Momentangeschwindigkeit merken
	_currentStepsPerSecond = stepsPerSecond;
}

void StepperMotor::AccelerateToSpeed (short stepsPerSecond)
{
	// Zielgeschwindigkeit setzen
	_wantedStepsPerSecond = stepsPerSecond;
}

void StepperMotor::UpdateSpeed ()
{
	// Geschwindigkeitsdifferenz bestimmen
	short pendingDifference = abs (_wantedStepsPerSecond - _currentStepsPerSecond);

	// Geschwindigkeitsifferenz evtl. begrenzen
	if (pendingDifference > 200)
	{
		pendingDifference = 200;
	}

	// Vorzeichen bestimmen
	short accelerationSign = (_wantedStepsPerSecond >= _currentStepsPerSecond ? 1 : -1);

	// Geschwindigkeit um Differenz erh�hen
	SetSpeed (_currentStepsPerSecond + (pendingDifference * accelerationSign));
}

bool StepperMotor::ProcessTick ()
{
	// Hochz�hlen und pr�fen, ob ein Schritt f�llig ist
	if ((_stepsDone += _stepsPerTick) >= 1)
	{
		// Einen Schritt ausf�hren
		Step ();

		// Schritt erledigt. (Z�hler hier nicht direkt auf 0 setzen um Drehung nicht zu st�ren)
		_stepsDone--;

		// Es wurde ein Schritt ausgef�hrt
		return true;
	}

	// Es stand keine Aktion aus
	return false;
}