#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Pong.GameManagement
{
    /// <summary>
    /// Clase que se encarga de actualizar y dibujar cada uno de los distintos menús
    /// </summary>
    public static class GameStateManager
    {
        #region Campos

        // Lista que contiene cada uno de los menús
        private static List<Menu> Menus = new List<Menu>();

        // Variables auxiliares para actualización y transición de menús
        private static Menu CurrentMenu;
        private static Menu TransitioningMenu;

        // Variables para el desplazamiento con teclas en los menús
        private static Keys[] menuKeys = { Keys.Enter, Keys.Up, Keys.Down, Keys.Left, Keys.Right };
        public static bool CanPressKey = true;
        private static KeyboardState keyboard;

        // Variables auxiliares para realizar el efecto de transición de menús
        private static bool isOnTransition = false;
        private static bool onFadeIn = false;
        private static float Alpha = 0f;

        #endregion

        #region Métodos principales

        /// <summary>
        /// Método que actualiza el menú actual
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
            // Si se está realizando una transición entonces continuar hasta que finalize
            if (isOnTransition)
            {
                PerformTranstition(gameTime);
            }
            // Si no entonces se actualiza el menú actual
            else
            {
                keyboard = Keyboard.GetState();

                CurrentMenu.Update(gameTime, keyboard);

                HandleInput();
            }
        }

        /// <summary>
        /// Método que dibuja el menú actual
        /// </summary>
        /// <param name="spritebatch">SpriteBatch necesario para dibujar</param>
        public static void Draw(SpriteBatch spritebatch)
        {
            CurrentMenu.Draw(spritebatch, null);

            // Si está en transición entonces se dibuja en pantalla completa una textura negra
            if (isOnTransition)
            {
                spritebatch.Begin();
                spritebatch.Draw(Global.t2d_BlackDot, Vector2.Zero, null, Color.White * Alpha, 0f, Vector2.Zero, new Vector2(Global.ScreenWidth, Global.ScreenHeight), SpriteEffects.None, 0f);
                spritebatch.End();
            }
        }

        #endregion

        #region Métodos auxiliares

        /// <summary>
        /// Método que se encarga de administrar las pulsaciones de teclado
        /// </summary>
        public static void HandleInput()
        {
            bool pressedKey = false;

            // Se determina si se ha presionado alguna de las teclas permitidas para la navegación
            for (int i = 0; i < menuKeys.Length; i++)
            {
                if (keyboard.IsKeyDown(menuKeys[i]))
                {
                    pressedKey = true;
                }
            }

            /* Dependiendo si se presionó o no una tecla (siempre y cuando sea una tecla permitida) se permite realizar otra
             * pulsación de tecla o bien esperar a que deje de presionar el botón para permitir presionar otra */
            CanPressKey = !pressedKey;
        }

        /// <summary>
        /// Método que se encarga de trasladarse a otro menú
        /// </summary>
        /// <param name="menuName">Nombre del menú</param>
        public static void GoToMenu(string menuName)
        {
            // Se busca por nombre el menú, y si lo encuentra entonces inicializa la transición
            for (int i = 0; i < Menus.Count; i++)
            {
                if (Menus[i].Name == menuName)
                {
                    Menus[i].UnselectControls();

                    TransitioningMenu = Menus[i];

                    isOnTransition = true;
                    onFadeIn = true;
                    Alpha = 0f;
                }
            }
        }

        /// <summary>
        /// Método que añade un nuevo menú al GameStateManager
        /// </summary>
        /// <param name="menu">Menú que se desea añadir</param>
        public static void AddMenu(Menu menu)
        {
            Menus.Add(menu);

            // Si el menú que se acaba de agregar es el primero en ser agregado entonces se prepara para ser actualizado
            if (Menus.Count == 1)
            {
                CurrentMenu = Menus[0];
            }
        }

        /// <summary>
        /// Método que se encarga de efectuar el efecto de transición a otro menú
        /// </summary>
        /// <param name="gameTime"></param>
        public static void PerformTranstition(GameTime gameTime)
        {
            // Se incrementa la opacidad hasta llegar a 1
            if (onFadeIn)
            {
                Alpha += (float)gameTime.ElapsedGameTime.TotalSeconds * 2f;

                // Si llega a uno la opacidad entonces se asigna false a la bandea(para iniciar a disminuir la opacidad)
                if (Alpha >= 1f)
                {
                    Alpha = 1f;
                    onFadeIn = false;

                    // Se comienza a dibujar el menú al cual se está transicionando
                    CurrentMenu = TransitioningMenu;
                }
            }
            // Se decrementa  la opacidad hasta llegar a 0
            else
            {
                Alpha -= (float)gameTime.ElapsedGameTime.TotalSeconds * 2f;

                /* Si llega a cero la opacidad entonces se le asigna false a la bandera permitiendo así que el menú al cual se
                 * transicionó se pueda actualizar */
                if (Alpha <= 0f)
                {
                    Alpha = 0f;
                    isOnTransition = false;
                }
            }
        }

        #endregion
    }
}
