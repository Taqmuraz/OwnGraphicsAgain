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
		Timer timer;

		private void CreatePanel(int width, int height)
		{
			if (panel != null) panel.Dispose();
			panel = this.AddControl<DrawPanel, int, int>(width, height);
			panel.Bounds = new Rectangle(0, 0, Width, Height);
			panel.SizeMode = PictureBoxSizeMode.StretchImage;
			panel.MouseMove += OnMouseMove;
		}

		private void ResizePanel()
		{
			//CreatePanel(Width, Height);
			panel.Bounds = new Rectangle(0, 0, Width, Height);
		}

		public MainWindow()
		{
			InitializeComponent();


			Size = new Size(1024, 640);
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
				Text = $"Fps : {fps.ToString()} {controlPositionInput.ToString()}";
			};
			fpsTimer.Interval = 100;
			fpsTimer.Start();

			try
			{
				string path = "C:/BLENDER_MODELS/Soldier.obj";
				model = ObjMeshManager.LoadMesh(path);

				Texture texture = new Texture("C:/BLENDER_MODELS/Soldier.png");
				model.material = new TextureMaterial(texture);
				texture.tiling = new Vector2(1f, 1f);
				//model.material = new ColorMaterial(Color.Green);

				float scale = 33f;

				Vector3 right = Vector3.right * scale;
				Vector3 up = Vector3.up * scale;
				Vector3 forward = Vector3.forward * scale;

				modelMatrix = Matrix4x4.CreateWorldMatrix(right, up, forward, new Vector3(0f, 0f, 0f));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			cameraFOVmul += e.Delta * 0.001f;
		}

		private Vector2 lastMouseLocation;
		private bool rotate;
		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			Vector2 location = new Vector2(e.Y, -e.X);
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
			controlPositionInput += move * cameraFOVmul;
		}

		DateTime lastFrameTime;
		int fpsMax;
		int fpsMin;
		int fps;
		Vector3 controlPositionInput = new Vector3(0f, 100f, 250f);
		Vector3 controlRotation;
		Vector3 modelRotationInput;
		Matrix4x4 modelMatrix;
		Matrix4x4 viewMatrix;
		float cameraFOVmul = 1f;

		private void CameraUpdate()
		{
			cameraFOVmul = cameraFOVmul.Clamp(0.5f, 2f);

			panel.fieldOfView = 60f * cameraFOVmul;
			panel.farPlane = 1f;

			viewMatrix = Matrix4x4.RotateAround(controlRotation, -controlPositionInput);
			
			panel.viewMatrix = viewMatrix;
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
			modelRotationInput = Vector3.zero;
			panel.DrawMesh(model, modelMatrix);
		}
	}
}
