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
            
            
            //Camera.Initialize(GraphicsDevice);
            
            mapaAlturas = Content.Load<Texture2D>("mapaAlturas");
            textura = Content.Load<Texture2D>("grass50x50");
            terreno = new Terreno(GraphicsDevice, mapaAlturas,mapaAlturas,1f,textura);
            terreno2 = new Terreno2(GraphicsDevice, mapaAlturas, mapaAlturas, 1f, textura);
            VertexPositionColorTexture[] vertices = terreno.getVertices();

            cameraSurfaceFollow = new CameraSurfaceFollow(graphics,vertices,mapaAlturas.Width);
            camera = new CameraAula(graphics);
            camera2 = new CameraVersao2();
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
            
          
            //camera2.input(gameTime, graphics);
<<<<<<< HEAD

            camera.UpdateInput(gameTime,graphics);
            

            //camera.input(gameTime,graphics);
            //cameraSurfaceFollow.UpdateInput(gameTime, graphics);

=======
            camera.UpdateInput(gameTime,graphics);
            //cameraSurfaceFollow.UpdateInput(gameTime, graphics);
>>>>>>> 11a2e1a017af360c698a37a2097f7a6c6fa6bf4f
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
<<<<<<< HEAD
            terreno2.Draw(GraphicsDevice,camera.view);
=======
            terreno.Draw(GraphicsDevice,camera.view);
>>>>>>> 11a2e1a017af360c698a37a2097f7a6c6fa6bf4f
            

            base.Draw(gameTime);
        }

     

        
    }
}
