using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;

namespace SecondApp
{
    class Game : GameWindow
    {
        private int VBO;
        private int VAO;
        private int EBO;

        Shader shader;
        Texture textureContainer;
        Texture textureFace;

        float[] vertices =
        {
            // positions          // colors           // texture coords
             0.5f,  0.5f, 0.0f,   1.0f, 0.0f, 0.0f,   1.0f, 1.0f,   // top right
             0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 0.0f,   1.0f, 0.0f,   // bottom right
            -0.5f, -0.5f, 0.0f,   0.0f, 0.0f, 1.0f,   0.0f, 0.0f,   // bottom left
            -0.5f,  0.5f, 0.0f,   1.0f, 1.0f, 0.0f,   0.0f, 1.0f    // top left 
        };

        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };


        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }


        protected override void OnLoad(EventArgs e)
        {
            // podesavamo boju na koju ce GL.Clear koristiti
            GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);

            shader = new Shader("shader.vert", "shader.frag");
            
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            EBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(float), indices, BufferUsageHint.StaticDraw);            

            int vertexAttribLocation = GL.GetAttribLocation(shader.GetHandle(), "aPos");
            GL.VertexAttribPointer(vertexAttribLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vertexAttribLocation);

            int vertexAttribColor = GL.GetAttribLocation(shader.GetHandle(), "aColor");
            GL.VertexAttribPointer(vertexAttribColor, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(vertexAttribColor);

            int vertexAttribTexCoord = GL.GetAttribLocation(shader.GetHandle(), "aTexCoord");
            GL.VertexAttribPointer(vertexAttribTexCoord, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(vertexAttribTexCoord);


            textureContainer = new Texture("container.png");
            textureFace = new Texture("awesomeface.png");

            shader.Use();
            shader.SetInt("texture1", 0);
            shader.SetInt("texture2", 1);


            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);


            GL.BindVertexArray(VAO);

            textureContainer.Use(TextureUnit.Texture0);
            textureFace.Use(TextureUnit.Texture1);
            shader.Use();

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);


            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
    }
}
