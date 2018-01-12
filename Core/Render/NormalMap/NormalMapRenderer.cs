using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Render.Common;
using UMLProgram.Core.Loaders;
using UMLProgram.Core.Loaders.Files;
using System.Drawing;
using UMLProgram.Core.Render.NormalMap.programs;
using static UMLProgram.Core.Input.Controller;

namespace UMLProgram.Core.Render.NormalMap {
    public partial class NormalMapRenderer {
        private static BufferService bufferService = new BufferService(true);
        private static RenderContext context;
       
        public static void LoadShaderProgram(Size clientSize) {
            context = new RenderContext(VertexShader.Text, FragmentShader.Text);
            SupplyShaderMatrices(clientSize);
            SupplyShaderVars();
            context.UseProgram();
        }
        public static void Load(Size clientSize) {
            LoadTextures();
            CreateVertexArray();
            modelKey = bufferService.Buffer(LoadObj());
            LoadShaderProgram(clientSize);
        }
        private static void CreateVertexArray() {
            vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayHandle);
        }
        private static void LoadTextures() {
            textureHandle = DDSLoader.Load(TEXTURE_FILE);
            normalMapHandle = BMPLoader.Load(NORMALMAP_FILE);
            specularMapHandle = DDSLoader.Load(SPECULARMAP_FILE);
        }
        private static IndexedD3Model2 LoadObj() {
            return ModelWorker.GetIndexedModelWithTangents(BlenderLoader.Load(CYLINDER_MODEL_FILE));
        }
        private static void SupplyShaderMatrices(Size clientSize) {
            float aspectRatio = clientSize.Width / (float)(clientSize.Height);
            Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, aspectRatio, 0.1f, 100, out projectionMatrix);
            viewMatrix = Matrix4.LookAt(new Vector3(4, 3, -3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            modelMatrix = Matrix4.Identity;
            projectionMatrixLocation = context.SetUniform4(ref projectionMatrix, "projection_matrix");
            viewMatrixLocation = context.SetUniform4(ref viewMatrix, "view_matrix");
            modelMatrixLocation = context.SetUniform4(ref modelMatrix, "model_matrix");
        }
        private static void SupplyShaderVars() {
            context.AddTexture(textureHandle);
            context.AddTexture(normalMapHandle);
            context.AddTexture(specularMapHandle);
            lightColorUniformLocation = context.SetUniform3(lightColorUniform, "light_color");
            lightPositionUniformLocation = context.SetUniform3(lightPositionUniform, "light_position_worldspace");
            lightPowerUniformLocation = context.SetUniform1( lightPowerUniform, "light_power");
        }
        public static void Update(ControllerData data) {
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(data.FOV), 4 / 3, 0.1f, 100, out projectionMatrix);
            Vector3 up = Vector3.Cross(data.Right, data.Direction);
            viewMatrix = Matrix4.LookAt(data.Position, data.Position + data.Direction, up);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);
        }
        public static void Draw() {
            int[] indices = bufferService.GetModel<IndexedD3Model2>(modelKey).Indices;
            int offset = bufferService.EnableAttributes<IndexedD3Model2>(modelKey,0);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, indices);
            bufferService.DisableAttributes<IndexedD3Model2>(offset);
        }
        public static void Clear() {
            bufferService.DisposeBuffers();
            context.DisposeAll();
            GL.DeleteVertexArray(vertexArrayHandle);
        }
    }
}
