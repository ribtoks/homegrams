// <summary>
/// System namespaces, used for Drawing or just usual
/// </summary>
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

/// <summary>
/// OpenTK namespaces, used in SimpleForm Class
/// </summary>
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL.Enums;
using OpenTK.Input;

namespace OpenGLStart
{
//	public enum RotationMode { Horisontal, Vertical, None };
    //simple class based on OpenTK.GameWindow    
    public class CubeAndTrianglesWindow : GameWindow
    {
		//data for cube
		private float horisontalAngle = 0.0f;
		private float verticalAngle = 0.0f;
		private float horisontalSpeed = 0.0f;
		private float verticalSpeed = 0.0f;//1.85f;
		private float epsilon = 0.05f;
		private float dead_epsilon = 0.005f;
		
		private float sqrt2 = (float)Math.Sqrt(2.0);
		
		//data for circle
		private float angle = 0.0f;
		private float speed = 0.4f;

		
//		RotationMode rotationMode = RotationMode.Horisontal;
		
		#region Constructors
        //default size of form
        public CubeAndTrianglesWindow()
            : base(800, 600, new GraphicsMode(16, 16))
        {			
            //sets event handler for key pressing
            Keyboard.KeyDown += new KeyDownEvent(Keyboard_KeyDown);
        }
        
        public CubeAndTrianglesWindow(int width, int height)
            : base(width, height, new GraphicsMode(16, 16))
        {
            //sets event handler for key pressing
            Keyboard.KeyDown += new KeyDownEvent(Keyboard_KeyDown);
        }
		#endregion
		
		#region OnPaint
		private void DrawCube()
        {
			//drawing cube
            GL.Begin(BeginMode.Quads);

            GL.Color3(Color.White);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);

            GL.Color3(Color.Chocolate);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);

            GL.Color3(Color.Brown);

            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);

            GL.Color3(Color.Blue);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);

            GL.Color3(Color.Gray);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);

            GL.Color3(Color.DarkGreen);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);

            GL.End();
        }
		
		private void DrawTriangles(float lengthParam, bool IsBlended, Color color)
		{
			GL.Color3(color);
			
			GL.Begin(BeginMode.Triangles);
			//red apex
			if (IsBlended)
				GL.Color3(1.0, 0.0, 0.0);
			GL.Vertex2(-lengthParam, -lengthParam);
            
            //green apex
			if (IsBlended)
				GL.Color3(0.0, 1.0, 0.0);
			GL.Vertex2(lengthParam, -lengthParam);

            //blue apex
			if (IsBlended)
				GL.Color3(0.0, 0.0, 1.0);
			GL.Vertex2(0, lengthParam);
			
			GL.End();
		}
		
		private void DrawClock()
		{
			//drawing clock weight
			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
//			GL.ShadeModel(ShadingModel.Flat);
			GL.Color3(Color.DarkSalmon);
			
			GL.Begin(BeginMode.Polygon);
			
			GL.Vertex2(-4.0, -4.0);
			GL.Vertex2(-4.5, -4.5);
			GL.Vertex2(-4.5, -5.0);
			GL.Vertex2(-4.0, -5.5);
			GL.Vertex2(-3.5, -5.0);
			GL.Vertex2(-3.5, -4.5);
			
			GL.End();
			
			
			//drawing clock axe
			GL.Begin(BeginMode.Lines);
			
			GL.Color3(Color.Bisque);			
			GL.Vertex2(-4.0, 4.0);			
			GL.Vertex2(-4.0, -4.1);			
			
			GL.End();
		}
        
        /// <summary>
        /// Insert code here that will do something
        /// </summary>
        /// <param name="e">
        /// A <see cref="OpenTK.UpdateFrameEventArgs"/>
        /// </param>
        public override void OnUpdateFrame (UpdateFrameEventArgs e)
        {            
        }
		
		private void UpdateSpeed(ref float speed)
		{
			if (Math.Abs(speed) > dead_epsilon)
			{
				if (speed > dead_epsilon)
					speed -= dead_epsilon;
				else
					speed += dead_epsilon;
			}
			else
				speed = 0;
		}
		
		private void DrawWholeClock(float ScaleFactor)
		{
			angle += speed * ScaleFactor;
			GL.Rotate(angle, -4.0f, 4.0, 0.0f);
			DrawClock();
			
			if (angle >= 27.0)
				speed = -0.4f;
			else
			{
				if (angle <= -28.0)
					speed = 0.4f;
				else
				{
					if (Math.Abs(Math.Abs(angle) - 27.0f) < 3)
					{
						speed = 0.2f*Math.Sign(speed);
					}
					else
						if (Math.Abs(Math.Abs(angle) - 27.0f) < 0.7)
						{
							speed = 0.05f*Math.Sign(speed);
						}
						else
							speed = 0.4f*Math.Sign(speed);
				}
			}
		}
		
		private void DrawBackGround(RenderFrameEventArgs e)
		{
			GL.LoadIdentity();		
			
			GL.Translate(0.0, 2.0, -15.0);
			
			GL.PushMatrix();
			GL.Rotate(45.0, -4.0, 4.0, -15.0);
			DrawTriangles(5, false, Color.DarkSlateGray);
			GL.PopMatrix();			
			
			DrawWholeClock((float)e.ScaleFactor);
		}
        
        /// <summary>
        /// Insert OnPaint code here
        /// </summary>
        /// <param name="e">
        /// A <see cref="OpenTK.RenderFrameEventArgs"/>
        /// </param>
        public override void OnRenderFrame (RenderFrameEventArgs e)
        {			
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			
			GL.MatrixMode(MatrixMode.Modelview);
			
			DrawBackGround(e);
			
			GL.LoadIdentity();
            Glu.LookAt(0.0, 5.0, 5.0,
                       0.0, 0.0, 0.0,
			           0.0, 1.0, 0.0);
			
			
			horisontalAngle += horisontalSpeed * (float)e.ScaleFactor;
			verticalAngle += verticalSpeed * (float)e.ScaleFactor;
	
						
			GL.Rotate(verticalAngle, 1.0f, 0.0f, 0.0f);
			
			GL.Rotate(horisontalAngle, 0.0f, 1.0f, 0.0f);
/*			
			double Angle = Math.PI*(verticalAngle/180.0);
			
			float NewX = 0.0f;
			float NewY = (float)Math.Cos(Angle);
			float NewZ = -(float)Math.Sin(Angle);
			
			GL.Rotate(horisontalAngle, NewX, NewY, NewZ);
*/			
			DrawCube();
			
			UpdateSpeed(ref horisontalSpeed);
			UpdateSpeed(ref verticalSpeed);
			
			this.SwapBuffers();
        }
		
		#endregion
        
        /// <summary>
        /// Occurs when the form is loaded
        /// </summary>
        /// <param name="e">
        /// A <see cref="System.EventArgs"/>
        /// </param>
        public override void OnLoad (EventArgs e)
        {
			base.OnLoad(e);            
            //sets the background of window, for example RoyalBlue
            GL.ClearColor(Color.RoyalBlue);
			GL.Enable(EnableCap.DepthTest);
        }
        
        /// <summary>
        /// Occurs when the form is resized
        /// </summary>
        /// <param name="e">
        /// A <see cref="OpenTK.Platform.ResizeEventArgs"/>
        /// </param>
        protected override void OnResize (OpenTK.Platform.ResizeEventArgs e)
        {			
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            double ratio = e.Width / (double)e.Height;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Glu.Perspective(45.0, ratio, 1.0, 64.0);
        }
        
        /// <summary>
        /// Occurs, when the key is pressed
        /// </summary>
        /// <param name="sender">
        /// A <see cref="KeyboardDevice"/>
        /// </param>
        /// <param name="key">
        /// A <see cref="Key"/>
        /// </param>
        void Keyboard_KeyDown(KeyboardDevice sender, Key key)
        {
            if (sender[Key.Escape])
                this.Exit();

            //ability to enable Fullscreen Mode using 'Ctrl' + 'Enter' keys
            if ((sender[Key.AltLeft] || sender[Key.AltRight])  &&  
                (sender[Key.Enter] || sender[Key.KeypadEnter]))
                if (this.WindowState == WindowState.Fullscreen)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Fullscreen;
			if (sender[Key.Left] ||  sender[Key.Right])
			{
//				rotationMode = RotationMode.Horisontal;
				if (sender[Key.Left])
					horisontalSpeed -= epsilon;
				else
					if (sender[Key.Right])
						horisontalSpeed += epsilon;
			}
			else
			{
				if (sender[Key.Down] ||  sender[Key.Up])
				{
//					rotationMode = RotationMode.Vertical;
					if (sender[Key.Up])
						verticalSpeed -= epsilon;
					else
						if (sender[Key.Down])
							verticalSpeed += epsilon;
				}
			}
        }
        
        /// <summary>
        /// Entry point for the application
        /// Also defines, that our program belongs to 
        /// Single Threated Apartment
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            //safe using of object
            //uses safe Dispose() method
            using (CubeAndTrianglesWindow CubeAndTrianglesWindow1 = new CubeAndTrianglesWindow())
            {
                //Run() - method of GameWindow
                CubeAndTrianglesWindow1.Run();
            }
        }
    }
}