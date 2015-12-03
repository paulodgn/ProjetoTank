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
        Vector3 posicaoBala, direcaoBala;

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

        public void PosicaoDirecaoBala()
        {
            //obter direcao rodando o vetor direcao verdadeiro do tank com o valor da rotacao da turret
           
            

            Vector3 offset = new Vector3(0, 3, 3);
            Matrix rotacao = Matrix.CreateRotationX(tank.CannonRotation) * Matrix.CreateRotationY(tank.TurretRotation) * Matrix.CreateFromQuaternion(tank.rotacaoFinal.Rotation);

            offset = Vector3.Transform(offset, rotacao);
            direcaoBala = Vector3.Transform(new Vector3(0,0,1), rotacao);
            posicaoBala = tank.position + offset;
        }

        public void disparaBala()
        {
            PosicaoDirecaoBala();
            balaTemp = balasNaoAtivas.First();
            balaTemp.position = posicaoBala;
            balaTemp.direcao = direcaoBala;
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
                if (bala.position.Y < -50f)
                {

                    removerBala(bala);
                }
                
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
