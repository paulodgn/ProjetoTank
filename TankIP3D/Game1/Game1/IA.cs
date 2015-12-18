using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    static class IA
    {
        static float distancia;
        static float distanciaMinima;
        static Vector3 somaForcas;
        static int cont;
        static public void Initialize()
        {
            
            distanciaMinima = 20f;
        }

        static public Vector3 GerirDistancia(List<Tank> listaTank, Tank tank)
        {
            cont = 0;
            somaForcas = Vector3.Zero;
            foreach (Tank otherTank in listaTank)
            {
                if (!otherTank.playerControl)
                {
                    distancia = Vector3.Distance(tank.position, otherTank.position);
                    if (distancia > 0 && distancia < distanciaMinima)
                    {
                        Vector3 diferenca = tank.position - otherTank.position;
                        Vector3.Normalize(diferenca);
                        somaForcas += diferenca;

                        cont++;
                    }
                }
                    
            }
            if (cont > 0)
            {
                somaForcas = somaForcas / cont;
            }

            return somaForcas;
        }

    }
}
 //foreach (Tank otherTank in listaTank)
 //           {
 //               if(tank != otherTank)
 //               {
 //                   //distancia = new Vector3(0, 0, 0);
 //                   distancia = distancia - (otherTank.position - tank.position);

 //                   if (Vector3.Distance(otherTank.position, tank.position) < distanciaMinima && distanciaMinima>0)
 //                   {
 //                       DebugShapeRenderer.AddLine(tank.position, tank.position + distancia, Color.Orange);
 //                       distancia -= (tank.position - otherTank.position);
 //                       Vector3.Normalize(distancia);
 //                   }
 //                   else
 //                   {
                        
 //                       distancia = new Vector3(0, 0, 0);

 //                   }

 //               }

                
 //           }
 //           return distancia;