using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.ShadowMap.Programs {
    public class BufferVertexShader {
        public static readonly string Text = @"
        #version 400 core
        layout(location = 0) in vec3 vertexPosition_modelspace;
        uniform mat4 depthMVP;

        void main(){
            gl_Position =  depthMVP * vec4(vertexPosition_modelspace,1);
        }
        ";
    }
}
