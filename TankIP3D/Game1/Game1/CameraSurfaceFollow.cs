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
        float grausPorPixel = MathHelper.ToRadians(20) / 100;
        float diferencaX,diferencaY;
        float yaw, pitch,strafe;
        Vector3 vetorBase;
        public Matrix view,worldMatrix;
        
        Matrix rotacao;
        
        VertexPositionColorTexture[] vertices;
        int alturaMapa;
        MouseState posicaoRatoInicial;
        

        public CameraSurfaceFollow(GraphicsDeviceManager graphics,VertexPositionColorTexture[] vertices, int alturaMapa)
        {
            this.alturaMapa = alturaMapa;
            velocidade = 0.05f;
            vetorBase = new Vector3(1, -0.5f, 0);
            this.vertices = vertices;
            posicao = new Vector3(50, findAltura(), 50);
            
            direcao = vetorBase;
            worldMatrix = Matrix.Identity;
            
            Mouse.SetPosition(graphics.GraphicsDevice.Viewport.Height / 2, graphics.GraphicsDevice.Viewport.Width / 2);
            posicaoRatoInicial = Mouse.GetState();
            this.frente();
            updateCamera();
        }

        //surface follow
        // metodo para descobrir os quatro vertices em redor da camara
        public float findAltura()
        {
            //A e B sao vertices superiores, C e D sao os vertices inferiores
            //A-----------B
            //C-----------D
            int xA, zA, xB, zB, xC, zC, xD, zD;
            float yA = 0, yB = 0, yC = 0, yD = 0;
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
        public void frente()
        {
            posicao.Y = findAltura();
            //time = gameTime.ElapsedGameTime.Milliseconds;
            posicao = posicao + velocidade * direcao;
            target = posicao+direcao;//posicao + direcao;

        }

        public void moverTras(GameTime gameTime)
        {
            posicao.Y = findAltura();
            time = gameTime.ElapsedGameTime.Milliseconds;
            posicao = posicao - velocidade * direcao;
            target = posicao + direcao;//posicao + direcao;
        }

        
        public void rodarEsquerda(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            //yaw = yaw + velocidade;//(yaw + velocidade);
            yaw -= diferencaX *grausPorPixel;
        }

        public void rodarDireita(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            //yaw = yaw - velocidade;//(yaw - velocidade);
            yaw -= diferencaX * grausPorPixel;
        }

        public void rodarCima(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            //pitch = pitch + 0.01f;
            pitch -= diferencaY * grausPorPixel;
        }

        public void rodarBaixo(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            //pitch = pitch - 0.01f;
            pitch -= diferencaY * grausPorPixel;
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
        
        public void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.W))
            {
                this.frente();
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

         
            MouseState mouseState = Mouse.GetState();
            if(mouseState!=posicaoRatoInicial)
            {
                diferencaX = mouseState.Position.X - posicaoRatoInicial.Position.X;
                diferencaY = mouseState.Position.Y - posicaoRatoInicial.Position.Y;
                if (mouseState.X < posicaoRatoInicial.X)
                {
                    this.rodarEsquerda(gameTime);
                }
                if (mouseState.X > posicaoRatoInicial.X || kb.IsKeyDown(Keys.Right))
                {                    
                    this.rodarDireita(gameTime);
                }
                if (mouseState.Y > posicaoRatoInicial.Y || kb.IsKeyDown(Keys.Down))
                {
                    this.rodarBaixo(gameTime);
                    
                }
                if (mouseState.Y < posicaoRatoInicial.Y || kb.IsKeyDown(Keys.Up))
                {
                    this.rodarCima(gameTime);
                }
                try
                {
                    Mouse.SetPosition(graphics.GraphicsDevice.Viewport.Height / 2, graphics.GraphicsDevice.Viewport.Width / 2);
                }
                catch (Exception e)
                { }
                updateCamera();
                
            }
            
            
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

// para a camara rodar mais suavemente
//dif x
//grausporpixel=10graus/100
//yaw+=difx*grauporpixel
//createfromyawpitchroll(mathhelper.toradians(yaw)