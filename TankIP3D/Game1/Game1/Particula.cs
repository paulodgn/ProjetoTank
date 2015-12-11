using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    class Particula
    {
        BasicEffect effect;
        VertexPositionColor[] vertices;
        public Vector3 posicaoInicial, direcao, velocidade; 
        float posicaoX, posicaoY;
        public Vector3 posicao;
        Matrix worldMatrix;
        float raioDisco, anguloDisco, angulo;
        GraphicsDevice device;
        float magnitudeLargura, magnitudeAltura;//magnitude é  distancia entre centro e borda do disco
        float velocidadeQueda, direcaoEsguelhaX, direcaoEsguelhaZ;
        public float direcaoDeEsguelha;
        Vector3 centro;
        float randomMagnitude, randomPosicao;
        float time, TotalTime;
        Vector3 novaVelocidade;
        float larguraRetangulo, alturaRetangulo;
        public Particula(GraphicsDevice device, float largura,float altura ,Vector3 centro)
        {
            this.centro = centro;
            this.larguraRetangulo = largura;
            this.alturaRetangulo = altura;
            this.device = device;
            //vetices que compoem a particula.
            vertices = new VertexPositionColor[2];
            
            effect = new BasicEffect(device);
            worldMatrix = Matrix.Identity;
            // direcaoDeEsguelha define o limite de inclinaçao que a direcao da particula pode obter.
            direcaoDeEsguelha = 0.2f;
            //define a velocidade a que a particula se desloca na vertical.
            velocidadeQueda = 0.05f;

            
        }

        public void CreateParticle(GameTime gametime, Vector3 posicaoCentro, float larguraRetangulo, float alturaRetangulo, Vector3 novaDirecao, Tank tank)
        {
            centro = posicaoCentro;
            time += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            //dgeracao de valores random para definir posicao e magnitude
            randomPosicao = RandomGenerator.getRandomNext();
            randomMagnitude = RandomGenerator.getRandomMinMax();
            
            //calcular angulo e magnitude para a posiçao inicial da particula
            anguloDisco = randomPosicao * time * (float)(2 * Math.PI / 360);
            magnitudeLargura = randomMagnitude ;
            magnitudeAltura = randomMagnitude /** (2 * alturaRetangulo - alturaRetangulo)*/;
            
            //para definir a posicao soma-se ao centro o valor do raio mais a magnitude para que encontre
            //em ponto intermiedio entre o centro e o limite exterior do disco.
            posicaoX = centro.X + larguraRetangulo * magnitudeLargura;
            posicaoY = centro.Y + alturaRetangulo * magnitudeAltura;
            this.posicao = new Vector3(posicaoX, posicaoY, centro.Z);
            //rotacaoParticulas(posicao, tank);
            
            //criaçao dos vertices que compoem a particula, um recebe a posicao calculada o outro é criado um pouco abaixo.
            vertices[0].Position = this.posicao;
            vertices[0].Color = Color.White;
            vertices[1].Position = this.posicao + new Vector3(0,0.001f,0);
            vertices[1].Color = Color.White;
            
            //define-se uma direcao aleatoria para x e z, para que as gotas nao tenham todas a mesma direcao.
            //direcaoEsguelhaX = RandomGenerator.getRandomNextDouble() * (2 * direcaoDeEsguelha - direcaoDeEsguelha);
            //direcaoEsguelhaZ = RandomGenerator.getRandomNextDouble() * (2 * direcaoDeEsguelha - direcaoDeEsguelha);
            //novaVelocidade = new Vector3(direcaoEsguelhaX,-1f,direcaoEsguelhaX);
            Vector3 direcao = RandomGenerator.getRandomNextDouble() * novaDirecao + new Vector3(0,1,0);
            velocidade = direcao;
            
        }

        public void Update(GameTime gametime)
        {
            time = (float)gametime.ElapsedGameTime.TotalSeconds ;

            Vector3 acelaracao = new Vector3(0, -0.98f, 0);
            velocidade = velocidade + acelaracao * velocidadeQueda;
            posicao = posicao + velocidade * time;
            // actualiza-se a posicao, somando-lhe a direcao(velocidade) e multiplica-se pela velocidade de queda definida no construtor.
            
            vertices[0].Position = posicao;
            vertices[1].Position = posicao + new Vector3(0, 0.02f, 0);


            //a=(0,-9.8,0)
            //v=v0+a*t
            //p=p0+v*t
        }

        public void Draw(Matrix Cview, Matrix Cproj)
        {
            
            effect.TextureEnabled = false;
            effect.VertexColorEnabled = true;
            this.effect.View = Cview;
            this.effect.Projection = Cproj;

            effect.World = worldMatrix;

            effect.CurrentTechnique.Passes[0].Apply();
            //cada instancia da partcula desenha os seus dois vertices
            device.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
            
        }

        public void rotacaoParticulas(Vector3 posicaoParticula, Tank tank)
        {
           // Matrix rotacaoparticula = Matrix.CreateFromQuaternion(tank.rotacaoFinal.Rotation);
            //posicaoParticula = Vector3.Transform(posicaoParticula, tank.world);
            Matrix rotacaoparticula =  Matrix.CreateFromQuaternion(tank.rotacaoFinal.Rotation) * Matrix.CreateTranslation(posicaoParticula);
            this.worldMatrix =  Matrix.CreateScale(0.05f) * rotacaoparticula;
            
            //return posicaoParticula;
        }

       
    }
}
