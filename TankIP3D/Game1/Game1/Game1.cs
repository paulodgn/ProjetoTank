using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        CameraTank cameraTank;
        Terreno terreno;
        Texture2D mapaAlturas, textura;
        BasicEffect effect;
        Vector2 mousePosition;
        float posicaoInicialRatoX, posicaoInicialRatoY;
        Vector2 posicaoRato;
        Plano plano;
        Terreno2 terreno2;
        Tank tank;
        Tank tankEnimigo;
        ColisionManager colisionManager;
        List<Tank> listaTanques;
        Bullet bala;
        enum CameraAtiva
        {
            fps,
            free,
            cameraTank
        };
        CameraAtiva cameraAtiva;
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

            DebugShapeRenderer.Initialize(GraphicsDevice);
            listaTanques = new List<Tank>();
            Create3DAxis.Initialize(GraphicsDevice);
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
            terreno = new Terreno(GraphicsDevice, mapaAlturas, mapaAlturas, 1f, textura);
            //terreno2 = new Terreno2(GraphicsDevice, mapaAlturas, mapaAlturas, 1f, textura);
            VertexPositionNormalTexture[] vertices = terreno.getVertices();
            tank = new Tank(GraphicsDevice, terreno.getVertices(), terreno.larguraMapa,new Vector3(10,20,10), true,Content);
            tankEnimigo = new Tank(GraphicsDevice, terreno.getVertices(), terreno.larguraMapa,new Vector3(80,20,80) ,false,Content);

            listaTanques.Add(tankEnimigo);

            colisionManager = new ColisionManager(listaTanques);


            cameraSurfaceFollow = new CameraSurfaceFollow(graphics, vertices, mapaAlturas.Width);
            camera = new CameraAula(graphics);
            cameraTank = new CameraTank(graphics, vertices, mapaAlturas.Width, tank.getPosition(), tank.getWorldMAtrix(),tank.view);
            camera2 = new CameraVersao2();
            effect = new BasicEffect(GraphicsDevice);
            mousePosition = new Vector2(0, 0);
            IsMouseVisible = false;
            cameraAtiva = CameraAtiva.free;

            //float aspectRatio = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
            
            //effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 0.1f, 1000.0f);
            tank.LoadContent(Content);
            tankEnimigo.LoadContent(Content);
            
            //tank.world = Matrix.CreateRotationY(MathHelper.ToRadians(90));
            //tank.world = Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateScale(.001f); 
            //tank.world.Scale = new Vector3(0.01f, 0.01f, 0.01f);

            //bala = new Bullet(new Vector3(1, 1, 1), tank, Content);
            //bala.LoadContent(Content);
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




            //camera.input(gameTime,graphics);
            //cameraSurfaceFollow.UpdateInput(gameTime, graphics);


            //camera.UpdateInput(gameTime,graphics);
            //cameraSurfaceFollow.UpdateInput(gameTime, graphics);
            escolherCamara();
            if (cameraAtiva == CameraAtiva.fps)
            {
                cameraSurfaceFollow.UpdateInput(gameTime, graphics);
                //tank.view = cameraSurfaceFollow.view;
                //tank.projection = cameraSurfaceFollow.projection;
            }
            else if(cameraAtiva==CameraAtiva.free)
            {
                camera.UpdateInput(gameTime, graphics);
                //tank.view = camera.view;
                //tank.projection = camera.projection;
            }
            else
            {
                
                //cameraSurfaceFollow.updateCamera();
                //cameraTank.UpdateInput(gameTime, graphics,tank.getPosition());
                cameraTank.updateCamera(tank.getPosition(), tank.getWorldMAtrix(),tank.view,tank);
            }
            tank.Update(gameTime, tank);
            tankEnimigo.Update(gameTime,tank);
            
            //bala.Update(gameTime,tank);
            colisionManager.UpdateColisions(tank);
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



            if (cameraAtiva == CameraAtiva.fps)
            {
                terreno.Draw(GraphicsDevice, cameraSurfaceFollow.view, cameraSurfaceFollow.projection);
                tank.Draw(cameraSurfaceFollow.view, cameraSurfaceFollow.projection);
                tankEnimigo.Draw(cameraSurfaceFollow.view, cameraSurfaceFollow.projection);
                //terreno2.Draw2(GraphicsDevice, cameraSurfaceFollow.view);
                DebugShapeRenderer.Draw(gameTime, cameraSurfaceFollow.view, cameraSurfaceFollow.projection);
                //bala.Draw(cameraSurfaceFollow.view, cameraSurfaceFollow.projection);
               
            }
            else if(cameraAtiva == CameraAtiva.free)
            {
                terreno.Draw(GraphicsDevice, camera.view, camera.projection);
                DebugShapeRenderer.AddBoundingSphere(tank.boundingSphere, Color.Orange);
                DebugShapeRenderer.AddBoundingSphere(tankEnimigo.boundingSphere, Color.Orange);
                DebugShapeRenderer.Draw(gameTime, camera.view, camera.projection);
                tank.Draw(camera.view, camera.projection);
                tankEnimigo.Draw(camera.view, camera.projection);
                //bala.Draw(camera.view, camera.projection);
                // terreno2.Draw2(GraphicsDevice, camera.view);
                
            }
            else
            {
                terreno.Draw(GraphicsDevice, cameraTank.view, cameraTank.projection);
                DebugShapeRenderer.AddBoundingSphere(tank.boundingSphere, Color.Orange);
                DebugShapeRenderer.AddBoundingSphere(tankEnimigo.boundingSphere, Color.Orange);
                DebugShapeRenderer.Draw(gameTime, cameraTank.view, cameraTank.projection);
                tank.Draw(cameraTank.view, cameraTank.projection);
                tankEnimigo.Draw(cameraTank.view, cameraTank.projection);
                //bala.Draw(cameraTank.view, cameraTank.projection);
                
            }


            
            
            base.Draw(gameTime);
        }

        public void escolherCamara()
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.F1))
            {

                cameraAtiva = CameraAtiva.fps;
            }
            if (kb.IsKeyDown(Keys.F2))
            {
                cameraAtiva = CameraAtiva.free;
            }
            if (kb.IsKeyDown(Keys.F3))
            {
                cameraAtiva = CameraAtiva.cameraTank;
            }
        }



    }
}
