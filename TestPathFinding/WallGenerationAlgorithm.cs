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
        public static List<Type> WGAList = new List<Type>()
        {
            typeof(RandomWalls),
            typeof(RandPrim),
        };

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
        public virtual void DrawMenu(SpriteBatch sbatch)
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
                item.Update();
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
                new Button("-0.05", 165, 60, Color.Black, () =>
                {
                    WallDensity -= 0.05f;
                })
            };
        }
        public RandomWalls() : this(Game1.simulation.Grid) { }
        public override void Generate()
        {
            foreach(Cell cell in grid.GetAllCells) cell.IsWall = new Random().NextDouble() < WallDensity;
        }
        public override void DrawMenu(SpriteBatch sbatch)
        {
            base.DrawMenu(sbatch);
            List<ADrawable> list = new List<ADrawable>()
            {
                new Label($"{WallDensity}", new Point(165, 30), Color.Black)
            };

            list.ForEach(a => a.Draw(sbatch));
        }
    }
    public class RandPrim : WallGenerationAlgorithm
    {
        private List<Cell> wallList= new List<Cell>();
        public RandPrim(Grid grid) : base("Rand Prim", grid)
        {
            HasBorderWalls = true;

            MenuItems = new List<ADrawable>()
            {
            };
        }
        public RandPrim() : this(Game1.simulation.Grid) { }
        public override void Generate()
        {
            foreach (Cell c in grid.GetAllCells) c.IsWall = true;
            grid.EndCell.IsWall = false;
            grid.StartCell.IsWall = false;

            Generate(grid.StartCell);
        }
        public void Generate(Cell cell)
        {
            wallList.AddRange(cell.GetNeighbors.Where(n => n.IsWall));
            wallList.RemoveAll(n => n.GetNeighbors.Count(n2 => !n2.IsWall) != 1);
            if(wallList.Count > 0)
            {
                Cell nextWall = wallList[new Random().Next(wallList.Count)];
                wallList.Remove(nextWall);
                nextWall.IsWall = false;
                Game1.Instance.AddEvent(new TimedEvent(() => Generate(nextWall), 0));
            } else
            {
                grid.EndCell.IsWall = false;
                foreach (Cell c in grid.EndCell.GetNeighbors) c.IsWall = false;
            }
        }
    }
}
