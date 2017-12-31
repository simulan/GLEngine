using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Render.Rectangle.Programs;

namespace UMLProgram.Core.Render.Rectangle {
    public class RectangleRenderer {
        private static Matrix4 projectionMatrix, viewMatrix;
        private static int vertexShaderHandle,
            fragmentShaderHandle,
            shaderProgramHandle,
            viewMatrixLocation,
            projectionMatrixLocation,
            vaoHandle,
            vertexBufferHandle,
            indicesBufferHandle;

        public static void Load(Size clientSize) {
            CreateShaders(clientSize);
            CreateVertexBuffers();
            BindBuffersToShaders();
        }
        public static void Render() {
            GL.BindVertexArray(vaoHandle);
            GL.DrawElements(PrimitiveType.TriangleStrip, RectangleVertexData.Indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        private static void CreateShaders(Size clientSize) {
            CompileVertexShader();
            CompileFragmentShader();
            SetShaderProgram();
            SetMatrix(clientSize);
        }
        private static void CompileVertexShader() {
            vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, VertexShader.Text);
            GL.CompileShader(vertexShaderHandle);
            Trace.WriteLine(GL.GetShaderInfoLog(vertexShaderHandle));
        }
        private static void CompileFragmentShader() {
            fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle, FragmentShader.Text);
            GL.CompileShader(fragmentShaderHandle);
            Trace.WriteLine(GL.GetShaderInfoLog(fragmentShaderHandle));
        }
        private static void SetShaderProgram() {
            shaderProgramHandle = GL.CreateProgram();
            GL.AttachShader(shaderProgramHandle, vertexShaderHandle);
            GL.AttachShader(shaderProgramHandle, fragmentShaderHandle);
            GL.LinkProgram(shaderProgramHandle);
            Trace.WriteLine(GL.GetProgramInfoLog(shaderProgramHandle));
            GL.UseProgram(shaderProgramHandle);
        }
        private static void SetMatrix(Size clientSize) {
            projectionMatrixLocation = GL.GetUniformLocation(shaderProgramHandle, "projection_matrix");
            viewMatrixLocation = GL.GetUniformLocation(shaderProgramHandle, "modelview_matrix");
            
            float aspectRatio = clientSize.Width / (float)(clientSize.Height);
            Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, aspectRatio, 1, 100, out projectionMatrix);
            viewMatrix = Matrix4.LookAt(new Vector3(0, 0, 25), new Vector3(0, 0, 1), new Vector3(0, 1, 0));

            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);
        }

        private static void CreateVertexBuffers() {
            BufferVertices();
            BufferIndices();
            ClearBufferReferences();
        }
        private static void BufferVertices() {
            GL.GenBuffers(1, out vertexBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                new IntPtr(RectangleVertexData.Vertices.Length * Vector3.SizeInBytes), RectangleVertexData.Vertices, BufferUsageHint.StaticDraw);
        }
        private static void BufferIndices() {
            GL.GenBuffers(1, out indicesBufferHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * RectangleVertexData.Indices.Length), RectangleVertexData.Indices, BufferUsageHint.StaticDraw);
        }
        private static void ClearBufferReferences() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private static void BindBuffersToShaders() {
            GL.GenVertexArrays(1, out vaoHandle);
            GL.BindVertexArray(vaoHandle);

            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
            GL.BindAttribLocation(shaderProgramHandle, 0, "in_position");

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesBufferHandle);
            GL.BindVertexArray(0);
        }
    }
}
