using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestPathFinding.WallGenerationAlgorithm;

namespace TestPathFinding
{

    public class Label<T> : ADrawable
    {
        private string Text { get; set; }
        private ValueContainer<T> Value { get; set; }
        public Label(T value, Rectangle transform, Color color) : base(transform, color) 
        {
            Value = new ValueContainer<T>(value);
        }
        public void SetValue(T value)
        {
            Value = new ValueContainer<T>(value);
        }
        public Label(T value, Point position, Color color) : base(new Rectangle(position, new Point(0)), color)
        {
            Value = new ValueContainer<T>(value);
        }
        public override void Update()
        {

        }
        public override void Draw(SpriteBatch sbatch)
        {
            sbatch.DrawString(Game1.font, Value.Value.ToString(), new Vector2(Transform.X, Game1.simulation.CellSize * Game1.simulation.Grid.Height + Transform.Y), DrawColor);
        }
    }
}