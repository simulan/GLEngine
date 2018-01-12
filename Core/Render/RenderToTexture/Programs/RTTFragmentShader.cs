using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.RenderToTexture.Programs {
    public class RTTFragmentShader {
        public static readonly string Text = @"
            #version 400 core            
            uniform sampler2D renderedTexture;
            uniform float time;

            in vec2 UV;
            out vec3 color;

            void main(){
	            color = texture( renderedTexture, UV  ).xyz ;
            }        
        ";
    }
}

