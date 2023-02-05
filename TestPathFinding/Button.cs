using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPathFinding
{
    public class Button
    {
        private string text;
        private Rectangle transform;
        private bool IsHovered
        {
            get
            {
                MouseState mState = Mouse.GetState();

                return mState.X > transform.X && mState.Y > transform.Y && mState.X < transform.Right && mState.Y < transform.Bottom;
            }
        }
        public Action Action { get; }

        public Button(string text, Rectangle transform, Action action)
        {
            this.text = text;
            this.transform = transform;
            Action = action;
        }
        public Button(string text, int x, int y, Action action)
        {
            this.text = text;
            transform = new Rectangle(x, y, Convert.ToInt32(Game1.font.MeasureString(text).X) + 5, Convert.ToInt32(Game1.font.MeasureString(text).Y) + 5);
            Action = action;
        }

        public void Update()
        {
            if(IsHovered && Game1.LeftClick)
            {
                Action.Invoke();
            }
        }

        public void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(Game1.px1, transform, IsHovered ? Color.White * 0.5f : Color.LightGray);
            sbatch.Draw(Game1.px1, new Rectangle(transform.X, transform.Y, transform.Width, 2), Color.Black);
            sbatch.Draw(Game1.px1, new Rectangle(transform.X, transform.Y + transform.Height, transform.Width, 2), Color.Black);
            sbatch.Draw(Game1.px1, new Rectangle(transform.X, transform.Y, 2, transform.Height), Color.Black);
            sbatch.Draw(Game1.px1, new Rectangle(transform.X + transform.Width, transform.Y, 2, transform.Height + 2), Color.Black);
            sbatch.DrawString(Game1.font, text, new Vector2(transform.X + transform.Width / 2 - Game1.font.MeasureString(text).X / 2 + 2, transform.Y + 5), Color.Black);
        }
    }
}
