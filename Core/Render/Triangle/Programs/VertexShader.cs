using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Triangle.Programs {
    public class VertexShader {
        public static readonly string Text = @"
            #version 330 core
            layout(location = 0) in vec3 vertexPosition_modelspace;
            uniform mat4 projection_matrix;
            uniform mat4 view_matrix;
            uniform mat4 model_matrix;

            void main(){
              gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition_modelspace, 1);
            }
        ";
    }
}
