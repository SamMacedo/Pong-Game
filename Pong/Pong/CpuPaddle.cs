#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Pong.CustomMenus;

#endregion

namespace Pong
{
    class CpuPaddle : Paddle
    {
        #region Campos

        float collisionPointY;

        bool changuedDirection = false;

        enum Directions
        {
            None,
            Up,
            Down
        };

        Directions movingDirection = Directions.None;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa la barra controlada por el CPU
        /// </summary>
        public CpuPaddle()
            : base(758, PaddleColor.Red)
        {
            // Se le asigna el punto de colisión en Y = al centro de la barra
            collisionPointY = Height / 2;

            // Asignación de velocidad de acuerdo a la dificultad
            switch (Global.Dificulty)
            {
                case 0:
                    Speed = 5f;
                    break;
                case 1:
                    Speed = 7f;
                    break;
                case 2:
                    Speed = 10f;
                    break;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que actualiza la pelota
        /// </summary>
        /// <param name="ball"></param>
        public override void Update(Ball ball)
        {
            Move(ball);

            base.CheckCollisions(ball);
        }

        /// <summary>
        /// Método que se encarga de realizar el movmiento de la barra tomando en cuenta la dificultad
        /// </summary>
        /// <param name="ball">Pelota con la que va a interactuar la barra</param>
        private void Move(Ball ball)
        {
            // Fácil
            if (Global.Dificulty == 0)
            {
                EasyMovement(ball);
            }

            // Medio
            else if (Global.Dificulty == 1)
            {
                MediumMovement(ball);
            }

            // Difícil
            else
            {
                HardMovement(ball);
            }

            // Se limita la posición de la barra
            Position.Y = MathHelper.Clamp(Position.Y, MinY, MaxY - Height);
        }

        /// <summary>
        /// Método que dibuja la barra
        /// </summary>
        /// <param name="spr">SpriteBatch necesario para dibujar</param>
        public override void Draw(SpriteBatch spr)
        {
            base.Draw(spr);
        }

        #endregion

        #region Dificultad Fácil

        /// <summary>
        /// Esta forma de movimiento es simple, siempre trata de golpear la pelota con el centro de la barra
        /// </summary>
        /// <param name="ball"></param>
        private void EasyMovement(Ball ball)
        {
            // Se obtienen los centros de los dos objetos
            float centroPelota = ball.Position.Y + ball.Height / 2;
            float centroPaddle = Position.Y + Height / 2;

            float MoveAmountY;

            // Se calcula la distancia en Y de los dos objetos
            float DiferenciaY = Math.Abs(centroPelota - centroPaddle);

            // Se asegura que la cantidad a mover no exceda la velodidad
            if (DiferenciaY >= Speed)
            {
                MoveAmountY = Speed;
            }
            else
            {
                MoveAmountY = DiferenciaY;
            }

            // Movimiento ya sea en dirección hacia arriba o abajo
            if (centroPelota > centroPaddle)
            {
                Position.Y += MoveAmountY;
            }
            else if (centroPelota < centroPaddle)
            {
                Position.Y -= MoveAmountY;
            }
        }

        #endregion

        #region Dificultad Media

        /// <summary>
        /// Esta forma de movimiento es un poco mas complicada, lo que hace es asignar un punto random de colisión a la barra,
        /// de esta manera la barra chocará con la pelota en distintas partes
        /// </summary>
        /// <param name="ball"></param>
        private void MediumMovement(Ball ball)
        {
            // Si ha ocurrido un cambio de dirreción de movimiento entonces asigna un nuevo punto de colisión de la barra
            if (changuedDirection)
            {
                Random random = new Random();

                collisionPointY = MathHelper.Lerp(ball.Height / 2, Height - ball.Height / 2, (float)random.NextDouble());

                changuedDirection = false;
            }

            // Cálculo del punto de colisión de la barra tomando en cuenta su posición
            float pointPaddle = Position.Y + collisionPointY;

            float centroPelota = ball.Position.Y + ball.Height / 2;

            // Se calcula la distancia en Y de los dos objetos
            float DiferenciaY = Math.Abs(centroPelota - pointPaddle);

            float MoveAmountY;

            // Se asegura que la cantidad a mover no exceda la velodidad
            if (DiferenciaY >= Speed)
            {
                MoveAmountY = Speed;
            }
            else
            {
                MoveAmountY = DiferenciaY;
            }

            // Movimiento ya sea en dirección hacia arriba o abajo
            if (centroPelota > pointPaddle)
            {
                Position.Y += MoveAmountY;

                // Cambio de dirección
                if (movingDirection != Directions.Down)
                {
                    changuedDirection = true;
                }

                movingDirection = Directions.Down;
            }
            else if (centroPelota < pointPaddle)
            {
                Position.Y -= MoveAmountY;

                // Cambio de dirección
                if (movingDirection != Directions.Up)
                {
                    changuedDirection = true;
                }

                movingDirection = Directions.Up;
            }
        }

        #endregion

        #region Dificultad Difícil

        /// <summary>
        /// Esta forma de movimiento se casi igual al de media dificultad, la diferencia es que en esta forma hay más
        /// probabilidad de que la pelota choque con las esquinas de la barra
        /// </summary>
        /// <param name="ball"></param>
        private void HardMovement(Ball ball)
        {
            // Si ha ocurrido un cambio de dirrecion de movimiento entonces asigna un nuevo punto de colisión de la barra
            if (changuedDirection)
            {
                Random random = new Random();

                // Unica parte que cambia del movimiento de media dificultad
                collisionPointY = MathHelper.Lerp(0, Height, (float)random.NextDouble());

                changuedDirection = false;
            }

            // Cálculo del punto de colisión de la barra tomando en cuenta su posición
            float pointPaddle = Position.Y + collisionPointY;

            float centroPelota = ball.Position.Y + ball.Height / 2;

            // Se calcula la distancia en Y de los dos objetos
            float DiferenciaY = Math.Abs(centroPelota - pointPaddle);


            float MoveAmountY;

            // Se asegura que la cantidad a mover no exceda la velodidad
            if (DiferenciaY >= Speed)
            {
                MoveAmountY = Speed;
            }
            else
            {
                MoveAmountY = DiferenciaY;
            }

            // Movimiento ya sea en dirección hacia arriba o abajo
            if (centroPelota > pointPaddle)
            {
                Position.Y += MoveAmountY;

                // Cambio de dirección
                if (movingDirection != Directions.Down)
                {
                    changuedDirection = true;
                }

                movingDirection = Directions.Down;
            }
            else if (centroPelota < pointPaddle)
            {
                Position.Y -= MoveAmountY;

                // Cambio de dirección
                if (movingDirection != Directions.Up)
                {
                    changuedDirection = true;
                }

                movingDirection = Directions.Up;
            }
        }

        #endregion
    }
}
