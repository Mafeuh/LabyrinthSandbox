using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPathFinding
{
    public class Cell
    {
        private readonly Grid grid;
        private Point position;
        public Cell ParentCell { get; set; }
        public bool IsVisited { get; set; } = false;
        public bool IsWall { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public bool ParentOnPath { get; set; } = false;
        public bool IsDead { get;set; } = false;
        public Color GetColor { 
            get
            {
                if (grid.StartCell == this) return Color.Green;
                if (grid.EndCell == this) return Color.Red;
                if(ParentOnPath) return Color.Yellow;
                if (IsActive) return Color.Blue;
                if(IsDead) return Color.DarkGray;
                if (IsVisited) return Color.LightBlue;
                if (IsWall) return Color.Gray;
                return Color.White;
            } 
        }

        public Cell(Grid grid, int posX, int posY) : this(grid)
        {
            position = new Point(posX, posY);
        }
        public Cell(Grid grid)
        {
            this.grid = grid;
        }
        public List<Cell> GetUnvisitedNeighbors => GetNeighbors.Where(n => !n.IsVisited).ToList();
        public List<Cell> GetNeighbors
        {
            get
            {
                List<Cell> cells = new List<Cell>();
                if(position.X - 1 >= 0) cells.Add(grid.GetCell(position.X - 1, position.Y));
                if(position.X + 1 < grid.Width - 1) cells.Add(grid.GetCell(position.X + 1, position.Y));
                if(position.Y - 1 >= 0) cells.Add(grid.GetCell(position.X, position.Y - 1));
                if(position.Y + 1 < grid.Height - 1) cells.Add(grid.GetCell(position.X, position.Y + 1));

                return cells;
            }
        }
        public List<Cell> GetRoundNeighbors
        {
            get
            {
                List<Cell> cells = GetNeighbors;
                if(position.X - 1 >= 0)
                {
                    if (position.Y - 1 >= 0) cells.Add(grid.GetCell(position.X - 1, position.Y - 1));
                    if (position.Y + 1 < grid.Height - 1) cells.Add(grid.GetCell(position.X - 1, position.Y + 1));
                }
                if(position.X + 1 < grid.Width)
                {
                    if (position.Y - 1 >= 0) cells.Add(grid.GetCell(position.X + 1, position.Y - 1));
                    if(position.Y + 1 < grid.Height - 1) cells.Add(grid.GetCell(position.X + 1, position.Y + 1));
                }
                return cells;
            }
        }
        public void Draw(SpriteBatch sbatch, int size)
        {
            sbatch.Draw(Game1.px1, new Rectangle(position.X * size, position.Y * size, size, size), GetColor);
        }
    }
}
