#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Pong.GameManagement;
using Pong.GameManagement.Controls;
using Pong.CustomMenus;

#endregion

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Campos

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Bandera para determinar si se debe de salir del juego
        public static bool Exit = false;

        // Creación de los distintos menús
        PostIntroMenu postIntroMenu = new PostIntroMenu("PostIntroMenu");
        IntroMenu introMenu = new IntroMenu("IntroMenu");
        MainMenu mainMenu = new MainMenu("MainMenu");
        OptionsMenu optionsMenu = new OptionsMenu("OptionsMenu");
        GametypeMenu gametypeMenu = new GametypeMenu("GametypeMenu");
        PlayerOptionsMenu playerOptionsMenu = new PlayerOptionsMenu("PlayerOptionsMenu");
        CpuOptionsMenu cpuOptionsMenu = new CpuOptionsMenu("CpuOptionsMenu");
        GamePlayScreen gamePlayScreen = new GamePlayScreen("GamePlayScreen");

        #endregion

        #region Constructor

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        #endregion

        #region Inicialización

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Global.ScreenWidth = graphics.PreferredBackBufferWidth;
            Global.ScreenHeight = graphics.PreferredBackBufferHeight;

            // Se agregan los menús al GameStateManager
            GameStateManager.AddMenu(postIntroMenu);
            GameStateManager.AddMenu(introMenu);
            GameStateManager.AddMenu(mainMenu);
            GameStateManager.AddMenu(optionsMenu);
            GameStateManager.AddMenu(gametypeMenu);
            GameStateManager.AddMenu(playerOptionsMenu);
            GameStateManager.AddMenu(cpuOptionsMenu);
            GameStateManager.AddMenu(gamePlayScreen);

            base.Initialize();
        }

        #endregion

        #region Carga y descarga de contenido

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Fondo de los menús y textura para las transiciones
            Global.t2d_MenusBackground = Content.Load<Texture2D>("MenusBackground");
            Global.t2d_BlackDot = Content.Load<Texture2D>("BlackTexture");

            // Sonidos
            Global.sfx_Scroll = Content.Load<SoundEffect>("Scroll");
            Global.sfx_Click = Content.Load<SoundEffect>("Click");
            Global.sfx_Switch = Content.Load<SoundEffect>("Switch");
            Global.sfx_BallBounce = Content.Load<SoundEffect>("Bounce3");
            Global.sfx_Score = Content.Load<SoundEffect>("Score");

            // Música
            Global.sng_Menus = Content.Load<Song>("DST-Canopy");
            Global.sng_PlayGround = Content.Load<Song>("DST-CauseWay");

            // Fuentes
            Global.font_Font1 = Content.Load<SpriteFont>("Fuente");
            Global.font_Font2 = Content.Load<SpriteFont>("Fuente2");

            // Texturas del intro
            IntroMenu.Background = Content.Load<Texture2D>("IntroBackground");
            IntroMenu.Logo = Content.Load<Texture2D>("Logo");

            // Cabeceras de los menús
            MainMenu.Header = Content.Load<Texture2D>("Title_Main");
            OptionsMenu.Header = Content.Load<Texture2D>("Title_Options");
            GametypeMenu.Header = Content.Load<Texture2D>("Title_Play");
            CpuOptionsMenu.Header = Content.Load<Texture2D>("Title_GameOptions");
            PlayerOptionsMenu.Header = Content.Load<Texture2D>("Title_GameOptions");

            // Texturas del campo de juego
            Ball.Texture = Content.Load<Texture2D>("Ball");
            Paddle.RedTexture = Content.Load<Texture2D>("RedPaddle");
            Paddle.GreenTexture = Content.Load<Texture2D>("GreenPaddle");
            GamePlayScreen.playgroundTexture = Content.Load<Texture2D>("PlayGround");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        #region Actualización

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Exit)
                this.Exit();

            // Actualización del menú actual
            GameStateManager.Update(gameTime);

            base.Update(gameTime);
        }

        #endregion

        #region Dibujado

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Dibujado del menú actual
            GameStateManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        #endregion
    }
}
