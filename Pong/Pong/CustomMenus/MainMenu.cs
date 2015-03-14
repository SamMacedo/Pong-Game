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
using Pong.GameManagement.Controls;

#endregion

namespace Pong.CustomMenus
{
    public class MainMenu : Menu
    {
        #region Campos

        // Textura y variables auxiliares para el encabezado del menú
        public static Texture2D Header;
        private float headerScale = 1f;
        private const float scaleSpeed = 0.0005f;
        private float scaleTimer = 0f;
        private bool isGrowing = true;

        // Controles del menú
        ClickControl playClickControl = new ClickControl("Jugar");
        ClickControl optionsClickControl = new ClickControl("Opciones");
        ClickControl exitClickControl = new ClickControl("Salir");

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa el menú
        /// </summary>
        /// <param name="name">Nombre del menú</param>
        public MainMenu(string name)
            : base(name)
        {
            // Asignación de Handlers para los eventos de los controles
            this.playClickControl.OnClicked += new ClickControl.ControlClickHandler(playClickControl_OnClicked);
            this.optionsClickControl.OnClicked += new ClickControl.ControlClickHandler(optionsClickControl_OnClicked);
            this.exitClickControl.OnClicked += new ClickControl.ControlClickHandler(exitClickControl_OnClicked);

            // Se agregan los controles al menú
            AddControl(playClickControl);
            AddControl(optionsClickControl);
            AddControl(exitClickControl);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que ocurre al hacer click en "Salir"
        /// </summary>
        /// <param name="control">Control que produce el evento</param>
        void exitClickControl_OnClicked(Control control)
        {
            Game1.Exit = true;
        }

        /// <summary>
        /// Evento que ocurre al hacer click en "Opciones"
        /// </summary>
        /// <param name="control">Control que produce el evento</param>
        void optionsClickControl_OnClicked(Control control)
        {
            GameStateManager.GoToMenu("OptionsMenu");
        }

        /// <summary>
        /// Evento que ocurre al hacer click en "Jugar"
        /// </summary>
        /// <param name="control">Control que produce el evento</param>
        void playClickControl_OnClicked(Control control)
        {
            GameStateManager.GoToMenu("GametypeMenu");
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que se encarga de actualizar el menú
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboard"></param>
        public override void Update(GameTime gameTime, KeyboardState keyboard)
        {
            base.Update(gameTime, keyboard);

            // Se obtienen los milisegundos transcurridos
            scaleTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Si el encabezado está creciendo
            if (isGrowing)
            {
                // Se incrementa su tamaño
                headerScale += scaleSpeed;

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
                headerScale -= scaleSpeed;

                // Cada 1.5 segundos se intercala ente creciendo/decreciendo
                if (scaleTimer > 1500f)
                {
                    isGrowing = true;
                    scaleTimer = 0f;
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
            // Se dibujan los controles y el fondo
            base.Draw(spritebatch, Global.t2d_MenusBackground);

            // Se dibuja el encabezado
            spritebatch.Begin();
            spritebatch.Draw(Header, new Vector2(Global.ScreenWidth/2, 125), null, Color.White, 0f, new Vector2(Header.Width/2, Header.Height/2), headerScale, SpriteEffects.None, 0f);
            spritebatch.End();
        }

        #endregion
    }
}
