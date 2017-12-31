using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Triangle.Programs {
    public class FragmentShader {
        public static readonly string Text = @"
            #version 330 core
            out vec3 color;
            void main(){
              color = vec3(1,0,0);
            }
        ";
    }
}
