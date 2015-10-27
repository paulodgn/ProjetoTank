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
        VertexPositionColorTexture[] vertices;
        int vertexCount;
        short[] indice;
        float[,] alturas;
         
        Texture2D texturaMapa;
        int tamanhoMapa;
        Color[] valoresMapaAlturas;
        Texture2D texturaTerreno;

        Plano plano;
        public Terreno(GraphicsDevice device,Texture2D imagemMapaAlturas,Texture2D texturaPlano,float tamanhoPlano,Texture2D textura)
        {

            texturaTerreno = textura;
            tamanhoMapa = (imagemMapaAlturas.Height * imagemMapaAlturas.Width);
            this.texturaMapa = imagemMapaAlturas;
            effect = new BasicEffect(device);
            worldMatrix = Matrix.Identity;
            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;

            effect.View = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 10.0f), Vector3.Zero, Vector3.Up);//para onde aponta a camara
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 0.01f, 1000.0f);
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;
            effect.Texture = texturaTerreno;
            effect.TextureEnabled = true;

            lerMapaAlturas(imagemMapaAlturas);
            createGeometry(device);

            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionColorTexture>(vertices);
            
            indexBuffer = new IndexBuffer(device, typeof(short), indice.Length, BufferUsage.None);
            indexBuffer.SetData<short>(indice);

            this.plano = new Plano(device, texturaPlano, tamanhoPlano);
        }

        public void createGeometry(GraphicsDevice device)
        {
            vertexCount = tamanhoMapa;
            vertices = new VertexPositionColorTexture[vertexCount];
            float escala = 0.05f;
            //ler imagem

            //criar vertices
            int coordenadaTexturaY=0;
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
                            vertices[x * texturaMapa.Width + z] = new VertexPositionColorTexture(new Vector3(x, alturas[x, z] * escala, z), Color.White,new Vector2(0,coordenadaTexturaY));// texturaX=0,texturaY=0
                            coordenadaTexturaY = 1;
                        }
                        
                        else
                        {
                            //texturaX=0, texturaY=1
                            vertices[x * texturaMapa.Width + z] = new VertexPositionColorTexture(new Vector3(x, alturas[x, z] * escala, z), Color.White, new Vector2(0, coordenadaTexturaY));// texturaX=0,texturaY=1
                            coordenadaTexturaY = 0;
                        }
                    }
                    //se x é impar cria vertice variando a coordenada de textura de y entre 0 e 1. Coordenada de textura x é sempre 1
                    else
                    {
                        if (coordenadaTexturaY == 0)
                        {
                            //texturaX=1,texturaY=0
                            vertices[x * texturaMapa.Width + z] = new VertexPositionColorTexture(new Vector3(x, alturas[x, z] * escala, z), Color.White, new Vector2(1, coordenadaTexturaY));
                            coordenadaTexturaY = 1;
                        }
                        else
                        {
                            //texturaX=1, texturaY=1
                            vertices[x * texturaMapa.Width + z] = new VertexPositionColorTexture(new Vector3(x, alturas[x, z] * escala, z), Color.White, new Vector2(1, coordenadaTexturaY));
                            coordenadaTexturaY = 0;
                        }
                    }
                }
            }
            //
          
           
            //aplicar textura

            //criar indice
            indice = new short[(texturaMapa.Height * 2)*(texturaMapa.Height-1)];
            for (int i = 0; i < indice.Length/2; i++)
            {
                indice[2 * i] = (short)(i );
                indice[2 * i + 1] = (short)(i + texturaMapa.Width);
                
            }
            
            //vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColorTexture), vertices.GetLength(0), BufferUsage.WriteOnly);
            //indexBuffer = new IndexBuffer(device, typeof(short), indice.Length, BufferUsage.None);
        }

        //get vertices
        public VertexPositionColorTexture[] getVertices()
        {
            return (vertices);
        }
        public void lerMapaAlturas(Texture2D texturaMapa)
        {
            valoresMapaAlturas = new Color[tamanhoMapa];
            texturaMapa.GetData(valoresMapaAlturas);
            alturas = new float[texturaMapa.Height , texturaMapa.Width];
            

            /*z*texturaMapa da nos a linha + x para avancarmos*/
            for (int x = 0; x < texturaMapa.Height; x++)
            {
                for (int z = 0; z < texturaMapa.Width; z++)
                {
                    alturas[x,z] = valoresMapaAlturas[z*texturaMapa.Width+x].R;    
                }
                
                //Console.WriteLine(alturas[i]);
            }


          
        }

        public void Draw(GraphicsDevice device,/*CameraVersao2 camera*/ CameraSurfaceFollow camera)
        {
            //plano.Draw(device, camera);
            effect.View = camera.view;
            //effect.View = Camera.View;
            //effect.World = Camera.World;
            //effect.Projection = Camera.Projection;
            effect.World = worldMatrix;
             effect.CurrentTechnique.Passes[0].Apply();

            device.SetVertexBuffer(vertexBuffer);
            device.Indices = indexBuffer;
            //int var = 0;
            for (int i = 0; i < texturaMapa.Width-1; i++)
            {
                //device.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleStrip, vertices, i * texturaMapa.Width, texturaMapa.Width * 2 , indice, 0, texturaMapa.Width * 2-2 );
                device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, i * texturaMapa.Width, 0, texturaMapa.Width * 2, 0, texturaMapa.Width * 2 - 2);
                
            }
           
        }
    }
}

//}