using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace SecondApp
{
    class Cube : GameObjectInterface
    {
        private float[] Vertices { get; set; }
        private uint[] Indices { get; set; }
        private Vector3 _position;
        public Vector3 Position {
            get {
                return _position;
            }
            set {
                _position = value;
                Model = Matrix4.Identity;
                Model *= Matrix4.CreateTranslation(value);
            }
        }
        private Matrix4 Model { get; set; }
        public Shader Shader { get; set; }

        private List<Texture> _textures;
        private List<Texture> Textures {
            get {
                return _textures;
            }
            set {
                _textures = value;
                Shader.Use();
                for (int i = 0; i < _textures.Count; i++)
                {
                    _textures[i].Use(TextureUnit.Texture0 + i);
                    Shader.SetInt("texture" + (i + 1), i);
                }
            }
        }

        public int VAO { get; set; }
        private int VBO { get; set; }
        private int EBO { get; set; }


        public Cube(float[] vertices, uint[] indices, Vector3 position, Shader shader, List<Texture> textures)
        {
            Vertices = vertices;
            Indices = indices;
            Shader = shader;
            Position = position;
            Textures = textures;

            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            EBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(float), Indices, BufferUsageHint.StaticDraw);

            int vertexAttribLocation = GL.GetAttribLocation(Shader.GetHandle(), "aPos");
            GL.VertexAttribPointer(vertexAttribLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vertexAttribLocation);

            int vertexAttribTexCoord = GL.GetAttribLocation(Shader.GetHandle(), "aTexCoord");
            GL.VertexAttribPointer(vertexAttribTexCoord, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(vertexAttribTexCoord);
        }

        public void Draw(Matrix4 viewMatrix)
        {
            GL.BindVertexArray(VAO);
            Shader.Use();
            Shader.SetMatrix4("model", Model);
            Shader.SetMatrix4("view", viewMatrix);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        public void DrawBatched()
        {
            Shader.SetMatrix4("model", Model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
    }
}
