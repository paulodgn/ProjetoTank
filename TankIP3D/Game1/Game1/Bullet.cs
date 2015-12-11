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
        public BoundingSphere boundingSphere;
        public Bullet(Tank tank,ContentManager content)
        {
            playerTank = tank;
            vetorBase = new Vector3(0, 0, 1);
            
           
            world = Matrix.CreateScale(0.3f) * Matrix.CreateTranslation(position);
            LoadContent(content);

            Vector3 offset = new Vector3(0, 2, 3);
            Matrix rotacao = Matrix.CreateRotationX(tank.CannonRotation) * Matrix.CreateRotationY(tank.TurretRotation) * Matrix.CreateFromQuaternion(tank.rotacaoFinal.Rotation);
            
            offset = Vector3.Transform(offset, rotacao);
            direcao = Vector3.Transform(Vector3.Cross(tank.newRigth, tank.newNormal), rotacao);
            position = tank.position + offset;
            boundingSphere = new BoundingSphere();
            boundingSphere.Radius = 0.7f;
        }

        public void LoadContent(ContentManager content)
        {
            velocidade = 0.5f;
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
            boundingSphere.Center = this.position;
            time += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 4096f;

            position += (Vector3.Normalize(direcao) * velocidade);
            position.Y -= 0.098f * (time * time);
            world = Matrix.CreateScale(0.3f) * Matrix.CreateTranslation(position);

            
        }

        public void Draw(Matrix cameraView, Matrix cameraProjection)
        {
            bulletModel.Root.Transform = world;
            DebugShapeRenderer.AddBoundingSphere(boundingSphere, Color.Red);
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
