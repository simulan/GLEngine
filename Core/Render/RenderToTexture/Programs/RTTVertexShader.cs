using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.RenderToTexture.Programs {
    public class RTTVertexShader {
        public static readonly string Text = @"
            #version 400 core
            layout(location = 0) in vec3 vertexPosition_modelspace;

            out vec2 UV;

            void main(){
	            gl_Position =  vec4(vertexPosition_modelspace,1);
	            UV = (vertexPosition_modelspace.xy+vec2(1,1))/2.0;
            }
        ";
    }
}
