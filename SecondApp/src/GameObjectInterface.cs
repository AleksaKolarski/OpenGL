using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondApp
{
    interface GameObjectInterface
    {
        int VAO { get; set; }
        Shader Shader { get; set; }
        void Draw(Matrix4 viewMatrix);
        void DrawBatched();
    }
}
