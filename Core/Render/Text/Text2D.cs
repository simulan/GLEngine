using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Text {
    interface Text2D {
        void Load(string texturePath);
        void Print(string text, int x, int y, int size);
        void Clear();
    }
}
