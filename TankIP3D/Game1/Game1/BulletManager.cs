using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    static class BulletManager
    {

        static List<Bullet> balasAtivas;
        static List<Bullet> balasNaoAtivas;
        static List<Bullet> copiaBalasAtivas;
        static int numeroDeBalas;
        static Tank tank;
        static ContentManager content;
        static Bullet balaTemp;
        static Vector3 posicaoBala, direcaoBala;
        
        
        static public void Initialize(Tank tankQueDispara, ContentManager cont)
        {
            balasAtivas = new List<Bullet>(500);
            balasNaoAtivas = new List<Bullet>(500);
            tank = tankQueDispara;
            content = cont;
            numeroDeBalas = 500;
            copiaBalasAtivas = balasAtivas;
            
            for (int i = 0; i < numeroDeBalas; i++)
            {
                balasNaoAtivas.Add(new Bullet(tank, content));
            }
        }

        static public void PosicaoDirecaoBala()
        {
            
            Vector3 offset = new Vector3(0, 2, 3);
            Matrix rotacao = Matrix.CreateRotationX(tank.CannonRotation) * Matrix.CreateRotationY(tank.TurretRotation) * Matrix.CreateFromQuaternion(tank.rotacaoFinal.Rotation);

            offset = Vector3.Transform(offset, rotacao);
            direcaoBala = Vector3.Transform(new Vector3(0,0,1), rotacao);
            posicaoBala = tank.position + offset;
        }

        static public void disparaBala()
        {
            PosicaoDirecaoBala();
            balaTemp = balasNaoAtivas.First();
            
            balaTemp.position = posicaoBala;
            balaTemp.direcao = direcaoBala;
            
            balasAtivas.Add(balaTemp);
            balasNaoAtivas.Remove(balaTemp);
            
        }

        static public void removerBala(Bullet bala)
        {
            
            balasAtivas.Remove(bala);
            balasNaoAtivas.Add(bala);
            
        }

       

        static public void UpdateBalas(GameTime gameTime)
        {
            //copiaBalasAtivas = balasAtivas.ToList();
            foreach (Bullet bala in balasAtivas)
            {
                bala.Update(gameTime, tank);
                             
            }
            balasAtivas.RemoveAll(b => b.position.Y < -50f);
            balasAtivas.RemoveAll(b => b.balaDestruida == true);
        }
        static public void DrawBalas(Matrix view, Matrix projection)
        {
            foreach (Bullet bala in balasAtivas)
            {
                
                bala.Draw(view, projection);
            }
        }

        static public List<Bullet> getListaBalasAtivas()
        {
            return balasAtivas;
        }

    }
}
