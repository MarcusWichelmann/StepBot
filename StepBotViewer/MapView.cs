using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace StepBotViewer
{
	public partial class MapView : UserControl
	{
		private Factory factory;
		private RenderTargetProperties renderTargetProperties;
		private HwndRenderTargetProperties hwndRenderTargetProperties;

		private WindowRenderTarget windowRenderTarget;

		private RawColor4 backgroundColor;

		private SolidColorBrush borderColor;
		private SolidColorBrush wayColor;
		private SolidColorBrush currentPositionColor;

		private bool graphicsInitialized = false;

		private Scene scene = null;

		public MapView()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.Opaque, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
		}

		public void ChangeScene(Scene scene)
		{
			this.scene = scene;
			this.scene.SceneUpdated += currentScene_SceneUpdated;
		}

		private void InitializeGraphics()
		{
			factory = new Factory(FactoryType.SingleThreaded, DebugLevel.None);

			renderTargetProperties = new RenderTargetProperties(new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied));

			hwndRenderTargetProperties = new HwndRenderTargetProperties();
			hwndRenderTargetProperties.Hwnd = this.Handle;
			hwndRenderTargetProperties.PixelSize = new SharpDX.Size2(Width, Height);
			hwndRenderTargetProperties.PresentOptions = PresentOptions.None;

			windowRenderTarget = new WindowRenderTarget(factory, renderTargetProperties, hwndRenderTargetProperties);

			backgroundColor = new RawColor4(BackColor.R, BackColor.G, BackColor.B, BackColor.A);

			borderColor = new SolidColorBrush(windowRenderTarget, new RawColor4(0.5f, 0.5f, 0.5f, 1));
			wayColor = new SolidColorBrush(windowRenderTarget, new RawColor4(0, 0, 1, 0.3f));
			currentPositionColor = new SolidColorBrush(windowRenderTarget, new RawColor4(0, 0, 1, 1));

			graphicsInitialized = true;
		}

		private void UnloadGraphics()
		{
			windowRenderTarget?.Dispose();
			factory?.Dispose();

			windowRenderTarget = null;
			factory = null;
		}

		private void DrawScene()
		{
			double scaleFactorX = Width / scene.MapWidth;
			double scaleFactorY = Height / scene.MapHeight;

			double startX = 0;
			double startY = 0;

			if(scaleFactorX > scaleFactorY)
			{
				scaleFactorX = scaleFactorY;
				startX = (Width - (scene.MapWidth * scaleFactorX)) / 2;
			}
			else if(scaleFactorY > scaleFactorX)
			{
				scaleFactorY = scaleFactorX;
				startY = (Height - (scene.MapHeight * scaleFactorY)) / 2;
			}

			double mapWidth = scene.MapWidth * scaleFactorX;
			double mapHeight = scene.MapHeight * scaleFactorY;

			double mapOriginX = startX + (scene.MapOriginX * scaleFactorX);
			double mapOriginY = startY + (scene.MapOriginY * scaleFactorY);

			// Kartenrahmen
			windowRenderTarget.DrawRectangle(new RawRectangleF((float)startX, (float)startY, (float)(startX + mapWidth), (float)(startY + mapHeight)), borderColor);

			lock(scene.Waypoints)
			{
				// Gefahrene Strecke einzeichnen
				double[] lastWaypoint = (double[])scene.Waypoints[0];
				foreach(double[] waypoint in scene.Waypoints.GetRange(1, scene.Waypoints.Count - 1))
				{
					windowRenderTarget.DrawLine(new RawVector2((float)(mapOriginX + (lastWaypoint[0] * scaleFactorX)), (float)(mapOriginY + (lastWaypoint[1] * scaleFactorY))),
						new RawVector2((float)(mapOriginX + (waypoint[0] * scaleFactorX)), (float)(mapOriginY + (waypoint[1] * scaleFactorY))),
						wayColor);

					lastWaypoint = waypoint;
				}

				// Aktuelle Roboterposition
				windowRenderTarget.DrawLine(new RawVector2((float)(mapOriginX + (lastWaypoint[0] * scaleFactorX)), (float)(mapOriginY + (lastWaypoint[1] * scaleFactorY))),
						new RawVector2((float)(mapOriginX + (scene.CurrentPositionX * scaleFactorX)), (float)(mapOriginY + (scene.CurrentPositionY * scaleFactorY))),
						wayColor);
				windowRenderTarget.FillEllipse(new Ellipse(new RawVector2((float)(mapOriginX + (scene.CurrentPositionX * scaleFactorX)),
					(float)(mapOriginY + (scene.CurrentPositionY * scaleFactorY))),
					(float)scaleFactorX, (float)scaleFactorY), currentPositionColor);
			}
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if(!graphicsInitialized || (!DesignMode))
				InitializeGraphics();
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);

			if(graphicsInitialized)
				UnloadGraphics();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if(graphicsInitialized && Width > 0 && Height > 0)
				windowRenderTarget.Resize(new SharpDX.Size2(Width, Height));
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if(!graphicsInitialized)
				return;

			windowRenderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
			windowRenderTarget.BeginDraw();

			try
			{
				windowRenderTarget.Clear(backgroundColor);

				if(scene != null)
					DrawScene();
			}
			finally
			{
				windowRenderTarget.EndDraw();
			}
		}

		private void currentScene_SceneUpdated(EventArgs e)
		{
			Invalidate();
		}
	}
}
