using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using UMLProgram.Core.Loaders.Files;
using UMLProgram.Core.Render.Common;

namespace UMLProgram.Core.Render.Common {
    public class IndexedD3Model2BufferAdapter : BufferAdapter<IndexedD3Model2, IndexedD3Model2BufferHandle> {
        private Dictionary<int, Tuple<IndexedD3Model2, IndexedD3Model2BufferHandle>> models = new Dictionary<int, Tuple<IndexedD3Model2, IndexedD3Model2BufferHandle>>();
        private int ATTRIBUTES = 5;

        public int Buffer(object modelObject) {
            IndexedD3Model2 model = modelObject as IndexedD3Model2;
            IndexedD3Model2BufferHandle handle = new IndexedD3Model2BufferHandle();
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
            if (model.Tan != null && model.Tan.Length != 0) {
                GL.GenBuffers(1, out handle.tan);
                GL.BindBuffer(BufferTarget.ArrayBuffer, handle.tan);
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(model.Tan.Length * Vector3.SizeInBytes), model.Tan, BufferUsageHint.StaticDraw);
            }
            if (model.Bitan != null && model.Bitan.Length != 0) {
                GL.GenBuffers(1, out handle.bitan);
                GL.BindBuffer(BufferTarget.ArrayBuffer, handle.bitan);
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(model.Bitan.Length * Vector3.SizeInBytes), model.Bitan, BufferUsageHint.StaticDraw);
            }
            models.Add(handle.vertex, Tuple.Create<IndexedD3Model2,IndexedD3Model2BufferHandle>(model,handle));
            return handle.vertex;
        }
        public IndexedD3Model2BufferHandle GetHandle(int modelKey) { return models[modelKey].Item2; }
        public IndexedD3Model2 GetModel(int modelKey) { return models[modelKey].Item1; }
        public int EnableAttributes(int modelKey,int offset) {
            IndexedD3Model2BufferHandle handle = models[modelKey].Item2;
            int[] indices = models[modelKey].Item1.Indices;
            GL.EnableVertexAttribArray(offset + 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle.vertex);
            GL.VertexAttribPointer(offset + 0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.EnableVertexAttribArray(offset + 1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle.uv);
            GL.VertexAttribPointer(offset + 1, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
            GL.EnableVertexAttribArray(offset + 2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle.normal);
            GL.VertexAttribPointer(offset + 2, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.EnableVertexAttribArray(offset + 3);
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle.tan);
            GL.VertexAttribPointer(offset + 3, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.EnableVertexAttribArray(offset + 4);
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle.bitan);
            GL.VertexAttribPointer(offset + 4, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, indices);
            return offset += ATTRIBUTES ;
        }
        public int DisableAttributes(int offset) {
            for (int i = 1; i <= ATTRIBUTES; i++) GL.DisableVertexAttribArray(offset-i);
            return offset - ATTRIBUTES;
        }
        public void DisposeBuffers() {
            foreach(IndexedD3Model2BufferHandle handle in models.Values.Select(t => t.Item2)) {
                GL.DeleteBuffers(5, new int[] { handle.vertex, handle.uv, handle.normal,handle.tan,handle.bitan });
            }
            models.Clear();
        }
    }
}
