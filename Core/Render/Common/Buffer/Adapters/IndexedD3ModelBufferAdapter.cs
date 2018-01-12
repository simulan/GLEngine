using System;
using System.Linq;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using UMLProgram.Core.Loaders.Files;

namespace UMLProgram.Core.Render.Common {
    public class IndexedD3ModelBufferAdapter : BufferAdapter<IndexedD3Model,IndexedD3ModelBufferHandle> {
        private Dictionary<int, Tuple<IndexedD3Model, IndexedD3ModelBufferHandle>> models = new Dictionary<int, Tuple<IndexedD3Model, IndexedD3ModelBufferHandle>>();
        private const int ATTRIBUTES = 3;

        public int Buffer(Object m) {
            IndexedD3Model model = m as IndexedD3Model;
            IndexedD3ModelBufferHandle handle = new IndexedD3ModelBufferHandle();
            if (model.Vertices != null && model.Vertices.Length != 0) {
                GL.GenBuffers(1, out handle.vertex);
                GL.BindBuffer(BufferTarget.ArrayBuffer, handle.vertex);
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(model.Vertices.Length * Vector3.SizeInBytes), model.Vertices, BufferUsageHint.StaticDraw);
            }
            if (model.UVs != null && model.UVs.Length != 0) {
                GL.GenBuffers(1, out handle.uv);
                GL.BindBuffer(BufferTarget.ArrayBuffer, handle.uv);
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, new IntPtr(model.UVs.Length * Vector2.SizeInBytes), model.UVs, BufferUsageHint.StaticDraw);
            }
            if (model.Normals != null && model.Normals.Length != 0) {
                GL.GenBuffers(1, out handle.normal);
                GL.BindBuffer(BufferTarget.ArrayBuffer, handle.normal);
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(model.Normals.Length * Vector3.SizeInBytes), model.Normals, BufferUsageHint.StaticDraw);
            }
            models.Add(handle.vertex, Tuple.Create<IndexedD3Model, IndexedD3ModelBufferHandle>(model, handle));
            return handle.vertex;
        }
        public IndexedD3ModelBufferHandle GetHandle(int modelKey) { return models[modelKey].Item2; }
        public IndexedD3Model GetModel(int modelKey) { return models[modelKey].Item1; }
        public int EnableAttributes(int modelKey,int offset) {
            IndexedD3ModelBufferHandle handle = models[modelKey].Item2;
            GL.EnableVertexAttribArray(offset+0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle.vertex);
            GL.VertexAttribPointer(offset+0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.EnableVertexAttribArray(offset+1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle.uv);
            GL.VertexAttribPointer(offset + 1, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
            GL.EnableVertexAttribArray(offset +2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle.normal);
            GL.VertexAttribPointer(offset + 2, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            return offset + ATTRIBUTES;
        }
        public int DisableAttributes(int offset) {
            for (int i = 1; i <= ATTRIBUTES; i++) GL.DisableVertexAttribArray(offset - i);
            return offset - ATTRIBUTES;
        }
        public void DisposeBuffers() {
            foreach (IndexedD3ModelBufferHandle handle in models.Values.Select(t => t.Item2)) {
                GL.DeleteBuffers(3, new int[] { handle.vertex, handle.uv, handle.normal });
            }
            models.Clear();
        }
    }
}
