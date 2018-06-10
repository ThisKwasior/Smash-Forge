﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Smash_Forge.Rendering
{
    abstract class Texture
    {
        // This should only be set once by GL.GenTexture().
        public int Id { get; }

        protected TextureTarget textureTarget = TextureTarget.Texture2D;

        public PixelInternalFormat PixelInternalFormat { get; }

        private TextureMinFilter minFilter;
        public TextureMinFilter MinFilter
        {
            get { return minFilter; }
            set
            {
                Bind();
                minFilter = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, (int)value);
            }
        }

        private TextureMagFilter magFilter;
        public TextureMagFilter MagFilter
        {
            get { return magFilter; }
            set
            {
                Bind();
                magFilter = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, (int)value);
            }
        }

        private TextureWrapMode textureWrapS;
        public TextureWrapMode TextureWrapS
        {
            get { return textureWrapS; }
            set
            {
                Bind();
                textureWrapS = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureWrapS, (int)value);
            }
        }

        private TextureWrapMode textureWrapT;
        public TextureWrapMode TextureWrapT
        {
            get { return textureWrapT; }
            set
            {
                Bind();
                textureWrapT = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureWrapT, (int)value);
            }
        }

        private TextureWrapMode textureWrapR;
        public TextureWrapMode TextureWrapR
        {
            get { return textureWrapR; }
            set
            {
                Bind();
                textureWrapR = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureWrapR, (int)value);
            }
        }

        public Texture(TextureTarget target, int width, int height, PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba)
        {
            // These should only be set once at object creation.
            Id = GL.GenTexture();
            textureTarget = target;
            PixelInternalFormat = pixelInternalFormat;

            Bind();

            // The GL texture needs to be updated in addition to initializing the variables.
            TextureWrapS = TextureWrapMode.ClampToEdge;
            TextureWrapT = TextureWrapMode.ClampToEdge;
            TextureWrapR = TextureWrapMode.ClampToEdge;
            MinFilter = TextureMinFilter.NearestMipmapLinear;
            MagFilter = TextureMagFilter.Linear;

            // Setup the format and mip maps.
            GL.TexImage2D(textureTarget, 0, PixelInternalFormat, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        ~Texture()
        {
            // The context probably isn't current here, so any GL function will crash.
            // TODO: Manage resources.
        }

        public void Bind()
        {
            GL.BindTexture(textureTarget, Id);
        }
    }
}