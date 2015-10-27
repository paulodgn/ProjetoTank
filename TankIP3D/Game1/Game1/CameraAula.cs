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
        float grausPorPixel = MathHelper.ToRadians(20) / 100;
        float yaw, pitch, roll,strafe;
        float diferencaX, diferencaY;
        Vector3 vetorBase;
        public Matrix view, projection,worldMatrix;
        Vector2 posicaoRato;
        Matrix rotacao;
        Vector3 vUpaux;
        Vector3 vUp;
        MouseState posicaoRatoInicial;
        public CameraAula(GraphicsDeviceManager graphics)
        {
          
            velocidade = 0.5f;
            vetorBase = new Vector3(1, -0.5f, 0);
            posicao = new Vector3(-50, 50, -30);
            direcao = vetorBase;
            worldMatrix = Matrix.Identity;
            Mouse.SetPosition(graphics.GraphicsDevice.Viewport.Height / 2, graphics.GraphicsDevice.Viewport.Width / 2);
            posicaoRatoInicial = Mouse.GetState();
            this.frente();
            updateCamera();
        }

    
        public void frente()
        {
            
            //time = gameTime.ElapsedGameTime.Milliseconds;
            posicao = posicao + velocidade * direcao;
            target = posicao+direcao;//posicao + direcao;

        }

        public void moverTras(GameTime gameTime)
        {
            
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
            
            time = gameTime.ElapsedGameTime.Milliseconds;
            this.strafe = strafe + velocidade * time;
            posicao = posicao - velocidade * Vector3.Cross(direcao, Vector3.Up);
            
            target = posicao + direcao;

        }

        public void strafeDireita(GameTime gameTime, float strafe)
        {
            
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
            if (mouseState != posicaoRatoInicial)
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
