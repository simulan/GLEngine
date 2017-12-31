using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Loaders.Files;

namespace UMLProgram.Core.Loaders {
    public class DDSLoader {
        private const int FILECODE_SIZE = 4;
        private const int HEADER_SIZE = 124;

        public static int Load(string filename) {
            FileStream stream = new FileStream(filename,FileMode.Open);
            ValidateDDSFileStream(stream);
            DDS dds = new DDS();
            dds.header = GetHeaderFromBytes(GetHeaderInBytes(stream));
            dds.Buffer = GetBufferInBytes(stream, CalculateBufferSize(dds.header));
            stream.Close();
            int textureHandle = -1;
            GL.GenTextures(1, out textureHandle);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            LoadMipMaps(dds);
            return textureHandle;
        }
        private static byte[] GetHeaderInBytes(FileStream stream) {
            byte[] result = new byte[HEADER_SIZE];
            int bytesRead = stream.Read(result,0,HEADER_SIZE);
            if (bytesRead < 1) {
                throw new IOException("can not read header ("+HEADER_SIZE+" bytes)");
            }
            return result;
        }
        private static int CalculateBufferSize(DDS.Data header) {
            return header.MipMapCount > 1 ? header.LinearSize * 2 : header.LinearSize;
        }
        private static byte[] GetBufferInBytes(FileStream stream,int size) {
            byte[] buffer = new byte[size];
            int bufferResult = stream.Read(buffer, 0, size);
            if (bufferResult < 1) {
                throw new IOException("can not read buffer " + buffer.Length + " bytes");
            }
            return buffer;
        }
        private static void ValidateDDSFileStream(FileStream stream) {
            if (stream.CanRead) {
                if (!IsDDS(stream)) {
                    throw new IOException("only .dds filecode is supported.");
                }
            } else {
                throw new IOException(stream.Name + " does not support reading atm.");
            }
        }
        //warning should define MAX_MIPMAP_LEVELS AND MIN HERE
        private static void LoadMipMaps(DDS file) {
            int blockSize = (file.header.Format==(int)PixelInternalFormat.CompressedRgbaS3tcDxt1Ext) ? 8 : 16;
            int offset = 0;
            for (int level = 0; level < file.header.MipMapCount; level++) {
                int size = ((file.header.Width+3)/4)*((file.header.Height+3)/4)*blockSize;
                PixelInternalFormat format = GetDDSFormat(file.header);
                GL.CompressedTexImage2D(TextureTarget.Texture2D, level, format, file.header.Width, file.header.Height,0,size,file.Buffer);
                offset += size;
                file.header.Width /= 2;
                file.header.Height /= 2;
            }
        }
        private static bool IsDDS(FileStream stream) {
            byte[] fileCodeInBytes = new byte[FILECODE_SIZE];
            int result = stream.Read(fileCodeInBytes, 0, FILECODE_SIZE);
            if (result > 0) {
                char[] fileCode = new char[FILECODE_SIZE];
                for (int i = 0; i < FILECODE_SIZE; i++) {
                    fileCode[i] = Convert.ToChar(fileCodeInBytes[i]);
                }
                String fullFileCode = new string(fileCode);
                if (fullFileCode.ToLower().Contains("dds")) {
                    return true;
                } else {
                    return false;
                }
            } else {
                throw new IOException("can not read first "+FILECODE_SIZE+" bytes");
            }
        }
        private static DDS.Data GetHeaderFromBytes(Byte[] header) {
            int height = BitConverter.ToInt32(header, 8);
            int width = BitConverter.ToInt32(header, 12);
            int linearSize = BitConverter.ToInt32(header, 16);
            int mipMapCount = BitConverter.ToInt32(header, 24);
            int fourCC = BitConverter.ToInt32(header, 80);
            return new DDS.Data(height,width,linearSize,mipMapCount,fourCC);
        }
        private static PixelInternalFormat GetDDSFormat(DDS.Data header) {
            switch (header.FourCC) {
                case DDS.Data.FOURCC_DXT1:
                    return PixelInternalFormat.CompressedRgbaS3tcDxt1Ext;
                case DDS.Data.FOURCC_DXT3:
                    return PixelInternalFormat.CompressedRgbaS3tcDxt3Ext;
                case DDS.Data.FOURCC_DXT5:
                    return PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;
                default:
                    throw new IOException("ImageLoader.cs : DDS input has no valic FourCC header");
            }
        }
    }
}
