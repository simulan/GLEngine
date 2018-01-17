using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.ShadowMap {
    public partial class ShadowMapRenderer {
        private const string EXTRA_FILE = "C:\\Work\\My CSharp\\UMLProgram\\specular.dds";
        private const string MAP_MODEL_FILE = "C:\\Work\\My CSharp\\UMLProgram\\lightmap.obj";
        private Vector3 lightColor = new Vector3(0.8f,0.8f,0.8f);
        private Vector3 lightPosition = new Vector3(5, 0, 0);
        private Matrix4 depthMVP;
        private int lightPower = 40;
        private int modelKey;
        private int textureHandle,
            framebufferHandle,
            quadVertexBuffer,
            vertexBuffer,
            depthMVPLocation,
            drawBufferProgramHandle,
            lightColorUniformLocation,
            lightPositionUniformLocation,
            lightPowerUniformLocation,
            depthRenderBuffer,
            depthTexture;
        
    }
}
