#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Pong.GameManagement.Controls
{
    class NumericSelectorControl : Control
    {
        #region Campos

        // Variables auxiliares para modificar el texto del control
        private StringBuilder stringbuilder;
        private int originalTextIndex;

        // Variables auxiliares para permitir el desplazamiento y aceleración del mismo
        private float slideInterval;
        private const float initialSlideInterval = 250f;
        private const float fastestSlideInterval = 10;
        private const float intervalAcceleration = 50f;
        private int intervalCounter = 0;

        // Bandera que indica si se va a comenzar un desplazamiento
        private bool onSlideStart = false;

        private Keys previousKey;

        #endregion

        #region Delegados y eventos

        // Delegado que indica cómo debe de ser el cuerpo del método del evento
        public delegate void ValueChangedHandler(Control control);

        // Evento basado en el delegado de arriba
        public event ValueChangedHandler ValueChanged;

        #endregion

        #region Propiedades

        /// <summary>
        /// Texto que muestra el control
        /// </summary>
        public override string Text
        {
            get { return stringbuilder.ToString(); }
            set { stringbuilder = new StringBuilder(value); }
        }

        /// <summary>
        /// Valor mínimo que puede mostrar el control
        /// </summary>
        public int MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        int minValue;

        /// <summary>
        /// Valor máximo que puede mostrar el control
        /// </summary>
        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        int maxValue;

        /// <summary>
        /// Valor actual del control
        /// </summary>
        public int Value
        {
            get { return _value; }
            set 
            {
                _value = (int)MathHelper.Clamp(value, MinValue, MaxValue);

                SetText();

                // Se provoca el evento
                ValueChanged.Invoke(this);
            }
        }

        int _value;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa el control
        /// </summary>
        /// <param name="text">Texto que mostrará el control</param>
        public NumericSelectorControl(string text)
            : base(text)
        {
            // Valores mínimo y máximo por default
            MinValue = 0;
            MaxValue = 100;

            Text = text;

            originalTextIndex = stringbuilder.Length - 1;

            // Se selecciona por default el valor mínimo
            _value = minValue;

            SetText();
        }

        #endregion

        #region Métodos principales

        /// <summary>
        /// Método que actualiza el control
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboard"></param>
        /// <param name="CanPresskey">Indicar si es posible interactuar con el control o no con el teclado</param>
        public override void Update(GameTime gameTime, KeyboardState keyboard, bool CanPresskey)
        {
            if (Selected)
            {
                // Se inicia el desplazamiento
                if (CanPresskey)
                {
                    onSlideStart = true;
                }

                // Si se va a comenzar el desplazamiento entonces se reinicia el intervalo de tiempo
                if (onSlideStart)
                {
                    slideInterval = 0f;
                    onSlideStart = false;
                }

                // Desplazamiento hacia la izquierda
                if (keyboard.IsKeyDown(Keys.Left))
                {
                    UpdateIntervals((float)gameTime.ElapsedGameTime.TotalMilliseconds, -1);

                    // Si se ha cambiado la dirección de desplazamiento entonces se resetea el desplazamiento
                    if (previousKey == Keys.Right)
                    {
                        Reset();
                    }

                    previousKey = Keys.Left;
                }

                // Desplazamiento hacia la derecha
                else if (keyboard.IsKeyDown(Keys.Right))
                {
                    UpdateIntervals((float)gameTime.ElapsedGameTime.TotalMilliseconds, +1);

                    // Si se ha cambiado la dirección de desplazamiento entonces se resetea el desplazamiento
                    if (previousKey == Keys.Left)
                    {
                        Reset();
                    }

                    previousKey = Keys.Right;
                }

                // Si no se ha presionado ninguna tecla entonces se resetea el desplazamiento
                else
                {
                    Reset();
                }
            }
            else
            {
                Reset();
            }

            base.Update(gameTime, keyboard, CanPresskey);
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
        /// Método que se encarga de aplicar aceleración a los intervalos de tiempo entre cada des
        /// </summary>
        /// <param name="elapsedMiliseconds">Milisegunos transcurridos</param>
        /// <param name="diference">Cambio que se realizará al valor del control entre cada intervalo</param>
        private void UpdateIntervals(float elapsedMiliseconds, int diference)
        {
            // Actualización del intervalo
            slideInterval -= elapsedMiliseconds;

            // Si ha terminado el intervalo actual...
            if (slideInterval <= 0f)
            {
                // Si el prox. intervalo no será mas rápido que lo máximo permitido...
                if ((initialSlideInterval - (intervalCounter * intervalAcceleration)) > fastestSlideInterval)
                {
                    // Se incrementa el contador de intervalos transcurridos
                    intervalCounter++;

                    // Se cambia la velocidad del prox. intervalo
                    slideInterval = initialSlideInterval - (intervalCounter * intervalAcceleration);
                }
                // Si no entonces la velocidad del prox. intervalo será igual a la velocidad máxima 
                else
                {
                    slideInterval = fastestSlideInterval;
                }

                // Se reproduce un sonido solo en caso de no querer cambiar menos del valor minimo o más del valor máximo
                if (Value + diference >= MinValue && Value + diference <= MaxValue)
                {
                    Global.sfx_Switch.Play(Global.Volume_Sfx, 0f, 0f);
                }

                // Se cambia el valor del control
                Value = Value + diference;
            }
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
            stringbuilder.Append(Value);
        }

        /// <summary>
        /// Método que resetea el contador de intervalos
        /// </summary>
        private void Reset()
        {
            intervalCounter = 0;
        }

        #endregion
    }
}
