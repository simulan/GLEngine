using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.NormalMap {
    public partial class NormalMapRenderer {
        private static Matrix4 projectionMatrix, viewMatrix, modelMatrix;
        private static Vector3 lightColorUniform = new Vector3(1.0f, 1.0f, 1.0f);
        private static Vector3 lightPositionUniform = new Vector3(5, 0, 0);
        private static int modelKey;
        private static float lightPowerUniform = 60.0f;
        private static int textureHandle,
                           normalMapHandle,
                           specularMapHandle,
                           vertexArrayHandle,
                           projectionMatrixLocation,
                           modelMatrixLocation,
                           viewMatrixLocation,
                           lightColorUniformLocation,
                           lightPowerUniformLocation,
                           lightPositionUniformLocation;
        private const String TEXTURE_FILE = "C:\\Work\\My CSharp\\UMLProgram\\diffuse.dds";
        private const String NORMALMAP_FILE = "C:\\Work\\My CSharp\\UMLProgram\\normal.bmp";
        private const String SPECULARMAP_FILE = "C:\\Work\\My CSharp\\UMLProgram\\specular.dds";
        private const String CYLINDER_MODEL_FILE = "C:\\Work\\My CSharp\\UMLProgram\\cylinder.obj";
    }
}
