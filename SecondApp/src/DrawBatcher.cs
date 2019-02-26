using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondApp
{
    class DrawBatcher
    {
        List<GameObjectInterface> Objects { get; set; }
        Shader Shader { get; set; }
        int VAO { get; set; }

        public DrawBatcher(List<GameObjectInterface> objects)
        {
            Objects = objects;
        }

        public void Draw(Matrix4 viewMatrix)
        {
            if(Objects.Count > 0)
            {
                Shader = Objects.ElementAt(0).Shader;
                VAO = Objects.ElementAt(0).VAO;
                GL.BindVertexArray(VAO);
                Shader.Use();
                Shader.SetMatrix4("view", viewMatrix);
            }
            foreach(GameObjectInterface obj in Objects)
            {
                obj.DrawBatched();
            }
        }
    }
}
