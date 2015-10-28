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

        
      

        public void rodarDireitaEsquerda(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            //yaw = yaw - velocidade;//(yaw - velocidade);
            yaw += diferencaX * grausPorPixel;
        }

        public void rodarCimaBaixo(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.Milliseconds;
            //pitch = pitch + 0.01f;
            pitch += diferencaY * grausPorPixel;
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

        public void UpdateInput(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.W))
            {
                this.frente();
                
            }
            if (kb.IsKeyDown(Keys.S))
            {
                this.moverTras(gameTime);
                
            }
            if (kb.IsKeyDown(Keys.Q))
            {
                this.strafeEsquerda(gameTime, 0.08f);
                
            }
            if (kb.IsKeyDown(Keys.E))
            {
                this.strafeDireita(gameTime, 0.08f);
            }
            
            MouseState mouseState = Mouse.GetState();
            if (mouseState != posicaoRatoInicial)
            {
                diferencaX = mouseState.Position.X - posicaoRatoInicial.Position.X;
                diferencaY = mouseState.Position.Y - posicaoRatoInicial.Position.Y;
                this.rodarDireitaEsquerda(gameTime);
                this.rodarCimaBaixo(gameTime);
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
