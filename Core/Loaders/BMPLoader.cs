using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using UMLProgram.Core.Loaders.Files;

namespace UMLProgram.Core.Loaders {
    public class BMPLoader {
        private const int FILECODE_SIZE = 4;
        private const int HEADER_SIZE = 54;

        public static int Load(string file) {
            FileStream stream = new FileStream(file, FileMode.Open);
            ValidateBMPFileStream(stream);
            BMP bmp = new BMP();
            bmp.Header = GetHeaderFromBytes(GetHeaderInBytes(stream));
            bmp.Buffer = GetBufferInBytes(stream,bmp.Header.ImageSize);
            stream.Close();
            return LoadTexture(bmp);
        }
        private static int LoadTexture(BMP bmp) {
            int textureID;
            GL.GenTextures(1,out textureID);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, bmp.Header.Width, bmp.Header.Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte,(byte[]) bmp.Buffer);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return textureID;
        }
        private static BMP.Data GetHeaderFromBytes(byte[] header) {
            BMP.Data result = new BMP.Data();
            result.DataPosition = BitConverter.ToInt32(header, 0x0A);
            result.ImageSize = BitConverter.ToInt32(header, 0x1E);
            result.Width = BitConverter.ToInt32(header, 0x0E);
            result.Height = BitConverter.ToInt32(header, 0x12);
            if (result.DataPosition == 0) result.DataPosition = 54;
            if (result.ImageSize == 0) result.ImageSize = result.Width * result.Height * 3;
            return result;
        }
        private static byte[] GetBufferInBytes(FileStream stream, int size) {
            byte[] buffer = new byte[size];
            int bufferResult = stream.Read(buffer, 0, size);
            if (bufferResult < 1) {
                throw new IOException("can not read buffer " + buffer.Length + " bytes");
            }
            return buffer;
        }
        private static byte[] GetHeaderInBytes(FileStream stream) {
            byte[] result = new byte[HEADER_SIZE];
            int bytesRead = stream.Read(result, 0, HEADER_SIZE);
            if (bytesRead < 1) {
                throw new IOException("can not read header (" + HEADER_SIZE + " bytes)");
            }
            return result;
        }
        private static void ValidateBMPFileStream(FileStream stream) {
            if (stream.CanRead) {
                if (!IsBMP(stream)) {
                    throw new IOException("only .bmp filecode is supported.");
                }
            } else {
                throw new IOException(stream.Name + " does not support reading atm.");
            }
        }
        private static bool IsBMP(FileStream stream) {
            byte[] fileCodeInBytes = new byte[FILECODE_SIZE];
            int result = stream.Read(fileCodeInBytes, 0, FILECODE_SIZE);
            if (result > 0) {
                char[] fileCode = new char[FILECODE_SIZE];
                for (int i = 0; i < FILECODE_SIZE; i++) {
                    fileCode[i] = Convert.ToChar(fileCodeInBytes[i]);
                }
                String fullFileCode = new string(fileCode);
                if (fullFileCode.ToLower().Contains("bm")) {
                    return true;
                } else {
                    return false;
                }
            } else {
                throw new IOException("can not read first " + FILECODE_SIZE + " bytes");
            }
        }
    }
}
