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
		Mesh model;
		Mesh flat;
		Timer timer;

		private void CreatePanel(int width, int height)
		{
			if (panel != null) panel.Dispose();
			panel = this.AddControl<DrawPanel, int, int>(width, height);
			panel.Bounds = new Rectangle(0, 0, Width, Height);
			panel.SizeMode = PictureBoxSizeMode.StretchImage;
			panel.MouseMove += OnMouseMove;

			panel.Paint += OnPanelPaint;
		}

		private void OnPanelPaint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawString($"Model translation : {modelMatrix.column_3}", Font, Brushes.White, new PointF());
		}

		private void ResizePanel()
		{
			//CreatePanel(Width, Height);
			panel.Bounds = new Rectangle(0, 0, Width, Height);
		}

		public MainWindow()
		{
			InitializeComponent();


			Size = new Size(280, 180);
			CreatePanel(Width, Height);
			SizeChanged += (s, e) => ResizePanel();

			timer = new Timer();
			timer.Tick += (s, e) => Draw();
			timer.Interval = 10;
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
				model = ObjMeshManager.LoadMesh(path);
				path = "F:/BLENDER_MODELS/FlyStation.obj";

				flat = ObjMeshManager.LoadMesh(path);
				Texture texture = new Texture("F:/BLENDER_MODELS/Soldier.png");
				model.material = new TextureMaterial(texture);
				flat.material = new ColorMaterial(Color.Gray);


				float scale = 5f;

				Vector3 right = Vector3.right * scale;
				Vector3 up = Vector3.up * scale;
				Vector3 forward = Vector3.forward * scale;

				modelMatrix = Matrix4x4.CreateWorldMatrix(right, up, forward, Vector3.zero);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
		}

		private Vector2 lastMouseLocation;
		private bool rotate;
		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			Vector2 location = new Vector2(-e.Y, e.X);
			if (rotate)
			{
				controlRotation += (Vector3)(location - lastMouseLocation) * 0.5f;
			}
			lastMouseLocation = location;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			float rot = 10f;

			switch (e.KeyCode)
			{
				case Keys.R:
					rotate = true;
					break;

				case Keys.Up: modelRotationInput.x -= rot; break;
				case Keys.Down: modelRotationInput.x += rot; break;
				case Keys.Left: modelRotationInput.y -= rot; break;
				case Keys.Right: modelRotationInput.y += rot; break;
			}
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			switch (e.KeyCode)
			{
				case Keys.R:
					rotate = false;
					break;
			}
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);

			Vector3 move = new Vector3();
			switch (e.KeyChar)
			{
				case 'a': move.x -= 1f; break;
				case 'd': move.x += 1f; break;
				case 'q': move.y -= 1f; break;
				case 'e': move.y += 1f; break;
				case 'w': move.z += 1f; break;
				case 's': move.z -= 1f; break;
			}
			controlPositionInput += move * 0.001f;
		}

		DateTime lastFrameTime;
		int fpsMax;
		int fpsMin;
		int fps;
		Vector3 controlPositionInput;
		Vector3 controlRotation;
		Vector3 modelRotationInput;
		Matrix4x4 modelMatrix;

		private void CameraUpdate()
		{
			Matrix4x4 projection = Matrix4x4.CreateFrustumMatrix(-0.2f, 0.2f, -0.2f, 0.2f, 0.1f, 10f);
			
			panel.projectionMatrix = projection;
			Matrix4x4 view = Matrix4x4.RotateAround(controlRotation, Vector3.zero);
			panel.viewMatrix = view;
		}

		private void Draw()
		{
			panel.Clear();

			CameraUpdate();

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
			modelMatrix = Matrix4x4.RotateAround(modelRotationInput, Vector3.zero) * modelMatrix;
			modelMatrix = modelMatrix.Translate(controlPositionInput);

			controlPositionInput = modelRotationInput = Vector3.zero;

			panel.DrawMesh(model, modelMatrix);
		}
	}
}
