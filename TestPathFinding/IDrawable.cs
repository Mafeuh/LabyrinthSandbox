using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPathFinding
{
    public abstract class ADrawable
    {
        public Rectangle Transform { get; set; }
        public Color DrawColor { get; set; }
        public ADrawable(Rectangle rectangle, Color color)
        {
            Transform = rectangle;
            DrawColor = color;
        }
        public abstract void Update();
        public abstract void Draw(SpriteBatch sbatch);
    }
}
