using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace SecondApp
{
    class Cube : GameObjectInterface
    {
        private int textureCount = 0;

        private float[] Vertices { get; set; }
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

        public int VAO { get; set; }
        private int VBO { get; set; }


        public Cube(float[] vertices, Vector3 position, Shader shader)
        {
            Vertices = vertices;
            Shader = shader;
            Position = position;

            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            int vertexAttribLocation = GL.GetAttribLocation(Shader.GetHandle(), "aPos");
            GL.VertexAttribPointer(vertexAttribLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vertexAttribLocation);

            vertexAttribLocation = GL.GetAttribLocation(Shader.GetHandle(), "aNormal");
            GL.VertexAttribPointer(vertexAttribLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(vertexAttribLocation);

            int vertexAttribTexCoord = GL.GetAttribLocation(Shader.GetHandle(), "aTexCoords");
            GL.VertexAttribPointer(vertexAttribTexCoord, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(vertexAttribTexCoord);
        }

        public void Rotate(float angle)
        {
            Model = Matrix4.Identity;
            Model *= Matrix4.CreateRotationY(angle);
            Model *= Matrix4.CreateTranslation(Position);
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
