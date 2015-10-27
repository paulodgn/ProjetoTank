using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        CameraAula camera;
        CameraVersao2 camera2;
        CameraSurfaceFollow cameraSurfaceFollow;
        Terreno terreno;
        Texture2D mapaAlturas,textura;
        BasicEffect effect;
        Vector2 mousePosition;
        float posicaoInicialRatoX, posicaoInicialRatoY;
        Vector2 posicaoRato;
        Plano plano;
        Terreno2 terreno2;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1000;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Criar e definir o resterizerState a utilizar para desenhar a geometria
            RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.CullMode = CullMode.None;
            //rasterizerState.FillMode = FillMode.WireFrame;
            rasterizerState.MultiSampleAntiAlias = true;
            GraphicsDevice.RasterizerState = rasterizerState;
            // TODO: use this.Content to load your game content here
            camera = new CameraAula();
            camera2 = new CameraVersao2();
            
            //Camera.Initialize(GraphicsDevice);
            
            mapaAlturas = Content.Load<Texture2D>("mapaAlturas");
            textura = Content.Load<Texture2D>("grass50x50");
            terreno = new Terreno(GraphicsDevice, mapaAlturas,mapaAlturas,1f,textura);
            terreno2 = new Terreno2(GraphicsDevice, mapaAlturas, mapaAlturas, 1f, textura);
            VertexPositionColorTexture[] vertices = terreno.getVertices();

            cameraSurfaceFollow = new CameraSurfaceFollow(vertices,mapaAlturas.Width);
            effect = new BasicEffect(GraphicsDevice);
            mousePosition = new Vector2(0, 0);
            IsMouseVisible = false;
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //Camera.Update(gameTime, GraphicsDevice);
            //input(gameTime);
            
            MouseState mouseState = Mouse.GetState();
            ////se a posicao se mantiver(rato parado) centra o rato
            if (mouseState.X == mousePosition.X && mouseState.Y == mousePosition.Y)
            {
                try
                {
                   Mouse.SetPosition(graphics.GraphicsDevice.Viewport.Height / 2, graphics.GraphicsDevice.Viewport.Width / 2);
                }
                catch (Exception e)
                { }
            }
            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;
            //camera2.input(gameTime, graphics);
            //camera.input(gameTime,graphics);
            cameraSurfaceFollow.input(gameTime, graphics);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            terreno2.Draw(GraphicsDevice,cameraSurfaceFollow);
            

            base.Draw(gameTime);
        }

        void input(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.W))
            {
                camera.frente(gameTime);
            }
            if (kb.IsKeyDown(Keys.S))
            {
                camera.moverTras(gameTime);
            }
            if (kb.IsKeyDown(Keys.Q))
            {
                camera.strafeEsquerda(gameTime, 0.08f);
            }
            if (kb.IsKeyDown(Keys.E))
            {
                camera.strafeDireita(gameTime, 0.08f);
            }

            //rato
            MouseState mouseState = Mouse.GetState();
            //rotacao em x
            if (mouseState.X < posicaoRato.X)
            {
                camera.rodarDireita(gameTime, 0.01f);
            }
            if (mouseState.X > posicaoRato.X)
            {
                camera.rodarEsquerda(gameTime, 0.01f);

            }
            //rotacao em y
            if (mouseState.Y > posicaoRato.Y)
            {
                camera.rodarBaixo(gameTime, 0.01f);
            }
            if (mouseState.Y < posicaoRato.Y)
            {
                camera.rodarCima(gameTime, 0.01f);
            }
            posicaoRato.X = mouseState.X;
            posicaoRato.Y = mouseState.Y;


        }
    }
}
