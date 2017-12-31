using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using UMLProgram.Core.Loaders.Files;
using OpenTK;

namespace UMLProgram.Core.Render.Common {
    public class ModelBuffer2 {
        private Dictionary<int, Tuple<IndexedD3Model2, BufferHandle>> models = new Dictionary<int, Tuple<IndexedD3Model2, BufferHandle>>();
        public struct BufferHandle {
            public int vertex;
            public int uv;
            public int normal;
            public int tan;
            public int bitan;
        }
        public Tuple<IndexedD3Model2, BufferHandle> this[int i] { get { return models[i]; } }

        public int Add(IndexedD3Model2 model) {
            BufferHandle handle = Buffer(model);
            int key = handle.vertex;
            models.Add(handle.vertex, Tuple.Create<IndexedD3Model2, BufferHandle>(model, handle));
            return key;
        }
        private BufferHandle Buffer(IndexedD3Model2 m) {
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
            if (m.Tan != null && m.Tan.Length != 0) {
                GL.GenBuffers(1, out handle.tan);
                GL.BindBuffer(BufferTarget.ArrayBuffer, handle.tan);
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(m.Tan.Length * Vector3.SizeInBytes), m.Tan, BufferUsageHint.StaticDraw);
            }
            if (m.Bitan != null && m.Bitan.Length != 0) {
                GL.GenBuffers(1, out handle.bitan);
                GL.BindBuffer(BufferTarget.ArrayBuffer, handle.bitan);
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(m.Bitan.Length * Vector3.SizeInBytes), m.Bitan, BufferUsageHint.StaticDraw);
            }
            return handle;
        }
    }
}
