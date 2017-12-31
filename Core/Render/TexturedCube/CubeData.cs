using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.TexturedCube {
    public class CubeData {
        public class Texture {
            public static readonly Vector2[] UVs = {
                new Vector2(0.000059f, 1.0f-0.000004f),
                new Vector2(    0.000103f, 1.0f-0.336048f),
                new Vector2(    0.335973f, 1.0f-0.335903f),
                new Vector2(1.000023f, 1.0f-0.000013f),
                new Vector2(0.667979f, 1.0f-0.335851f),
                new Vector2(0.999958f, 1.0f-0.336064f),
                new Vector2(0.667979f, 1.0f-0.335851f),
                new Vector2(0.336024f, 1.0f-0.671877f),
                new Vector2(0.667969f, 1.0f-0.671889f),
                new Vector2(1.000023f, 1.0f-0.000013f),
                new Vector2(0.668104f, 1.0f-0.000013f),
                new Vector2(0.667979f, 1.0f-0.335851f),
                new Vector2(0.000059f, 1.0f-0.000004f),
                new Vector2(0.335973f, 1.0f-0.335903f),
                new Vector2(0.336098f, 1.0f-0.000071f),
                new Vector2(0.667979f, 1.0f-0.335851f),
                new Vector2(0.335973f, 1.0f-0.335903f),
                new Vector2(0.336024f, 1.0f-0.671877f),
                new Vector2(1.000004f, 1.0f-0.671847f),
                new Vector2(0.999958f, 1.0f-0.336064f),
                new Vector2(0.667979f, 1.0f-0.335851f),
                new Vector2(0.668104f, 1.0f-0.000013f),
                new Vector2(0.335973f, 1.0f-0.335903f),
                new Vector2(0.667979f, 1.0f-0.335851f),
                new Vector2(0.335973f, 1.0f-0.335903f),
                new Vector2(0.668104f, 1.0f-0.000013f),
                new Vector2(0.336098f, 1.0f-0.000071f),
                new Vector2(0.000103f, 1.0f-0.336048f),
                new Vector2(0.000004f, 1.0f-0.671870f),
                new Vector2(0.336024f, 1.0f-0.671877f),
                new Vector2(0.000103f, 1.0f-0.336048f),
                new Vector2(0.336024f, 1.0f-0.671877f),
                new Vector2(0.335973f, 1.0f-0.335903f),
                new Vector2(0.667969f, 1.0f-0.671889f),
                new Vector2(1.000004f, 1.0f-0.671847f),
                new Vector2(0.667979f, 1.0f-0.335851f)
            };
        }
        public static readonly Vector3[] Vertices = {
            new Vector3(-1.0f,-1.0f,-1.0f),
            new Vector3(-1.0f,-1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f,-1.0f),
            new Vector3(-1.0f,-1.0f,-1.0f),
            new Vector3(-1.0f, 1.0f,-1.0f),
            new Vector3(1.0f,-1.0f, 1.0f),
            new Vector3(-1.0f,-1.0f,-1.0f),
            new Vector3(1.0f,-1.0f,-1.0f),
            new Vector3(1.0f, 1.0f,-1.0f),
            new Vector3(1.0f,-1.0f,-1.0f),
            new Vector3(-1.0f,-1.0f,-1.0f),
            new Vector3(-1.0f,-1.0f,-1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f,-1.0f),
            new Vector3(1.0f,-1.0f, 1.0f),
            new Vector3(-1.0f,-1.0f, 1.0f),
            new Vector3(-1.0f,-1.0f,-1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f,-1.0f, 1.0f),
            new Vector3(1.0f,-1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f,-1.0f,-1.0f),
            new Vector3(1.0f, 1.0f,-1.0f),
            new Vector3(1.0f,-1.0f,-1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f,-1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f,-1.0f),
            new Vector3(-1.0f, 1.0f,-1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f,-1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f,-1.0f, 1.0f)
        };

    }
}
