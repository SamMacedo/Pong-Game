using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Pong
{
    /// <summary>
    /// Clase que se encarga de almacenar los recursos y variables globales
    /// </summary>
    public static class Global
    {
        public static int ScreenWidth;
        public static int ScreenHeight;

        public static Texture2D t2d_MenusBackground;
        public static Texture2D t2d_BlackDot;

        public static SpriteFont font_Font1;
        public static SpriteFont font_Font2;

        public static int Dificulty;
        public static int MaxScore;
        public static float Volume_Music;
        public static float Volume_Sfx;

        public static SoundEffect sfx_Scroll;
        public static SoundEffect sfx_Click;
        public static SoundEffect sfx_Switch;

        public static SoundEffect sfx_BallBounce;
        public static SoundEffect sfx_Score;

        public static Song sng_Menus;
        public static Song sng_PlayGround;

        public static bool AgainstCpu;

        /// <summary>
        /// Método que reproduce una canción (Menus o PlayGround)
        /// </summary>
        /// <param name="name">Nombre de la canción (Menus o PlayGround)</param>
        public static void PlayMusic(string name)
        {
            if (name == "Menus")
            {
                MediaPlayer.Play(sng_Menus);
            }
            else if (name == "PlayGround")
            {
                MediaPlayer.Play(sng_PlayGround);
            }

            MediaPlayer.IsRepeating = true;
        }
    }
}
