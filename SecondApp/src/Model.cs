using Assimp;
using Assimp.Unmanaged;
using OpenTK;
using System.Collections.Generic;

namespace SecondApp
{
    class Model
    {
        List<Mesh> meshes = new List<Mesh>();
        string directory;
        List<TextureStruct> textures_loaded = new List<TextureStruct>();

        public Vector3 Position { get; private set; }


        public Model(string path)
        {
            loadModel(path);
        }


        public void Draw(Shader shader)
        {
            for(int i = 0; i < meshes.Count; i++)
            {
                meshes[i].Draw(shader);
            }
        }

        public void Scale(float scale)
        {
            foreach(Mesh mesh in meshes)
            {
                mesh.Scale(scale);
            }
        }

        public void Translate(Vector3 translation)
        {
            Position = translation;
            foreach(Mesh mesh in meshes)
            {
                mesh.Translate(translation);
            }
        }

        public void Rotate(float angle)
        {
            foreach(Mesh mesh in meshes)
            {
                mesh.Rotate(angle);
            }
        }

        void loadModel(string path)
        {
            using(AssimpContext context = new AssimpContext())
            {
                Scene scene = context.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs | PostProcessSteps.GenerateNormals);
                if(scene == null || (scene.SceneFlags & SceneFlags.Incomplete) == SceneFlags.Incomplete  || scene.RootNode == null)
                {
                    System.Console.WriteLine("Greska pri ucitavanju " + path);
                    return;
                }
                directory = path.Substring(0, path.LastIndexOf('/'));
                processNode(scene.RootNode, scene);
            }
        }

        void processNode(Node node, Scene scene)
        {
            for(int i = 0; i < node.MeshCount; i++)
            {
                Assimp.Mesh mesh = scene.Meshes[node.MeshIndices[i]];
                meshes.Add(processMesh(mesh, scene));
            }
            for(int i = 0; i < node.ChildCount; i++)
            {
                processNode(node.Children[i], scene);
            }
        }

        Mesh processMesh(Assimp.Mesh mesh, Scene scene)
        {
            List<VertexStruct> vertices = new List<VertexStruct>();
            List<int> indices = new List<int>();
            List<TextureStruct> textures = new List<TextureStruct>();
            

            // process vertices
            for(int i = 0; i < mesh.VertexCount; i++)
            {
                VertexStruct vertex = new VertexStruct();
                Vector3 vector3;
                vector3 = new Vector3(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z);
                vertex.Position = vector3;
                if (mesh.HasNormals)
                {
                    vector3 = new Vector3(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z);
                    vertex.Normal = vector3;
                }
                if (mesh.HasTextureCoords(0))
                {
                    Vector2 vector2 = new Vector2();
                    vector2.X = mesh.TextureCoordinateChannels[0][i].X;
                    vector2.Y = mesh.TextureCoordinateChannels[0][i].Y;
                    vertex.TexCoords = vector2;
                }
                else
                {
                    vertex.TexCoords = new Vector2(0.0f, 0.0f);
                }
                vertices.Add(vertex);
            }

            // process indices
            for(int i = 0; i < mesh.FaceCount; i++)
            {
                Face face = mesh.Faces[i];
                for(int j = 0; j < face.IndexCount; j++)
                {
                    indices.Add(face.Indices[j]);
                }
            }

            // process material
            if(mesh.MaterialIndex >= 0)
            {
                Material material = scene.Materials[mesh.MaterialIndex];
                //System.Console.WriteLine(material.Name + " " + material.ShadingMode);
                TextureStruct[] diffuseMaps = loadMaterialTextures(material, TextureType.Diffuse, "texture_diffuse");
                for(int i = 0; i < diffuseMaps.Length; i++)
                {
                    textures.Add(diffuseMaps[i]);
                }
                TextureStruct[] specularMaps = loadMaterialTextures(material, TextureType.Specular, "texture_specular");
                for(int i = 0; i < specularMaps.Length; i++)
                {
                    textures.Add(specularMaps[i]);
                }
            }

            return new Mesh(mesh.Name, vertices.ToArray(), indices.ToArray(), textures.ToArray());
        }

        TextureStruct[] loadMaterialTextures(Material material, TextureType type, string typeName)
        {
            List<TextureStruct> textures = new List<TextureStruct>();
            for(int i = 0; i < material.GetMaterialTextureCount(type); i++)
            {
                TextureSlot textureSlot;
                material.GetMaterialTexture(type, i, out textureSlot);

                bool skip = false;
                for (int j = 0; j < textures_loaded.Count; j++)
                {
                    if (textures_loaded[j].path.Equals(textureSlot.FilePath))
                    {
                        textures.Add(textures_loaded[j]);
                        skip = true;
                        break;
                    }
                }
                if(skip == false)
                {
                    TextureStruct texture = new TextureStruct();
                    texture.id = new Texture(directory + "/" + textureSlot.FilePath).texture;
                    texture.type = typeName;
                    texture.path = textureSlot.FilePath;
                    textures.Add(texture);
                    textures_loaded.Add(texture);
                }
            }
            return textures.ToArray();
        }
    }
}
