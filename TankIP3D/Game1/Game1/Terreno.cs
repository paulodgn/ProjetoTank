using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    class Terreno
    {
        BasicEffect effect;
        Matrix worldMatrix;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        VertexPositionNormalTexture[] vertices;
        int vertexCount;
        short[] indice;
        float[,] alturas;
        public int larguraMapa;
        Texture2D texturaMapa;
        int tamanhoMapa;
        Color[] valoresMapaAlturas;
        Texture2D texturaTerreno;


        Plano plano;
        public Terreno(GraphicsDevice device, Texture2D imagemMapaAlturas, Texture2D texturaPlano, float tamanhoPlano, Texture2D textura)
        {
            larguraMapa = imagemMapaAlturas.Width;
            texturaTerreno = textura;
            tamanhoMapa = (imagemMapaAlturas.Height * imagemMapaAlturas.Width);
            this.texturaMapa = imagemMapaAlturas;
            effect = new BasicEffect(device);
            worldMatrix = Matrix.Identity;
            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;

            effect.View = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 10.0f), Vector3.Zero, Vector3.Up);//para onde aponta a camara
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 0.01f, 1000.0f);
            effect.LightingEnabled = true;
            effect.EnableDefaultLighting();
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.Direction = new Vector3(1, -1, 1);
            effect.DirectionalLight0.SpecularColor = new Vector3(0.2f, 0.2f, 0.2f);
            effect.SpecularPower = 100f;
            effect.SpecularColor = new Vector3(1, 1, 1);
            effect.DirectionalLight1.Enabled = false;
            effect.DirectionalLight2.Enabled = false;
            //effect.AmbientLightColor = new Vector3(0, 0.1f, 0.1f);
            //effect.VertexColorEnabled = true;
            effect.Texture = texturaTerreno;
            effect.TextureEnabled = true;
            effect.EmissiveColor = new Vector3(0, 0, 1);


            //effect.LightingEnabled = true;
            //effect.EnableDefaultLighting();

            //effect.DirectionalLight0.Enabled = true;
            //effect.DirectionalLight0.Direction = new Vector3(0, 0, -1);
            ////effect.VertexColorEnabled = true;
            //effect.Texture = this.textura;
            //effect.TextureEnabled = true;


            lerMapaAlturas(imagemMapaAlturas);
            createGeometry(device);

            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);

            indexBuffer = new IndexBuffer(device, typeof(short), indice.Length, BufferUsage.None);
            indexBuffer.SetData<short>(indice);

            //this.plano = new Plano(device, texturaPlano, tamanhoPlano);
        }

        public void createGeometry(GraphicsDevice device)
        {
            vertexCount = tamanhoMapa;
            vertices = new VertexPositionNormalTexture[vertexCount];
            float escala = 0.05f;
            //ler imagem

            //criar vertices
            int coordenadaTexturaY = 0;
            for (int x = 0; x < texturaMapa.Height; x++)
            {
                for (int z = 0; z < texturaMapa.Width; z++)
                {
                    //se x é par
                    if (x % 2 == 0)
                    {
                        //criar vertice variando a coordenada de textura y entre 0 e 1.Coordenada de textura x é sempre 0 
                        if (coordenadaTexturaY == 0)
                        {
                            vertices[x * texturaMapa.Width + z] = new VertexPositionNormalTexture(new Vector3(x, alturas[x, z] * escala, z), Vector3.Up, new Vector2(0, coordenadaTexturaY));// texturaX=0,texturaY=0
                            coordenadaTexturaY = 1;
                        }

                        else
                        {
                            //texturaX=0, texturaY=1
                            vertices[x * texturaMapa.Width + z] = new VertexPositionNormalTexture(new Vector3(x, alturas[x, z] * escala, z), Vector3.Up, new Vector2(0, coordenadaTexturaY));// texturaX=0,texturaY=1
                            coordenadaTexturaY = 0;
                        }
                    }
                    //se x é impar cria vertice variando a coordenada de textura de y entre 0 e 1. Coordenada de textura x é sempre 1
                    else
                    {
                        if (coordenadaTexturaY == 0)
                        {
                            //texturaX=1,texturaY=0
                            vertices[x * texturaMapa.Width + z] = new VertexPositionNormalTexture(new Vector3(x, alturas[x, z] * escala, z), Vector3.Up, new Vector2(1, coordenadaTexturaY));
                            coordenadaTexturaY = 1;
                        }
                        else
                        {
                            //texturaX=1, texturaY=1
                            vertices[x * texturaMapa.Width + z] = new VertexPositionNormalTexture(new Vector3(x, alturas[x, z] * escala, z), Vector3.Up, new Vector2(1, coordenadaTexturaY));
                            coordenadaTexturaY = 0;
                        }
                    }
                }
            }
            //


            //aplicar textura

            //criar indice
            indice = new short[(texturaMapa.Height * 2) * (texturaMapa.Height - 1)];
            //indice = new short[(texturaMapa.Height - 1) * (texturaMapa.Width - 1) * 6];
            for (int i = 0; i < indice.Length / 2; i++)
            //for (int i = 0; i < texturaMapa.Height -1; i++)
            {
                indice[2 * i] = (short)(i);
                indice[2 * i + 1] = (short)(i + texturaMapa.Width);

            }

            //vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColorTexture), vertices.GetLength(0), BufferUsage.WriteOnly);
            //indexBuffer = new IndexBuffer(device, typeof(short), indice.Length, BufferUsage.None);


            //normais

            for (int i = 2; i < indice.Length; i++)
            {

                VertexPositionNormalTexture vertice = vertices[indice[i]];
                VertexPositionNormalTexture verticeAnterior = vertices[indice[i - 1]];
                VertexPositionNormalTexture verticeAnterior2 = vertices[indice[i - 2]];

                Vector3 vector1 = verticeAnterior.Position - vertice.Position;
                Vector3 vector2 = verticeAnterior2.Position - vertice.Position;
                Vector3 normal = Vector3.Cross(vector1, vector2);

                normal.Normalize();

                //vertices[i].Normal += normal;
                //vertices[i - 1].Normal += normal;
                //vertices[i - 2].Normal += normal;

                vertices[indice[i]].Normal = normal;
                vertices[indice[i - 1]].Normal = normal;
                vertices[indice[i - 2]].Normal = normal;



            }

            //normais segundo round




        }

        //get vertices
        public VertexPositionNormalTexture[] getVertices()
        {
            return (vertices);
        }
        public void lerMapaAlturas(Texture2D texturaMapa)
        {
            valoresMapaAlturas = new Color[tamanhoMapa];
            texturaMapa.GetData(valoresMapaAlturas);
            alturas = new float[texturaMapa.Height, texturaMapa.Width];


            /*z*texturaMapa da nos a linha + x para avancarmos*/
            for (int x = 0; x < texturaMapa.Height; x++)
            {
                for (int z = 0; z < texturaMapa.Width; z++)
                {
                    alturas[x, z] = valoresMapaAlturas[z * texturaMapa.Width + x].R;
                }

                //Console.WriteLine(alturas[i]);
            }



        }

        public void Draw(GraphicsDevice device, Matrix cameraView, Matrix cameraProj)
        {
            //plano.Draw(device, camera);
            effect.View = cameraView;
            effect.Projection = cameraProj;
            //effect.View = Camera.View;
            //effect.World = Camera.World;
            //effect.Projection = Camera.Projection;
            effect.World = worldMatrix;
            effect.CurrentTechnique.Passes[0].Apply();

            device.SetVertexBuffer(vertexBuffer);
            device.Indices = indexBuffer;
            //int var = 0;
            for (int i = 0; i < texturaMapa.Width - 1; i++)
            {
                //device.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleStrip, vertices, i * texturaMapa.Width, texturaMapa.Width * 2 , indice, 0, texturaMapa.Width * 2-2 );
                device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, i * texturaMapa.Width, 0, texturaMapa.Width * 2, 0, texturaMapa.Width * 2 - 2);
                //device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount/3);
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                DebugShapeRenderer.AddLine(vertices[i].Position,
                vertices[i].Position + vertices[i].Normal,
                Color.Red);
            }



        }
    }
}

//}