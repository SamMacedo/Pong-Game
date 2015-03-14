#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Pong.GameManagement.Controls
{
    public class ClickControl : Control
    {
        #region Delegados y eventos

        // Delegado que indica cómo debe de ser el cuerpo del método del evento
        public delegate void ControlClickHandler(Control control);

        // Evento basado en el delegado de arriba
        public event ControlClickHandler OnClicked;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa el control
        /// </summary>
        /// <param name="text">Texto que mostrará el control</param>
        public ClickControl(string text)
            : base(text)
        {
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que actualiza el control
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboard"></param>
        /// <param name="CanPresskey">Indicar si es posible interactuar con el control o no con el teclado</param>
        public override void Update(GameTime gameTime, KeyboardState keyboard, bool CanPressKey)
        {
            // Se determina si se hizo click en el control
            if (Selected && CanPressKey)
            {
                if (keyboard.IsKeyDown(Keys.Enter))
                {
                    // Se provoca el evento 
                    OnClicked.Invoke(this);

                    Global.sfx_Click.Play(Global.Volume_Sfx, 0f, 0f);
                }
            }

            base.Update(gameTime, keyboard, CanPressKey);
        }

        /// <summary>
        /// Método que dibuja el control
        /// </summary>
        /// <param name="spritebatch">SpriteBatch necesario para dibujar</param>
        /// <param name="drawPosition">Posición en la que se dibujará el control</param>
        public override void Draw(SpriteBatch spritebatch, Vector2 drawPosition)
        {
            base.Draw(spritebatch, drawPosition);
        }

        #endregion
    }
}
