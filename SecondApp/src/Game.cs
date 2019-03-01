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

        //Model model;

        Line lineX, lineY, lineZ;
        Shader lineShader;

        List<GameObjectInterface> cubes;
        DrawBatcher cubeBatcher;
        Shader cubeShader;
        Texture textureContainer;
        Texture textureFace;
        Texture textureContainer2;
        Texture textureContainer2_specular;

        Shader cubeLightShader;
        CubeLight cubeLight1, cubeLight2;

        Camera camera;

        float[] cubeVertices =
        {
            // positions          // normals           // texture coords
                -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,
                 0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f,  0.0f,
                 0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f,  1.0f,
                 0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f,  1.0f,
                -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f,  1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,

                -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,
                 0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f,  0.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f,  1.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f,  1.0f,
                -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f,  1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,

                -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
                -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f,  1.0f,
                -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
                -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
                -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,

                 0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
                 0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f,
                 0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
                 0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
                 0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,

                -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f,  1.0f,
                 0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f,  1.0f,
                 0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f,  0.0f,
                 0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f,  0.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f,  0.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f,  1.0f,

                -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f,
                 0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f,  1.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f,  0.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f,  0.0f,
                -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,
                -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f
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

            //textureContainer = new Texture("container.png");
            //textureFace = new Texture("awesomeface.png");
            textureContainer2 = new Texture("container2.png");
            textureContainer2_specular = new Texture("container2_specular.png");

            cubes = new List<GameObjectInterface>();
            int x = 50;
            for(int i = -x; i <= x; i += 1)
            {
            //for (int j = 0; j <= x; j += 2)
            //{
            for (int k = -x; k <= x; k += 1)
            {
            cubes.Add(new Cube(cubeVertices, new Vector3((float)i, ((float)Math.Sin(MathHelper.DegreesToRadians(i* 10 + 90)) * 5.0f + (float)Math.Cos(MathHelper.DegreesToRadians(k*10)) * 5.0f) -10.0f, (float)k), cubeShader));

            //if(i == 0 || i == x || j == 0 || j == x || k == 0 || k == x)
            //cubes.Add(new Cube(cubeVertices, new Vector3(i, 1, k), cubeShader, textures));
            }
            //}
            }
            Cube cube = new Cube(cubeVertices, new Vector3(0.0f, 0.0f, 0.0f), cubeShader);
            cubes.Add(cube);
            cubeBatcher = new DrawBatcher(cubes);

            cubeLightShader = new Shader("CubeLightShader.vert", "CubeLightShader.frag");
            cubeLight1 = new CubeLight(cubeLightVertices, new Vector3(1.2f, 1.0f, 2.0f), cubeLightShader);
            //cubeLight2 = new CubeLight(cubeLightVertices, new Vector3(0.0f, 0.0f, 4.0f), cubeLightShader);

            camera = new Camera(new Vector3(30.0f, 4.0f, 30.0f), new Vector3(0.0f, 1.0f, 0.0f), -135.0f, -25.0f, 45.0f, 32.5f, 0.12f);


            Matrix4 projectionMatrix = Matrix4.Identity;
            projectionMatrix *= Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Width/Height, 0.01f, 1000.0f);
            cubeShader.Use();
            cubeShader.SetMatrix4("projection", projectionMatrix);
            cubeShader.SetVec3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            cubeShader.SetFloat("material.shininess", 64.0f);
            /*
           Here we Set all the uniforms for the 5/6 types of lights we have. We have to Set them manually and index 
           the proper PointLight struct in the array to Set each uniform variable. This can be done more code-friendly
           by defining light types as classes and Set their values in there, or by using a more efficient uniform approach
           by using 'Uniform buffer objects', but that is something we'll discuss in the 'Advanced GLSL' tutorial.
        */
            // directional light
            cubeShader.SetVec3("dirLight.direction", new Vector3(1.0f, 1.0f, 1.0f));
            cubeShader.SetVec3("dirLight.ambient", new Vector3(0.05f, 0.05f, 0.05f));
            cubeShader.SetVec3("dirLight.diffuse", new Vector3(0.1f, 0.1f, 0.1f));
            cubeShader.SetVec3("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));
            // point light 1
            cubeShader.SetVec3("pointLights[0].position", new Vector3(20.0f, -10.0f, 20.0f));
            cubeShader.SetVec3("pointLights[0].ambient", new Vector3(0.0f, 0.0f, 0.0f));
            cubeShader.SetVec3("pointLights[0].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
            cubeShader.SetVec3("pointLights[0].specular", new Vector3(1.0f, 1.0f, 1.0f));
            cubeShader.SetFloat("pointLights[0].constant", 1.0f);
            cubeShader.SetFloat("pointLights[0].linear", 0.09f);
            cubeShader.SetFloat("pointLights[0].quadratic", 0.032f);
            // point light 2
            cubeShader.SetVec3("pointLights[1].position", new Vector3(20.0f, -10.0f, -20.0f));
            cubeShader.SetVec3("pointLights[1].ambient", new Vector3(0.0f, 0.0f, 0.0f));
            cubeShader.SetVec3("pointLights[1].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
            cubeShader.SetVec3("pointLights[1].specular", new Vector3(1.0f, 1.0f, 1.0f));
            cubeShader.SetFloat("pointLights[1].constant", 1.0f);
            cubeShader.SetFloat("pointLights[1].linear", 0.09f);
            cubeShader.SetFloat("pointLights[1].quadratic", 0.032f);
            // point light 3
            cubeShader.SetVec3("pointLights[2].position", new Vector3(-20.0f, -10.0f, -20.0f));
            cubeShader.SetVec3("pointLights[2].ambient", new Vector3(0.0f, 0.0f, 0.0f));
            cubeShader.SetVec3("pointLights[2].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
            cubeShader.SetVec3("pointLights[2].specular", new Vector3(1.0f, 1.0f, 1.0f));
            cubeShader.SetFloat("pointLights[2].constant", 1.0f);
            cubeShader.SetFloat("pointLights[2].linear", 0.09f);
            cubeShader.SetFloat("pointLights[2].quadratic", 0.032f);
            // point light 4
            cubeShader.SetVec3("pointLights[3].position", new Vector3(-20.0f, -10.0f, 20.0f));
            cubeShader.SetVec3("pointLights[3].ambient", new Vector3(0.0f, 0.0f, 0.0f));
            cubeShader.SetVec3("pointLights[3].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
            cubeShader.SetVec3("pointLights[3].specular", new Vector3(1.0f, 1.0f, 1.0f));
            cubeShader.SetFloat("pointLights[3].constant", 1.0f);
            cubeShader.SetFloat("pointLights[3].linear", 0.09f);
            cubeShader.SetFloat("pointLights[3].quadratic", 0.032f);
            // spotLight
            cubeShader.SetVec3("spotLight.position", camera.position);
            cubeShader.SetVec3("spotLight.direction", camera.front);
            cubeShader.SetVec3("spotLight.ambient", new Vector3(0.0f, 0.0f, 0.0f));
            cubeShader.SetVec3("spotLight.diffuse", new Vector3(10.0f, 10.0f, 10.0f));
            cubeShader.SetVec3("spotLight.specular", new Vector3(10.0f, 10.0f, 10.0f));
            cubeShader.SetFloat("spotLight.constant", 1.0f);
            cubeShader.SetFloat("spotLight.linear", 0.09f);
            cubeShader.SetFloat("spotLight.quadratic", 0.032f);
            cubeShader.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            cubeShader.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(15.0f)));
            cubeShader.AddTexture(textureContainer2, "material.diffuse");
            cubeShader.AddTexture(textureContainer2_specular, "material.specular");
            lineShader.Use();
            lineShader.SetMatrix4("projection", projectionMatrix);
            cubeLightShader.Use();
            cubeLightShader.SetMatrix4("projection", projectionMatrix);

            

            GL.Viewport(ClientRectangle);

            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            broj += e.Time;
            cubeLight1.Position = new Vector3((float)Math.Sin(broj) * 20, -10.0f, (float)Math.Cos(broj) * 20);
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
                camera.ProcessMouseMovement(Mouse.offsetX(), Mouse.offsetY(), Mouse.scrollOffset());

                Matrix4 projectionMatrix = Matrix4.Identity;
                projectionMatrix *= Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.fov), (float)Width / Height, 0.1f, 1000.0f);
                cubeShader.Use();
                cubeShader.SetMatrix4("projection", projectionMatrix);
                cubeShader.SetVec3("viewPos", camera.position);
                cubeShader.SetVec3("spotLight.position", camera.position);
                cubeShader.SetVec3("spotLight.direction", camera.front);
                lineShader.Use();
                lineShader.SetMatrix4("projection", projectionMatrix);
                cubeLightShader.Use();
                cubeLightShader.SetMatrix4("projection", projectionMatrix);
            }
            



            Matrix4 viewMatrix;
            viewMatrix = camera.GetViewMatrix();


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
