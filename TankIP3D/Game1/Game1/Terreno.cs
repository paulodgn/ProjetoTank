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
        VertexPositionColor[] vertices;
        int vertexCount;
        short[] indice;
        float[,] alturas;
         
        Texture2D texturaMapa;
        int tamanhoMapa;
        Color[] valoresMapaAlturas;

        Plano plano;
        public Terreno(GraphicsDevice device,Texture2D imagemMapaAlturas,Texture2D texturaPlano,float tamanhoPlano)
        {

            
            tamanhoMapa = (imagemMapaAlturas.Height * imagemMapaAlturas.Width);
            this.texturaMapa = imagemMapaAlturas;
            effect = new BasicEffect(device);
            worldMatrix = Matrix.Identity;
            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;

            effect.View = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 10.0f), Vector3.Zero, Vector3.Up);//para onde aponta a camara
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 0.01f, 1000.0f);
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;

            lerMapaAlturas(imagemMapaAlturas);
            createGeometry(device);
            
            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionColor>(vertices);
            
            indexBuffer = new IndexBuffer(device, typeof(short), indice.Length, BufferUsage.None);
            indexBuffer.SetData<short>(indice);

            this.plano = new Plano(device, texturaPlano, tamanhoPlano);
        }

        public void createGeometry(GraphicsDevice device)
        {
            vertexCount = tamanhoMapa;
            vertices = new VertexPositionColor[vertexCount];
            float escala = 0.05f;
            //ler imagem

            //criar vertices
            for (int x = 0; x < texturaMapa.Height; x++)
            {
                for (int z = 0; z < texturaMapa.Width; z++)
                {
                    vertices[x * texturaMapa.Width + z] = new VertexPositionColor(new Vector3(x, alturas[x, z] * escala, -z), Color.Red);
                }
            }

           
            //aplicar textura

            //criar indice
            indice = new short[(texturaMapa.Height * texturaMapa.Width)*2];
            for (int i = 0; i < indice.Length/2; i++)
            {
                indice[2 * i] = (short)(i );
                indice[2 * i + 1] = (short)(i + texturaMapa.Width);
                
            }
            
            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColorTexture), vertices.GetLength(0), BufferUsage.WriteOnly);
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

        public void Draw(GraphicsDevice device,CameraAula camera)
        {
            plano.Draw(device, camera);
            effect.View = camera.view;
            //effect.View = Camera.View;
            //effect.World = Camera.World;
            //effect.Projection = Camera.Projection;
            effect.World = worldMatrix;
            effect.CurrentTechnique.Passes[0].Apply();

            device.SetVertexBuffer(vertexBuffer);
            int var = 0;
            for (int i = 0; i < texturaMapa.Width-1; i++)
            {
                device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, i * texturaMapa.Width, texturaMapa.Width*2-1, indice, var, texturaMapa.Width*2-3);
                
            }
           
        }
    }
}

//}