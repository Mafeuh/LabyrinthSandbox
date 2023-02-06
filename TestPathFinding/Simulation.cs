using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestPathFinding
{
    public class Simulation
    {
        private List<Cell> activeCells = new List<Cell>();
        public bool HasStarted { get; set; } = false;
        public bool AutoPlay { get; set; } = false;
        public bool Reverse { get; set; } = false;
        public bool IsPaused { get; set; } = true;
        public bool IsFinished { get; set; } = false;
        public DateTime NextStepTime { get; set; } = DateTime.Now;
        public int CellSize = 1;
        public Grid Grid { get; set; }
        public WallGenerationAlgorithm WGA { get; set; }
        public List<Cell> PathToShow { get; set; } = new List<Cell>();
        public int NextStepDelay { get; set; } = 100;
        public Simulation(Grid grid) 
        {
            Grid = grid;
            activeCells.Add(grid.StartCell);
        }
        public void InputMain()
        {
            if (
                !HasStarted
                &&
                Game1.Instance.CurrentState.X < Grid.Width * CellSize &&
                Game1.Instance.CurrentState.Y < Grid.Height * CellSize &&
                Game1.Instance.CurrentState.X > 0 &&
                Game1.Instance.CurrentState.Y > 0
                )
            {
                if (Game1.LeftClick)
                {
                    Grid.StartCell = Grid.GetCell(Game1.Instance.CurrentState.X / CellSize, Game1.Instance.CurrentState.Y / CellSize);
                    activeCells.Clear();
                    activeCells.Add(Grid.StartCell);
                }
                if (Game1.RightClick)
                {
                    Grid.EndCell = Grid.GetCell(Game1.Instance.CurrentState.X / CellSize, Game1.Instance.CurrentState.Y / CellSize);
                }
            }
        }
        public void Update()
        {
            if (IsFinished && PathToShow.Count > 0 && NextStepTime <= DateTime.Now)
            {
                PathToShow[0].ParentOnPath = true;
                PathToShow.RemoveAt(0);
                NextStepTime = DateTime.Now.AddMilliseconds(NextStepDelay / 4);
                return;
            }

            if(activeCells.Count == 0)
            {
                foreach (Cell cell in Grid.GetAllCells.Where(c => c.IsVisited)) cell.IsDead = true;
                IsFinished = true;
                return;
            }

            foreach(Cell cell in activeCells)
            {
                if(cell == Grid.EndCell)
                {
                    EndSimulation();
                    return;
                }
            }
            NextStepTime = NextStepTime.AddMilliseconds(NextStepDelay);
            if (Reverse) PrevStep();
            else NextStep();
        }
        public void NextStep()
        {
            List<Cell> nextCells = new List<Cell>();
            foreach(Cell cell in activeCells)
            {
                foreach(Cell c in cell.GetNeighbors.Where(c => !c.IsVisited && !c.IsWall))
                {
                    c.IsVisited = true;
                    c.IsActive = true;
                    c.ParentCell = cell;
                    nextCells.Add(c);
                }
                cell.IsActive = false;
            }
            activeCells = nextCells;
        }
        public void SpawnGridWalls()
        {
            ClearGrid();
            WGA.Generate();
        }
        public void PrevStep()
        {
            List<Cell> prevCells = new List<Cell>();
            foreach(Cell cell in activeCells)
            {
                cell.ParentCell = null;
                cell.IsVisited = false;
            }
        }
        public void Draw(SpriteBatch sbatch)
        {
            List<ADrawable> ui = new List<ADrawable>()
            {
                new Label("Simulation", new Point(5, 2), Color.Black),
                new Label(WGA.AlgorithmName, new Point(5, 20), Color.Black),
                new Button("<", new Point(5, 40), new Point(30, 0), Color.Black, () => { PrevWallGenType(); }),
                new Button(">", new Point(40, 40), new Point(30, 0), Color.Black,() => { NextWallGenType(); }),
                new Button("Clear", new Point(5, 70), new Point(65, 0), Color.Black, () =>
                {
                    ClearGrid();
                    HasStarted = false;
                }),
                new Button("Generate Walls", 120, 2, Color.Black,() => 
                { 
                    WGA.Generate();
                    HasStarted = true;
                }),
                new Button("Start/Stop/Resume", 400, 10, Color.Black, () => 
                {
                    HasStarted = true;

                    SwitchPauseState(); 
                }),
                new Label($"{Game1.Instance.CurrentState.X / CellSize}", new Point(300, 10), Color.Black),
                new Label($"{Game1.Instance.CurrentState.Y / CellSize}", new Point(350, 10), Color.Black),
            };
            foreach (var element in ui)
            {
                if (element is Button button) button.Update();
                element.Draw(sbatch);
            }

            Grid.Draw(sbatch, CellSize);
            WGA.DrawMenu(sbatch);
        }

        public void SwitchPauseState()
        {
            NextStepTime = DateTime.Now.AddMilliseconds(100);
            IsPaused = !IsPaused;
        }

        public void EndSimulation()
        {
            IsFinished = true;
            List<Cell> winnerPath = new List<Cell>();
            Cell c = Grid.EndCell;
            while (c != Grid.StartCell)
            {
                winnerPath.Add(c);
                c = c.ParentCell;
                //c.ParentOnPath = true;
            }
            winnerPath.Reverse();
            PathToShow = winnerPath;
        }

        public void ClearGrid()
        {
            PathToShow.Clear();
            activeCells = new List<Cell>()
            {
                Grid.StartCell
            };
            IsFinished = false;
            IsPaused = true;
            Grid.Clear();
        }

        public void NextWallGenType()
        {
            int indexOfNext = WallGenerationAlgorithm.WGAList.IndexOf(WGA.GetType()) + 1;
            if (indexOfNext == WallGenerationAlgorithm.WGAList.Count)
            {
                indexOfNext = 0;
            }

            WGA = (WallGenerationAlgorithm)Activator.CreateInstance(WallGenerationAlgorithm.WGAList[indexOfNext]);
        }

        public void PrevWallGenType()
        {
            int indexOfPrev = WallGenerationAlgorithm.WGAList.IndexOf(WGA.GetType()) - 1;
            if(indexOfPrev == -1)
            {
                indexOfPrev = WallGenerationAlgorithm.WGAList.Count - 1;
            }
            WGA = (WallGenerationAlgorithm)Activator.CreateInstance(WallGenerationAlgorithm.WGAList[indexOfPrev]);
        }
    }
}
