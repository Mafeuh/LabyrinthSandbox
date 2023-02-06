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

    public class Label : ADrawable
    {
        private string Text { get; set; }
        public Label(string text, Rectangle transform, Color color) : base(transform, color) 
        {
            Text = text;
        }
        public Label(string text, Point position, Color color) : base(new Rectangle(position, new Point(0)), color)
        {
            Text = text;
        }
        public override void Update() { }
        public override void Draw(SpriteBatch sbatch)
        {
            sbatch.DrawString(Game1.font, Text, new Vector2(Transform.X, Game1.simulation.CellSize * Game1.simulation.Grid.Height + Transform.Y), DrawColor);
        }
    }
}