using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace UMLProgram.Core.Render.Common {
    public class RenderContext {
        private const int TEXTURE_UNIT_START = 33984;
        private int id;
        private int textureUnitOffset=0;

        public RenderContext(string vertexShader,string fragmentShader) {
            id = ShaderProgram.Create(vertexShader, fragmentShader);
        }
        public int GetProgramHandle() { return id; }
        public void UseProgram() { GL.UseProgram(id); }

        public int SetUniform1(float value,string shaderVariableName) {
            int uniformLocation = GL.GetUniformLocation(id, shaderVariableName);
            GL.Uniform1(uniformLocation, value);
            return uniformLocation;
        }
        public int SetUniform3(Vector3 value, string shaderVariableName) {
            int uniformLocation = GL.GetUniformLocation(id, shaderVariableName);
            GL.Uniform3(uniformLocation,ref value);
            return uniformLocation;
        }
        public int SetUniform4(ref Matrix4 value, string shaderVariableName) {
            int uniformLocation = GL.GetUniformLocation(id, shaderVariableName);
            GL.UniformMatrix4(uniformLocation,false,ref value);
            return uniformLocation;
        }
        public void AddTexture(int textureHandle) {
            GL.ActiveTexture((TextureUnit) TEXTURE_UNIT_START + textureUnitOffset);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            GL.Uniform1(textureHandle, textureUnitOffset);
            textureUnitOffset++;
        }
        public void ResetTextures() {
            textureUnitOffset = TEXTURE_UNIT_START;
        }
        public void DisposeAll() {
            while (textureUnitOffset >= TEXTURE_UNIT_START) {
                GL.DeleteTexture(textureUnitOffset);
                textureUnitOffset--;
            }
            GL.DeleteProgram(id);
        }
    }
}
