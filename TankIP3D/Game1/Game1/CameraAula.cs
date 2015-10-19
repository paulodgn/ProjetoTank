using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class CameraAula
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

        public CameraAula()
        {
          
            velocidade = 0.8f;
            vetorBase = new Vector3(1, -0.5f, 0);
            posicao = new Vector3(-50, 50, -30);
            direcao = vetorBase;
            worldMatrix = Matrix.Identity;
            
        }

    
        public void frente(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            posicao = posicao + velocidade * direcao;
            target = posicao+direcao;//posicao + direcao;

        }

        public void moverTras(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            posicao = posicao - velocidade * direcao;
            target = posicao + direcao;//posicao + direcao;

        }

        
        public void rodarDireita(GameTime gameTime,float rodarX)
        {
            //yaw++;
            //yaw=yaw+grausPorPixels*pixels
            //yaw=yaw+velocidade*ellpasedgametime
            //gerar matrix rotacao yawpitchroll
            //rodar vetor base com a matriz rotacao, metodo matriz ou de vetor -> novo vetor de direcao
            //gerar novo target
            //gerar novamente a view

            //novadirecao=vector3.transform(baseVector,rot)rot=yawpitchroll
            time = gameTime.ElapsedGameTime.Milliseconds;
            yaw = yaw + velocidade *time / 1000;
            rotacao = Matrix.CreateFromYawPitchRoll(yaw, 0, pitch);
            worldMatrix = rotacao;
            direcao = Vector3.Transform(vetorBase, rotacao);
            target = posicao + direcao;

            


        }

        public void rodarEsquerda(GameTime gameTime,float rodarX)
        {
            
            time = gameTime.ElapsedGameTime.Milliseconds;
            yaw = yaw - velocidade *time / 1000;
            rotacao = Matrix.CreateFromYawPitchRoll(yaw, 0, pitch);
            worldMatrix = rotacao;
            direcao = Vector3.Transform(vetorBase, rotacao);
            
            target = posicao + direcao;

            
        }

        public void rodarCima(GameTime gameTime, float rodarY)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            pitch = pitch + velocidade * time/1000;
            rotacao = Matrix.CreateFromYawPitchRoll(yaw, 0, pitch);
            worldMatrix = rotacao;
            direcao = Vector3.Transform(vetorBase, rotacao);

            target = posicao + direcao;


        }

        public void rodarBaixo(GameTime gameTime,float rodarY)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            pitch = pitch - velocidade * time / 1000;
            rotacao = Matrix.CreateFromYawPitchRoll(yaw, 0, pitch);
            worldMatrix = rotacao;
            direcao = Vector3.Transform(vetorBase, rotacao);
            target = posicao + direcao;


        }

        public void strafeEsquerda(GameTime gameTime,float strafe)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            this.strafe = strafe + velocidade * time;
            posicao = posicao - velocidade * Vector3.Cross(vetorBase, Vector3.Up);
            
            target = posicao + vetorBase;

        }

        public void strafeDireita(GameTime gameTime, float strafe)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            this.strafe = strafe + velocidade * time;
            posicao = posicao + velocidade * Vector3.Cross(vetorBase, Vector3.Up);

            target = posicao + vetorBase;

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
                this.rodarDireita(gameTime, 0.001f);
                view = Matrix.CreateLookAt(posicao, target, Vector3.Up);
            }
            if (mouseState.X > posicaoRato.X)
            {
                this.rodarEsquerda(gameTime, 0.001f);
                view = Matrix.CreateLookAt(posicao, target, Vector3.Up);

            }
            //rotacao em y
            if (mouseState.Y > posicaoRato.Y)
            {
                this.rodarBaixo(gameTime, 0.001f);
                view = Matrix.CreateLookAt(posicao, target, Vector3.Up);
            }
            if (mouseState.Y < posicaoRato.Y)
            {
                this.rodarCima(gameTime, 0.001f);
                view = Matrix.CreateLookAt(posicao, target, Vector3.Up);
            }
            
            posicaoRato.X = mouseState.X;
            posicaoRato.Y = mouseState.Y;
        }
    }
}
//vector3.outerproduct -> dir*normal=andarlado
//vector3.transform
