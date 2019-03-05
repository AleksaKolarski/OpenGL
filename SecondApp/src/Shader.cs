using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace SecondApp
{
    public class Shader
    {
        int shaderProgram;
        private Dictionary<string, int> uniformCache;
        private int textureCount = 0;

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

            uniformCache = new Dictionary<string, int>();
        }

        public Shader(string vertexPath, string fragmentPath, List<string> uniformNames) : this(vertexPath, fragmentPath)
        {
            foreach(string uniformName in uniformNames)
            {
                uniformCache.Add(uniformName, GL.GetUniformLocation(shaderProgram, uniformName));
            }
        }

        public int GetHandle()
        {
            return shaderProgram;
        }

        public void Use()
        {
            GL.UseProgram(shaderProgram);
        }

        public void AddTexture(Texture texture, string target)
        {
            texture.Use(TextureUnit.Texture0 + textureCount);
            SetInt(target, textureCount);
            textureCount++;
        }

        public void SetInt(string name, int value)
        {
            int location = GetCachedUniformLocation(name);
            if (location == -1)
            {
                //Console.WriteLine("shader.SetInt("+name+", "+value+") greska");
            }
            GL.Uniform1(location, value);
        }

        public void SetFloat(string name, float value)
        {
            int location = GetCachedUniformLocation(name);
            if (location == -1)
            {
                //Console.WriteLine("shader.SetFloat(" + name + ", " + value + ") greska");
            }
            GL.Uniform1(location, value);
        }

        public void SetVec3(string name, Vector3 value)
        {
            int location = GetCachedUniformLocation(name);
            if (location == -1)
            {
                //Console.WriteLine("shader.SetVec3(" + name + ", " + value + ") greska");
            }
            GL.Uniform3(location, value);
        }

        public void SetMatrix4(string name, Matrix4 value)
        {
            int location = GetCachedUniformLocation(name);
            if (location == -1)
            {
                //Console.WriteLine("shader.SetMatrix4(" + name + ", " + value + ") greska");
            }
            GL.UniformMatrix4(location, false, ref value);
        }


        private int GetCachedUniformLocation(string name)
        {
            int result;
            if (uniformCache.TryGetValue(name, out result))
            {
                return result;
            }
            result = GL.GetUniformLocation(shaderProgram, name);
            uniformCache.Add(name, result);
            return result;
        }
    }
}
