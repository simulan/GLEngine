using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Rectangle {
    public class RectangleVertexData {
        public static readonly Vector3[] Vertices = new Vector3[]{
            new Vector3(-1.0f, -1.0f,  1.0f),
            new Vector3( -1.0f, 1.0f,  1.0f),
            new Vector3( 1.0f, -1.0f,  1.0f),
            new Vector3( 1.0f, 1.0f,  1.0f),
        };
        public static readonly int[] Indices = { 0, 1, 2, 3 };
    }
}
