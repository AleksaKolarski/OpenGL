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
        string name;
        VertexStruct[] vertices;
        int[] indices;
        TextureStruct[] textures;
        int VAO, VBO, EBO;

        Matrix4 modelMatrix = Matrix4.Identity;

        public Mesh(string name, VertexStruct[] vertices, int[] indices, TextureStruct[] textures)
        {
            this.name = name;
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
                string uniformName = textures[i].type;
                if(uniformName == "texture_diffuse")
                {
                    number = (diffuseNr++).ToString();
                }
                else if(uniformName == "texture_specular")
                {
                    number = (specularNr++).ToString();
                }
                shader.SetInt(uniformName + number, i);
                
                GL.BindTexture(TextureTarget.Texture2D, textures[i].id);
                //System.Console.WriteLine("Binding "+ uniformName + " texture " + textures[i].path);
            }
            shader.SetInt("texture_diffuse_count", diffuseNr-1);
            shader.SetInt("texture_specular_count", specularNr - 1);
            //if (diffuseNr == 1)
            //{
                //System.Console.WriteLine("Mesh " + name + " contains no diffuse textures");
            //}
            //if(specularNr == 1)
            //{
                //System.Console.WriteLine("Mesh " + name + " contains no specular textures");
            //}

            shader.SetMatrix4("model", modelMatrix);

            GL.BindVertexArray(VAO);
            GL.DrawElements(BeginMode.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);
            modelMatrix = Matrix4.Identity;
        }

        public void Scale(float scale)
        {
            modelMatrix *= Matrix4.CreateScale(scale);
        }

        public void Translate(Vector3 translation)
        {
            modelMatrix *= Matrix4.CreateTranslation(translation);
        }

        public void Rotate(float angle)
        {
            modelMatrix *= Matrix4.CreateRotationY(angle);
        }

        private void setupMesh()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            EBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(typeof(VertexStruct)) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(VertexStruct)), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(VertexStruct)), Marshal.OffsetOf(typeof(VertexStruct), "Normal"));

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(VertexStruct)), Marshal.OffsetOf(typeof(VertexStruct), "TexCoords"));

            GL.BindVertexArray(0);

            modelMatrix = Matrix4.Identity;
            
        }
    }
}
