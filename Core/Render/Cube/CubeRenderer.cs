using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using UMLProgram.Core.Render.Cube.Programs;
using System.Drawing;

namespace UMLProgram.Core.Render.Cube {
    public static class CubeRenderer {
        private static Matrix4 projectionMatrix, modelviewMatrix;
        private static int vertexShaderHandle,
            fragmentShaderHandle,
            shaderProgramHandle,
            modelviewMatrixLocation,
            projectionMatrixLocation,
            vaoHandle,
            vertexBufferHandle,
            normalsBufferHandle,
            indicesBufferHandle;

        public static void Load(Size clientSize) {
            CreateShaders(clientSize);
            CreateVertexBuffers();
            BindBuffersToShaders();
        }
        public static void Render() {
            GL.BindVertexArray(vaoHandle);
            GL.DrawElements(PrimitiveType.Triangles, CubeVertexData.Indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
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
            modelviewMatrixLocation = GL.GetUniformLocation(shaderProgramHandle, "modelview_matrix");

            float aspectRatio = clientSize.Width / (float)(clientSize.Height);
            Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, aspectRatio, 1, 100, out projectionMatrix); 
            modelviewMatrix = Matrix4.LookAt(new Vector3(0, 3, 5), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(modelviewMatrixLocation, false, ref modelviewMatrix);
        }
        private static void CreateVertexBuffers() {
            BufferVertices();
            BufferNormals();
            BufferIndices();
            ClearBufferReferences();
        }

        private static void ClearBufferReferences() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private static void BufferIndices() {
            GL.GenBuffers(1, out indicesBufferHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * CubeVertexData.Indices.Length), CubeVertexData.Indices, BufferUsageHint.StaticDraw);
        }

        private static void BufferNormals() {
            GL.GenBuffers(1, out normalsBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalsBufferHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(CubeVertexData.Vertex.Length * Vector3.SizeInBytes), CubeVertexData.Vertex, BufferUsageHint.StaticDraw);
        }

        private static void BufferVertices() {
            GL.GenBuffers(1, out vertexBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                new IntPtr(CubeVertexData.Vertex.Length * Vector3.SizeInBytes), CubeVertexData.Vertex, BufferUsageHint.StaticDraw);
        }

        private static void BindBuffersToShaders() {
            // GL3 allows us to store the vertex layout in a "vertex array object" (VAO).
            // This means we do not have to re-issue VertexAttribPointer calls
            // every time we try to use a different vertex layout - these calls are
            // stored in the VAO so we simply need to bind the correct VAO.
            GL.GenVertexArrays(1, out vaoHandle);
            GL.BindVertexArray(vaoHandle);

            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
            GL.BindAttribLocation(shaderProgramHandle, 0, "in_position");

            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalsBufferHandle);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
            GL.BindAttribLocation(shaderProgramHandle, 1, "in_normal");

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesBufferHandle);

            GL.BindVertexArray(0);
        }

    }
}
