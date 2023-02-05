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
    public class RandPrim : WallGenerationAlgorithm
    {
        public RandPrim(Grid grid) : base("Depth First", grid)
        {
            HasBorderWalls = true;

            MenuItems = new List<ADrawable>()
            {

            };
        }
        public override void Generate()
        {
            foreach (Cell c in grid.GetAllCells) c.IsWall = true;
            grid.EndCell.IsWall = false;
            grid.StartCell.IsWall = false;

            List<Cell> wallList = new List<Cell>();

            Cell current = grid.StartCell;

            for(int i = 0; i < 100; i++)
            {
                wallList.AddRange(current.GetNeighbors.Where(n => n.IsWall && n.GetNeighbors.Count(n2 => !n2.IsWall) == 1));
                current = wallList[new Random().Next(wallList.Count)];
                wallList.Remove(current);
                current.IsWall = false;
            }
            /* Commencer au premier noeud
             * Ajouter tous ses murs voisinnants dans la liste des murs à visiter, SAUF ceux qui ont plus de 2 voisins "Chemins"
             * Tant qu'il y a des murs dans la liste, prendre un mur au hasard de la liste et recommencer.
             * 
             */
        }
    }
}
