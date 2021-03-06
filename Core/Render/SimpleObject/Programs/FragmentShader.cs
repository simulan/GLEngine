﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.SimpleObject.Programs {
    public class FragmentShader {
        public static readonly string Text = @"
            #version 400 core
            uniform vec3 light_color;
            uniform int light_power;
            uniform sampler2D myTextureSampler;
            uniform vec3 light_position_worldspace;
            
            in vec2 UV;
            in vec3 Position_worldspace;
            in vec3 EyeDirection_cameraspace;
            in vec3 LightDirection_cameraspace;
            in vec3 Normal_cameraspace;            
            
            out vec3 color;

            void main(){
                vec3 n = normalize( Normal_cameraspace );
                vec3 l = normalize( LightDirection_cameraspace );
                vec3 E = normalize( EyeDirection_cameraspace );
                vec3 R = reflect(-l,n);

                float distance = distance(Position_worldspace,light_position_worldspace);           
                float cosTheta = clamp( dot( E,R ), 0,1 );
                vec3 materialColor = texture( myTextureSampler, UV ).rgb;
                vec3 diffuseColor = materialColor * light_color * light_power * cosTheta / distance * distance;
                vec3 ambientColor = materialColor * vec3(0.3, 0.3, 0.3);
                vec3 specularColor = materialColor * light_color * light_power * pow(cosTheta,2) / (distance * distance);
                color = diffuseColor+ambientColor+specularColor;
            }
        ";
    } 
}
