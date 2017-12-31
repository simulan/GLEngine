using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.SimpleObject.Programs {
    public class VertexShader {
        public static readonly string Text = @"
            #version 400 core
            layout(location = 0) in vec3 vertexPosition_modelspace;
            layout(location = 1) in vec2 vertexUV;
            layout(location = 2) in vec3 vertexNormal_modelspace;
            uniform mat4 projection_matrix;
            uniform mat4 view_matrix;
            uniform mat4 model_matrix;
            uniform vec3 light_color;
            uniform int light_power;
            uniform vec3 light_position_worldspace;
            
            out vec2 UV;
            out vec3 Position_worldspace;
            out vec3 EyeDirection_cameraspace;
            out vec3 LightDirection_cameraspace;
            out vec3 Normal_cameraspace;            

            void main(){
                gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition_modelspace, 1);

                Position_worldspace = (model_matrix * vec4(vertexPosition_modelspace, 1)).xyz;
                vec3 vertexPosition_cameraspace = ( view_matrix * model_matrix * vec4(vertexPosition_modelspace,1)).xyz;
                EyeDirection_cameraspace = vec3(0,0,0) - vertexPosition_cameraspace;
                vec3 LightPosition_cameraspace = ( view_matrix * vec4(light_position_worldspace,1) ).xyz;
                LightDirection_cameraspace = LightPosition_cameraspace + EyeDirection_cameraspace;

                Normal_cameraspace = ( view_matrix * model_matrix * vec4(vertexNormal_modelspace,0) ).xyz; 
                UV = vertexUV;
            }
        ";
    }
}
