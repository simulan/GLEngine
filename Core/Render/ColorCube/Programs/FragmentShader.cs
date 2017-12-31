using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.ColorCube.Programs {
    public class FragmentShader {
        public static readonly string Text = @"
            #version 330 core
            in vec3 fragmentColor;
            out vec3 color;
            void main(){
                color = vec3(1,0,0);
            }
        ";
    }
    //              color = fragmentColor;

}
