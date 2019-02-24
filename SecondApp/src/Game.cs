using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Drawing;

namespace SecondApp
{
    class Game : GameWindow
    {

        private double broj = 0;

        private int VBO;
        private int VAO;
        private int EBO;

        Shader shader;
        Texture textureContainer;
        Texture textureFace;

        Camera camera;

        float[] vertices =
        {
            // positions         // texture coords
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };

        Vector3[] cubePositions =
        {
            new Vector3( 0.0f,  0.0f,  0.0f),
            new Vector3( 2.0f,  5.0f, -15.0f),
            new Vector3(-1.5f, -2.2f, -2.5f),
            new Vector3(-3.8f, -2.0f, -12.3f),
            new Vector3( 2.4f, -0.4f, -3.5f),
            new Vector3(-1.7f,  3.0f, -7.5f),
            new Vector3( 1.3f, -2.0f, -2.5f),
            new Vector3( 1.5f,  2.0f, -2.5f),
            new Vector3( 1.5f,  0.2f, -1.5f),
            new Vector3(-1.3f,  1.0f, -1.5f)
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
            GL.Enable(EnableCap.DepthTest);

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
            GL.VertexAttribPointer(vertexAttribLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vertexAttribLocation);

            int vertexAttribTexCoord = GL.GetAttribLocation(shader.GetHandle(), "aTexCoord");
            GL.VertexAttribPointer(vertexAttribTexCoord, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(vertexAttribTexCoord);


            textureContainer = new Texture("container.png");
            textureFace = new Texture("awesomeface.png");

            

            shader.Use();
            textureContainer.Use(TextureUnit.Texture0);
            textureFace.Use(TextureUnit.Texture1);
            shader.SetInt("texture1", 0);
            shader.SetInt("texture2", 1);


            Matrix4 projectionMatrix = Matrix4.Identity;
            projectionMatrix *= Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Width/Height, 0.1f, 100.0f);
            shader.SetMatrix4("projection", projectionMatrix);

            camera = new Camera(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 0.0f, -1.0f), -90.0f, 0.0f, 2.5f, 0.12f);

            GL.Viewport(ClientRectangle);

            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            broj += 2.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);



            Keyboard.updateState();
            if (Keyboard.checkKey(Key.W))
            {
                camera.ProcessKeyboard(Camera.CameraMovement.FORWARD, (float)e.Time);
            }
            if (Keyboard.checkKey(Key.S))
            {
                camera.ProcessKeyboard(Camera.CameraMovement.BACKWARD, (float)e.Time);
            }
            if (Keyboard.checkKey(Key.A))
            {
                camera.ProcessKeyboard(Camera.CameraMovement.LEFT, (float)e.Time);
            }
            if (Keyboard.checkKey(Key.D))
            {
                camera.ProcessKeyboard(Camera.CameraMovement.RIGHT, (float)e.Time);
            }
            if (Keyboard.checkKey(Key.Escape))
            {
                Exit();
            }

            if (Focused)
            {
                Mouse.updateState(Width, Height);
                camera.ProcessMouseMovement(Mouse.offsetX(), Mouse.offsetY());
            }

            Matrix4 viewMatrix;
            viewMatrix = camera.GetViewMatrix();
            shader.SetMatrix4("view", viewMatrix);


            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            for (uint i = 0; i < 10; i++)
            {
                Matrix4 model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation(cubePositions[i]);
                float angle = 20.0f * i;
                model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(i * 20.0f));
                model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(i * 20.0f));
                model *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(i * 20.0f));
                shader.SetMatrix4("model", model);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }


            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

    }
}
