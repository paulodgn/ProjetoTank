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
        Vector3 lastPosition;
        public ColisionManager(List<Tank> LTanques)
        {
            this.listaTanques = LTanques;
            
        }
        //adiionar tanque á lista
        public void Add(Tank tank)
        {
            listaTanques.Add(tank);
        }


        //detetar colisoes dos inimigos com tanque do player
        private void colisionDetection(Tank tank)
        {
           
            
                foreach (var segundoTank in listaTanques)
                {
                    
                    if(tank.boundingSphere.Intersects(segundoTank.boundingSphere))
                    {
                        //tank.velocidade = 0;
                        segundoTank.velocidade = 0;
                    }
                    else
                    {
                        segundoTank.velocidade = 0.07f;
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
