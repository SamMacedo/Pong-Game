#region Directivas uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Pong.GameManagement;
using Pong.GameManagement.Controls;

#endregion

namespace Pong.CustomMenus
{
    class PlayerOptionsMenu : Menu
    {
        #region Campos

        // Textura y variables auxiliares para el encabezado del menú
        public static Texture2D Header;
        private float headerScale = 1f;
        private const float scaleSpeed = 0.0005f;
        private float scaleTimer = 0f;
        private bool isGrowing = true;

        // Controles del menú
        NumericSelectorControl maxScoreControl = new NumericSelectorControl("Puntuacion Maxima: ");
        ClickControl readyControl = new ClickControl("Listo");
        ClickControl returnControl = new ClickControl("Regresar");

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa el menú
        /// </summary>
        /// <param name="name">Nombre del menú</param>
        public PlayerOptionsMenu(string name)
            : base(name)
        {
            // Asignación de Handlers para los eventos de los controles
            this.maxScoreControl.ValueChanged += new NumericSelectorControl.ValueChangedHandler(maxScoreControl_ValueChanged);
            this.readyControl.OnClicked += new ClickControl.ControlClickHandler(readyControl_OnClicked);
            this.returnControl.OnClicked += new ClickControl.ControlClickHandler(returnControl_OnClicked);

            // Se asignan por default los valores a los controles
            maxScoreControl.MinValue = 1;
            maxScoreControl.MaxValue = 25;
            maxScoreControl.Value = 10;

            // Se agregan los controles al menú
            AddControl(maxScoreControl);
            AddControl(readyControl);
            AddControl(returnControl);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que ocurre al hacer click en "Regresar"
        /// </summary>
        /// <param name="control">Control que produce el evento</param>
        void returnControl_OnClicked(Control control)
        {
            GameStateManager.GoToMenu("GametypeMenu");
        }

        /// <summary>
        /// Evento que ocurre al hacer click en "Listo"
        /// </summary>
        /// <param name="control">Control que produce el evento</param>
        void readyControl_OnClicked(Control control)
        {
            Global.MaxScore = maxScoreControl.Value;

            GamePlayScreen.ResetGame();
            GameStateManager.GoToMenu("GamePlayScreen");

            Global.PlayMusic("PlayGround");
        }

        /// <summary>
        /// Evento que ocurre al cambiar el valor de "Puntuacion Maxima: "
        /// </summary>
        /// <param name="control">Control que produce el evento</param>
        void maxScoreControl_ValueChanged(Control control)
        {
            Global.MaxScore = maxScoreControl.Value;
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
            spritebatch.Draw(Header, new Vector2(Global.ScreenWidth / 2, 150), null, Color.White, 0f, new Vector2(Header.Width / 2, Header.Height / 2), headerScale, SpriteEffects.None, 0f);
            spritebatch.End();
        }

        #endregion
    }
}
