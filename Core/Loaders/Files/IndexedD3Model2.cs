using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace UMLProgram.Core.Loaders.Files {
    public class IndexedD3Model2 : IndexedD3Model {
        public Vector3[] Bitan;
        public Vector3[] Tan;
         
        public IndexedD3Model2(Vector3[] vertices, Vector2[] uvs, Vector3[] normals, int[] indices,Vector3[] tan,Vector3[] bitan) : base(vertices, uvs, normals, indices) {
            Tan = tan;
            Bitan = bitan;
        }
    }
}
