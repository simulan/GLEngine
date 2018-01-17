using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace UMLProgram.Core.Render.LightMap {
    public partial class LightMapRenderer {
        private static Matrix4 projectionMatrix, modelMatrix, viewMatrix;
        private static int modelKey,
            lightMapHandle,
            vertexArrayHandle,
            projectionMatrixLocation,
            modelMatrixLocation,
            viewMatrixLocation;
        private const string LIGHTMAP_FILE = "C:\\Work\\My CSharp\\UMLProgram\\lightmap.tga";
        private const string MODEL_FILE = "C:\\Work\\My CSharp\\UMLProgram\\lightmap.obj";
    }
}
