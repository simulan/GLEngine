using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Loaders.Files {
    public class BMP {
        public Data Header;
        public byte[] Buffer;

        public class Data {
            public int DataPosition { get; set; }
            public int ImageSize { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
