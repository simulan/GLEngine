using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Base {
    public class BaseRenderer {
        protected Matrix4 projectionMatrix, viewMatrix, modelMatrix;
        protected int projectionMatrixLocation,
            modelMatrixLocation,
            viewMatrixLocation,
            vertexArrayHandle;

        public virtual void Load(Size clientSize) {
        }
        public virtual void Draw() {
        }
        public virtual void Clear() {
        }

    }
}
