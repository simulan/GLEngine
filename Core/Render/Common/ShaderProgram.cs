using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace UMLProgram.Core.Render.Common {
    public static class ShaderProgram {
        public static int Create(string vertexShader,string fragmentShader) {
            int vertexShaderHandle = CompileVertexShader(vertexShader);
            int fragmentShaderHandle = CompileFragmentShader(fragmentShader);
            int shaderProgramHandle = CreateShaderProgram(vertexShaderHandle, fragmentShaderHandle);
            DisposeShaders(shaderProgramHandle,vertexShaderHandle, fragmentShaderHandle);
            return shaderProgramHandle;
        }
        private static int CompileVertexShader(string vertexShader) {
            int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShader);
            GL.CompileShader(vertexShaderHandle);
            Trace.WriteLine(GL.GetShaderInfoLog(vertexShaderHandle));
            return vertexShaderHandle;
        }
        private static int CompileFragmentShader(string fragmentShader) {
            int fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle, fragmentShader);
            GL.CompileShader(fragmentShaderHandle);
            Trace.WriteLine(GL.GetShaderInfoLog(fragmentShaderHandle));
            return fragmentShaderHandle;
        }
        private static int CreateShaderProgram(int vertexShaderHandle, int fragmentShaderHandle) {
            int shaderProgramHandle = GL.CreateProgram();
            GL.AttachShader(shaderProgramHandle, vertexShaderHandle);
            GL.AttachShader(shaderProgramHandle, fragmentShaderHandle);
            GL.LinkProgram(shaderProgramHandle);
            GL.UseProgram(shaderProgramHandle);
            Trace.WriteLine(GL.GetProgramInfoLog(shaderProgramHandle));
            return shaderProgramHandle;
        }
        private static void DisposeShaders(int shaderProgramHandle,int vertexShaderHandle, int fragmentShaderHandle) {
            GL.DetachShader(shaderProgramHandle, vertexShaderHandle);
            GL.DetachShader(shaderProgramHandle, fragmentShaderHandle);
            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(fragmentShaderHandle);
        }
    }
}
