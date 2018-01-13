using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using UMLProgram.Core.Loaders.Files;
using System.Diagnostics;

namespace UMLProgram.Core.Loaders {
    public class TGALoader {
        private const int HEADER_SIZE = 18;

        public static int Load(string path) {
            FileStream stream = new FileStream(path,FileMode.Open,FileAccess.Read);
            ValidateTGAStream(stream);
            TGA image = new TGA();
            image.Header = GetHeaderFromBytes(stream);
            image.Buffer = GetBufferInBytes(stream, CalculateBodySize(image.Header));

            stream.Close();
            int textureHandle ;
            GL.GenTextures(1, out textureHandle);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Header.Width, image.Header.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Buffer);
            return textureHandle;
        }
        public static TGA.Data GetHeaderFromBytes(FileStream stream) {
            byte[] bytes = new byte[HEADER_SIZE];
            int result = stream.Read(bytes,0, HEADER_SIZE);
            if (result > 0) {
                TGA.Data header = new TGA.Data();
                header.IdLength = bytes[0];
                header.ColorMapType = bytes[1];
                header.ImageType = bytes[2];
                header.ColorMap = new TGA.Data.ColorMapData(
                    BitConverter.ToInt16(bytes, 3),
                    BitConverter.ToInt16(bytes, 5),
                    bytes[7]);
                header.XOrigine = BitConverter.ToInt16(bytes, 8);
                header.YOrigine = BitConverter.ToInt16(bytes, 10);
                header.Width = BitConverter.ToInt16(bytes, 12);
                header.Height = BitConverter.ToInt16(bytes, 14);
                header.BitsPerPixel = bytes[16];
                header.ImageDescriptor = bytes[17];
                return header;
            }
            throw new IOException("File did not have TGA Header bytes");
        }
        public static int CalculateBodySize(TGA.Data header) {
            return header.Height * header.Width * (header.BitsPerPixel / 8);
        }
        public static byte[] GetBufferInBytes(FileStream stream, int size) {
            byte[] body = new byte[size];
            int result = stream.Read(body, 0, size);
            if (result == 0) {
                throw new IOException("can't calculate & read body size.");
            }
            return body;
        }
        private static void ValidateTGAStream(FileStream stream) {
            if (!stream.CanRead) {
                throw new IOException("can't read file at this moment.");
            }
        }
    }
}
