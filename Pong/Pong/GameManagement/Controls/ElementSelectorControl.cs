#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Pong.GameManagement;

#endregion

namespace Pong.GameManagement.Controls
{
    /// <summary>
    /// Los objetos de esta clase son controles quu tienen una lista de elementos los cuales pueden ser seleccionados
    /// </summary>
    /// <typeparam name="T">Indica el tipo del cual serán los elementos del control</typeparam>
    public class ElementSelectorControl<T> : Control where T: IComparable<T>
    {
        #region Campos

        // Lista de elementos
        private List<T> elements;

        // Indices de elementos
        private int index;
        private int previousIndex;

        // Variables auxiliares para modificar el texto del control
        private StringBuilder stringbuilder;
        private int originalTextIndex;

        #endregion

        #region Delegados y eventos

        // Delegado que indica cómo debe de ser el cuerpo del método del evento
        public delegate void ValueChangedHandler(Control control);

        // Evento basado en el delegado de arriba
        public event ValueChangedHandler ValueChangued;

        #endregion

        #region Propiedades

        /// <summary>
        /// Valor actual o seleccionado de la lista de elementos del control
        /// </summary>
        public T Value
        {
            get { return elements[index]; }
            set
            {
                // Ciclo que busca el valor que se quiere seleccionar y si lo encuentra en el listado entonces lo selecciona
                for (int i = 0; i < elements.Count; i++)
                {
                    if (elements[i].CompareTo(value) == 0)
                    {
                        index = i;
                        SetText();

                        // Se provoca el evento
                        ValueChangued.Invoke(this);
                    }
                }
            }
        }

        /// <summary>
        /// Texto que muestra el control
        /// </summary>
        public override string Text
        {
            get { return stringbuilder.ToString(); }
            set { stringbuilder = new StringBuilder(value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa el control
        /// </summary>
        /// <param name="text">Texto que mostrará el control</param>
        public ElementSelectorControl(string text)
            : base(text)
        {
            // Se inicializa la lista de elementos
            elements = new List<T>();

            Text = text;

            originalTextIndex = stringbuilder.Length - 1;
        }

        #endregion

        #region Métodos principales

        /// <summary>
        /// Método que actualiza el control
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboard"></param>
        /// <param name="CanPresskey">Indicar si es posible interactuar con el control o no con el teclado</param>
        public override void Update(GameTime gameTime, KeyboardState keyboard, bool CanPressKey)
        {
            if (Selected && CanPressKey)
            {
                if (elements.Count > 0)
                {
                    previousIndex = index;

                    // Desplazamiento a la izquierda de elementos
                    if (keyboard.IsKeyDown(Keys.Left))
                    {
                        index--;

                        Global.sfx_Switch.Play(Global.Volume_Sfx, 0f, 0f);
                    }
                    // Desplazamiento a la derecha de elementos
                    else if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.Enter))
                    {
                        index++;

                        Global.sfx_Switch.Play(Global.Volume_Sfx, 0f, 0f);
                    }

                    // Si está seleccionado el último elemento y se desplaza a la derecha entonces se selecciona el primero
                    if (index > elements.Count - 1)
                    {
                        index = 0;
                    }
                    // Si está seleccionado el primer elemento y se desplaza a la izquierda entonces se selecciona el último
                    else if (index < 0)
                    {
                        index = elements.Count - 1;
                    }

                    // Si han cambiado los índices entonces se provoca el evento
                    if (previousIndex != index)
                    {
                        ValueChangued.Invoke(this);

                        SetText();
                    }
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

        #region Métodos auxiliares

        /// <summary>
        /// Método que agrega un nuevo elemento al listado de elementos del control
        /// </summary>
        /// <param name="element">Valor del elemento</param>
        public void AddElement(T element)
        {
            elements.Add(element);

            // Si es el primer elemento en ser agregado entonces se selecciona el mismo
            if (elements.Count == 1)
            {
                index = 0;
            }

            SetText();
            
        }

        /// <summary>
        /// Método que modifica el texto que muestra el control
        /// </summary>
        private void SetText()
        {
            // Se remueve el texto sobrante
            if (stringbuilder.Length - 1 > originalTextIndex)
            {
                stringbuilder.Remove(originalTextIndex + 1, stringbuilder.Length - (originalTextIndex + 1));
            }

            // Se concatena al texto original el valor actual o seleccionado del listado de elementos del control
            stringbuilder.Append(elements[index]);
        }

        #endregion
    }
}
