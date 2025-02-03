using Raylib_cs;
using System.Numerics;

namespace TANKS
{
    internal class Program
    {
        public class Game
        {
            const int WinWidth = 1920;
            const int WinHeight = 1080;
            bool gameRunning;

            private Tank P1Tank;

            public static void Main()
            {
                Game game = new Game();
                game.Run();
            }

            public void Run()
            {
                Init();
                GameLoop();
            }

            private void Init()
            {
                Raylib.InitWindow(WinWidth, WinHeight, "TANKS!");
                int sizeXY = WinWidth / 20;
                P1Tank = new Tank(
                    sizeXY,
                    new Rectangle(WinWidth / 2 + sizeXY*0.125f, WinHeight / 2, sizeXY * 0.75f, sizeXY),
                    Color.Blue,
                    new Vector2(sizeXY / 2, sizeXY / 2),
                    0f,
                    new Vector2(WinWidth / 2, WinHeight / 2),
                    new Vector2(WinWidth / 2, WinHeight / 2 + sizeXY),
                    0f,
                    new Rectangle(WinWidth / 2 - sizeXY / 4, WinHeight / 2, sizeXY / 4, sizeXY),
                    new Rectangle(WinWidth / 2 + sizeXY*1f, WinHeight / 2, sizeXY / 4, sizeXY)
                );
                gameRunning = true;
            }

            private void GameLoop()
            {
                while (Raylib.WindowShouldClose() == false && gameRunning)
                {
                    UpdateGame();
                    DrawGame();
                }
                Raylib.CloseWindow();
            }

            private void UpdateGame()
            {
                if (Raylib.IsKeyDown(KeyboardKey.W))
                {
                    Vector2 direction = new Vector2(
                        (float)Math.Sin(P1Tank.Rotation * Math.PI / 180f),
                        -(float)Math.Cos(P1Tank.Rotation * Math.PI / 180f)
                    );
                    P1Tank.UpdatePosition(new Vector2(
                        P1Tank.Base.X + direction.X * 100f * Raylib.GetFrameTime(),
                        P1Tank.Base.Y + direction.Y * 100f * Raylib.GetFrameTime()
                    ));
                }
                if (Raylib.IsKeyDown(KeyboardKey.S))
                {
                    Vector2 direction = new Vector2(
                        (float)Math.Sin(P1Tank.Rotation * Math.PI / 180f),
                        -(float)Math.Cos(P1Tank.Rotation * Math.PI / 180f)
                    );
                    P1Tank.UpdatePosition(new Vector2(
                        P1Tank.Base.X - direction.X * 80f * Raylib.GetFrameTime(),
                        P1Tank.Base.Y - direction.Y * 80f * Raylib.GetFrameTime()
                    ));
                }
                if (Raylib.IsKeyDown(KeyboardKey.D) && (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.S))) P1Tank.Rotation += 30f * Raylib.GetFrameTime();
                if (Raylib.IsKeyDown(KeyboardKey.A) && (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.S))) P1Tank.Rotation -= 30f * Raylib.GetFrameTime();
                if (Raylib.IsKeyDown(KeyboardKey.Right)) P1Tank.GunRotation += 0.5f * Raylib.GetFrameTime();
                if (Raylib.IsKeyDown(KeyboardKey.Left)) P1Tank.GunRotation -= 0.5f * Raylib.GetFrameTime();
                P1Tank.UpdateCenterAndBarrel();
            }

            private void DrawGame()
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DarkGreen);
                Raylib.DrawLine(0, WinHeight / 2, WinWidth, WinHeight / 2, Color.Black);
                Raylib.DrawLine(WinWidth / 2, 0, WinWidth / 2, WinHeight, Color.Black);
                DrawTank(P1Tank);
                Raylib.EndDrawing();
            }

            public class Tank
            {
                public int SizeXY { get; private set; }
                public Rectangle Base { get; set; }
                public Rectangle BaseOffLeft { get; set; }
                public Rectangle BaseOffRight { get; set; }
                public Color Color { get; set; }
                public Vector2 Origin { get; private set; }
                public float Rotation { get; set; }
                public Vector2 Center { get; set; }
                public Vector2 BarrelEnd { get; set; }
                public float GunRotation { get; set; }

                public Tank(int sizeXY, Rectangle baseRect, Color color, Vector2 origin, float rotation, Vector2 center, Vector2 barrelEnd, float gunRotation, Rectangle baseOffLeft, Rectangle baseOffRight)
                {
                    SizeXY = sizeXY;
                    Base = baseRect;
                    Color = color;
                    Origin = origin;
                    Rotation = rotation;
                    Center = center;
                    BarrelEnd = barrelEnd;
                    GunRotation = gunRotation;
                    BaseOffLeft = baseOffLeft;
                    BaseOffRight = baseOffRight;
                }

                public void UpdatePosition(Vector2 newPosition)
                {
                    Base = new Rectangle(newPosition.X, newPosition.Y, Base.Width, Base.Height);

                    float angleRadians = Rotation * MathF.PI / 180f;

                    Vector2 offsetLeft = new Vector2(-SizeXY / 2 + SizeXY / 4, 0);
                    Vector2 offsetRight = new Vector2(SizeXY / 2 + SizeXY / 4, 0);

                    Vector2 rotatedOffsetLeft = new Vector2(
                        offsetLeft.X * MathF.Cos(angleRadians) - offsetLeft.Y * MathF.Sin(angleRadians),
                        offsetLeft.X * MathF.Sin(angleRadians) + offsetLeft.Y * MathF.Cos(angleRadians)
                    );

                    Vector2 rotatedOffsetRight = new Vector2(
                        offsetRight.X * MathF.Cos(angleRadians) - offsetRight.Y * MathF.Sin(angleRadians),
                        offsetRight.X * MathF.Sin(angleRadians) + offsetRight.Y * MathF.Cos(angleRadians)
                    );

                    BaseOffLeft = new Rectangle(newPosition.X + rotatedOffsetLeft.X, newPosition.Y + rotatedOffsetLeft.Y, BaseOffLeft.Width, BaseOffLeft.Height);
                    BaseOffRight = new Rectangle(newPosition.X + rotatedOffsetRight.X, newPosition.Y + rotatedOffsetRight.Y, BaseOffRight.Width, BaseOffRight.Height);

                    UpdateCenterAndBarrel();
                }


                public void UpdateCenterAndBarrel()
                {
                    Center = new Vector2(Base.X, Base.Y);
                    float angleRadians = GunRotation + Rotation * (MathF.PI / 180f);
                    BarrelEnd = new Vector2(
                        Center.X + MathF.Sin(angleRadians) * SizeXY,
                        Center.Y - MathF.Cos(angleRadians) * SizeXY
                    );
                }
            }

            private void DrawTank(Tank tank)
            {
                Raylib.DrawRectanglePro(tank.BaseOffLeft, tank.Origin, tank.Rotation, Color.DarkBlue);
                Raylib.DrawRectanglePro(tank.BaseOffRight, tank.Origin, tank.Rotation, Color.DarkBlue);
                Raylib.DrawRectanglePro(tank.Base, tank.Origin, tank.Rotation, tank.Color);
                Raylib.DrawLineEx(tank.Center, tank.BarrelEnd, 10f, Color.DarkGray);
                Raylib.DrawCircleV(tank.Center, tank.Base.Width / 3, Color.DarkBlue);
                Raylib.DrawCircleV(tank.Center, tank.Base.Width / 4, Color.SkyBlue);
            }
        }
    }
}