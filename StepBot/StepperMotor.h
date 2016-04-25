// Stellt einen Schrittmotor dar.
class StepperMotor
{
private:
	// Der DIR-Pin des Motortreibers
	unsigned char _directionPin;

	// Der STEP-Pin des Motortreibers
	unsigned char _stepperPin;

	// Die Anzahl Schritte pro Umdrehung
	unsigned short _stepsPerRound;

	// Die Maximalgeschwindigkeit
	unsigned short _maxStepsPerSecond;

	// Die Zeit in Mikrosekunden zwischen Interrupt-Ticks
	unsigned short _triggerInterval;

	// Die Invertierung der Drehrichtung
	bool _inverted;

	// Die Momentane Geschwindigkeit.
	short _currentStepsPerSecond;

	// Die Zielgeschwindigkeit der Beschleunigung.
	short _wantedStepsPerSecond;

	// Die Anzahl an "virtuellen" Schritten pro Tick, abhängig von der gewünschten Geschwindigkeit
	float _stepsPerTick;

	// Der Zähler für ausgeführte "virtuelle" Schritte
	float _stepsDone;

	// Löste einen Einzelschritt aus.
	void Step ();

	// Setzt die Drehrichtung des Motors.
	void SetDirection (bool direction);

	// Setzt die Geschwindigkeit des Motors in Schritten pro Sekunde.
	void SetSpeed (short stepsPerSecond);

public:
	// Konstruktor.
	StepperMotor (unsigned char directionPin, unsigned char stepperPin, unsigned short stepsPerRound, unsigned short minStepTime, unsigned short triggerInterval, bool inverted);

	// Initialisiert den Motor.
	void Initialize ();

	// Beginnt eine neue Beschleunigung mit der angegebenen Zielgeschwindigkeit.
	void AccelerateToSpeed (short stepsPerSecond);

	// Wird regelmäßig ausgeführt und aktualisiert die Geschwindigkeit für eine lineare Beschleunigung.
	short UpdateSpeed ();

	// Gibt die aktuelle Geschwindigkeit zurück
	short GetSpeed ();

	// Wird bei jedem Interrupt-Tick ausgelöst und verarbeitet ausstehende Schritte.
	bool ProcessTick ();
};