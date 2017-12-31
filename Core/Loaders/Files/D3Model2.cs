using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Loaders.Files {
    public class D3Model2 : D3Model {
        public Vector3[] Tangents;
        public Vector3[] Bitangents;

        public D3Model2() {}

        public D3Model2(int capacity) : base(capacity) {
            Tangents = new Vector3[capacity];
            Bitangents = new Vector3[capacity];
        }
    }
}
