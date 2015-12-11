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
        private void colisionDetection(Tank tank, BulletManager bulletManager)
        {
           
                //colisao de tanques inimigos com tanque do player
                foreach (var TankInimigo in listaTanques)
                {

                    if (tank.boundingSphere.Intersects(TankInimigo.boundingSphere))
                    {
                        //tank.velocidade = 0;
                        TankInimigo.velocidade = 0;
                    }
                    else
                    {
                        TankInimigo.velocidade = 0.07f;
                    }
                }
                //colisao das balas com tanques inimigos
                //obter lista de balas ativas
                listaBalas = tank.bulletManager.getListaBalasAtivas();
                foreach (Bullet bala in listaBalas)
                {
                    foreach (var TankInimigo in listaTanques)
	                {
		                if(bala.boundingSphere.Intersects(TankInimigo.boundingSphere))
                        {
                            //tankinimigo destruido
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
