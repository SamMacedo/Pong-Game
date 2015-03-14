#region Directivas de uso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Pong;
using Pong.GameManagement;

#endregion

namespace Pong.CustomMenus
{
    /// <summary>
    /// Menú que solamente sirve para proporcionar una transición al Intro
    /// </summary>
    class PostIntroMenu : Menu
    {
        bool flag = true;

        public PostIntroMenu(string name)
            : base(name)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard)
        {
            if (flag)
            {
                GameStateManager.GoToMenu("IntroMenu");
                flag = false;
            }
        }

        public override void Draw(SpriteBatch spritebatch, Texture2D BackgroundTexture)
        {
        }
    }
}
