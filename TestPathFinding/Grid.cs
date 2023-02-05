using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPathFinding
{
    public class Grid
    {
        private static Grid Instance;
        public static int NextGridWidth;
        public static int NextGridHeight;

        public Cell StartCell { get; set; }
        public Cell EndCell { get; set; }
        public int Width => CellGrid.Count;
        public int Height => CellGrid[0].Count;
        public List<List<Cell>> CellGrid { get; }
        public double GetVisitedPercentage => GetAllCells.Count(c => c.IsVisited) / GetAllCells.Count(c => !c.IsWall);
        public Grid(int width, int height)
        {
            CellGrid = new List<List<Cell>>();
            
            for(int i = 0; i < width; i++)
            {
                List<Cell> list = new List<Cell>();
                for(int j = 0; j < height; j++)
                {
                    list.Add(new Cell(this, i, j));
                }
                CellGrid.Add(list);
            }
            StartCell = CellGrid[0][height - 1];
            EndCell = CellGrid[width - 1][0];

            Instance = this;
            NextGridWidth= Width;
            NextGridHeight= Height;
        }
        public List<Cell> GetAllCells
        {
            get
            {
                List<Cell> cells = new List<Cell>();
                foreach(List<Cell> lc in CellGrid)
                {
                    foreach(Cell c in lc)
                    {
                        cells.Add(c);
                    }
                }
                return cells;
            }
        }
        public Cell GetCell(int x, int y)
        {
            if(x < 0 || y < 0 || x > Width - 1 || y > Height - 1) throw new ArgumentException();
            return CellGrid[x][y];
        }

        public void AddWalls(float complexity)
        {
            foreach (List<Cell> lc in CellGrid) for (int i = 0; i < lc.Count; i++) lc[i].IsWall = new Random().NextDouble() < complexity;
            EndCell.IsWall = false;
            StartCell.IsWall = false;
        }
        public void Draw(SpriteBatch sbatch, int cellSize)
        {
            foreach(List<Cell> lc in CellGrid)
                foreach(Cell cell in lc)
                    cell.Draw(sbatch, cellSize);

            for(int i = 0; i <= CellGrid.Count; i++)
            {
                sbatch.Draw(Game1.px1, new Rectangle(i * cellSize, 0, 1, CellGrid[0].Count * cellSize), Color.Black);
            }
            for(int i = 0; i <= CellGrid[0].Count; i++)
            {
                sbatch.Draw(Game1.px1, new Rectangle(0, i * cellSize, Game1.WindowWidth, 1), Color.Black);
            }
        }

        public void Clear()
        {
            foreach(List<Cell> lc in CellGrid)
            {
                foreach(Cell c in lc)
                {
                    c.IsVisited = false;
                    c.IsActive = false;
                    c.ParentOnPath = false;
                }
            }
        }
    }
}
