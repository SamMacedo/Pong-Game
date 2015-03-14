#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Pong.GameManagement;

#endregion

namespace Pong
{
    /// <summary>
    /// Clase base que sirve para proporcionar funcionalidad básica a los menús
    /// </summary>
    public abstract class Menu
    {
        #region Campos

        // Variables que indican los indices de los controles
        private int previousSelectedControlIndex;
        private int selectedControlIndex;

        // Lista que almacena cada uno de los controles que contiene el menú
        private List<Control> Controls;

        #endregion

        #region Propiedades

        /// <summary>
        /// Nombre del menú
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string name;

        /// <summary>
        /// Indica a partir del centro la diferencia en pixeles en la que se deberán de dibujar los controles, 
        /// por ejemplo si le indicamos 10, entonces se dibujarán los controles desde la posición: centro de la
        /// pantalla + 10
        /// </summary>
        public int YDrawCenterDiference
        {
            get { return yDrawCenterDiference; }
            set { yDrawCenterDiference = value; }
        }
        int yDrawCenterDiference;

        /// <summary>
        /// Indica si el menú tiene controles
        /// </summary>
        public bool HasControls
        {
            get
            {
                if (Controls != null)
                {
                    return Controls.Count > 0;
                }

                return false;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa el menú
        /// </summary>
        /// <param name="name">Nombre del menú</param>
        public Menu(string name)
        {
            this.Name = name;

            this.previousSelectedControlIndex = 0;
            this.selectedControlIndex = 0;

            this.Controls = null;

            this.YDrawCenterDiference = 75;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que actualiza el menú y todos sus controles
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboard"></param>
        public virtual void Update(GameTime gameTime, KeyboardState keyboard)
        {
            if (HasControls)
            {
                // Si es posible navegar entre los distintos controles...
                if (GameStateManager.CanPressKey)
                {
                    previousSelectedControlIndex = selectedControlIndex;

                    // Navegación hacia arriba
                    if (keyboard.IsKeyDown(Keys.Up))
                    {
                        selectedControlIndex--;

                        /* Si se encuentra seleccionado el primer control y se navega hacia arriba entonces se selecciona
                         * el último control */
                        if (selectedControlIndex < 0)
                        {
                            selectedControlIndex = Controls.Count - 1;
                        }

                        Global.sfx_Scroll.Play(Global.Volume_Sfx, 0f, 0f);
                    }

                    // Navegación hacia abajo
                    else if (keyboard.IsKeyDown(Keys.Down))
                    {
                        selectedControlIndex++;

                        /* Si se encuentra seleccionado el último control y se navega hacia abajo entonces se selecciona
                         * el primer control */
                        if (selectedControlIndex > Controls.Count - 1)
                        {
                            selectedControlIndex = 0;
                        }

                        Global.sfx_Scroll.Play(Global.Volume_Sfx, 0f, 0f);
                    }

                    // Selección de controles
                    Controls[previousSelectedControlIndex].Selected = false;
                    Controls[selectedControlIndex].Selected = true;
                }

                // Se actualizan cada uno de los controles
                for (int i = 0; i < Controls.Count; i++)
                {
                    Controls[i].Update(gameTime, keyboard, GameStateManager.CanPressKey);
                }
            }
        }

        /// <summary>
        /// Método que dibuja el menú y todos sus controles
        /// </summary>
        /// <param name="spritebatch">SpriteBatch necesario para dibujar</param>
        /// <param name="BackgroundTexture">Textura para ser dibujada como fondo</param>
        public virtual void Draw(SpriteBatch spritebatch, Texture2D BackgroundTexture)
        {
            spritebatch.Begin();

            // Primero se dibuja el fondo
            spritebatch.Draw(BackgroundTexture, Vector2.Zero, Color.White);

            if (HasControls)
            {
                // Se calcula la altura que ocupara cada string
                float spacing = Global.font_Font1.MeasureString("A").Y;

                // Se determina la posición inicial para comenzar a dibujar cada control
                Vector2 drawPosition = new Vector2(Global.ScreenWidth / 2, Global.ScreenHeight / 2);

                // Ciclo que se encarga de recorrer la posición de dibujado en Y de acuerdo a la cantitad de controles
                for (int i = 1; i < Controls.Count; i++)
                {
                    drawPosition.Y -= spacing / 2;
                }

                // Se le suma a la cantidad de dibujado la diferencia respecto al centro en Y
                drawPosition.Y += this.YDrawCenterDiference;

                // Se dibujan cada uno de los controles
                for (int i = 0; i < Controls.Count; i++)
                {
                    Controls[i].Draw(spritebatch, drawPosition);

                    drawPosition.Y += spacing;
                }
            }

            spritebatch.End();
        }

        #endregion

        #region Métodos auxiliares

        /// <summary>
        /// Método que agrega un nuevo control al menú
        /// </summary>
        /// <param name="control">Control que va a ser agregado</param>
        public void AddControl(Control control)
        {
            // Si la lista está vacía entonces se inicializa y se selecciona el primer control
            if (Controls == null)
            {
                Controls = new List<Control>();
                control.Selected = true;
            }

            // Se agrega el control
            Controls.Add(control);    
        }

        /// <summary>
        /// Método que resetea la selección de controles, dejando como seleccionado el primer control
        /// </summary>
        public void UnselectControls()
        {
            if (HasControls)
            {
                for (int i = 0; i < Controls.Count; i++)
                {
                    Controls[i].Unselect();
                }

                selectedControlIndex = 0;
            }
        }

        #endregion
    }
}
