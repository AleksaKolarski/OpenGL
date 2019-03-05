using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.MetaData;
using SixLabors.ImageSharp.MetaData.Profiles.Exif;
using SixLabors.ImageSharp.MetaData.Profiles.Icc;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;

namespace SecondApp
{
    public class Texture
    {
        public int texture;

        public Texture(string path)
        {
            texture = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, texture);
            Use();


            Image<Rgba32> image = Image.Load(path);


            //Rgba32[] tempPixels = image.GetPixelSpan().ToArray();
            //List<byte> pixels = new List<byte>();

            //foreach(Rgba32 p in tempPixels)
            //{
                //pixels.Add(p.R);
                //pixels.Add(p.G);
                //pixels.Add(p.B);
                //pixels.Add(p.A);
            //}

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.GetPixelSpan().ToArray());

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public int GetHandle()
        {
            return texture;
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, texture);
        }
    }
}
