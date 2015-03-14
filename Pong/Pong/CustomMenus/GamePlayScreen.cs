#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Pong.GameManagement;
using Pong.GameManagement.Controls;

#endregion

namespace Pong.CustomMenus
{
    class GamePlayScreen : Menu
    {
        #region Campos

        // Textura de fondo
        public static Texture2D playgroundTexture;

        // Jugadores y Pelota
        public static Paddle player1;
        public static Paddle player2;
        public static Ball ball;

        // Bandera que indica el momento en el que un jugador gana
        private static  bool PlayerWon = false;

        // Variables auxiliares que permiten dar efecto de movimiento a los mensajes de texto
        private static float textScale = 1f;
        private static float scaleSpeed = 0.0005f;
        private static float scaleTimer = 0f;
        private static bool isGrowing = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa el menú
        /// </summary>
        /// <param name="name">Nombre del menú</param>
        public GamePlayScreen(string name)
            : base(name)
        {
        }

        #endregion

        #region Métodos principales

        /// <summary>
        /// Método que se encarga de actualizar el menú
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboard"></param>
        public override void Update(GameTime gameTime, KeyboardState keyboard)
        {
            // Si todavia no gana algún jugador...
            if (!PlayerWon)
            {
                // Si la pelota no ha salido del campo (anotació) entonces se actualizan los jugadores y la pelota
                if (ball.CanMove)
                {
                    player1.Update(ball);
                    player2.Update(ball);
                    ball.Update(player1, player2);
                }
                else
                {
                    // La posicion de los jugadores se centra
                    player1.Position.Y = Global.ScreenHeight / 2 - player1.Height / 2;
                    player2.Position.Y = Global.ScreenHeight / 2 - player2.Height / 2;

                    // Se muestra un mensaje de texto
                    UpdateText(gameTime);

                    // Solo se puede mover hasta que se presione <Enter>
                    if (keyboard.IsKeyDown(Keys.Enter))
                    {
                        ball.CanMove = true;

                        Global.sfx_Click.Play(Global.Volume_Sfx, 0f, 0f);
                    }
                }
            }
            // Si ya ha ganado un jugador entonces...
            else
            {
                // La posicion de los jugadores se centra
                player1.Position.Y = Global.ScreenHeight / 2 - player1.Height / 2;
                player2.Position.Y = Global.ScreenHeight / 2 - player2.Height / 2;

                // Se muestra un mensaje de texto
                UpdateText(gameTime);

                // Regresar al menú principal cuando se presione <Enter>
                if (keyboard.IsKeyDown(Keys.Enter))
                {
                    GameStateManager.GoToMenu("MainMenu");

                    Global.sfx_Click.Play(Global.Volume_Sfx, 0f, 0f);

                    Global.PlayMusic("Menus");
                }
            }

            /* Si la puntuación de cualquiera de los dos jugadores alcanza la puntuación máxima entonces se enciende
             * la bandera indicando que alguno de ellos ha ganado la partida */
            if (player1.Score == Global.MaxScore)
            {
                PlayerWon = true;
            }
            else if (player2.Score == Global.MaxScore)
            {
                PlayerWon = true;
            }
        }

        /// <summary>
        /// Método que dibuja el menú
        /// </summary>
        /// <param name="spritebatch">SpriteBatch necesario para dibujar</param>
        /// <param name="BackgroundTexture">Textura de Fondo (se debe mandar null ya que internamente dentro del método de indica la textura que se va a dibujar)</param>
        public override void Draw(SpriteBatch spriteBatch, Texture2D BackgroundTexture)
        {
            spriteBatch.Begin();

            // Primero se dibuja el fondo
            spriteBatch.Draw(playgroundTexture, Vector2.Zero, Color.White);

            // Se dibujan los jugadores y la pelota
            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            ball.Draw(spriteBatch);

            // Si el juego no ha terminado...
            if (!PlayerWon)
            {
                // Si se ha anotado un punto...
                if (!ball.CanMove)
                {
                    // Se dibuja una textura negra con transparencia
                    spriteBatch.Draw(Global.t2d_BlackDot, Vector2.Zero, null, Color.White * 0.5f, 0f, Vector2.Zero, new Vector2(Global.ScreenWidth, Global.ScreenHeight), SpriteEffects.None, 0f);



                    // Se encuentra el centro del texto que se va a mostrar
                    Vector2 origin = Global.font_Font2.MeasureString("Presiona <Enter> para empezar") / 2f;

                    // Finalmente se dibuja el mensaje de texto
                    spriteBatch.DrawString(Global.font_Font2, "Presiona <Enter> para empezar", new Vector2(Global.ScreenWidth/2, 300f), Color.White, 0f, origin, textScale, SpriteEffects.None, 0f);
                }
            }
            // Si el juego ha terminado...
            else
            {
                // Se dibuja una textura negra con transparencia
                spriteBatch.Draw(Global.t2d_BlackDot, Vector2.Zero, null, Color.White * 0.5f, 0f, Vector2.Zero, new Vector2(Global.ScreenWidth, Global.ScreenHeight), SpriteEffects.None, 0f);



                // Se encuentra el centro del texto que se va a mostrar
                Vector2 origin = Global.font_Font2.MeasureString("Fin del juego") / 2f;

                // Finalmente se dibuja el mensaje de texto
                spriteBatch.DrawString(Global.font_Font2, "Fin del juego", new Vector2(Global.ScreenWidth / 2, 200f), Color.White, 0f, origin, textScale, SpriteEffects.None, 0f);



                // Se encuentra el centro del texto que se va a mostrar
                origin = Global.font_Font1.MeasureString("Presiona <Enter> para regresar al menu principal") / 2f;

                // Finalmente se dibuja el mensaje de texto
                spriteBatch.DrawString(Global.font_Font1, "Presiona <Enter> para regresar al menu principal", new Vector2(Global.ScreenWidth / 2, 400f), Color.White, 0f, origin, textScale, SpriteEffects.None, 0f);
            }

            // Despliegue de la puntuación
            spriteBatch.DrawString(Global.font_Font2, player1.Score.ToString(), new Vector2(200, 50), Color.White);
            spriteBatch.DrawString(Global.font_Font2, player2.Score.ToString(), new Vector2(580, 50), Color.White);

            spriteBatch.End();
        }

        #endregion

        #region Métodos auxiliares

        /// <summary>
        /// Método que se encarga de empezar una partida nueva
        /// </summary>
        public static void ResetGame()
        {
            player1 = new Paddle(20, Keys.W, Keys.S, Paddle.PaddleColor.Green);

            // Dependiendo si se está jugando contra el CPU o no se crea el objeto del segundo jugador
            if (!Global.AgainstCpu)
            {
                player2 = new Paddle(758, Keys.Up, Keys.Down, Paddle.PaddleColor.Red);
            }
            else
            {
                player2 = new CpuPaddle();
            }


            ball = new Ball();

            PlayerWon = false;
        }

        /// <summary>
        /// Método que se encarga de 
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateText(GameTime gameTime)
        {
            // Se obtienen los milisegundos transcurridos
            scaleTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Si el encabezado está creciendo
            if (isGrowing)
            {
                // Se incrementa su tamaño
                textScale += scaleSpeed;

                // Cada 1.5 segundos se intercala ente creciendo/decreciendo
                if (scaleTimer > 1500f)
                {
                    isGrowing = false;
                    scaleTimer = 0f;
                }
            }

            // Si está decreciendo
            else
            {
                // Se disminuye su tamaño
                textScale -= scaleSpeed;

                // Cada 1.5 segundos se intercala ente creciendo/decreciendo
                if (scaleTimer > 1500f)
                {
                    isGrowing = true;
                    scaleTimer = 0f;
                }
            }
        }

        #endregion
    }
}
