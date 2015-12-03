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
        public Matrix world,view,projection;
        public Vector3 position,direcao;
        float velocidade;
        float time;
        Vector3 vetorBase;
        Tank playerTank;
        public Bullet(Tank tank,ContentManager content)
        {
            playerTank = tank;
            vetorBase = new Vector3(1, 0, 0);
            
            //direcao = position - (tank.position + tank.newNormal);
            world = Matrix.CreateScale(1f) * Matrix.CreateTranslation(position);
            LoadContent(content);

            Vector3 offset = new Vector3(0, 3, 0);
            Matrix rotacao = Matrix.CreateRotationX(tank.CannonRotation) * Matrix.CreateRotationY(tank.TurretRotation) * Matrix.CreateFromQuaternion(tank.rotacaoFinal.Rotation);
            
            offset = Vector3.Transform(offset, rotacao);
            direcao = Vector3.Transform(Vector3.Cross(tank.newRigth, tank.newNormal), rotacao);
            position = tank.position + offset;
            
        }

        public void LoadContent(ContentManager content)
        {
            velocidade = 0.2f;
            bulletModel = content.Load<Model>("Sphere");


        }

        public void direcaoBala( Tank tank)
        {
            
            //obter direcao rodando o vetor direcao verdadeiro do tank com o valor da rotacao da turret
            Matrix rotacaoParaDirecao = Matrix.CreateRotationY(tank.TurretRotation);
            Vector3 direcaoTurret = Vector3.Transform(Vector3.Cross(tank.newRigth, tank.newNormal), rotacaoParaDirecao);

            Vector3 novoDireita = Vector3.Cross(tank.newNormal, direcaoTurret);
            novoDireita.Normalize();

            Matrix rotacaoCanon = Matrix.CreateFromAxisAngle(novoDireita, tank.CannonRotation);

            direcao = Vector3.Transform(direcaoTurret, rotacaoCanon);



         
        }

        public void Update(GameTime gameTime,Tank tank)
        {
            

            //isto está bem...
            //time += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 4096f;
            ////obter direcao rodando o vetor direcao verdadeiro do tank com o valor da rotacao da turret
            //Matrix rotacaoParaDirecao = Matrix.CreateRotationY(tank.TurretRotation);
            //Vector3 direcaoTurret = Vector3.Transform(Vector3.Cross(tank.newRigth, tank.newNormal), rotacaoParaDirecao);

            //Vector3 novoDireita = Vector3.Cross(tank.newNormal, direcaoTurret);
            //novoDireita.Normalize();

            //Matrix rotacaoCanon = Matrix.CreateFromAxisAngle(novoDireita, tank.CannonRotation);

            //direcao = Vector3.Transform(direcaoTurret, rotacaoCanon);
            time += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 4096f;
            //direcaoBala( tank);
            
            position += (Vector3.Normalize(direcao) * velocidade);
            position.Y -= 0.098f * (time * time);
            world = Matrix.CreateScale(0.8f) * Matrix.CreateTranslation(position);
           
            
                
            //...até aqui, Nao Apagar
            //Vector3 offset = (position * 0.01f) + new Vector3(0, 3.5f, 4);
            //Matrix rot = Matrix.CreateRotationY(TurretRotation);
            //Vector3 transformOffset = Vector3.Transform(offset, rot);

            //Vector3 novoDireita = Vector3.Cross(newNormal, transformOffset);
            
            //novoDireita.Normalize();


            
            //Matrix rot2 = Matrix.CreateFromAxisAngle(novoDireita, CannonRotation / 2);

            //Vector3 transformOffset2 = Vector3.Transform(transformOffset, rot2);
            //finalTrasnf = Vector3.Transform(transformOffset2, rotacaoFinal);
            
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
