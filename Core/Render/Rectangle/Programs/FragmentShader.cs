using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Rectangle.Programs {
    public class FragmentShader {
        public static readonly string Text = @"
            #version 130
            precision highp float;
            const vec4 lightColor = vec4(0.9, 0.4, 0.7,1);
            out vec4 out_frag_color;
            void main(void)
            {
              out_frag_color = lightColor;
            }";
    }
}
