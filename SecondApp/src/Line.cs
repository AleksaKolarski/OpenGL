using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace SecondApp
{
    class Line
    {
        private float[] Vertices { get; set; }
        private Vector3 _position;
        public Vector3 Position
        {
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
        private Shader Shader { get; set; }
        private Vector3 Color { get; set; }

        private int VAO { get; set; }
        private int VBO { get; set; }


        public Line(float[] vertices, Vector3 position, Shader shader, Vector3 color)
        {
            Vertices = vertices;
            Shader = shader;
            Position = position;
            Color = color;

            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            int vertexAttribLocation = GL.GetAttribLocation(Shader.GetHandle(), "aPos");
            GL.VertexAttribPointer(vertexAttribLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vertexAttribLocation);
        }

        public void Draw()
        {
            GL.BindVertexArray(VAO);
            Shader.Use();
            Shader.SetMatrix4("model", Model);
            //Shader.SetMatrix4("view", viewMatrix);
            Shader.SetVec3("color", Color);
            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
        }
    }
}
