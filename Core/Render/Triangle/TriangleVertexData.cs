using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Triangle {
    class TriangleVertexData {
        public static readonly Vector3[] Vertex = new Vector3[]{
            new Vector3(-1.0f, -1.0f, 0.0f),
            new Vector3(1.0f, -1.0f, 0.0f),
            new Vector3(0.0f,  1.0f, 0.0f)
        };
        //public static readonly int[] Indices = { 0, 1, 2, 3 };
    }
}
