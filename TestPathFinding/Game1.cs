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
        public MouseState CurrentState;
        private MouseState prevState;
        public static Simulation simulation = new Simulation(new Grid(400, 200));
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

            //simulation.WGA = new RandomWalls(simulation.Grid);
            simulation.WGA = new RandPrim(simulation.Grid);
        }

        public void AddEvent(TimedEvent timedEvent)
        {
            events.Add(timedEvent);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for(int i = 0; i < events.Count; i++)
            {
                if (events[i].ExecutionTime <= DateTime.Now)
                {
                    events[i].Action.Invoke();
                    events.RemoveAt(i);
                }
            }

            CurrentState = Mouse.GetState();

            LeftClick = CurrentState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released;
            RightClick = CurrentState.RightButton == ButtonState.Pressed && prevState.RightButton == ButtonState.Released;
            MiddleClick = CurrentState.MiddleButton == ButtonState.Pressed && prevState.MiddleButton == ButtonState.Released;

            prevState = Mouse.GetState();

            simulation.WGA.Update();

            simulation.InputMain();

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