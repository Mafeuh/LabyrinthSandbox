using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestPathFinding
{
    public abstract class WallGenerationAlgorithm
    {
        protected Grid grid;
        public virtual bool HasBorderWalls { get; set; }
        public string AlgorithmName { get; set; }
        public List<ADrawable> MenuItems { get; set; }
        public WallGenerationAlgorithm(string algorithmName, Grid grid)
        {
            AlgorithmName = algorithmName;
            this.grid = grid;
        }
        public abstract void Generate();
        public void DrawMenu(SpriteBatch sbatch)
        {
            foreach (var item in MenuItems)
            {
                item.Draw(sbatch);
                if (item is Button b) b.Update();
            }
        }
        public void Update()
        {
            foreach(var item in MenuItems)
            {
                if(item is Button button) button.Update();
            }
        }
    }
    public class RandomWalls : WallGenerationAlgorithm
    {
        public float WallDensity { get; set; } = 0.5f;
        public RandomWalls(Grid grid) : base("Random Walls", grid)
        {
            HasBorderWalls = false;

            MenuItems = new List<ADrawable>()
            {
                new Label($"{this.WallDensity}", new Point(165, 30), Color.Black),
                new Button("-0.05", 165, 60, Color.Black, () =>
                {
                    WallDensity = Convert.ToSingle(Math.Round(WallDensity - 0.05f, 2));
                    (MenuItems[0] as Label).Text = WallDensity.ToString();
                })
            };
        }
        public override void Generate()
        {
            foreach(Cell cell in grid.GetAllCells) cell.IsWall = new Random().NextDouble() < WallDensity;
        }
    }
    public class DepthFirst : WallGenerationAlgorithm
    {
        public DepthFirst(Grid grid) : base("Depth First", grid)
        {
            HasBorderWalls = true;

            MenuItems = new List<ADrawable>()
            {

            };
        }
        public override void Generate()
        {
            Generate(grid.StartCell);
        }
        public void Generate(Cell currentCell)
        {
            Cell nextCell;
            int nbNeighbors = currentCell.GetNeighbors.Count(n => !n.IsVisited);
            if (nbNeighbors == 0)
            {
                currentCell.IsVisited = true;
                nextCell = currentCell.GetNeighbors[new Random().Next(nbNeighbors)];
            } else
            {
                nextCell = currentCell.ParentCell;
            }
            Game1.Instance.AddEvent(new TimedEvent(() => Generate(nextCell), 100));
            Generate(nextCell);
        }
    }
}
