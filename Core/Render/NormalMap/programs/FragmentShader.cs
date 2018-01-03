using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.NormalMap.programs {
    public class FragmentShader {
        public static readonly string Text = @"
            #version 400 core
            uniform vec3 light_color;
            uniform float light_power;
            uniform sampler2D myTextureSampler;
            uniform sampler2D myNormalMap;
            uniform sampler2D mySpecularMap;
            uniform vec3 light_position_worldspace;
            in vec2 UV;
            in vec3 Position_worldspace;
            in vec3 EyeDirection_tangentspace;
            in vec3 LightDirection_tangentspace;
            in vec3 Normal_tangentspace;            
            out vec3 color;

            void main(){
                //UV.y might need to be -UV.y , can't see as texture is symmetrical
                vec3 n = normalize( texture( myNormalMap, vec2(UV.x,UV.y) ).rgb*2.0 - 1.0 );
                vec3 l = normalize( LightDirection_tangentspace );
                vec3 E = normalize( EyeDirection_tangentspace );
                vec3 R = reflect(-l,n);

                float distance = distance(light_position_worldspace , Position_worldspace);           
                float cosTheta = clamp( dot( n,l ), 0,1 );
                float cosAlpha = clamp( dot( E,R ), 0,1 );

                vec3 materialColor = texture( myTextureSampler, UV ).rgb;
                vec3 diffuseColor = materialColor * light_color * light_power * cosTheta / (distance * distance);
                vec3 ambientColor = materialColor * vec3(0.1, 0.1, 0.1);
                vec3 specularColor = texture( mySpecularMap, UV).rgb * light_color * light_power * pow(cosAlpha,5) / (distance * distance);
                color = diffuseColor+ambientColor+specularColor;
            }
        ";
    }//+ambientColor+specularColor
}
