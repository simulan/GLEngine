using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Loaders.Files {
    public class DDS {
        public Data header { get; set; }
        public byte[] Buffer { get; set; }

        public class Data {
            public const int FOURCC_DXT1 = 0x31545844;
            public const int FOURCC_DXT3 = 0x33545844;
            public const int FOURCC_DXT5 = 0x35545844;

            public Data(int height,int width,int linearSize,int mipMapCount,int fourCC) {
                this.Height = height;
                this.Width = width;
                this.LinearSize = linearSize;
                this.MipMapCount = mipMapCount;
                this.FourCC = fourCC;
            }

            public int Height { get; set; }
            public int Width { get; set; }
            public int LinearSize { get; set; }
            public int MipMapCount { get; set; }
            public int FourCC { get; set; }
            public int Format { get; set; }
        }
    }
}
