using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class CameraSurfaceFollow
    {
        Vector3 posicao, direcao, target;
        float velocidade, time;
        float grausPorPixel = 20 / 100;
        float yaw, pitch, roll,strafe;
        Vector3 vetorBase;
        public Matrix view, projection,worldMatrix;
        Vector2 posicaoRato;
        Matrix rotacao;
        Vector3 vUpaux;
        Vector3 vUp;
        VertexPositionColorTexture[] vertices;
        int alturaMapa;

        public CameraSurfaceFollow(VertexPositionColorTexture[] vertices, int alturaMapa)
        {
            this.alturaMapa = alturaMapa;
            velocidade = 0.009f;
            vetorBase = new Vector3(1, -0.5f, 0);
            posicao = new Vector3(50, 10, 50);
            direcao = vetorBase;
            worldMatrix = Matrix.Identity;
            this.vertices = vertices;
        }

        //surface follow
        // metodo para descobrir os quatro vertices em redor da camara
        public float findAltura()
        {
            //A e B sao vertices superiores, C e D sao os vertices inferiores
            //A-----------B
            //C-----------D
            int xA, zA, xB, zB, xC, zC, xD, zD;
            float yA=0,yB=0,yC=0,yD=0;
            xA = (int)this.posicao.X;
            zA = (int)this.posicao.Z;

            xB = xA + 1;
            zB = zA;

            xC = xA;
            zC = zA + 1;

            xD = xB;
            zD = zC;

            //encontrar valor de Y de cada vertice

            yA = vertices[xA * alturaMapa + zA].Position.Y;
            yB = vertices[xB * alturaMapa + zB].Position.Y;
            yC = vertices[xC * alturaMapa + zC].Position.Y;
            yD = vertices[xD * alturaMapa + zD].Position.Y;

            //foreach (var vertice in vertices)
            //{
            //    if(vertice.Position.X==xA && vertice.Position.Z==zA)
            //    {
            //        yA = vertice.Position.Y;
            //    }
            //    if (vertice.Position.X == xB && vertice.Position.Z == zB)
            //    {
            //        yB = vertice.Position.Y;
            //    }
            //    if (vertice.Position.X == xC && vertice.Position.Z == zC)
            //    {
            //        yC = vertice.Position.Y;
            //    }
            //    if (vertice.Position.X == xD && vertice.Position.Z == zD)
            //    {
            //        yD = vertice.Position.Y;
            //    }
            //}

            //calcular nova altura da camara
            float yAB, yCD, cameraY;

            yAB = (1 - (this.posicao.X - xA)) * yA + (this.posicao.X - xA) * yB;
            yCD = (1 - (this.posicao.X - xC)) * yC + (this.posicao.X - xC) * yD;
            cameraY = (1 - (this.posicao.Z - zA)) * yAB + (this.posicao.Z - zA) * yCD;
            return (cameraY+1);
        }

        //surface follow end

        //Movimento da camara
        public void frente(GameTime gameTime)
        {
            posicao.Y = findAltura();
            time = gameTime.ElapsedGameTime.Milliseconds;
            posicao = posicao + 0.05f * direcao;
            target = posicao+direcao;//posicao + direcao;

        }

        public void moverTras(GameTime gameTime)
        {
            posicao.Y = findAltura();
            time = gameTime.ElapsedGameTime.Milliseconds;
            posicao = posicao - 0.05f * direcao;
            target = posicao + direcao;//posicao + direcao;
        }

        
        public void rodarDireita(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            yaw = yaw + velocidade;//(yaw + velocidade);
        }

        public void rodarEsquerda(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            yaw = yaw - velocidade;//(yaw - velocidade);
        }

        public void rodarCima(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            pitch = pitch + velocidade;
        }

        public void rodarBaixo(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            pitch = pitch - velocidade;
        }

        public void strafeEsquerda(GameTime gameTime,float strafe)
        {
            posicao.Y = findAltura();
            time = gameTime.ElapsedGameTime.Milliseconds;
            this.strafe = strafe + velocidade * time;
            posicao = posicao - velocidade * Vector3.Cross(direcao, Vector3.Up);
            
            target = posicao + direcao;

        }

        public void strafeDireita(GameTime gameTime, float strafe)
        {
            posicao.Y = findAltura();
            time = gameTime.ElapsedGameTime.Milliseconds;
            this.strafe = strafe + velocidade * time;
            posicao = posicao + velocidade * Vector3.Cross(direcao, Vector3.Up);

            target = posicao + direcao;

        }
        
        public void input(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.W))
            {
                this.frente(gameTime);
                view = Matrix.CreateLookAt(posicao, target, Vector3.Up);
            }
            if (kb.IsKeyDown(Keys.S))
            {
                this.moverTras(gameTime);
                view = Matrix.CreateLookAt(posicao, target, Vector3.Up);
            }
            if (kb.IsKeyDown(Keys.Q))
            {
                this.strafeEsquerda(gameTime, 0.08f);
                view = Matrix.CreateLookAt(posicao, target, Vector3.Up);
            }
            if (kb.IsKeyDown(Keys.E))
            {
                this.strafeDireita(gameTime, 0.08f);
                view = Matrix.CreateLookAt(posicao, target, Vector3.Up);
            }

            //rato
            MouseState mouseState = Mouse.GetState();
            //rotacao em x
            if (mouseState.X < posicaoRato.X)
            {
                
                this.rodarDireita(gameTime);
                updateCamera();
            }
            if (mouseState.X > posicaoRato.X)
            {
                this.rodarEsquerda(gameTime);
                updateCamera();

            }
            //rotacao em y
            if (mouseState.Y > posicaoRato.Y)
            {
                this.rodarBaixo(gameTime);
                updateCamera();
            }
            if (mouseState.Y < posicaoRato.Y)
            {
                this.rodarCima(gameTime);
                updateCamera();
            }
            
            posicaoRato.X = mouseState.X;
            posicaoRato.Y = mouseState.Y;
        }

        public void updateCamera()
        {
            rotacao = Matrix.CreateFromYawPitchRoll(yaw, 0, pitch);
            worldMatrix = rotacao;
            direcao = Vector3.Transform(vetorBase, rotacao);
            target = posicao + direcao;
            view = Matrix.CreateLookAt(posicao, target, Vector3.Up);
        }
    }
}
//vector3.outerproduct -> dir*normal=andarlado
//vector3.transform


//surface follow
//usar o xyz do terreno no cpu para usar na posicao da camara
//usar uma matriz para guardar as coordenadas dos vertices para usar na camara
//float alturas[]alturas
//procurar a posicao da camara no array, y=alturas[z*larguraTerreno+x]+offset
//camara raranente vai estar em cima de um vertice
//esta num sitio algures entre 4 vertices
//para descobrir os vertices fazer um cast para int das corrdenadas da camara. do x e do z
//interpolacao bilinear
//fazer primeiro uma dimensao x e depois noutra em z
//a*---------------------x---------b*
//yab
//x -valor de y>
//c*---------------------x---------d*
//ycb
//a coordenada x de a=cast para int do x da camara
//z vvertc a=int z da camara
//xb=xa+1
//zb=za
//ya
//yb
//yc
//yd
//Yab=(1-(X-Xa))Ya+(x-Xa)Ya
//Ycd=(1-(X-Xc))Yc+(x-Xc)Yd
//Y=(1-(z-za))Yab+(z-za)Ycd
//camara nao pode sair do terreno