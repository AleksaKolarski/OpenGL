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

        Line lineX, lineY, lineZ;
        Shader lineShader;

        List<GameObjectInterface> cubes;
        DrawBatcher cubeBatcher;
        Shader cubeShader;
        Texture textureContainer;
        Texture textureFace;

        Shader cubeLightShader;
        CubeLight cubeLight1, cubeLight2;

        Camera camera;

        float[] cubeVertices =
        {
            // positions         // tex coords  // normals
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,  0.0f,  0.0f, -1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,  0.0f,  0.0f, -1.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,  0.0f,  0.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,  0.0f,  0.0f, 1.0f,

            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f, -1.0f,  0.0f,  0.0f,

             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,  1.0f,  0.0f,  0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 1.0f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,  0.0f, -1.0f,  0.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,  0.0f,  1.0f,  0.0f
        };

        float[] cubeLightVertices =
        {
            -0.5f, -0.5f, -0.5f,  
             0.5f, -0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f, -0.5f,  0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,

            -0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,

             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,

            -0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f,  0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f, -0.5f
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

            lineShader = new Shader("LineShader.vert", "LineShader.frag");
            float[] lineXVertices =
            {
                0.0f, 0.0f, 0.0f,
                100.0f, 0.0f, 0.0f
            };
            float[] lineYVertices =
            {
                0.0f, 0.0f, 0.0f,
                0.0f, 100.0f, 0.0f
            };
            float[] lineZVertices =
            {
                0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 100.0f
            };
            lineX = new Line(lineXVertices, new Vector3(0.0f, 0.0f, 0.0f), lineShader, new Vector3(1.0f, 0.0f, 0.0f));
            lineY = new Line(lineYVertices, new Vector3(0.0f, 0.0f, 0.0f), lineShader, new Vector3(0.0f, 1.0f, 0.0f));
            lineZ = new Line(lineZVertices, new Vector3(0.0f, 0.0f, 0.0f), lineShader, new Vector3(0.0f, 0.0f, 1.0f));

            cubeShader = new Shader("CubeShader.vert", "CubeShader.frag");

            textureContainer = new Texture("container.png");
            textureFace = new Texture("awesomeface.png");

            List<Texture> textures = new List<Texture>();
            textures.Add(textureContainer);
            textures.Add(textureFace);
            cubes = new List<GameObjectInterface>();
            int x = 100;
            for(int i = -x; i <= x; i += 1)
            {
                //for (int j = 0; j <= x; j += 1)
                //{
                    for (int k = -x; k <= x; k += 1)
                    {
                        cubes.Add(new Cube(cubeVertices, new Vector3((float)i, ((float)Math.Sin(MathHelper.DegreesToRadians(i* 10 + 90)) * 5.0f + (float)Math.Cos(MathHelper.DegreesToRadians(k*10)) * 5.0f) -10.0f, (float)k), cubeShader, textures));

                        //if(i == 0 || i == x || j == 0 || j == x || k == 0 || k == x)
                            //cubes.Add(new Cube(cubeVertices, new Vector3(i, 1, k), cubeShader, textures));
                    }
                //}
            }
            cubeBatcher = new DrawBatcher(cubes);

            cubeLightShader = new Shader("CubeLightShader.vert", "CubeLightShader.frag");
            cubeLight1 = new CubeLight(cubeLightVertices, new Vector3(4.0f, 2.0f, 4.0f), cubeLightShader);
            //cubeLight2 = new CubeLight(cubeLightVertices, new Vector3(0.0f, 0.0f, 4.0f), cubeLightShader);


            Matrix4 projectionMatrix = Matrix4.Identity;
            projectionMatrix *= Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Width/Height, 0.01f, 1000.0f);
            cubeShader.Use();
            cubeShader.SetMatrix4("projection", projectionMatrix);
            cubeShader.SetVec3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
            lineShader.Use();
            lineShader.SetMatrix4("projection", projectionMatrix);
            cubeLightShader.Use();
            cubeLightShader.SetMatrix4("projection", projectionMatrix);

            camera = new Camera(new Vector3(4.0f, 4.0f, 4.0f), new Vector3(0.0f, 1.0f, 0.0f), -135.0f, -35.0f, 45.0f, 32.5f, 0.12f);

            GL.Viewport(ClientRectangle);

            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            broj += e.Time * 3;
            cubeLight1.Position = new Vector3( (float)Math.Sin(broj) * 100, 40.0f, (float)Math.Cos(broj) * 100);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {

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
                camera.ProcessMouseMovement(Mouse.offsetX(), Mouse.offsetY(), Mouse.scrollOffset());

                Matrix4 projectionMatrix = Matrix4.Identity;
                projectionMatrix *= Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.fov), (float)Width / Height, 0.1f, 1000.0f);
                cubeShader.Use();
                cubeShader.SetMatrix4("projection", projectionMatrix);
                cubeShader.SetVec3("viewPos", camera.position);
                cubeShader.SetVec3("lightPos", cubeLight1.Position);
                lineShader.Use();
                lineShader.SetMatrix4("projection", projectionMatrix);
                cubeLightShader.Use();
                cubeLightShader.SetMatrix4("projection", projectionMatrix);
            }


            Matrix4 viewMatrix;
            viewMatrix = camera.GetViewMatrix();



            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            lineX.Draw(viewMatrix);
            lineY.Draw(viewMatrix);
            lineZ.Draw(viewMatrix);

            cubeLight1.Draw(viewMatrix);
            //cubeLight2.Draw(viewMatrix);

            cubeBatcher.Draw(viewMatrix);

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

    }
}
