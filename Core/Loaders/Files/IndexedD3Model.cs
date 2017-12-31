using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Loaders.Files {
    public class IndexedD3Model {
        public Vector3[] Vertices;
        public Vector2[] UVs;
        public Vector3[] Normals;
        public int[] Indices;

        public IndexedD3Model() { }

        public IndexedD3Model(Vector3[] vertices,Vector2[] uvs,Vector3[] normals,int[] indices) {
            Vertices = vertices;
            UVs = uvs;
            Normals = normals;
            Indices = indices;
        }

    }
}
