#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

#endregion

namespace Pong
{
    class Paddle
    {
        #region Campos

        // Texturas para dibujar la barrita
        public static Texture2D RedTexture;
        public static Texture2D GreenTexture;

        // Limites de movimiento en el eje Y
        protected int MinY;
        protected int MaxY;

        protected float Speed = 7.0f;

        // Permite obtener pulsaciones de teclado
        protected KeyboardState keyboard;

        public Vector2 Position; 

        // Teclas para controlar el movimiento
        private Keys UpKey;
        private Keys DownKey;

        public int Score;

        // Posibles colores de la barrita
        public enum PaddleColor
        {
            Red,
            Green
        };

        protected PaddleColor paddleColor;

        #endregion

        #region Propiedades

        /// <summary>
        /// Altura de la barrita en pixeles
        /// </summary>
        public int Height
        {
            get { return GreenTexture.Height; }
        }

        /// <summary>
        /// Ancho de la barrita en pixeles
        /// </summary>
        public int Width
        {
            get { return GreenTexture.Width; }
        }

        /// <summary>
        /// CollisionBox que se utilizará para las colisiones con la pelota
        /// </summary>
        public Rectangle CollisionBox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Width, Height); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa la la barra
        /// </summary>
        /// <param name="X">Posisión inicial en el eje X</param>
        /// <param name="Up">Tecla con la que se moverá hacia arriba</param>
        /// <param name="Down">Tecla con la que se moverá hacia abajo</param>
        /// <param name="paddleColor">Color de la barra</param>
        public Paddle(int X, Keys Up, Keys Down, PaddleColor paddleColor)
        {
            // Se inicializa la posición X en el parametro recibido, y Y en la mitad de la pantalla
            Position = new Vector2(X, Global.ScreenHeight / 2 - Height / 2);

            UpKey = Up;
            DownKey = Down;

            this.paddleColor = paddleColor;

            // Se determina los limites de movimiento en el eje Y
            MinY = 20;
            MaxY = 460;

            Score = 0;
        }

        /// <summary>
        /// Constructor auxiliar en el que no se controla la barra con teclas (Jugador CPU) 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="paddleColor"></param>
        public Paddle(int X, PaddleColor paddleColor)
        {
            // Se inicializa la posición X en el parametro recibido, y Y en la mitad de la pantalla
            Position = new Vector2(X, Global.ScreenHeight / 2 - Height / 2);

            this.paddleColor = paddleColor;

            // Se determina los limites de movimiento en el eje Y
            MinY = 20;
            MaxY = 460;

            Score = 0;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que actualiza la barra
        /// </summary>
        /// <param name="ball">Pelota con la que se checan colisiones</param>
        public virtual void Update(Ball ball)
        {
            Move();

            CheckCollisions(ball);
        }

        /// <summary>
        /// Método que se encarga de mover la barra
        /// </summary>
        protected virtual void Move()
        {
            // Se obtiene el estado del teclado
            keyboard = Keyboard.GetState();

            // Se mueve la barra de acuerdo a las teclas presionadas
            if (keyboard.IsKeyDown(UpKey))
            {
                Position.Y -= Speed;
            }
            if (keyboard.IsKeyDown(DownKey))
            {
                Position.Y += Speed;
            }

            // Se limita la posición de la barra
            Position.Y = MathHelper.Clamp(Position.Y, MinY, MaxY - Height);
        }

        /// <summary>
        /// Método que se encarga de checar colisoines en la pelota, en caso de haber una colisión entonces
        /// cambia el ángulo de la pelota
        /// </summary>
        /// <param name="ball">Pelota con la que se checarán las colisiones</param>
        protected void CheckCollisions(Ball ball)
        {
            // Objeto auxiliar para determinar si ha ocurrido una posición
            Rectangle RectanguloCollision = Rectangle.Intersect(this.CollisionBox, ball.CollisionBox);

            // Si ha ocurrido una colisión...
            if (!RectanguloCollision.IsEmpty)
            {
                // Se calculan las variables auxiliares para determinar el nuevo ángulo de la pelota
                int CentroPelota = RectanguloCollision.Y + (RectanguloCollision.Height / 2);

                int CentroPaddle = (int)Position.Y + (Height / 2);

                /* Se calcula la diferencia entre el centro de la pelota y el centro de la barra y se divide entre
                 * la mitad de la altura de la barra para sacar la relación (por ejemplo entre mas se acerque éste
                 * valor a 1 entonces la colisión ocurrió más cerca a la esquina de la barra, entre más se acerque
                 * a 0 entonces la colisión ocurrió más cerca al centro de la barra) */
                float diferenciaCentro = (float)(CentroPaddle - CentroPelota) / (float)(Height / 2);

                // Lado derecho
                if ((ball.Angle >= 0 && ball.Angle < 90) || (ball.Angle <= 360 && ball.Angle > 270))
                {
                    // Collision en la mitad superior de la barra
                    if (diferenciaCentro > 0)
                    {
                        ball.Angle = MathHelper.Lerp(180, 135, diferenciaCentro);
                    }
                    // Collision en la mitad inferior de la barra
                    else
                    {
                        diferenciaCentro = Math.Abs(diferenciaCentro);
                        ball.Angle = MathHelper.Lerp(180, 225, diferenciaCentro);
                    }

                    // Se reasigna la posición de la pelota
                    ball.Position.X = this.Position.X - ball.Width;
                }
                // Lado izquierdo
                else
                {
                    // Collision en la mitad superior de la barra
                    if (diferenciaCentro > 0)
                    {
                        ball.Angle = MathHelper.Lerp(0, 45, diferenciaCentro);
                    }
                    // Collision en la mitad inferior de la barra
                    else
                    {
                        diferenciaCentro = Math.Abs(diferenciaCentro);
                        ball.Angle = MathHelper.Lerp(360, 315, diferenciaCentro);
                    }

                    // Se reasigna la posición de la pelota
                    ball.Position.X = this.Position.X + Width;
                }

                // Incremento de velocidad de la pelota
                if (ball.Speed < ball.maxSpeed)
                {
                    ball.Speed += 0.5f;
                }

                Global.sfx_BallBounce.Play(Global.Volume_Sfx, 0f, 0f);
            }
        }

        /// <summary>
        /// Método que dibuja la barra
        /// </summary>
        /// <param name="spr">Spritebatch necesario para dibujar</param>
        public virtual void Draw(SpriteBatch spr)
        {
            // Dependiendo del valor de paddleColor se dibuja la barra con la textura adecuada
            if (paddleColor == PaddleColor.Green)
            {
                spr.Draw(GreenTexture, Position, Color.White);
            }
            else
            {
                spr.Draw(RedTexture, Position, Color.White);
            }
        }

        #endregion
    }
}
