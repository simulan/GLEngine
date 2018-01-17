using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.ShadowMap.Programs {
    public class BufferFragmentShader {
        public static readonly string Text = @"
            #version 400 core
            layout(location = 0) out float fragmentdepth;

            void main(){
                // Not really needed, OpenGL does it anyway
                fragmentdepth = gl_FragCoord.z;
            }
        ";
    }
}
