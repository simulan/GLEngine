using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Loaders.Files;

namespace UMLProgram.Core.Render.Common {
    public class ModelWorker {
        public static IndexedD3Model IndexData(D3Model data) {
            List<int> indices = new List<int>();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            for (int i = 0; i < data.Vertices.Count(); i++) {
                int indexDuplicate = GetIndex(vertices, uvs, normals, data.Vertices[i], data.UVs[i], data.Normals[i]);
                if (indexDuplicate > -1) {
                    indices.Add(indexDuplicate);
                } else {
                    indices.Add(vertices.Count);
                    vertices.Add(data.Vertices[i]);
                    uvs.Add(data.UVs[i]);
                    normals.Add(data.Normals[i]);
                }
            }
            return new IndexedD3Model(vertices.ToArray(), uvs.ToArray(), normals.ToArray(), indices.ToArray());
        }
        public static IndexedD3Model2 GetIndexedModelWithTangents(D3Model data) {
            D3Model2 model = GetTangentsAndBitangens(data);
            return IndexDataWithTangents(model);
        }
        public static IndexedD3Model2 IndexDataWithTangents(D3Model2 data) {
            List<int> indices = new List<int>();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector3> tan = new List<Vector3>();
            List<Vector3> bitan = new List<Vector3>();
            for (int i = 0; i < data.Vertices.Count(); i++) {
                int indexDuplicate = GetIndex(vertices, uvs, normals, data.Vertices[i], data.UVs[i], data.Normals[i]);
                if (indexDuplicate > -1) {
                    indices.Add(indexDuplicate);
                    tan[indexDuplicate] += data.Tangents[i];
                    bitan[indexDuplicate] += data.Bitangents[i];
                } else {
                    indices.Add(vertices.Count);
                    vertices.Add(data.Vertices[i]);
                    uvs.Add(data.UVs[i]);
                    normals.Add(data.Normals[i]);
                    tan.Add(data.Tangents[i]);
                    bitan.Add(data.Bitangents[i]);
                }
            }
            return new IndexedD3Model2(vertices.ToArray(), uvs.ToArray(), normals.ToArray(), indices.ToArray(),tan.ToArray(),bitan.ToArray());
        }
        public static D3Model2 GetTangentsAndBitangens(D3Model data) {
            int length = data.Vertices.Length;
            D3Model2 result = new D3Model2(length);
            for (int i = 0; i < length; i += 3) {
                Vector3 v0 = data.Vertices[i + 0];
                Vector3 v1 = data.Vertices[i + 1];
                Vector3 v2 = data.Vertices[i + 2];
                Vector2 uv0 = data.UVs[i + 0];
                Vector2 uv1 = data.UVs[i + 1];
                Vector2 uv2 = data.UVs[i + 2];
                Vector3 deltaPos1 = v1 - v0;
                Vector3 deltaPos2 = v2 - v0;
                Vector2 deltaUV1 = uv1 - uv0;
                Vector2 deltaUV2 = uv2 - uv0;
                float r = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV1.Y * deltaUV2.X);
                Vector3 tan = (deltaPos1 * deltaUV2.Y - deltaPos2 * deltaUV1.Y) * r;
                Vector3 bitan = (deltaPos2 * deltaUV1.X - deltaPos1 * deltaUV2.X) * r;
                result.Tangents[i] = tan;
                result.Tangents[i+1] = tan;
                result.Tangents[i+2] = tan;
                result.Bitangents[i] = bitan;
                result.Bitangents[i + 1] = bitan;
                result.Bitangents[i + 2] = bitan;
            }
            result.Vertices = data.Vertices;
            result.UVs = data.UVs;
            result.Normals = data.Normals;
            return result;
        }
        private static int GetIndex(List<Vector3> vertices, List<Vector2> uvs, List<Vector3> normals, Vector3 vertex, Vector2 uv, Vector3 normal) {
            int result = -1;
            for (int i = 0; i < vertices.Count(); i++) {
                bool similarVertex = vertices[i].Equals(vertex);
                bool similarUV = uvs[i].Equals(uv);
                bool similarNormal = normals[i].Equals(normal);
                if (similarVertex && similarUV && similarNormal) {
                    return i;
                }
            }
            return result;
        }
    }
}
