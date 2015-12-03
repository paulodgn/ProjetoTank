using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    class BulletManager
    {

        List<Bullet> balasAtivas;
        List<Bullet> balasNaoAtivas;
        List<Bullet> copiaBalasAtivas;
        int numeroDeBalas;
        Tank tank;
        ContentManager content;
        Bullet balaTemp;

        public BulletManager(Tank tank, ContentManager content)
        {
            balasAtivas = new List<Bullet>(500);
            balasNaoAtivas = new List<Bullet>(500);
            this.tank = tank;
            this.content = content;
            numeroDeBalas = 500;
            copiaBalasAtivas = balasAtivas;
        }

        public void Initialize()
        {
            for (int i = 0; i < numeroDeBalas; i++)
            {
                balasNaoAtivas.Add(new Bullet(tank, content));
            }
        }

        public Vector3 direcaoBala()
        {
            //obter direcao rodando o vetor direcao verdadeiro do tank com o valor da rotacao da turret
            Matrix rotacaoParaDirecao = Matrix.CreateRotationY(tank.TurretRotation);
            Vector3 direcaoTurret = Vector3.Transform(Vector3.Cross(tank.newRigth, tank.newNormal), rotacaoParaDirecao);

            Vector3 novoDireita = Vector3.Cross(tank.newNormal, direcaoTurret);
            novoDireita.Normalize();
            novoDireita = Vector3.Transform(novoDireita, tank.rotacaoFinal);
            Matrix rotacaoCanon = Matrix.CreateFromAxisAngle(novoDireita, tank.CannonRotation) ;

            Vector3 direcao = Vector3.Transform(direcaoTurret, rotacaoCanon);
            
            


            return direcao;
        }

        public void disparaBala()
        {
            balaTemp = balasNaoAtivas.First();
            //balaTemp.position = tank.posicaoBala;
            //balaTemp.direcao = tank.direcaoBala;
            balasAtivas.Add(balaTemp);
            balasNaoAtivas.Remove(balaTemp);
            
        }

        public void removerBala(Bullet bala)
        {
            
            balasAtivas.Remove(bala);
            balasNaoAtivas.Add(bala);
            
        }

       

        public void UpdateBalas(GameTime gameTime)
        {
            copiaBalasAtivas = balasAtivas.ToList();
            foreach (Bullet bala in copiaBalasAtivas)
            {
                bala.Update(gameTime, this.tank);
                //if(bala.position.Y < -50f)
                //{
                   
                //    removerBala(bala);
                //}
                
            }
            
        }
        public void DrawBalas(Matrix view, Matrix projection)
        {
            foreach (Bullet bala in balasAtivas)
            {
                
                bala.Draw(view, projection);
            }
        }

    }
}
