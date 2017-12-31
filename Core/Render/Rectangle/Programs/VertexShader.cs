using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Rectangle.Programs {
    public class VertexShader {
        public static readonly string Text = @"
            #version 130
            precision highp float;
            uniform mat4 projection_matrix;
            uniform mat4 modelview_matrix;
            in vec3 in_position;

            void main(void)
            {
              //works only for orthogonal modelview
              gl_Position = projection_matrix * modelview_matrix * vec4(in_position, 1);
            }";
    }
}