using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    class SistemaParticulas
    {
        List<Particula> listaParticulas;
        List<Particula> listaParticulasAtiva;
        List<Particula> listaSemGestao;
        GraphicsDevice device;
        float raioDisco;
        Particula particulaTemp;
        float direcaoDeEsguelhaOffset;
        public Vector3 posicaoCentro;
        public BasicEffect effect;
        public Matrix worldMatrix;
        int quantidadeParticulas;
        Matrix view, projection;
        float alturaRetangulo, larguraRetangulo;
        Tank tank;
        public SistemaParticulas(GraphicsDevice device , Vector3 centro, float largura, float altura)
        {
            
            quantidadeParticulas = 3000;
            posicaoCentro = centro;
            this.device = device;
            
            listaParticulas = new List<Particula>(quantidadeParticulas);
            listaParticulasAtiva = new List<Particula>(quantidadeParticulas);
            listaSemGestao = new List<Particula>(quantidadeParticulas);
            
            
            
            direcaoDeEsguelhaOffset = 0.3f;

            effect = new BasicEffect(device);
            worldMatrix = Matrix.Identity;
            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;
            //effect.View = Matrix.CreateLookAt(new Vector3(0.0f, 0f, 3f), Vector3.Zero, Vector3.Up);
            //effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 0.1f, 1000.0f);
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;

            this.larguraRetangulo = largura;
            this.alturaRetangulo = altura;
            CriarParticulas(quantidadeParticulas);
        }
        //a lista de particulas nao ativa é preenchida com a quantidade de particulas desejada
        public void CriarParticulas(int quantidadeParticulas)
        {
            for (int i = 0; i < quantidadeParticulas; i++)
            {
                listaParticulas.Add(new Particula(device,larguraRetangulo,alturaRetangulo, posicaoCentro));
            }
        }

        public void Update(GameTime gametime, Vector3 posicao, Vector3 novaDirecao, Tank tank)
        {
            this.posicaoCentro = posicao;
            //para cada Update retiram-se 3 particulas da lista nao ativa e colocam-se as mesmas na lista de particulas ativas.
            for (int i = 0; i < 3; i++)
            {
                if (listaParticulasAtiva.Count < 2000)
                {
                    //particula temporaria recebe a primeira particula da lista de nao ativas.
                    particulaTemp = listaParticulas.First();
                    //calcula posicao e direcao.
                    particulaTemp.CreateParticle(gametime, posicaoCentro, larguraRetangulo, alturaRetangulo, novaDirecao, tank);
                    //adiciona particula a lista ativa.
                    listaParticulasAtiva.Add(particulaTemp);
                    //remove da lista nao ativa.
                    listaParticulas.Remove(particulaTemp);
                }
            }


            foreach (Particula p in listaParticulasAtiva)
            {
                //Update de cada particula da lista ativa.
                p.Update(gametime);
                //se a particula ultrapassar a posicao em Y de -0.5...
                if (p.posicao.Y < -10f)
                {
                    //...é adicionada á lista nao ativa...
                    listaParticulas.Add(p);
                }

            }
            //... e é removida da lista ativa.
            listaParticulasAtiva.RemoveAll(particula => particula.posicao.Y < -10f);
            
        }

        public void Draw(Matrix view, Matrix proj)
        {
            //cada particula na lista ativa é desenhada.
            foreach (Particula p in listaParticulasAtiva)
            {
                p.Draw(view,proj);
            }
            
        }
            

        public BasicEffect getEffect()
        {
            return effect;
        }


    }
}
