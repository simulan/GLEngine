using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.RenderToTexture.Programs {
    public class VertexShader {
        public static readonly string Text = @"
            #version 400 core
            layout(location = 0) in vec3 vertexPosition_modelspace;
            layout(location = 1) in vec2 vertexUV;
            layout(location = 2) in vec3 vertexNormal_modelspace;
            layout(location = 3) in vec3 vertexTangent_modelspace;
            layout(location = 4) in vec3 vertexBitangent_modelspace;

            uniform mat4 projection_matrix;
            uniform mat4 view_matrix;
            uniform mat4 model_matrix;
            uniform vec3 light_color;
            uniform float light_power;
            uniform vec3 light_position_worldspace;
            
            out vec2 UV;
            out vec3 Position_worldspace;
            out vec3 EyeDirection_tangentspace;
            out vec3 LightDirection_tangentspace;

            void main(){
                mat4 MV4x4 = view_matrix * model_matrix;
                gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition_modelspace, 1);

                mat3 MV3x3 = mat3(MV4x4);
                vec3 vertexNormal_cameraspace = MV3x3 * normalize(vertexNormal_modelspace);
                vec3 vertexTangent_cameraspace = MV3x3 * normalize(vertexTangent_modelspace);
                vec3 vertexBitangent_cameraspace = MV3x3 * normalize(vertexBitangent_modelspace);

                mat3 TBN = transpose(mat3(
                    vertexTangent_cameraspace,
                    vertexBitangent_cameraspace,
                    vertexNormal_cameraspace
                ));

                Position_worldspace = (model_matrix * vec4(vertexPosition_modelspace, 1)).xyz;
                vec3 vertexPosition_cameraspace = (view_matrix * model_matrix * vec4(vertexPosition_modelspace,1)).xyz;
                vec3 EyeDirection_cameraspace = vec3(0,0,0) - vertexPosition_cameraspace;
                vec3 LightPosition_cameraspace = (view_matrix * vec4(light_position_worldspace,1)).xyz;
                vec3 LightDirection_cameraspace = LightPosition_cameraspace + EyeDirection_cameraspace;
                vec3 Normal_cameraspace = (view_matrix * model_matrix * vec4(vertexNormal_modelspace,0)).xyz; 

                LightDirection_tangentspace = TBN * LightDirection_cameraspace;
                EyeDirection_tangentspace = TBN * EyeDirection_cameraspace;

                UV = vertexUV;
            }
        ";
    }
}
