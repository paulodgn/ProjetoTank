using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    static class IA
    {
        static Vector3 distancia;
        static float distanciaMinima;
        static public void Initialize()
        {
            distancia = new Vector3(0, 0, 0);
            distanciaMinima = 4f;
        }

        static public Vector3 GerirDistancia(List<Tank> listaTank, Tank tank)
        {
            foreach (Tank otherTank in listaTank)
            {
                if(tank != otherTank)
                {
                    distancia = distancia - (otherTank.position - tank.position);
                    if(Vector3.Distance(tank.position, otherTank.position) < distanciaMinima)
                    {
                        distancia -= distancia - (tank.position - otherTank.position);
                        Vector3.Normalize(distancia);
                    }
                    else
                    {
                        //break;
                        distancia = new Vector3(0, 0, 0);
                    }

                }

                
            }
            return distancia;

            
        }

    }
}
