using Raylib_cs;
using System.Diagnostics;
using System.Numerics;

namespace LunarLander
{
    internal class Ship
    {
        public int screen_width;
        public int screen_height;
        Color ship_color = Color.White;
        Color flame_color = Color.Orange;

        Vector2 position;
        public Vector2 velocity;

        bool engineRunning;
        float engineAcceleration;
        Vector2 engineDirection;
        float engineFuel = 8000;
        float engineFuelConsumption;

        public bool shipHasLanded;
        public Vector2 shipBottom;
        float shipSpeed;

        public void Init()
        {
            position = new Vector2(screen_width / 2, screen_height * 0.1f);
            velocity = new Vector2(0, 0);
            engineDirection = new Vector2(0, -1);
            engineAcceleration = 75;
            engineFuelConsumption = 2000;
            shipHasLanded = false;
        }

        public void Update()
        {
            float frameTime = Raylib.GetFrameTime();
            Vector2 gravity = new Vector2(0.0f, 1.0f);
            Vector2 direction = new Vector2(0.0f, 0.0f);

            if (Raylib.IsKeyDown(KeyboardKey.W) && engineFuel > 0 && !shipHasLanded)
            {
                engineRunning = true;
                direction += engineDirection;
                engineFuel -= engineFuelConsumption * frameTime;
            }
            else
            {
                engineRunning = false;
                direction -= engineDirection;
            }
            Vector2 acceleration = direction * engineAcceleration + gravity;
            velocity += acceleration * frameTime;
            position += velocity * frameTime;
            Console.WriteLine((int)velocity.Y);
        }

        public void Draw()
        {
            float shipWidth = screen_width * 0.02f;
            float shipHeight = screen_height * 0.05f;

            DrawShip(shipWidth, shipHeight);

            if (engineRunning)
            {
                DrawFlame(shipWidth, shipHeight);
            }

            DrawFuel();
        }
        
        void DrawShip(float shipWidth, float shipHeight)
        {
            shipBottom = new Vector2 (screen_width / 2, (int)position.Y + (int)shipHeight / 2);

            Vector2 A = new Vector2(position.X, position.Y - shipHeight);                 // Top point
            Vector2 B = new Vector2(position.X - shipWidth, position.Y + shipHeight / 2); // Bottom left
            Vector2 C = new Vector2(position.X + shipWidth, position.Y + shipHeight / 2); // Bottom right
            Raylib.DrawTriangle(A, B, C, ship_color);
        }

        void DrawFlame(float shipWidth, float shipHeight)
        {
            float flameLength = shipHeight * 0.75f;
            Vector2 A = new Vector2(position.X, position.Y + shipHeight + flameLength);       // Bottom point
            Vector2 B = new Vector2(position.X - shipWidth / 2, position.Y + shipHeight / 2); // Top left
            Vector2 C = new Vector2(position.X + shipWidth / 2, position.Y + shipHeight / 2); // Top right
            Raylib.DrawTriangle(C, B, A, flame_color);
        }

        void DrawFuel()
        {
            float FuelWidth = screen_width * 0.05f;
            float FuelHeight = screen_height * 0.00005f * engineFuel;

            Raylib.DrawRectangle((int)FuelWidth, screen_height - (int)FuelHeight - (int)FuelWidth, (int)FuelWidth, (int)FuelHeight, Color.Yellow);
        }
    }
}