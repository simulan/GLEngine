using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Loaders;
using OpenTK.Graphics.OpenGL;
using UMLProgram.Core.Render.Text.Programs;
using System.Diagnostics;
using OpenTK;
using UMLProgram.Core.Render.Common;

namespace UMLProgram.Core.Render.Text {
    public class Text2DRenderer {
        private static readonly int VERTICES_CHAR= 6;
        private static int vertexBufferHandle,
            uvBufferHandle,
            textureHandle,
            shaderProgramHandle;
        private static Vector2[] vertices;
        private static Vector2[] uvs;

        public static void Load(string ddsTexture) {
            shaderProgramHandle = ShaderProgram.Create(VertexShader.Text, FragmentShader.Text);
            textureHandle = DDSLoader.Load(ddsTexture);
        }
        private static void Activate() {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            GL.UseProgram(shaderProgramHandle);
        }
        private static void Draw() {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBufferHandle);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);
            GL.Disable(EnableCap.Blend);
        }
        public static void Print(string text, int x, int y, int size) {
            vertices = new Vector2[text.Length*VERTICES_CHAR];
            uvs = new Vector2[text.Length*VERTICES_CHAR];
            GenerateVerticesAndUVs(text, x, y, size);
            BufferVertices();
            BufferUVs();
            Activate();
            Draw();
        }
        private static void GenerateVerticesAndUVs(string text, int x, int y, int size) {
            for (int i = 0; i < text.Length; i++) {
                char c = text[i];
                int vertexIndex = i + i * (VERTICES_CHAR - 1);
                Vector2 leftTop = new Vector2(x + i * size, y + size);
                Vector2 rightTop = new Vector2(x + i * size + size, y + size);
                Vector2 leftBottom = new Vector2(x + i * size, y);
                Vector2 rightBottom = new Vector2(x + i * size + size, y);
                vertices[vertexIndex] = leftTop;
                vertices[vertexIndex + 1] = leftBottom;
                vertices[vertexIndex + 2] = rightTop;
                vertices[vertexIndex + 3] = rightBottom;
                vertices[vertexIndex + 4] = rightTop;
                vertices[vertexIndex + 5] = leftBottom;
                float uvX = (c % 16) / 16.0f;
                float uvY = (c / 16) / 16.0f;
                float unit = 1.0f / 16;
                Vector2 uvLeftTop = new Vector2(uvX, uvY);
                Vector2 uvRightTop = new Vector2(uvX+unit, uvY);
                Vector2 uvRightBottom = new Vector2(uvX+unit, uvY+unit);
                Vector2 uvLeftBottom = new Vector2(uvX, uvY+unit);
                uvs[vertexIndex] = uvLeftTop;
                uvs[vertexIndex + 1] = uvLeftBottom;
                uvs[vertexIndex + 2] = uvRightTop;
                uvs[vertexIndex + 3] = uvRightBottom;
                uvs[vertexIndex + 4] = uvRightTop;
                uvs[vertexIndex + 5] = uvLeftBottom;
            }
        }
        private static void BufferVertices() {
            GL.GenBuffers(1, out vertexBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, new IntPtr(vertices.Count() * Vector2.SizeInBytes), vertices.ToArray(), BufferUsageHint.StaticDraw);
        }
        private static void BufferUVs() {
            GL.GenBuffers(1, out uvBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBufferHandle);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, new IntPtr(uvs.Count() * Vector2.SizeInBytes), uvs.ToArray(), BufferUsageHint.StaticDraw);
        }
        public static void Clear() {
            GL.DeleteBuffers(2, new int[] {  vertexBufferHandle,uvBufferHandle });
            GL.DeleteTexture(textureHandle);
            GL.DeleteProgram(shaderProgramHandle);
        }
    }
}
