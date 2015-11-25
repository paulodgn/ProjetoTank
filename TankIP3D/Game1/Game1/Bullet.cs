using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    class Bullet
    {

        Model bulletModel;
        Matrix world,view,projection;
        Vector3 position,direcao;
        public Bullet(Vector3 posicao)
        {
            this.position = posicao;
            direcao = new Vector3(1, 1, 1);
            world = Matrix.CreateScale(1f) * Matrix.CreateTranslation(position);
        }

        public void LoadContent(ContentManager content)
        {
            bulletModel = content.Load<Model>("Sphere");
        }

        public void Draw(Matrix cameraView, Matrix cameraProjection)
        {
            bulletModel.Root.Transform = world;

            view = cameraView;
            projection = cameraProjection;

            // Draw the model.
            foreach (ModelMesh mesh in bulletModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = bulletModel.Root.Transform;
                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

    }
}
