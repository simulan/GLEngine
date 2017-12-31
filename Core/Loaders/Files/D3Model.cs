using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Loaders.Files {
    public class D3Model {
        public Vector3[] Vertices;
        public Vector2[] UVs;
        public Vector3[] Normals;

        public D3Model() {}
        public D3Model(int capacity) {
            Vertices = new Vector3[capacity];
            UVs = new Vector2[capacity];
            Normals = new Vector3[capacity];
        }
    }
}
