using Raylib_cs;
using System.Numerics;

namespace PONG
{
    internal class Program
    {
        public static int P1SCORE;
        public static int P2SCORE;

        public static int width = 1280;
        public static int height = 720;

        public static int PlayerWidth = width / 50;
        public static int PlayerHeight = height / 6;

        public static int minX = 0;
        public static int maxY = 0;

        public static int maxX = width;
        public static int minY = height;

        public static int offsetX = width / 18;
        public static int offsetY = height / 2;

        public static float speed = 300.0f;
        public static float modifier = 1.15f;

        public static Rectangle rec1 = new Rectangle(minX + offsetX - PlayerWidth, height / 2, PlayerWidth, PlayerHeight);
        public static Rectangle rec2 = new Rectangle(maxX - offsetX, height / 2, PlayerWidth, PlayerHeight);

        public static Vector2 center = new(width/2, height/2);
        public static float radius = 30f;
        public static Color color = Color.White;
        public static Vector2 direction = new(-1, -1);

        static void Main()
        {
            Raylib.InitWindow(width, height, "PONG");

            while (!Raylib.WindowShouldClose())
            {
                speed = Math.Clamp(speed, 300, 1000);

                center.X = Math.Clamp(center.X, 0, Raylib.GetScreenWidth());
                center.Y = Math.Clamp(center.Y, 0, Raylib.GetScreenHeight());

                center += direction * speed * Raylib.GetFrameTime();

                if (Raylib.IsKeyDown(KeyboardKey.W)) // P1 movement
                { rec1.Y -= 400f * Raylib.GetFrameTime(); }
                if (Raylib.IsKeyDown(KeyboardKey.S))
                { rec1.Y += 400f * Raylib.GetFrameTime(); }

                if (Raylib.IsKeyDown(KeyboardKey.Up)) // P2 movement
                { rec2.Y -= 400f * Raylib.GetFrameTime(); }
                if (Raylib.IsKeyDown(KeyboardKey.Down))
                { rec2.Y += 400f * Raylib.GetFrameTime(); }

                rec1.Y = Math.Clamp(rec1.Y, maxY, minY - PlayerHeight);
                rec2.Y = Math.Clamp(rec2.Y, maxY, minY - PlayerHeight);

                if (center.Y <= maxY + radius) // Top edge col
                {
                    direction.Y = Math.Abs(direction.Y); 
                    center.Y = maxY + radius; 
                }
                else if (center.Y + radius >= Raylib.GetScreenHeight()) // Bottom edge col
                {
                    direction.Y = -Math.Abs(direction.Y); 
                    center.Y = Raylib.GetScreenHeight() - radius; 
                }

                if (center.X <= minX + radius || center.X + radius >= Raylib.GetScreenWidth() + radius) // Score
                { 
                    if (center.X <= minX + radius) 
                    { P2SCORE += 1; }
                    else P1SCORE += 1;
                    center = new(width / 2, height / 2); speed = 200f; direction.X *= -1.0f; 
                }

                if (Raylib.CheckCollisionCircleRec(center, radius, rec1))
                {
                    if (center.Y < rec1.Y) // Top
                    {
                        direction.Y *= -1.0f;
                        center.Y = rec1.Y - radius; 
                    }
                    else if (center.Y > rec1.Y + PlayerHeight) // Bottom
                    {
                        direction.Y *= -1.0f;
                        center.Y = rec1.Y + PlayerHeight + radius; 
                    }
                    else // Front
                    {
                        direction.X *= -1.0f;
                        center.X = rec1.X + PlayerWidth + radius; 
                    }
                    speed *= modifier; 
                }

                if (Raylib.CheckCollisionCircleRec(center, radius, rec2))
                {
                    if (center.Y < rec2.Y) // Top
                    {
                        direction.Y *= -1.0f;
                        center.Y = rec2.Y - radius; 
                    }
                    else if (center.Y > rec2.Y + PlayerHeight) // Bottom
                    {
                        direction.Y *= -1.0f;
                        center.Y = rec2.Y + PlayerHeight + radius; 
                    }
                    else // Front
                    {
                        direction.X *= -1.0f;
                        center.X = rec2.X - radius; 
                    }
                    speed *= modifier; 
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DarkBlue);
                //////////////////////////////////////////////////////////////////////////////////
                Raylib.DrawCircleV(center, radius, color);
                Raylib.DrawRectangleRec(rec1, Color.SkyBlue);
                Raylib.DrawRectangleRec(rec2, Color.Gold);
                Raylib.DrawText($"Ball={(int)center.X}, {(int)center.Y}", 0, 0, 20, Color.White);
                Raylib.DrawText($"P1Y={(int)rec1.Y}", 0, 25, 20, Color.White);
                Raylib.DrawText($"P2Y={(int)rec2.Y}", 0, 50, 20, Color.White);
                Raylib.DrawText($"Speed={speed.ToString()}", 0, 75, 20, Color.White);
                Raylib.DrawText(P1SCORE.ToString(), width/4, 5, 50, Color.SkyBlue);
                Raylib.DrawText(P2SCORE.ToString(), width/4*3, 5, 50, Color.Gold);
                //////////////////////////////////////////////////////////////////////////////////
                Raylib.EndDrawing();
            }
        }
    }
}