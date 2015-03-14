#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Pong
{
    /// <summary>
    /// Clase que sirve como base para proporcionar funcionalidad básica a los controles
    /// </summary>
    public abstract class Control
    {
        #region Campos

        // Variables auxiliares que sirven para proporcionar el efecto de selección del control
        private float scale = 1f;
        private const float maxScale = 1.2f;
        private const float scaleSpeed = 0.02f;

        #endregion

        #region Propiedades

        /// <summary>
        /// Texto que muestra el control
        /// </summary>
        public virtual string Text
        {
            get { return text; }
            set { text = value; }
        }
        string text;

        /// <summary>
        /// Indica si el control se encuentra seleccionado
        /// </summary>
        public virtual bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
        bool selected;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa el control
        /// </summary>
        /// <param name="text">Texto que va a mostrar el control</param>
        public Control(string text)
        {
            this.Text = text;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que actualiza el control
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboard"></param>
        /// <param name="CanPresskey">Indicar si es posible interactuar con el control o no con el teclado</param>
        public virtual void Update(GameTime gameTime, KeyboardState keyboard, bool CanPresskey)
        {
            // Si está seleccionado entonces aumenta el tamaño del texto que muestra el control
            if (Selected)
            {
                scale += scaleSpeed;
                scale = MathHelper.Clamp(scale, 1f, maxScale);
            }
            // Si no entonces disminuye hasta 1 el tamaño del texto que muestra el control
            else
            {
                scale -= scaleSpeed;
                scale = MathHelper.Clamp(scale, 1f, maxScale);
            }
        }

        /// <summary>
        /// Método que dibuja el control
        /// </summary>
        /// <param name="spritebatch">SpriteBatch necesario para dibujar</param>
        /// <param name="drawPosition">Posición en la que se dibujará el control</param>
        public virtual void Draw(SpriteBatch spritebatch, Vector2 drawPosition)
        {
            // Para dibujar centrado el texto del control se calcula su tamaño y se divide entre 2
            Vector2 origin = Global.font_Font1.MeasureString(Text) / 2;

            // Si el control está seleccionado entonces se dibuja el texto de color amarillo
            if (Selected)
            {
                spritebatch.DrawString(Global.font_Font1, Text, drawPosition, Color.Yellow, 0f, origin, scale, SpriteEffects.None, 0f);
            }
            // Si no entonces se dibuja el texto de color blanco
            else
            {
                spritebatch.DrawString(Global.font_Font1, Text, drawPosition, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Método que resetea la selección del control
        /// </summary>
        public virtual void Unselect()
        {
            this.Selected = false;
            this.scale = 1f;
        }

        #endregion
    }
}
