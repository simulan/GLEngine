using OpenTK;

namespace UMLProgram.Core.Render.RenderToTexture {
    public partial class RTTRenderer {
        private static Matrix4 projectionMatrix, viewMatrix, modelMatrix;
        private static Vector3 lightColorUniform = new Vector3(1.0f, 1.0f, 1.0f);
        private static Vector3 lightPositionUniform = new Vector3(5, 0, 0);
        private static int modelKey;
        private static float lightPowerUniform = 60.0f;
        private static int textureHandle,
            quadVertexBuffer,
            depthRenderBuffer,
            targetFrameBuffer,
            targetTextureHandle,
            normalMapHandle,
            vertexArrayHandle,
            projectionMatrixLocation,
            modelMatrixLocation,
            viewMatrixLocation,
            lightColorUniformLocation,
            lightPowerUniformLocation,
            lightPositionUniformLocation,
            quadShaderProgramHandle,
            specularMapHandle;
        private const string TEXTURE_FILE = "C:\\Work\\My CSharp\\UMLProgram\\diffuse.dds";
        private const string NORMALMAP_FILE = "C:\\Work\\My CSharp\\UMLProgram\\normal.bmp";
        private const string SPECULARMAP_FILE = "C:\\Work\\My CSharp\\UMLProgram\\specular.dds";
        private const string CYLINDER_MODEL_FILE = "C:\\Work\\My CSharp\\UMLProgram\\cylinder.obj";

    }
}
