using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Input;

namespace UMLProgram.Core.Render.Base {
    public class InputRenderer : BaseRenderer {
        public void Update(Controller.ControllerData data) {
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(data.FOV), 4 / 3, 0.1f, 100, out projectionMatrix);
            Vector3 up = Vector3.Cross(data.Right, data.Direction);
            viewMatrix = Matrix4.LookAt(data.Position, data.Position + data.Direction, up);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);
        }
    }
}
