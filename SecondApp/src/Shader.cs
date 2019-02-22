using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace SecondApp
{
    public class Shader
    {
        int shaderProgram;

        public Shader(string vertexPath, string fragmentPath)
        {
            int vertexShader;
            string vertexShaderSource = Util.ReadFile("res/shaders/" + vertexPath);
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);
            string infoLogVert = GL.GetShaderInfoLog(vertexShader);
            if(infoLogVert != string.Empty)
            {
                Console.WriteLine(infoLogVert);
            }

            int fragmentShader;
            string fragmentShaderSource = Util.ReadFile("res/shaders/" + fragmentPath);
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);
            string infoLogFrag = GL.GetShaderInfoLog(fragmentShader);
            if(infoLogFrag != string.Empty)
            {
                Console.WriteLine(infoLogFrag);
            }

            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);
            string infoLogShaderProgram = GL.GetProgramInfoLog(shaderProgram);
            if(infoLogShaderProgram != string.Empty)
            {
                Console.WriteLine(infoLogShaderProgram);
            }

            GL.DetachShader(shaderProgram, vertexShader);
            GL.DetachShader(shaderProgram, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public int GetHandle()
        {
            return shaderProgram;
        }

        public void Use()
        {
            GL.UseProgram(shaderProgram);
        }

        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation(shaderProgram, name);
            if(location == -1)
            {
                Console.WriteLine("shader.SetInt greska");
            }
            GL.Uniform1(location, value);
        }
    }
}
