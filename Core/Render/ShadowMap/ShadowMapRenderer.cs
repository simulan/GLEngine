using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Input;
using UMLProgram.Core.Render.Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using UMLProgram.Core.Loaders.Files;
using UMLProgram.Core.Loaders;
using UMLProgram.Core.Render.Base;
using UMLProgram.Core.Render.ShadowMap.Programs;

namespace UMLProgram.Core.Render.ShadowMap {
    public partial class ShadowMapRenderer : BaseRenderer {
        private static BufferService service = new BufferService(true);
        private static RenderContext context;

        public override void Load(Size clientSize) {
            LoadTextures();
            CreateVertexArrayBuffer();

            drawBufferProgramHandle = ShaderProgram.Create(BufferVertexShader.Text, BufferFragmentShader.Text);
            CreateDepthBuffer();
            CreateDepthMVPMatrices();
            BufferQuadVertices();

            D3Model rawModel = BlenderLoader.Load(MAP_MODEL_FILE);
            modelKey = service.Buffer<IndexedD3Model>(ModelWorker.IndexData(rawModel));
            context = new RenderContext(VertexShader.Text, FragmentShader.Text);
            BindShaderData(clientSize);
        }
        private void LoadTextures() {

        }
        private void CreateVertexArrayBuffer() {
            vertexArrayHandle =  GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayHandle);
        }
        private void CreateDepthBuffer() {
            // Depth texture. Slower than a depth buffer, but you can sample it later in your shader
            GL.GenTextures(1,out depthTexture);
            GL.BindTexture(TextureTarget.Texture2D, depthTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent16, 1024, 1024, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest );
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            SetFrameBuffer();
            SetDepthBuffer();
        }
        private void SetFrameBuffer() {
            // The framebuffer, which regroups 0, 1, or more textures, and 0 or 1 depth buffer.
            GL.GenFramebuffers(1, out framebufferHandle);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebufferHandle);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, depthTexture, 0);
            GL.DrawBuffer(DrawBufferMode.None); // No color buffer is drawn to.
            // Always check that our framebuffer is ok
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                throw new Exception("ShadowMapRenderer.Framebuffer not complete on CreateDepthBuffer()");

        }
        private void SetDepthBuffer() {
            depthRenderBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthRenderBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, 1024, 768);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.Depth, RenderbufferTarget.Renderbuffer, depthRenderBuffer);
        }
        private void CreateDepthMVPMatrices() {
            Vector3 lightInvDir = new Vector3(0.5f, 2, 2);
            Matrix4 depthProjectionMatrix = Matrix4.CreateOrthographic(20.0f, 20.0f, 0f, 20.0f);
            Matrix4 depthViewMatrix = Matrix4.LookAt(lightInvDir, new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            Matrix4 depthModelMatrix = Matrix4.Identity;

            depthMVP = depthProjectionMatrix * depthViewMatrix * depthModelMatrix;
            depthMVPLocation = GL.GetUniformLocation(drawBufferProgramHandle, "depthMVP");
            GL.UniformMatrix4(depthMVPLocation, false, ref depthMVP);
        }
        private void BufferQuadVertices() {
            GL.CreateBuffers(1, out quadVertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVertexBuffer);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(QuadVertexBufferData.Vertices.Length * Vector3.SizeInBytes), QuadVertexBufferData.Vertices, BufferUsageHint.StaticDraw);
        }
        private void BindShaderData(Size clientSize) {
            float aspectRatio = clientSize.Width / (float)(clientSize.Height);
            Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, aspectRatio, 0.1f, 100, out projectionMatrix);
            viewMatrix = Matrix4.LookAt(new Vector3(15,2,-1), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            modelMatrix = Matrix4.Identity;

            projectionMatrixLocation = context.SetUniform4(ref projectionMatrix, "projection_matrix");
            viewMatrixLocation = context.SetUniform4(ref viewMatrix, "view_matrix");
            modelMatrixLocation = context.SetUniform4(ref modelMatrix, "model_matrix");
            lightColorUniformLocation = context.SetUniform3(lightColor, "light_color");
            lightPositionUniformLocation = context.SetUniform3(lightPosition, "light_position_worldspace");
            lightPowerUniformLocation = context.SetUniform1(lightPower, "light_power");
        }
        public override void Draw() {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthRenderBuffer);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            DrawToBuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            DrawFromBuffer();
        }
        private void DrawToBuffer() {
            int[] indices = service.GetModel<IndexedD3Model>(modelKey).Indices;
            ActivateDrawToBuffer();
            int offset = service.EnableAttributes<IndexedD3Model>(modelKey, 0);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, indices);
            offset = service.DisableAttributes<IndexedD3Model>(offset);
        }
        private void ActivateDrawToBuffer() {
            context.UseProgram();
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);
            GL.UniformMatrix4(modelMatrixLocation, false, ref modelMatrix);
            GL.Uniform3(lightColorUniformLocation, lightColor);
            GL.Uniform3(lightPositionUniformLocation, lightPosition);
            GL.Uniform1(lightPowerUniformLocation, lightPower);
        }
        private void DrawFromBuffer() {
            ActivateDrawFromBuffer();
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVertexBuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.DisableVertexAttribArray(0);
        }
        private void ActivateDrawFromBuffer() {
            GL.UseProgram(drawBufferProgramHandle);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, depthTexture);
            GL.Uniform1(depthTexture, 0);
            GL.UniformMatrix4(depthMVPLocation, false, ref depthMVP);
        }
        public override void Clear() {
            service.DisposeBuffers();
            context.DisposeAll();
            GL.DeleteBuffer(quadVertexBuffer);
            GL.DeleteVertexArray(vertexArrayHandle);
            GL.DeleteTexture(depthTexture);
            GL.DeleteProgram(drawBufferProgramHandle);
        }
    }
}
