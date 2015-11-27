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
        float velocidade;
        float time;
        Vector3 vetorBase;
        public Bullet(Vector3 posicao,Tank tank,ContentManager content)
        {
            vetorBase = new Vector3(1, 0, 0);
            position = posicao;
            //direcao = position - (tank.position + tank.newNormal);
            world = Matrix.CreateScale(1f) * Matrix.CreateTranslation(position);
            LoadContent(content);
        }

        public void LoadContent(ContentManager content)
        {
            velocidade = 0.5f;
            bulletModel = content.Load<Model>("Sphere");


        }

        public void Update(GameTime gameTime,Tank tank)
        {
            //Vector3 offset = (tank.position * 0.01f) + new Vector3(0,4,4);
            //Matrix rot = Matrix.CreateRotationY(tank.TurretRotation);
            //Vector3 transformOffset = Vector3.Transform(offset, rot);
            //Matrix rot2 = Matrix.CreateRotationX(tank.CannonRotation);
            //Vector3 transformOffset2 = Vector3.Transform(transformOffset, rot2);
            //Vector3 finalTrasnf = Vector3.Transform(transformOffset2, tank.rotacaoFinal);



            //this.position = finalTrasnf;
            //world = Matrix.CreateScale(1f) * Matrix.CreateTranslation(position);

            //isto está bem...
            time += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 4096f;
            //obter direcao rodando o vetor direcao verdadeiro do tank com o valor da rotacao da turret
            Matrix rotacaoParaDirecao = Matrix.CreateRotationY(tank.TurretRotation);
            direcao = Vector3.Transform(Vector3.Cross(tank.newRigth,tank.newNormal), rotacaoParaDirecao);
            position += (Vector3.Normalize( direcao) * velocidade);
            position.Y -= 0.98f * (time * time);
            world = Matrix.CreateScale(0.8f) * Matrix.CreateTranslation(position);
           
            //...até aqui, Nao Apagar


            //Vector3 offset = new Vector3(0, 20, -40);
            //rotacao = Matrix.CreateRotationY(MathHelper.ToRadians(tank.rotacaoY));
            //Vector3 transformOffset = Vector3.Transform(offset, rotacao);
            //posicao = transformOffset + posicaoTank;
            
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
