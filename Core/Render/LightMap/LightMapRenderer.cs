using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UMLProgram.Core.Input.Controller;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using UMLProgram.Core.Render.Common;
using UMLProgram.Core.Loaders.Files;
using UMLProgram.Core.Loaders;
using UMLProgram.Core.Render.LightMap.programs;
using System.Drawing;

namespace UMLProgram.Core.Render.LightMap {
    public partial class LightMapRenderer {
        private static BufferService bufferService = new BufferService(true);
        private static RenderContext context;

        public static void Load(Size clientSize) {
            CreateVertexArray();
            LoadTextures();
            modelKey = bufferService.Buffer<IndexedD3Model>(ModelWorker.IndexData(BlenderLoader.Load(MODEL_FILE)));
            context = new RenderContext(VertexShader.Text, FragmentShader.Text);
            BindShaderData(clientSize);
        }
        private static void CreateVertexArray() {
            vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayHandle);
        }
        private static void LoadTextures() {
            lightMapHandle = TGALoader.Load(LIGHTMAP_FILE);
        }
        private static void BindShaderData(Size clientSize) {
            float aspectRatio = clientSize.Width / (float)(clientSize.Height);
            Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, aspectRatio, 0.1f, 100, out projectionMatrix);
            viewMatrix = Matrix4.LookAt(new Vector3(4, 3, -3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            modelMatrix = Matrix4.Identity;

            projectionMatrixLocation = context.SetUniform4(ref projectionMatrix, "projection_matrix");
            viewMatrixLocation = context.SetUniform4(ref viewMatrix, "view_matrix");
            modelMatrixLocation = context.SetUniform4(ref modelMatrix, "model_matrix");
            context.AddTexture(lightMapHandle);
        }
        public static void Draw() {
            int[] indices = bufferService.GetModel<IndexedD3Model>(modelKey).Indices;
            int offset = bufferService.EnableAttributes<IndexedD3Model>(modelKey, 0);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, indices);
            offset = bufferService.DisableAttributes<IndexedD3Model>(offset);
        }
        public static void Update(ControllerData data) {
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(data.FOV), 4 / 3, 0.1f, 100, out projectionMatrix);
            Vector3 up = Vector3.Cross(data.Right, data.Direction);
            viewMatrix = Matrix4.LookAt(data.Position, data.Position + data.Direction, up);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);
        }
        public static void Clear() {
            bufferService.DisposeBuffers();
            context.DisposeAll();
        }
    }
}
