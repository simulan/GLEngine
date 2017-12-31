using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.TexturedCube.Programs {
    public class FragmentShader {
        public static readonly string Text = @"
        #version 330 core
        in vec2 UV;
        out vec3 color;
        uniform sampler2D myTextureSampler;

        void main(){
            // Output color = color of the texture at the specified UV
            color = texture( myTextureSampler, UV ).rgb;
        }  
        ";
    }
}
