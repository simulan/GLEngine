﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using UMLProgram.Core.Render.Common;
using UMLProgram.Core.Loaders;
using UMLProgram.Core.Loaders.Files;
using System.Drawing;
using UMLProgram.Core.Render.NormalMap.programs;
using static UMLProgram.Core.Input.Controller;

/*
 * Note that used texture & normalmap or bmp/dds
 * tga would have yielded greater quality
 */
namespace UMLProgram.Core.Render.NormalMap {
    public class NormalMapRenderer {
        private static ModelBuffer2 modelBuffer = new ModelBuffer2();
        private static Matrix4 projectionMatrix, viewMatrix, modelMatrix;
        private static Vector3 lightColorUniform = new Vector3(1.0f, 1.0f, 1.0f);
        private static Vector3 lightPositionUniform = new Vector3(5, 0, 0);
        private static int modelKey;
        private static float lightPowerUniform = 60.0f;
        private static int textureHandle,
            normalMapHandle,
            vertexArrayHandle,
            shaderProgramHandle,
            projectionMatrixLocation,
            modelMatrixLocation,
            viewMatrixLocation,
            lightColorUniformLocation,
            lightPowerUniformLocation,
            lightPositionUniformLocation,
            specularMapHandle;

        public static void Activate() {
            GL.UseProgram(shaderProgramHandle);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            GL.Uniform1(textureHandle, 0);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, normalMapHandle);
            GL.Uniform1(normalMapHandle, 1);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, specularMapHandle);
            GL.Uniform1(specularMapHandle, 2);
        }
        public static void Load(Size clientSize) {
            LoadTexture();
            CreateVertexArray();
            modelKey = modelBuffer.Add(LoadObj());
            shaderProgramHandle = ShaderProgram.Create(VertexShader.Text, FragmentShader.Text);
            BindShaderData(clientSize);
        }
        private static void CreateVertexArray() {
            vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayHandle);
        }
        private static void LoadTexture() {
            String file = "C:\\Work\\My CSharp\\UMLProgram\\diffuse.dds";
            String normalMap = "C:\\Work\\My CSharp\\UMLProgram\\normal.bmp";
            String specularMap = "C:\\Work\\My CSharp\\UMLProgram\\specular.dds";
            textureHandle = DDSLoader.Load(file);
            normalMapHandle = BMPLoader.Load(normalMap);
            specularMapHandle = DDSLoader.Load(specularMap);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            GL.Uniform1(textureHandle, 0);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, normalMapHandle);
            GL.Uniform1(normalMapHandle, 1);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, specularMapHandle);
            GL.Uniform1(specularMapHandle, 2);
        }
        private static IndexedD3Model2 LoadObj() {
            String file = "C:\\Work\\My CSharp\\UMLProgram\\cylinder.obj";
            D3Model mod = BlenderLoader.Load(file);
            IndexedD3Model2 m =  ModelWorker.GetIndexedModelWithTangents(mod);
            return m;
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
        public static void Update(ControllerData data) {
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(data.FOV), 4 / 3, 0.1f, 100, out projectionMatrix);
            Vector3 up = Vector3.Cross(data.Right, data.Direction);
            viewMatrix = Matrix4.LookAt(data.Position, data.Position + data.Direction, up);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);
        }
        public static void Draw() {
            Activate();
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, modelBuffer[modelKey].Item2.vertex);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, modelBuffer[modelKey].Item2.uv);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
            GL.EnableVertexAttribArray(2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, modelBuffer[modelKey].Item2.normal);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.EnableVertexAttribArray(3);
            GL.BindBuffer(BufferTarget.ArrayBuffer, modelBuffer[modelKey].Item2.tan);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.EnableVertexAttribArray(4);
            GL.BindBuffer(BufferTarget.ArrayBuffer, modelBuffer[modelKey].Item2.bitan);
            GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.DrawElements(PrimitiveType.Triangles, modelBuffer[modelKey].Item1.Indices.Length, DrawElementsType.UnsignedInt, modelBuffer[modelKey].Item1.Indices);
            GL.DisableVertexAttribArray(4);
            GL.DisableVertexAttribArray(3);
            GL.DisableVertexAttribArray(2);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);

        }
        private static void DrawDebugNormals() {
            GL.UseProgram(0);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 MV = viewMatrix * modelMatrix;
            GL.LoadMatrix(ref MV);
            GL.Begin(PrimitiveType.Lines);
            for (int i = 0; i < modelBuffer[modelKey].Item1.Indices.Length; i++) {
                int index = modelBuffer[modelKey].Item1.Indices[i];
                Vector3 p = modelBuffer[modelKey].Item1.Vertices[index];
                GL.Vertex3(p);
                Vector3 o = Vector3.Normalize(modelBuffer[modelKey].Item1.Normals[index]);
                p += o * 0.1f;
                GL.Vertex3(p);
            }
            GL.End();
        }
        public static void Clear() {
            GL.DeleteBuffers(3, new int[] { modelBuffer[modelKey].Item2.vertex, modelBuffer[modelKey].Item2.uv, modelBuffer[modelKey].Item2.normal });
            GL.DeleteTexture(textureHandle);
            GL.DeleteProgram(shaderProgramHandle);
            GL.DeleteVertexArray(vertexArrayHandle);
        }
    }
}
