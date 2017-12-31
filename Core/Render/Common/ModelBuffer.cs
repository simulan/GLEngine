using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Loaders.Files;

namespace UMLProgram.Core.Render.Common {
    public class ModelBuffer {
        private Dictionary<int, Tuple<IndexedD3Model,BufferHandle>> models = new Dictionary<int, Tuple<IndexedD3Model, BufferHandle>>();
        public struct BufferHandle {
            public int vertex;
            public int uv;
            public int normal;
        }
        public Tuple<IndexedD3Model,BufferHandle> this[int i] { get{return models[i];} }

        public int Add(IndexedD3Model model) {
            BufferHandle handle = Buffer(model);
            int key = handle.vertex;
            models.Add(handle.vertex, Tuple.Create<IndexedD3Model,BufferHandle>(model, handle));
            return key;
        }
        private BufferHandle Buffer(IndexedD3Model m) {
            BufferHandle handle = new BufferHandle();
            if (m.Vertices != null && m.Vertices.Length != 0) {
                GL.GenBuffers(1, out handle.vertex);
                GL.BindBuffer(BufferTarget.ArrayBuffer, handle.vertex);
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(m.Vertices.Length * Vector3.SizeInBytes), m.Vertices, BufferUsageHint.StaticDraw);
            }
            if (m.UVs != null && m.UVs.Length != 0) {
                GL.GenBuffers(1, out handle.uv);
                GL.BindBuffer(BufferTarget.ArrayBuffer, handle.uv);
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, new IntPtr(m.UVs.Length * Vector2.SizeInBytes), m.UVs, BufferUsageHint.StaticDraw);
            }
            if (m.Normals != null && m.Normals.Length != 0) {
                GL.GenBuffers(1, out handle.normal);
                GL.BindBuffer(BufferTarget.ArrayBuffer, handle.normal);
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(m.Normals.Length * Vector3.SizeInBytes), m.Normals, BufferUsageHint.StaticDraw);
            }
            return handle;
        }
    }
}
