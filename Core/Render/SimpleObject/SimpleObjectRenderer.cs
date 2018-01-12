using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Input;
using UMLProgram.Core.Loaders;
using UMLProgram.Core.Loaders.Files;
using UMLProgram.Core.Render.Common;
using UMLProgram.Core.Render.Cube;
using UMLProgram.Core.Render.SimpleObject.Programs;

namespace UMLProgram.Core.Render.SimpleObject {
    public class SimpleObjectRenderer {
        private static BufferService bufferService = new BufferService();
        private static Matrix4 projectionMatrix, viewMatrix, modelMatrix;
        private static Vector3 lightColorUniform = new Vector3(0.8f, 0.8f, 0.8f);
        private static Vector3 lightPositionUniform = new Vector3(5, 5, 0);
        private static int modelKey;
        private static int lightPowerUniform = 6;
        private static int textureHandle,
            vertexArrayHandle,
            shaderProgramHandle,
            projectionMatrixLocation,
            modelMatrixLocation,
            viewMatrixLocation,
            lightColorUniformLocation,
            lightPowerUniformLocation,
            lightPositionUniformLocation;

        public static void Activate() {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            GL.UseProgram(shaderProgramHandle);
        }
        public static void Load(Size clientSize) {
            LoadTextures();
            CreateVertexArray();
            modelKey = bufferService.Buffer(LoadObj());
            shaderProgramHandle = ShaderProgram.Create(VertexShader.Text, FragmentShader.Text);
            BindShaderData(clientSize);
        }
        private static void CreateVertexArray() {
            vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayHandle);
        }
        private static void LoadTextures() {
            String file = "C:\\Work\\My CSharp\\UMLcreator\\UMLProgram\\texture.dds";
            textureHandle = DDSLoader.Load(file);
        }
        private static IndexedD3Model LoadObj() {
            String file = "C:\\Work\\My CSharp\\UMLcreator\\UMLProgram\\box.obj";
            return ModelWorker.IndexData( BlenderLoader.Load(file));
        }
        private static void BindShaderData(Size clientSize) {
            SupplyShaderMatrices(clientSize);
            SupplyShaderVars();
        }
        private static void SupplyShaderMatrices(Size clientSize) {
            projectionMatrixLocation = GL.GetUniformLocation(shaderProgramHandle, "projection_matrix");
            viewMatrixLocation = GL.GetUniformLocation(shaderProgramHandle, "view_matrix");
            modelMatrixLocation = GL.GetUniformLocation(shaderProgramHandle, "model_matrix");

            float aspectRatio = clientSize.Width / (float)(clientSize.Height);
            Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, aspectRatio, 0.1f, 100, out projectionMatrix);
            viewMatrix = Matrix4.LookAt(new Vector3(4, 3, -3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            modelMatrix = Matrix4.Identity;

            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);
            GL.UniformMatrix4(modelMatrixLocation, false, ref modelMatrix);
        }
        private static void SupplyShaderVars() {
            lightColorUniformLocation = GL.GetUniformLocation(shaderProgramHandle, "light_color");
            lightPowerUniformLocation = GL.GetUniformLocation(shaderProgramHandle, "light_power");
            lightPositionUniformLocation = GL.GetUniformLocation(shaderProgramHandle, "light_position_worldspace");
            GL.Uniform3(lightColorUniformLocation, ref lightColorUniform); 
            GL.Uniform1(lightPowerUniformLocation, lightPowerUniform);
            GL.Uniform3(lightPositionUniformLocation, ref lightPositionUniform);
        }
        public static void Update(Controller.ControllerData data) {
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(data.FOV), 4 / 3, 0.1f, 100, out projectionMatrix);
            Vector3 up = Vector3.Cross(data.Right, data.Direction);
            viewMatrix = Matrix4.LookAt(data.Position, data.Position + data.Direction, up);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);
        }
        public static void Draw() {
            int[] indices = bufferService.GetModel<IndexedD3Model>(modelKey).Indices;
            int offset = bufferService.EnableAttributes<IndexedD3Model>(modelKey, 0);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, indices);
            offset = bufferService.DisableAttributes<IndexedD3Model>(offset) ;
        }
        public static void Clear() {
            bufferService.DisposeBuffers();
            GL.DeleteTexture(textureHandle);
            GL.DeleteProgram(shaderProgramHandle);
            GL.DeleteVertexArray(vertexArrayHandle);
        }
    }
}
