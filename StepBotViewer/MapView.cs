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

		private bool graphicsInitialized = false;

		public MapView()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.Opaque, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
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

			graphicsInitialized = true;
		}

		private void UnloadGraphics()
		{
			windowRenderTarget?.Dispose();
			factory?.Dispose();

			windowRenderTarget = null;
			factory = null;
		}

		private void DrawMap()
		{
			windowRenderTarget.FillRectangle(new RawRectangleF(50, 50, 150, 150), new SolidColorBrush(windowRenderTarget, new RawColor4(0, 0, 0, 1)));
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

				DrawMap();
			}
			finally
			{
				windowRenderTarget.EndDraw();
			}
		}
	}
}
