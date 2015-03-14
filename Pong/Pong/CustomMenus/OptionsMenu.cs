#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Pong.GameManagement;
using Pong.GameManagement.Controls;

#endregion

namespace Pong.CustomMenus
{
    class OptionsMenu : Menu
    {
        #region Campos

        // Textura y variables auxiliares para el encabezado del menú
        public static Texture2D Header;
        private float headerScale = 1f;
        private const float scaleSpeed = 0.0005f;
        private float scaleTimer = 0f;
        private bool isGrowing = true;

        // Controles del menú
        NumericSelectorControl musicVolumeControl = new NumericSelectorControl("Volumen Musica: ");
        NumericSelectorControl sfxVolumeControl = new NumericSelectorControl("Volumen sfx: ");
        ClickControl returnControl = new ClickControl("Regresar");

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa el menú
        /// </summary>
        /// <param name="name">Nombre del menú</param>
        public OptionsMenu(string name)
            : base(name)
        {
            // Asignación de Handlers para los eventos de los controles
            this.musicVolumeControl.ValueChanged += new NumericSelectorControl.ValueChangedHandler(musicVolumeControl_ValueChanged);
            this.sfxVolumeControl.ValueChanged += new NumericSelectorControl.ValueChangedHandler(sfxVolumeControl_ValueChanged);
            this.returnControl.OnClicked += new ClickControl.ControlClickHandler(returnControl_OnClicked);

            // Se asignan por default los valores de 100 a los controles
            musicVolumeControl.Value = 100;
            sfxVolumeControl.Value = 100;

            // Se agregan los controles al menú
            AddControl(musicVolumeControl);
            AddControl(sfxVolumeControl);
            AddControl(returnControl);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que ocurre al cambiar el valor de "Volumen Musica"
        /// </summary>
        /// <param name="control">Control que produce el evento</param>
        void musicVolumeControl_ValueChanged(Control control)
        {
            Global.Volume_Music = (float)musicVolumeControl.Value / 100f;

            MediaPlayer.Volume = Global.Volume_Music;
        }

        /// <summary>
        /// Evento que ocurre al cambiar el valor de "Volumen sfx"
        /// </summary>
        /// <param name="control">Control que produce el evento</param>
        void sfxVolumeControl_ValueChanged(Control control)
        {
            Global.Volume_Sfx = (float)sfxVolumeControl.Value / 100f;
        }

        /// <summary>
        /// Evento que ocurre al hacer click en "Regresar"
        /// </summary>
        /// <param name="control">Control que produce el evento</param>
        void returnControl_OnClicked(Control control)
        {
            GameStateManager.GoToMenu("MainMenu");
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
