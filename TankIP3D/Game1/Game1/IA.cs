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
