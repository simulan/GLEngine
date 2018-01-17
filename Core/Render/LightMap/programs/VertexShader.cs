using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.LightMap.programs {
    public class VertexShader {
        public static readonly string Text = @"
            #version 400 core
            layout(location = 0) in vec3 vertexPosition_modelspace;
            layout(location = 1) in vec2 vertexUV;
            layout(location = 2) in vec3 vertexNormal;
    
            uniform mat4 projection_matrix;
            uniform mat4 view_matrix;
            uniform mat4 model_matrix;
                        
            out vec2 UV;

            void main(){
	            gl_Position =  projection_matrix * view_matrix * model_matrix * vec4(vertexPosition_modelspace,1);
	            UV = vertexUV;
            }
        ";
    }
}
