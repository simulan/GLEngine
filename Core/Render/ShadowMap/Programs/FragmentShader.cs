using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.ShadowMap.Programs {
    public class FragmentShader {
        public static readonly string Text = @"
            #version 400 core
            layout(location = 0) out vec3 color;

            uniform vec3 light_color;
            uniform float light_power;
            uniform vec3 light_position_worldspace;
            
            in vec2 UV;
            in vec3 Position_worldspace;
            in vec3 EyeDirection_cameraspace;
            in vec3 LightDirection_cameraspace;
            in vec3 Normal_cameraspace;

            void main(){
                vec3 materialColor = vec3(0.8,0.4,0.4);
                
                vec3 n = normalize( Normal_cameraspace );
                vec3 l = normalize( LightDirection_cameraspace);
                float cosTheta = clamp( dot(n,l), 0, 1);
                color = materialColor * light_color * cosTheta;
                
            }
        ";
    }

}
