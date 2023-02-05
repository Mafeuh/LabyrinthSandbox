using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestPathFinding
{
    public class Game1 : Game
    {
        public static Game1 Instance;
        public static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MouseState curState;
        private MouseState prevState;
        public static Simulation simulation = new Simulation(new Grid(50, 30));
        public static Texture2D px1;
        public static SpriteFont font;
        public static bool LeftClick;
        public static bool RightClick;
        public static bool MiddleClick;
        public static Color BackgroundColor = Color.CornflowerBlue;
        public static int WindowWidth => _graphics.PreferredBackBufferWidth;
        public static int WindowHeight => _graphics.PreferredBackBufferHeight;

        public List<TimedEvent> events = new List<TimedEvent>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
        }

        public void NewGrid(Grid grid)
        {
            int size = Math.Min(
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) / grid.Width,
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) / grid.Height - 200 / grid.Height);

            simulation.CellSize = size;

            _graphics.PreferredBackBufferWidth = grid.Width * size;

            if(_graphics.PreferredBackBufferWidth < 1300) { _graphics.PreferredBackBufferWidth = 1300; }

            _graphics.PreferredBackBufferHeight = grid.Height * size + 100;

            _graphics.ApplyChanges();

            /*buttons = new List<Button>()
            {
                new Button("Pause / Unpause", 10, simulation.Grid.Height * simulation.CellSize + 10, () => {
                    simulation.SwitchPauseState();
                }),
                new Button("Spawn new Walls", 200, simulation.Grid.Height * simulation.CellSize + 10, () =>
                {
                    simulation.SpawnGridWalls();
                }),

                new Button("<", 200, simulation.Grid.Height * simulation.CellSize + 60, () =>
                {
                    if(simulation.WallDensity >= 0.05f) {
                        simulation.WallDensity -= 0.05f;
                        simulation.WallDensity = Convert.ToSingle(Math.Round(simulation.WallDensity, 2));
                    }
                }),
                new Button(">", 270, simulation.Grid.Height * simulation.CellSize + 60, () =>
                {
                    if(simulation.WallDensity <=  0.95f) {
                        simulation.WallDensity += 0.05f;
                        simulation.WallDensity = Convert.ToSingle(Math.Round(simulation.WallDensity, 2));
                    }
                }),

                new Button("Reset Simulation", 400, simulation.Grid.Height * simulation.CellSize + 10, () =>
                {
                    simulation.ClearGrid();
                }),

                //BUTTONS FOR MANIPULATING THE SIMULATION'S STEP FREQUENCY

                new Button("+100", 130, simulation.Grid.Height * simulation.CellSize + 40, () =>
                {
                    simulation.NextStepDelay += 100;
                }),
                new Button("+50", 100, simulation.Grid.Height * simulation.CellSize + 40, () =>
                {
                    simulation.NextStepDelay += 50;
                }),
                new Button("-50", 100, simulation.Grid.Height * simulation.CellSize + 65, () =>
                {
                    simulation.NextStepDelay -= 50;
                    if(simulation.NextStepDelay <= 0) simulation.NextStepDelay = 1;
                }),
                new Button("-100", 130, simulation.Grid.Height * simulation.CellSize + 65, () =>
                {
                    simulation.NextStepDelay -= 100;
                    if(simulation.NextStepDelay <= 0) simulation.NextStepDelay = 1;
                }),



                //BUTTONS FOR MANIPULATING THE NEXT GRID'S WIDTH
                new Button("+50", 1000, simulation.Grid.Height * simulation.CellSize + 20, () =>
                {
                    Grid.NextGridWidth += 50;
                }),
                new Button("+10", 950, simulation.Grid.Height * simulation.CellSize + 20, () =>
                {
                    Grid.NextGridWidth += 10;
                }),
                new Button("+1", 900, simulation.Grid.Height * simulation.CellSize + 20, () =>
                {
                    Grid.NextGridWidth += 1;
                }),
                new Button("-50", 750, simulation.Grid.Height * simulation.CellSize + 20, () =>
                {
                    Grid.NextGridWidth -= 50;
                    if(Grid.NextGridWidth <= 0) Grid.NextGridWidth = 1;
                }),
                new Button("-10", 800, simulation.Grid.Height * simulation.CellSize + 20, () =>
                {
                    Grid.NextGridWidth -= 10;
                    if(Grid.NextGridWidth <= 0) Grid.NextGridWidth = 1;
                }),
                new Button("-1", 850, simulation.Grid.Height * simulation.CellSize + 20, () =>
                {
                    Grid.NextGridWidth -= 1;
                    if(Grid.NextGridWidth <= 0) Grid.NextGridWidth = 1;
                }),


                //BUTTONS FOR MANIPULATING THE NEXT GRID'S HEIGHT
                new Button("+50", 1000, simulation.Grid.Height * simulation.CellSize + 60, () =>
                {
                    Grid.NextGridHeight += 50;
                }),
                new Button("+10", 950, simulation.Grid.Height * simulation.CellSize + 60, () =>
                {
                    Grid.NextGridHeight += 10;
                }),
                new Button("+1", 900, simulation.Grid.Height * simulation.CellSize + 60, () =>
                {
                    Grid.NextGridHeight += 1;
                }),
                new Button("-50", 750, simulation.Grid.Height * simulation.CellSize + 60, () =>
                {
                    Grid.NextGridHeight -= 50;
                    if(Grid.NextGridHeight <= 0) Grid.NextGridHeight = 1;

                }),
                new Button("-10", 800, simulation.Grid.Height * simulation.CellSize + 60, () =>
                {
                    Grid.NextGridHeight -= 10;
                    if(Grid.NextGridHeight <= 0) Grid.NextGridHeight = 1;
                }),
                new Button("-1", 850, simulation.Grid.Height * simulation.CellSize + 60, () =>
                {
                    Grid.NextGridHeight -= 1;
                    if(Grid.NextGridHeight <= 0) Grid.NextGridHeight = 1;
                }),

                new Button("Generate", 1200, simulation.Grid.Height * simulation.CellSize + 40, () =>
                {
                    simulation.Grid = new Grid(Grid.NextGridWidth, Grid.NextGridHeight);
                    NewGrid(simulation.Grid);
                })
            };*/
            simulation.ClearGrid();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            Window.IsBorderless = true;
            
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            px1 = Content.Load<Texture2D>("1px");
            font = Content.Load<SpriteFont>("font");

            NewGrid(simulation.Grid);

            simulation.WGA = new RandomWalls(simulation.Grid);
        }

        public void AddEvent(TimedEvent timedEvent)
        {
            events.Add(timedEvent);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var ev in events.Where(e => e.ExecutionTime <= DateTime.Now)) ev.Action.Invoke();

            curState = Mouse.GetState();

            LeftClick = curState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released;
            RightClick = curState.RightButton == ButtonState.Pressed && prevState.RightButton == ButtonState.Released;
            MiddleClick = curState.MiddleButton == ButtonState.Pressed && prevState.MiddleButton == ButtonState.Released;

            prevState = Mouse.GetState();

            simulation.WGA.Update();

            if (!simulation.IsPaused)
            {
                if(DateTime.Now >= simulation.NextStepTime)
                {
                    simulation.Update();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);

            _spriteBatch.Begin();

            simulation.Draw(_spriteBatch);

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        public static void SetWindowName(string name)
        {
            Instance.Window.Title = name;
        }
    }
}