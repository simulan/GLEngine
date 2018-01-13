using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Format by https://en.wikipedia.org/wiki/Truevision_TGA
namespace UMLProgram.Core.Loaders.Files {
    public class TGA {
        public Data Header;
        public byte[] Buffer;

        public class Data {
            public int IdLength { get; set; }
            public int ColorMapType { get; set; }
            public int ImageType { get; set; }
            public ColorMapData ColorMap { get; set; }
            public int XOrigine { get; set; }
            public int YOrigine { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int BitsPerPixel { get; set; }
            public int ImageDescriptor { get; set; }

            public class ColorMapData {
                public int IndexOffset { get; set; }
                public int Entries { get; set; }
                public int BitsPerPixel { get; set; }

                public ColorMapData(int offset,int length,int pixelBits) {
                    this.IndexOffset = offset;
                    this.Entries = length;
                    this.BitsPerPixel = pixelBits;
                }
            }
        }
    }
}
