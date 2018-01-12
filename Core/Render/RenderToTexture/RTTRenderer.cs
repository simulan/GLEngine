using System;
using OpenTK.Graphics.OpenGL4;
using UMLProgram.Core.Render.Common;
using OpenTK;
using UMLProgram.Core.Loaders;
using UMLProgram.Core.Loaders.Files;
using static UMLProgram.Core.Input.Controller;
using System.Drawing;
using UMLProgram.Core.Render.RenderToTexture.Programs;

namespace UMLProgram.Core.Render.RenderToTexture {
    public partial class RTTRenderer {
        private static BufferService bufferService = new BufferService(true);
        private static RenderContext sceneContext;

        public static void ActivateSceneDraw() {
            sceneContext.ResetTextures();
            sceneContext.UseProgram();
            sceneContext.AddTexture(textureHandle);
            sceneContext.AddTexture(normalMapHandle);
            sceneContext.AddTexture(specularMapHandle);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);
            GL.UniformMatrix4(modelMatrixLocation, false, ref modelMatrix);
            GL.Uniform3(lightColorUniformLocation, lightColorUniform);
            GL.Uniform3(lightPositionUniformLocation, lightPositionUniform);
            GL.Uniform1(lightPowerUniformLocation, lightPowerUniform);
        }
        public static void ActivateQuadDraw() {
            GL.UseProgram(quadShaderProgramHandle);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, targetTextureHandle);
            GL.Uniform1(targetTextureHandle, 0);
            int time  = GL.GetUniformLocation(quadShaderProgramHandle, "time");
            GL.Uniform1(time, DateTime.Now.Second);
        }
        public static void Load(Size clientSize) {
            LoadTextures();
            CreateVertexArray();
            modelKey = bufferService.Buffer( ModelWorker.GetIndexedModelWithTangents(BlenderLoader.Load(CYLINDER_MODEL_FILE)));

            BufferQuadVertices();
            SetTargetTexture();

            sceneContext = new RenderContext(VertexShader.Text, FragmentShader.Text);
            quadShaderProgramHandle = ShaderProgram.Create(RTTVertexShader.Text, RTTFragmentShader.Text);
            BindShaderData(clientSize);
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
        private static void BufferQuadVertices() {
            GL.CreateBuffers(1, out quadVertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVertexBuffer);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(QuadVertexBufferData.Vertices.Length * Vector3.SizeInBytes), QuadVertexBufferData.Vertices, BufferUsageHint.StaticDraw);
        }
        private static void SetTargetTexture() {
            targetTextureHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, targetTextureHandle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 1024, 768, 0, PixelFormat.Rgb, PixelType.UnsignedByte,IntPtr.Zero);
            GL.TextureParameter(targetTextureHandle, TextureParameterName.TextureMagFilter,(int) TextureMagFilter.Nearest);
            GL.TextureParameter(targetTextureHandle, TextureParameterName.TextureMinFilter,(int) TextureMinFilter.Nearest);
            SetFrameBuffer();
            SetDepthBuffer();
        }
        private static void SetFrameBuffer() {
            targetFrameBuffer = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, targetFrameBuffer);

            //might ve to be done after depthbuffer
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, targetTextureHandle, 0);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete) {
                throw new Exception("Framebuffer incomplete");
            }
        }
        private static void SetDepthBuffer() {
            depthRenderBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthRenderBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, 1024, 768);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.Depth, RenderbufferTarget.Renderbuffer, depthRenderBuffer);
        }
        private static void BindShaderData(Size clientSize) {
            float aspectRatio = clientSize.Width / (float)(clientSize.Height);
            Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, aspectRatio, 0.1f, 100, out projectionMatrix);
            viewMatrix = Matrix4.LookAt(new Vector3(4, 3, -3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            modelMatrix = Matrix4.Identity;

            projectionMatrixLocation = sceneContext.SetUniform4(ref projectionMatrix, "projection_matrix");
            viewMatrixLocation = sceneContext.SetUniform4(ref viewMatrix, "view_matrix");
            modelMatrixLocation = sceneContext.SetUniform4(ref modelMatrix, "model_matrix");
            lightColorUniformLocation = sceneContext.SetUniform3(lightColorUniform, "light_color");
            lightPositionUniformLocation = sceneContext.SetUniform3(lightPositionUniform, "light_position_worldspace");
            lightPowerUniformLocation = sceneContext.SetUniform1(lightPowerUniform, "light_power");
            sceneContext.AddTexture(textureHandle);
            sceneContext.AddTexture(normalMapHandle);
            sceneContext.AddTexture(specularMapHandle);
        }
        public static void Update(ControllerData data) {
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(data.FOV), 4 / 3, 0.1f, 100, out projectionMatrix);
            Vector3 up = Vector3.Cross(data.Right, data.Direction);
            viewMatrix = Matrix4.LookAt(data.Position, data.Position + data.Direction, up);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);
        }
        public static void Draw() {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer,targetFrameBuffer);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            DrawSceneToFrameBuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            DrawFrameBufferToQuad();
        }
        private static void DrawSceneToFrameBuffer() {
            int[] indices = bufferService.GetModel<IndexedD3Model2>(modelKey).Indices;
            ActivateSceneDraw();
            int offset = bufferService.EnableAttributes<IndexedD3Model2>(modelKey,0);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, indices);
            offset = bufferService.DisableAttributes<IndexedD3Model2>(offset);
        }
        private static void DrawFrameBufferToQuad() {
            ActivateQuadDraw();
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVertexBuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.DisableVertexAttribArray(0);
        }
        public static void Clear() {
            bufferService.DisposeBuffers();
            sceneContext.DisposeAll();
            GL.DeleteProgram(quadShaderProgramHandle);
            GL.DeleteVertexArray(vertexArrayHandle);
        }
    }
}
