using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.LightMap.programs {
    public class FragmentShader {
        public static readonly string Text = @"
            #version 400 core
            uniform sampler2D myTextureSampler;

            in vec2 UV;
            out vec3 color;

            void main(){
	            color = texture( myTextureSampler, UV, -2.0 ).rgb;
            }
        ";
    }
}
