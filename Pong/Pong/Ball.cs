#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Pong
{
    class Ball
    {
        #region Campos

        public float Speed;
        public float maxSpeed = 20f;

        public static Texture2D Texture;
        public bool CanMove = false;
        private Random random = new Random();

        private int minX;
        private int maxX;

        private int minY;
        private int maxY;

        public Vector2 Position;
        private Vector2 InitialPosition;

        private float angle;

        #endregion

        #region Propiedades

        /// <summary>
        /// Alto en pixeles de la pelota
        /// </summary>
        public int Height
        {
            get { return Texture.Height; }
        }

        /// <summary>
        /// Ancho en pixeles de la pelota
        /// </summary>
        public int Width
        {
            get { return Texture.Width; }
        }

        /// <summary>
        /// Angulo en grados de la pelota
        /// </summary>
        public float Angle
        {
            get { return angle; }

            set { angle = value; }
        }

        /// <summary>
        /// CollisionBox que se utilizará para las colisiones con las barras
        /// </summary>
        public Rectangle CollisionBox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Width, Height); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor que inicializa la pelota
        /// </summary>
        public Ball()
        {
            // Se coloca la pelota en el centro de la pantalla
            InitialPosition = new Vector2(400 - Width / 2, 240 - Height / 2 + 10);

            ResetPosition();

            // Se asignan los valores min y max de movimiento permitido
            minX = 20;
            maxX = 780;

            minY = 20;
            maxY = 460;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que actualiza la pelota
        /// </summary>
        /// <param name="player1">Barra del jugador 1</param>
        /// <param name="player2">Barra del jugador 2</param>
        public void Update(Paddle player1, Paddle player2)
        {
            if (CanMove)
            {
                // Movimiento de acuerdo al ángulo actual de la pelota
                Position.X += Speed * (float)Math.Cos(MathHelper.ToRadians(Angle));
                Position.Y -= Speed * (float)Math.Sin(MathHelper.ToRadians(Angle));

                // Si se pasa del lado derecho de la pantalla...
                if (Position.X + Width > maxX)
                {
                    CanMove = false;
                    ResetPosition();
                    player1.Score += 1;

                    Global.sfx_Score.Play(Global.Volume_Sfx, 0f, 0f);
                }
                // Si se pasa del lado izquierdo de la pantalla...
                else if (Position.X < minX)
                {
                    CanMove = false;
                    ResetPosition();
                    player2.Score += 1;

                    Global.sfx_Score.Play(Global.Volume_Sfx, 0f, 0f);
                }
                // Si se pasa del lado superior de la pantalla...
                else if (Position.Y < minY)
                {
                    Angle = 360 - Angle;

                    Position.Y = minY;
                }
                // Si se pasa del lado superior de la pantalla...
                else if (Position.Y + Height > maxY)
                {
                    Angle = 360 - Angle;

                    Position.Y = maxY - Height;
                }
            }
        }

        /// <summary>
        /// Método que resetea la posición de la pelota
        /// </summary>
        private void ResetPosition()
        {
            Position = InitialPosition;
            Speed = 10f;

            int auxiliar = random.Next(1, 5);

            // Se asigna el ángulo de manera aleatoria...
            switch (auxiliar)
            {
                case 1:
                    Angle = 45;
                    break;
                case 2:
                    Angle = 135;
                    break;
                case 3:
                    Angle = 215;
                    break;
                case 4:
                    Angle = 315;
                    break;
            }
        }

        /// <summary>
        /// Método que dibuja la pelota
        /// </summary>
        /// <param name="spr">SpriteBatch necesario para dibujar</param>
        public void Draw(SpriteBatch spr)
        {
            spr.Draw(Texture, Position, Color.White);
        }

        #endregion
    }
}
