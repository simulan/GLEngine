using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Loaders.Files {
    public class TriIndexedD3Model {
            public List<Vector3> Vertices = new List<Vector3>();
            public List<Vector2> UVs = new List<Vector2>();
            public List<Vector3> Normals = new List<Vector3>();
            public List<TriangleIndices> Indices = new List<TriangleIndices>();

            public struct TriangleIndices {
                public Vector3 VertexIndices;
                public Vector3 UVIndices;
                public Vector3 NormalIndices;
                public TriangleIndices(Vector3 vectorIndices, Vector3 uvIndices, Vector3 normalIndices) {
                    VertexIndices = vectorIndices;
                    UVIndices = uvIndices;
                    NormalIndices = normalIndices;
                }
                public TriangleIndices(string[] firstGroup, string[] secondGroup, string[] thirdGroup) {
                    VertexIndices = new Vector3(AsFloat(firstGroup[0]), AsFloat(secondGroup[0]), AsFloat(thirdGroup[0]));
                    UVIndices = new Vector3(AsFloat(firstGroup[1]), AsFloat(secondGroup[1]), AsFloat(thirdGroup[1]));
                    NormalIndices = new Vector3(AsFloat(firstGroup[2]), AsFloat(secondGroup[2]), AsFloat(thirdGroup[2]));
                }
                private static float AsFloat(string number) {
                if (number.Equals("")) return 0;
                    return float.Parse(number, CultureInfo.InvariantCulture);
                }
            }
    }
}
