using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Input;
using UMLProgram.Core.Loaders;
using UMLProgram.Core.Render.ColorCube;
using UMLProgram.Core.Render.Cube;
using UMLProgram.Core.Render.LightMap;
using UMLProgram.Core.Render.NormalMap;
using UMLProgram.Core.Render.Rectangle;
using UMLProgram.Core.Render.RenderToTexture;
using UMLProgram.Core.Render.ShadowMap;
using UMLProgram.Core.Render.SimpleObject;
using UMLProgram.Core.Render.Text;
using UMLProgram.Core.Render.TexturedCube;
using UMLProgram.Core.Render.Triangle;

namespace UMLProgram.Core {
    public class UmlWindow : GameWindow {
        private Controller controller;
        private Rectangle innerWindow;
        private Vector3 spawnPosition = new Vector3(5, 5, 10);
        private const int DEFAULT_WIDTH = 1024;
        private const int DEFAULT_HEIGTH = 768;

        public UmlWindow() {
            ClientSize = new Size(DEFAULT_WIDTH,DEFAULT_HEIGTH);
            controller = new Controller(Keyboard.NumberOfKeys, spawnPosition);
        }
        protected override void OnLoad(EventArgs e) {
            CalculateInnerWindow();
            VSync = VSyncMode.On;
            GL.Enable(EnableCap.DepthTest);
            LightMapRenderer.Load(ClientSize);
            GL.ClearColor(Color.MidnightBlue);
            Closed += UmlWindow_Closed;
        }
        private void UmlWindow_Closed(object sender, EventArgs e) {
            LightMapRenderer.Clear();
        }

        private void CalculateInnerWindow() {
            int borderSize = (Bounds.Width - ClientSize.Width) / 2;
            int titleBarSize = Bounds.Height - ClientSize.Height - 2 * borderSize;
            innerWindow = new Rectangle(new Point(X + borderSize, Y + borderSize + titleBarSize), ClientSize);
        }
        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            controller.CalculateChanges(e.Time, new Point(Mouse.X, Mouse.Y),Mouse.Wheel,Keyboard.GetState());
            LightMapRenderer.Draw();
            LightMapRenderer.Update(controller.Data);
            OpenTK.Input.Mouse.SetPosition(innerWindow.Left + (Width / 2), innerWindow.Top + (Height / 2));
            SwapBuffers();
            if (Keyboard.GetState().IsKeyDown(OpenTK.Input.Key.Escape)) {
                Exit();
            }
        }        
    }
}
