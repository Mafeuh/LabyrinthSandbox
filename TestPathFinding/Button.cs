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
    public class Button : ADrawable
    {
        private string text;
        private bool IsHovered
        {
            get
            {
                MouseState mState = Mouse.GetState();

                return mState.X > Transform.X && mState.Y > Transform.Y && mState.X < Transform.Right && mState.Y < Transform.Bottom;
            }
        }
        public Action Action { get; }

        public Button(string text, Rectangle transform, Color color, Action action) : base(transform, color)
        {
            this.text = text;
            Action = action;
        }
        public Button(string text, int x, int y, Color color, Action action)
            : this(
                  text,
                  new Rectangle(x, y, Convert.ToInt32(Game1.font.MeasureString(text).X) + 5, Convert.ToInt32(Game1.font.MeasureString(text).Y) + 5),
                  color,
                  action)
        { }

        public void Update()
        {
            if(IsHovered && Game1.LeftClick)
            {
                Action.Invoke();
            }
        }

        public override void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(Game1.px1, Transform, IsHovered ? Color.White * 0.5f : DrawColor);
            sbatch.Draw(Game1.px1, new Rectangle(Transform.X, Transform.Y, Transform.Width, 2), Color.Black);
            sbatch.Draw(Game1.px1, new Rectangle(Transform.X, Transform.Y + Transform.Height, Transform.Width, 2), Color.Black);
            sbatch.Draw(Game1.px1, new Rectangle(Transform.X, Transform.Y, 2, Transform.Height), Color.Black);
            sbatch.Draw(Game1.px1, new Rectangle(Transform.X + Transform.Width, Transform.Y, 2, Transform.Height + 2), Color.Black);
            sbatch.DrawString(Game1.font, text, new Vector2(Transform.X + Transform.Width / 2 - Game1.font.MeasureString(text).X / 2 + 2, Transform.Y + 5), Color.Black);
        }
    }
}
