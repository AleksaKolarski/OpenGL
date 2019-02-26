using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace SecondApp
{
    class Game : GameWindow
    {

        private double broj = 0;

        List<GameObject> cubes;

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
            new Vector3(-1.0f,  0.0f,  0.0f),
            new Vector3( 1.0f,  0.0f,  0.0f),
            new Vector3(-2.0f,  1.0f,  0.0f),
            new Vector3( 2.0f,  1.0f,  0.0f),
            new Vector3(-3.0f,  2.0f,  0.0f),
            new Vector3( 3.0f,  2.0f,  0.0f),
            new Vector3(-3.0f,  3.0f,  0.0f),
            new Vector3( 3.0f,  3.0f,  0.0f),
            new Vector3(-3.0f,  4.0f,  0.0f),
            new Vector3( 3.0f,  4.0f,  0.0f),
        };

        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };


        Vector3[] lineVertices =
        {
            new Vector3( 1.0f, 0.0f, 0.0f),
            new Vector3( 0.0f, 1.0f, 0.0f),
            new Vector3( 0.0f, 0.0f, 1.0f),
        };

        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }


        protected override void OnLoad(EventArgs e)
        {
            // podesavamo boju na koju ce GL.Clear koristiti
            GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            shader = new Shader("shader.vert", "shader.frag");

            textureContainer = new Texture("container.png");
            textureFace = new Texture("awesomeface.png");

            List<Texture> textures = new List<Texture>();
            textures.Add(textureContainer);
            textures.Add(textureFace);
            cubes = new List<GameObject>();
            for(int i = -180; i < 180; i += 2)
            {
                //for(int j = 0; j < 1; j += 2)
                //{
                    for (int k = -180; k < 180; k += 2)
                    {
                        cubes.Add(new GameObject(vertices, indices, new Vector3(
                            (float)i, 
                            ((float)Math.Sin(MathHelper.DegreesToRadians(i*5)) * 25.0f + (float)Math.Cos(MathHelper.DegreesToRadians(k*5)) * 25.0f), (float)k), shader, textures));
                    }
                //}
            }

            shader.Use();
            Matrix4 projectionMatrix = Matrix4.Identity;
            projectionMatrix *= Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Width/Height, 0.01f, 1000.0f);
            shader.SetMatrix4("projection", projectionMatrix);

            camera = new Camera(new Vector3(0.0f, 150.0f, 7.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector3( 0.0f, 0.0f, -1.0f), 0.0f, -90.0f, 32.5f, 0.12f);

            GL.Viewport(ClientRectangle);

            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {

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

            foreach(GameObject cube in cubes)
            {
                cube.Draw();
            }
            

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

    }
}
