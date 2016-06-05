using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace StepBotViewer
{
	public class Scene
	{
		private const double WheelScope = Math.PI * 6.0;
		private const double WheelDistance = 14.8;

		public double CurrentPositionX { get; private set; }
		public double CurrentPositionY { get; private set; }
		public double CurrentDirection { get; private set; }

		public double RpmLeft { get; private set; }
		public double RpmRight { get; private set; }

		public double MapOriginX { get; private set; } = 50;
		public double MapOriginY { get; private set; } = 50;

		public double MapWidth { get; private set; } = 100;
		public double MapHeight { get; private set; } = 100;

		public ArrayList Waypoints = new ArrayList();

		private Timer timer = new Timer(100);

		private Stopwatch stopWatch = new Stopwatch();

		public event SceneUpdatedEventHandler SceneUpdated;

		public delegate void SceneUpdatedEventHandler(EventArgs e);

		public Scene(double startPositionX = 0, double startPositionY = 0, double startDirection = 0)
		{
			CurrentPositionX = startPositionX;
			CurrentPositionY = startPositionY;
			CurrentDirection = startDirection;

			Waypoints.Add(new double[] { 0, 0 });

			stopWatch.Start();

			timer.Elapsed += timer_Elapsed;
			timer.Start();
		}

        ~Scene () {

        }

		public void Stop()
		{
			timer.Stop();
		}

		public void ChangeSpeed(double rpmLeft, double rpmRight)
		{
			// Vorherige Bewegung beenden
			UpdatePosition();

			// Neue Geschwindigkeitswerte übernehmen
			RpmLeft = rpmLeft;
			RpmRight = rpmRight;

			// Die Szene wurde aktualisiert
			SceneUpdated?.Invoke(new EventArgs());
		}

		private void UpdatePosition()
		{
			// Verstrichene Zeit seit letzter Positionsaktualisierung bestimmen
			long elapsedTime = stopWatch.ElapsedMilliseconds;
			stopWatch.Restart();

			lock(Waypoints)
			{
				// Aktuelle Position als Wegpunkt eintragen
				Waypoints.Add(new double[] { CurrentPositionX, CurrentPositionY });
			}

			// Zurückgelegten Weg für diese Zeitspanne berechnen
			double wayLeft = SpeedToDistance(RpmLeft, elapsedTime);
			double wayRight = SpeedToDistance(RpmRight, elapsedTime);

			// Relative Verschiebung und Drehung der Pose berechnen
			double alpha = 0;
			double x = 0;
			double y = 0;

			if(wayLeft == wayRight)
			{
				alpha = 0;
				x = 0;
				y = wayLeft;
			}
			//else if((Math.Abs(wayLeft) == wayLeft) == (Math.Abs(wayRight) == wayRight) || wayLeft == 0 || wayRight == 0)
			else
			{
				double r = ((-wayLeft) * WheelDistance) / (wayLeft - wayRight);

				if(wayLeft != 0)
					alpha = DegreeToRadian((180.0 * wayLeft) / (Math.PI * r));
				else
					alpha = DegreeToRadian((180.0 * wayRight) / (Math.PI * (r + WheelDistance)));

				y = Math.Sin(alpha) * (r + (WheelDistance / 2));
				x = Math.Cos(alpha) * (r + (WheelDistance / 2)) - (r + (WheelDistance / 2));
			}

			// Verschiebung anhand aktueller Drehung durchführen
			CurrentPositionX += (Math.Cos(CurrentDirection) * x) - (Math.Sin(CurrentDirection) * y);
			CurrentPositionY += (Math.Cos(CurrentDirection) * y) + (Math.Sin(CurrentDirection) * x);

			// Neue Drehung übernehmen
			CurrentDirection += alpha;

			// Kartenausmaße ggf. aktualisieren
			if(CurrentPositionX < -MapOriginX)
				MapOriginX = -CurrentPositionX;

			if(CurrentPositionY < -MapOriginY)
				MapOriginY = -CurrentPositionY;

			if(CurrentPositionX > (MapWidth - MapOriginX))
				MapWidth = CurrentPositionX + MapOriginX;

			if(CurrentPositionY > (MapHeight - MapOriginY))
				MapHeight = CurrentPositionY + MapOriginY;
		}

		private double SpeedToDistance(double rpm, long milliseconds)
		{
			double wayPerMinute = rpm * WheelScope;
			double minutes = milliseconds / 1000.0 / 60.0;

			return wayPerMinute * minutes;
		}

		private double DegreeToRadian(double angle)
		{
			return Math.PI * angle / 180.0;
		}

		private void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			UpdatePosition();

			SceneUpdated?.Invoke(new EventArgs());
		}
	}
}
