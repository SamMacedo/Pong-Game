#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Pong;
using Pong.GameManagement;

#endregion

namespace Pong.CustomMenus
{
    class IntroMenu : Menu
    {
        #region Campos

        // Texturas
        public static Texture2D Background;
        public static Texture2D Logo;

        // Variables auxiliares para la modificación del texto
        private StringBuilder topStringBuilder;
        private StringBuilder bottomStringBuilder;

        // Banderas
        private bool showLogo = false;
        private bool exiting = false;

        // Tiempo de espera en milisegundos entre cada aparación de letra
        int WaitTime = 150;

        // Mensajes
        private string message = "Un juego hecho por";
        private string message2 = "Teragames";

        // Indice que indica la posición de la letra que se va a desplegar
        private int index = 0;

        // Opacidad del logo
        private float LogoAlpha = 0f;

        // Contador que proporciona un tiempo de espera ya que se han desplegado todas la letras
        int finalCounter = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa el Menú
        /// </summary>
        /// <param name="name">Nombre del menú</param>
        public IntroMenu(string name)
            : base(name)
        {
            // Inicialización de objetos
            topStringBuilder = new StringBuilder();
            bottomStringBuilder = new StringBuilder();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que actualiza el Menú
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboard"></param>
        public override void Update(GameTime gameTime, KeyboardState keyboard)
        {
            // Si no se ha motrado el logo entonces se despliega el mensaje de arriba
            if (!showLogo)
            {
                /* Se utiliza Mod para saber si el tiempo transcurrido es divisible entre 150 (lo que indicaría que han
                 * transcurrido x milisegundos donde x = es igual al valor de WaitTime) */
                if (gameTime.TotalGameTime.Milliseconds % WaitTime == 0)
                {
                    // Se concatena al mensaje el sig. caracter
                    topStringBuilder.Append(message[index]);

                    // Si el caracter no es un espacio en blanco entonces reproduce un sonido
                    if (message[index] != ' ')
                        Global.sfx_Scroll.Play(Global.Volume_Sfx, 0f, 0f);

                    index++;

                    // Si se ha mostrado todo el mensaje entonces se enciende la bandera para comenzar a mostrar el logo
                    if (topStringBuilder.Length == message.Length)
                    {
                        showLogo = true;
                        index = 0;
                    }
                }
            }
            else
            {
                // Se incrementa la opacidad del logo hasta llegar a 1
                if (LogoAlpha < 1f)
                {
                    LogoAlpha += 0.0075f;
                }
                else
                {
                    // Si todavia no se ha terminado de mostrar el mensaje de abajo
                    if (!exiting)
                    {
                        /* Se utiliza Mod para saber si el tiempo transcurrido es divisible entre 150 (lo que indicaría que
                         * han transcurrido x milisegundos donde x = es igual al valor de WaitTime) */
                        if (gameTime.TotalGameTime.Milliseconds % WaitTime == 0)
                        {
                            // Si todavia no se ha mostrado todo el mensaje entonces se concatena el sig. caracter
                            if (index < message2.Length)
                            {
                                bottomStringBuilder.Append(message2[index]);

                                if (message2[index] != ' ')
                                    Global.sfx_Scroll.Play(Global.Volume_Sfx, 0f, 0f);

                                index++;
                            }
                            // Si ya se mostró todo el mensaje entonces se enciende la bandera para avanzar al sig. menú
                            else
                            {
                                exiting = true;
                            }
                        }
                    }
                    /* Si la bandera exiting es true entonces se incrementa un contador hasta llegar a 2 segundos y luego
                     * se  realiza la transición al sig. menú */
                    else
                    {
                        finalCounter += gameTime.ElapsedGameTime.Milliseconds;

                        if (finalCounter > 2000)
                        {
                            GameStateManager.GoToMenu("MainMenu");

                            Global.PlayMusic("Menus");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método que dibuja el menú
        /// </summary>
        /// <param name="spritebatch">SpriteBatch necesario para dibujar</param>
        /// <param name="BackgroundTexture">Textura de Fondo (se debe mandar null ya que internamente dentro del método de indica la textura que se va a dibujar)</param>
        public override void Draw(SpriteBatch spritebatch, Texture2D BackgroundTexture)
        {
            spritebatch.Begin();

            // Primero se dibuja el fondo
            spritebatch.Draw(Background, Vector2.Zero, Color.White);

            // Mensaje de arriba
            spritebatch.DrawString(Global.font_Font1, topStringBuilder, new Vector2(275, 75), Color.White);

            // Logo
            spritebatch.Draw(Logo, new Vector2(Global.ScreenWidth / 2 - Logo.Width / 2, Global.ScreenHeight / 2 - Logo.Height / 2), Color.White * LogoAlpha);

            // Mensaje de abajo
            spritebatch.DrawString(Global.font_Font1, bottomStringBuilder, new Vector2(335, 350), Color.White);

            spritebatch.End();
        }

        #endregion
    }
}
