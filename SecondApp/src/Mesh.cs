using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace SecondApp
{
    struct VertexStruct
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TexCoords;
    }

    struct TextureStruct
    {
        public int id;
        public string type;
        public string path;
    }

    class Mesh
    {
        VertexStruct[] vertices;
        int[] indices;
        TextureStruct[] textures;
        int VAO, VBO, EBO;

        public Mesh(VertexStruct[] vertices, int[] indices, TextureStruct[] textures)
        {
            this.vertices = vertices;
            this.indices = indices;
            this.textures = textures;

            setupMesh();
        }

        public void Draw(Shader shader)
        {
            int diffuseNr = 1;
            int specularNr = 1;
            for (int i = 0; i < textures.Length; i++){
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                string number = "";
                string name = textures[i].type;
                if(name == "texture_diffuse")
                {
                    number = (diffuseNr++).ToString();
                }
                else if(name == "texture_specular")
                {
                    number = (specularNr++).ToString();
                }
                shader.SetFloat("material." + name + number, i);
                GL.BindTexture(TextureTarget.Texture2D, textures[i].id);
            }
            GL.ActiveTexture(TextureUnit.Texture0);

            GL.BindVertexArray(VAO);
            GL.DrawElements(BeginMode.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        private void setupMesh()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            EBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(vertices), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Marshal.SizeOf(indices), indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new VertexStruct()), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new VertexStruct()), Marshal.OffsetOf(typeof(VertexStruct), "Normal"));

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Marshal.SizeOf(new VertexStruct()), Marshal.OffsetOf(typeof(VertexStruct), "TexCoords"));

            GL.BindVertexArray(0);
        }
    }
}
