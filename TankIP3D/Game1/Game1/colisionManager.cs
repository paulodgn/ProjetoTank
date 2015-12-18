using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    class ColisionManager
    {
        List<Tank> listaTanques;
        List<Bullet> listaBalas;
        Vector3 lastPosition;
        public ColisionManager(List<Tank> LTanques)
        {
            this.listaTanques = LTanques;
            listaBalas = new List<Bullet>();
        }
        //adiionar tanque á lista
        public void Add(Tank tank)
        {
            listaTanques.Add(tank);
        }


        //detetar colisoes dos inimigos com tanque do player
        private void colisionDetection(Tank tank)
        {
           
                //colisao de tanques inimigos com tanque do player
                foreach (Tank Tank in listaTanques)
                {
                    if (!Tank.playerControl)
                    {
                        if (tank.boundingSphere.Intersects(Tank.boundingSphere))
                        {
                            //tank.velocidade = 0;
                            Tank.velocidade = 0;
                        }
                        else
                        {
                            Tank.velocidade = 0.07f;
                        }
                    }
                }
                //colisao das balas com tanques inimigos
                //obter lista de balas ativas
                listaBalas = BulletManager.getListaBalasAtivas();
                foreach (Bullet bala in listaBalas)
                {
                    foreach (Tank TankInimigo in listaTanques)
                    {
                        if (bala.boundingSphere.Intersects(TankInimigo.boundingSphere))
                        {
                            TankInimigo.tankDestroyed = true;
                            bala.balaDestruida = true;
                        }
                    }
                    

                }
                

        }
        //colisoes de balas com inimigos


        //update de colisoes
        public void UpdateColisions(Tank tank)
        {
            
            colisionDetection(tank);
        }

    }
}
