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

        public void Add(Tank tank)
        {
            listaTanques.Add(tank);
        }



        private void colisionDetection(Tank tank)
        {
            //foreach (var tank in listaTanques)
            //{
            
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
            //}
        }

        public void UpdateColisions(Tank tank)
        {
            
            colisionDetection(tank);
        }

    }
}
