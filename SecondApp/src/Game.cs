using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Diagnostics;

namespace SecondApp
{
    class Game : GameWindow
    {

        private double broj = 0;

        Model nanosuit;
        Shader shader;
        Shader outlineShader;

        Model sponza;
        Model lopta;
        Model football;

        Line lineX, lineY, lineZ;
        Shader lineShader;

        Camera camera;

        Skybox skybox;
        Shader skyboxShader;

        Shader cubeLightShader;
        CubeLight cubeLight1;
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

        private static OpenTK.Graphics.GraphicsMode GraphicsMode
        {
            get {
                var defaultMode = OpenTK.Graphics.GraphicsMode.Default;
                var custom = new OpenTK.Graphics.GraphicsMode(
                    defaultMode.ColorFormat,
                    defaultMode.Depth,
                    1, // enable stencil buffer
                    defaultMode.Samples,
                    defaultMode.ColorFormat,
                    defaultMode.Buffers,
                    defaultMode.Stereo);

                return custom;

            }
        }

        public Game(int width, int height, string title) : base(width, height, Game.GraphicsMode, title, GameWindowFlags.Default) { }


        protected override void OnLoad(EventArgs e)
        {
            // podesavamo boju na koju ce GL.Clear koristiti
            GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.StencilTest);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.CullFace);

            lineShader = new Shader("LineShader.vert", "LineShader.frag");
            float[] lineXVertices =
            {
                0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f
            };
            float[] lineYVertices =
            {
                0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f
            };
            float[] lineZVertices =
            {
                0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f
            };
            lineX = new Line(lineXVertices, new Vector3(0.0f, 0.0f, 0.0f), lineShader, new Vector3(1.0f, 0.0f, 0.0f));
            lineY = new Line(lineYVertices, new Vector3(0.0f, 0.0f, 0.0f), lineShader, new Vector3(0.0f, 1.0f, 0.0f));
            lineZ = new Line(lineZVertices, new Vector3(0.0f, 0.0f, 0.0f), lineShader, new Vector3(0.0f, 0.0f, 1.0f));

            camera = new Camera(new Vector3(0.0f, 1.0f, 1.0f), new Vector3(0.0f, 1.0f, 0.0f), -90.0f, 0.0f, 45.0f, 1.0f, 0.12f);

            nanosuit = new Model("res/models/nanosuit.obj");
            shader = new Shader("ModelShader.vert", "ModelShader.frag");
            outlineShader = new Shader("OutlineShader.vert", "OutlineShader.frag");

            sponza = new Model("res/models/sponza/sponza.obj");
            
            lopta = new Model("res/models/lopta/blenderLopta.obj");
            //lopta.Scale(3);
            //lopta.Translate(new Vector3(0, 10, 0));

            football = new Model("res/models/football/soccerball.obj");

            cubeLightShader = new Shader("CubeLightShader.vert", "CubeLightShader.frag");
            //cubeLight1 = new CubeLight(cubeLightVertices, new Vector3(1.2f, 1.0f, 2.0f), cubeLightShader);

            skybox = new Skybox("res/skybox/sea");
            skyboxShader = new Shader("SkyboxShader.vert", "SkyboxShader.frag");

            Matrix4 projectionMatrix = Matrix4.Identity;
            projectionMatrix *= Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Width / Height, 0.01f, 100.0f);

            lineShader.Use();
            lineShader.SetMatrix4("projection", projectionMatrix);
            shader.Use();
            shader.SetMatrix4("projection", projectionMatrix);
            cubeLightShader.Use();
            cubeLightShader.SetMatrix4("projection", projectionMatrix);

            GL.Viewport(ClientRectangle);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            /*
                Here we Set all the uniforms for the 5/6 types of lights we have. We have to Set them manually and index 
                the proper PointLight struct in the array to Set each uniform variable. This can be done more code-friendly
                by defining light types as classes and Set their values in there, or by using a more efficient uniform approach
                by using 'Uniform buffer objects', but that is something we'll discuss in the 'Advanced GLSL' tutorial.
            */
            // directional light
            shader.Use();
            shader.SetVec3("dirLight.direction", new Vector3(-1.0f, -1.0f, -1.0f));
            shader.SetVec3("dirLight.ambient", new Vector3(0.1f, 0.1f, 0.1f));
            shader.SetVec3("dirLight.diffuse", new Vector3(0.6f, 0.6f, 0.6f));
            shader.SetVec3("dirLight.specular", new Vector3(0.3f, 0.3f, 0.3f));
            // point light 1
            shader.SetVec3("pointLights[0].position", new Vector3(20.0f, -10.0f, 20.0f));
            shader.SetVec3("pointLights[0].ambient", new Vector3(0.0f, 0.0f, 0.0f));
            shader.SetVec3("pointLights[0].diffuse", new Vector3(2.0f, 2.0f, 2.0f));
            shader.SetVec3("pointLights[0].specular", new Vector3(0.5f, 0.5f, 0.5f));
            shader.SetFloat("pointLights[0].constant", 1.0f);
            shader.SetFloat("pointLights[0].linear", 0.35f);
            shader.SetFloat("pointLights[0].quadratic", 0.44f);
            // spotLight
            shader.SetVec3("spotLight.position", camera.position);
            shader.SetVec3("spotLight.direction", camera.front);
            shader.SetVec3("spotLight.ambient", new Vector3(0.0f, 0.0f, 0.0f));
            shader.SetVec3("spotLight.diffuse", new Vector3(4.0f, 4.0f, 4.0f));
            shader.SetVec3("spotLight.specular", new Vector3(2.0f, 2.0f, 2.0f));
            shader.SetFloat("spotLight.constant", 1.0f);
            shader.SetFloat("spotLight.linear", 0.7f);
            shader.SetFloat("spotLight.quadratic", 1.8f);
            shader.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(9.5f)));
            shader.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(13.0f)));

            shader.SetFloat("material.shininess", 64.0f);


            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            broj += e.Time;

            //cubeLight1.Position = new Vector3((float)Math.Sin(broj) * 10, 10, (float)Math.Cos(broj) * 10);

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //Console.WriteLine("Frame");
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            Matrix4 viewMatrix;

            if (Focused)
            {
                Keyboard.updateState();
                if (Keyboard.checkKey(Key.W))
                {
                    if (Keyboard.checkKey(Key.LShift))
                        camera.ProcessKeyboard(Camera.CameraMovement.FORWARD, (float)e.Time * 5);
                    else
                        camera.ProcessKeyboard(Camera.CameraMovement.FORWARD, (float)e.Time);
                }
                if (Keyboard.checkKey(Key.S))
                {
                    if (Keyboard.checkKey(Key.LShift))
                        camera.ProcessKeyboard(Camera.CameraMovement.BACKWARD, (float)e.Time * 5);
                    else
                        camera.ProcessKeyboard(Camera.CameraMovement.BACKWARD, (float)e.Time);
                }
                if (Keyboard.checkKey(Key.A))
                {
                    if (Keyboard.checkKey(Key.LShift))
                        camera.ProcessKeyboard(Camera.CameraMovement.LEFT, (float)e.Time * 5);
                    else
                        camera.ProcessKeyboard(Camera.CameraMovement.LEFT, (float)e.Time);
                }
                if (Keyboard.checkKey(Key.D))
                {
                    if (Keyboard.checkKey(Key.LShift))
                        camera.ProcessKeyboard(Camera.CameraMovement.RIGHT, (float)e.Time * 5);
                    else
                        camera.ProcessKeyboard(Camera.CameraMovement.RIGHT, (float)e.Time);
                }
                if (Keyboard.checkKey(Key.Escape))
                {
                    Exit();
                }

                if (Keyboard.checkKey(Key.Keypad1))
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                }
                if (Keyboard.checkKey(Key.Keypad2))
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                }
                if (Keyboard.checkKey(Key.Keypad9))
                {
                    shader.Use();
                    shader.SetInt("mode", 0);
                }
                if (Keyboard.checkKey(Key.Keypad8))
                {
                    shader.Use();
                    shader.SetInt("mode", 1);
                }
                if (Keyboard.checkKey(Key.Keypad7))
                {
                    shader.Use();
                    shader.SetInt("mode", 2);
                }
                if (Keyboard.checkKey(Key.Keypad6))
                {
                    shader.Use();
                    shader.SetInt("mode", 3);
                }
                if (Keyboard.checkKey(Key.Keypad5))
                {
                    shader.Use();
                    shader.SetInt("mode", 4);
                }

                Mouse.updateState(Width, Height);
                camera.ProcessMouseMovement(Mouse.offsetX(), Mouse.offsetY(), Mouse.scrollOffset());

                viewMatrix = camera.GetViewMatrix();

                // zoom
                Matrix4 projectionMatrix = Matrix4.Identity;
                projectionMatrix *= Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.fov), (float)Width / Height, 0.1f, 100.0f);

                lineShader.Use();
                lineShader.SetMatrix4("projection", projectionMatrix);
                lineShader.SetMatrix4("view", viewMatrix);

                shader.Use();
                shader.SetMatrix4("projection", projectionMatrix);
                shader.SetMatrix4("view", viewMatrix);
                //shader.SetMatrix4("model", Matrix4.Identity);
                shader.SetVec3("viewPos", camera.position);
                shader.SetVec3("spotLight.position", camera.position);
                shader.SetVec3("spotLight.direction", camera.front);
                shader.SetVec3("pointLights[0].position", lopta.Position);

                outlineShader.Use();
                outlineShader.SetMatrix4("projection", projectionMatrix);
                outlineShader.SetMatrix4("view", viewMatrix);

                cubeLightShader.Use();
                cubeLightShader.SetMatrix4("projection", projectionMatrix);
                cubeLightShader.SetMatrix4("view", viewMatrix);

                skyboxShader.Use();
                skyboxShader.SetMatrix4("projection", projectionMatrix);
                skyboxShader.SetMatrix4("view", new Matrix4(new Matrix3(viewMatrix)));
            }

            sponza.Scale(0.01f);
            nanosuit.Scale(0.12f);
            football.Scale(0.02f);
            lopta.Scale(0.2f);
            lopta.Translate(new Vector3((float)Math.Sin(broj) * 2.0f, 1.0f, (float)Math.Cos(broj) * 2.0f));

            GL.DepthMask(false);
            skyboxShader.Use();
            skybox.Draw();
            GL.DepthMask(true);

            lineShader.Use();
            lineX.Draw();
            lineY.Draw();
            lineZ.Draw();
            shader.Use();
            sponza.Draw(shader);
            football.Draw(shader);
            cubeLightShader.Use();
            lopta.Draw(cubeLightShader);
            shader.Use();
            nanosuit.Draw(shader);


            Context.SwapBuffers();

            //Console.WriteLine("\n\n\n\n");
            //Console.ReadLine();
        }

    }
}
