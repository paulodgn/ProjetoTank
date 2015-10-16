using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game1
{
    class Plano
    {
        BasicEffect effect;
        Matrix worldMatrix;
        VertexPositionColorTexture[] vertices;
        VertexBuffer vBuffer;
        Texture2D textura;
        float axisLenght;
        public Plano(GraphicsDevice device, Texture2D textura1,float tamanho)
        {
            textura = textura1;
            axisLenght = tamanho;
            effect = new BasicEffect(device);
            worldMatrix = Matrix.Identity;
            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;

            effect.View = Matrix.CreateLookAt(new Vector3(0.0f, 2.0f, 5.0f), Vector3.Zero, Vector3.Up);
            //effect.View = camera.view;

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 0.1f, 1000.0f);

            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;
            effect.Texture = this.textura;
            effect.TextureEnabled = true;
            Create3DAxis(device);

        }

        public void Create3DAxis(GraphicsDevice device)
        {
            
            int vertexCount = 6;
            vertices = new VertexPositionColorTexture[vertexCount];

            vertices[0] = new VertexPositionColorTexture(new Vector3(-axisLenght, 0.0f, -axisLenght), Color.White, new Vector2(0f, 0f));
            vertices[1] = new VertexPositionColorTexture(new Vector3(axisLenght, 0.0f, -axisLenght), Color.White, new Vector2(1f, 0f));
            vertices[2] = new VertexPositionColorTexture(new Vector3(-axisLenght, 0.0f, axisLenght), Color.White, new Vector2(0f, 1f));
            vertices[3] = new VertexPositionColorTexture(new Vector3(axisLenght, 0.0f, axisLenght), Color.White, new Vector2(1f, 1f));

            vBuffer = new VertexBuffer(device, typeof(VertexPositionColorTexture), vertices.GetLength(0), BufferUsage.WriteOnly);
        }

        public void Draw(GraphicsDevice device,CameraAula camera)
        {
            //effect.View = CameraAula.view;
            //effect.Projection = Camera.Projection;
            effect.View = camera.view;
            effect.World = worldMatrix;

            vBuffer.SetData(vertices);

            device.SetVertexBuffer(vBuffer);

            
            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);
            


        }

    }
}
