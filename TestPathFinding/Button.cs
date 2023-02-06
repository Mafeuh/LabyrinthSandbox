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
                Rectangle trueTransform = new Rectangle(Transform.X, Transform.Y + Game1.simulation.CellSize * Game1.simulation.Grid.Height, Transform.Width, Transform.Height);

                return mState.X > trueTransform.X && mState.Y > trueTransform.Y && mState.X < trueTransform.Right && mState.Y < trueTransform.Bottom;
            }
        }
        public Action Action { get; }

        public Button(string text, Point position, Point dimensions, Color color, Action action) : base(new Rectangle(position, dimensions), color)
        {
            if(dimensions.Y < Game1.font.MeasureString(text).X + 5)
            {
                dimensions.Y = (int)Game1.font.MeasureString(text).Y + 5;
            }

            Transform = new Rectangle(position, dimensions);

            this.text = text;
            Action = action;
        }
        public Button(string text, int x, int y, Color color, Action action)
            : this(
                  text,
                  new Point(x, y),
                  new Point(Convert.ToInt32(Game1.font.MeasureString(text).X) + 5, Convert.ToInt32(Game1.font.MeasureString(text).Y) + 5),
                  color,
                  action)
        { }

        public override void Update()
        {
            if(IsHovered && Game1.LeftClick)
            {
                Action.Invoke();
            }
        }

        public override void Draw(SpriteBatch sbatch)
        {
            Rectangle trueTransform = new Rectangle(Transform.X, Transform.Y + Game1.simulation.CellSize * Game1.simulation.Grid.Height, Transform.Width, Transform.Height);
            sbatch.Draw(Game1.px1, trueTransform, IsHovered ? Color.White * 0.5f : Color.Wheat);
            sbatch.Draw(Game1.px1, new Rectangle(trueTransform.X, trueTransform.Y, trueTransform.Width, 2), Color.Black);
            sbatch.Draw(Game1.px1, new Rectangle(trueTransform.X, trueTransform.Y + trueTransform.Height, trueTransform.Width, 2), Color.Black);
            sbatch.Draw(Game1.px1, new Rectangle(trueTransform.X, trueTransform.Y, 2, trueTransform.Height), Color.Black);
            sbatch.Draw(Game1.px1, new Rectangle(trueTransform.X + trueTransform.Width, trueTransform.Y, 2, trueTransform.Height + 2), Color.Black);
            sbatch.DrawString(Game1.font, text, new Vector2(trueTransform.X + trueTransform.Width / 2 - Game1.font.MeasureString(text).X / 2 + 2, trueTransform.Y + 5), Color.Black);
        }
    }
}
