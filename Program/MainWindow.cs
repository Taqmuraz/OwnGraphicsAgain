using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EnginePart;

namespace OwnGraphicsAgain
{
	public partial class MainWindow : Form
	{
		DrawPanel panel;
		Mesh mesh;
		Texture texture;
		Timer timer;

		private void CreatePanel(int width, int height)
		{
			if (panel != null) panel.Dispose();
			panel = this.AddControl<DrawPanel, int, int>(width, height);
			panel.Bounds = new Rectangle(0, 0, Width, Height);
			panel.SizeMode = PictureBoxSizeMode.StretchImage;
		}

		private void ResizePanel()
		{
			CreatePanel(Width, Height);
			//panel.Bounds = new Rectangle(0, 0, Width, Height);
		}

		public MainWindow()
		{
			InitializeComponent();


			Size = new Size(512, 256);
			CreatePanel(Width, Height);
			SizeChanged += (s, e) => ResizePanel();

			timer = new Timer();
			timer.Tick += (s, e) => Draw();
			timer.Interval = 20;
			timer.Start();
			Disposed += (s, e) => timer.Dispose();

			Timer fpsTimer = new Timer();
			fpsTimer.Tick += (s, e) =>
			{
				Text = $"Fps : {fps.ToString()} min : {fpsMin.ToString()} max : {fpsMax.ToString()}";
			};
			fpsTimer.Interval = 100;
			fpsTimer.Start();

			try
			{
				string path = "F:/BLENDER_MODELS/Soldier.obj";
				mesh = ObjMeshManager.LoadMesh(path);
				texture = new Texture("F:/BLENDER_MODELS/Soldier.png");
				mesh.material = new TextureMaterial(texture);
				//mesh.material = new ColorMaterial(Color.Green);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
			}
		}

		DateTime lastFrameTime;
		int fpsMax;
		int fpsMin;
		int fps;

		private void Draw()
		{
			panel.Clear();

			OnDraw();

			panel.Refresh();

			var now = DateTime.Now;
			fps = (int)(1d / (now - lastFrameTime).TotalSeconds);

			if (fps < fpsMin) fpsMin = fps;
			if (fps > fpsMax) fpsMax = fps;

			lastFrameTime = now;
		}

		protected virtual void OnDraw()
		{
			Vector2 size = panel.imageSize;
			float scale = size.y * 10f;

			Vector3 right = Vector3.right * scale;
			Vector3 up = Vector3.up * scale;
			Vector3 forward = Vector3.forward * scale;

			Matrix4x4 matrix = Matrix4x4.CreateRotationMatrix(new Vector3(0f, 1f, 0f) * (float)DateTime.Now.TimeOfDay.TotalSeconds * 30f);
			matrix *= Matrix4x4.CreateWorldMatrix(right, up, forward, new Vector3(size.x * 0.5f, size.y * 0.25f, 0f));

			panel.DrawMesh(mesh, matrix);
		}
	}
}
