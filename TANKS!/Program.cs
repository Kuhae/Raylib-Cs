using Raylib_cs;
using System.Data;
using System.Diagnostics;
using System.Numerics;
using static TANKS.Program.Game;

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
            private Tank P2Tank;
            private Obstacles Level1;
            private List<Projectile> projectiles = new List<Projectile>();
            public float rotationSpeed = 20f;
            public string Winner = "";

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
                Raylib.InitAudioDevice();

            int sizeXY = WinWidth / 20;
                // P1 properties
                P1Tank = new Tank(
                    true, // FireReady
                    3, // Hp
                    sizeXY, //SizeXY
                    new Rectangle(WinWidth / 16 + sizeXY * 0.125f, WinHeight / 2f, sizeXY * 0.75f, sizeXY), // BaseRect
                    Color.Blue, // Color
                    Color.DarkBlue, // Track Color
                    Color.DarkBlue, // Head Bottom Color
                    Color.SkyBlue, // Head Top Color
                    new Vector2(sizeXY / 2, sizeXY / 2), // Origin
                    0f, // Rotation
                    new Vector2(WinWidth / 2, WinHeight / 2), // Center
                    new Vector2(WinWidth / 2, WinHeight / 2), // PreviousPosition
                    0f, // PreviousRotation
                    new Vector2(WinWidth / 2, WinHeight / 2 + sizeXY), // BarrelEnd
                    0f, // Gun Rotation
                    new Rectangle(WinWidth / 16 - sizeXY / 8, WinHeight / 2, sizeXY / 4, sizeXY), // Left Track
                    new Rectangle(WinWidth / 16 + sizeXY / 1.145f, WinHeight / 2, sizeXY / 4, sizeXY) // Right Track
                );
                // P2 properties
                P2Tank = new Tank(
                    true, // FireReady
                    3, // Hp
                    sizeXY, //SizeXY
                    new Rectangle(WinWidth / 1.0625f + sizeXY * 0.125f, WinHeight / 2f, sizeXY * 0.75f, sizeXY), // BaseRect
                    Color.Orange, // Color
                    Color.Maroon, // Track Color
                    Color.Maroon, // Head Bottom Color
                    Color.Yellow, // Head Top Color
                    new Vector2(sizeXY / 2, sizeXY / 2), // Origin
                    0f, // Rotation
                    new Vector2(0, 0), // Center
                    new Vector2(0, 0), // PreviousPosition
                    0f, // PreviousRotation
                    new Vector2(WinWidth / 2, WinHeight / 2 + sizeXY), // BarrelEnd
                    0f, // Gun Rotation
                    new Rectangle(WinWidth / 1.0625f - sizeXY / 8, WinHeight / 2, sizeXY / 4, sizeXY), // Left Track
                    new Rectangle(WinWidth / 1.0625f + sizeXY / 1.145f, WinHeight / 2, sizeXY / 4, sizeXY) // Right Track
                );
                // obstacle sizes
                int obsWidth = WinWidth / 15;
                int obsHeight = WinHeight / 2;
                Level1 = new Obstacles(
                    new Rectangle(WinWidth / 6 + obsWidth / 2, WinHeight / 4 , obsWidth, obsHeight), 
                    new Rectangle(WinWidth / 1.375f, WinHeight / 4, obsWidth, obsHeight), 
                    Color.DarkBrown
                );

                gameRunning = true;
            }

            private void GameLoop()
            {
                while (Raylib.WindowShouldClose() == false && gameRunning)
                {
                    UpdateGame();
                    Collisions();
                    DrawGame();
                }

                Raylib.CloseWindow();
            }

            private void UpdateGame()
            {
                FireReady(P1Tank);
                Console.WriteLine(P1Tank.FireReady);
                FireReady(P2Tank);
                Console.WriteLine(P2Tank.FireReady);

                P1Tank.PreviousPosition = P1Tank.Center;
                P2Tank.PreviousPosition = P2Tank.Center;

                P1Tank.PreviousRotation = P1Tank.Rotation;
                P2Tank.PreviousRotation = P2Tank.Rotation;

                if (P1Tank.Hp > 0)
                { PlayerMovement(P1Tank); }
                else 
                {
                    Winner = "P2 Wins!";
                }
                if (P2Tank.Hp > 0)
                { PlayerMovement(P2Tank); }  
                else 
                {
                    Winner = "P1 Wins!";
                }

                foreach (var projectile in projectiles)
                {
                    projectile.UpdatePrj();
                }

                projectiles.RemoveAll(p => !p.Active);
            }

            private void PlayerMovement(Tank tank)
            {
                KeyboardKey Wk;
                KeyboardKey Sk;
                KeyboardKey Ak;
                KeyboardKey Dk;
                KeyboardKey Qk;
                KeyboardKey Ek;
                KeyboardKey Spacek;

                if (tank == P1Tank)
                {
                    Wk = KeyboardKey.W;
                    Sk = KeyboardKey.S;
                    Ak = KeyboardKey.A;
                    Dk = KeyboardKey.D;
                    Qk = KeyboardKey.Q;
                    Ek = KeyboardKey.E;
                    Spacek = KeyboardKey.Space;
                }
                else if (tank == P2Tank)
                {
                    Wk = KeyboardKey.Kp8;
                    Sk = KeyboardKey.Kp5;
                    Ak = KeyboardKey.Kp4;
                    Dk = KeyboardKey.Kp6;
                    Qk = KeyboardKey.Kp7;
                    Ek = KeyboardKey.Kp9;
                    Spacek = KeyboardKey.Kp0;
                }
                else return;

                if (Raylib.IsKeyPressed(Spacek))
                {
                    if (tank.FireReady)
                    {
                        Shoot(tank);
                    }
                }

                if (Raylib.IsKeyDown(Wk))
                {
                    Vector2 direction = new Vector2(
                        (float)Math.Sin(tank.Rotation * Math.PI / 180f),
                        -(float)Math.Cos(tank.Rotation * Math.PI / 180f)
                    );
                    tank.UpdatePosition(new Vector2(
                        tank.Base.X + direction.X * 120f * Raylib.GetFrameTime(),
                        tank.Base.Y + direction.Y * 120f * Raylib.GetFrameTime()
                    ));
                }

                if (Raylib.IsKeyDown(Sk))
                {
                    Vector2 direction = new Vector2(
                        (float)Math.Sin(tank.Rotation * Math.PI / 180f),
                        -(float)Math.Cos(tank.Rotation * Math.PI / 180f)
                    );
                    tank.UpdatePosition(new Vector2(
                        tank.Base.X - direction.X * 80f * Raylib.GetFrameTime(),
                        tank.Base.Y - direction.Y * 80f * Raylib.GetFrameTime()
                    ));
                }

                if (Raylib.IsKeyDown(Dk) && (Raylib.IsKeyDown(Wk) || Raylib.IsKeyDown(Sk)))
                {
                    float rotationSpeed = 30f * Raylib.GetFrameTime();
                    if (Raylib.IsKeyDown(Sk))
                        tank.Rotation -= rotationSpeed;
                    else
                        tank.Rotation += rotationSpeed;
                }

                if (Raylib.IsKeyDown(Ak) && (Raylib.IsKeyDown(Wk) || Raylib.IsKeyDown(Sk)))
                {
                    float rotationSpeed = 30f * Raylib.GetFrameTime();
                    if (Raylib.IsKeyDown(Sk))
                        tank.Rotation += rotationSpeed;
                    else
                        tank.Rotation -= rotationSpeed;
                }

                if (Raylib.IsKeyDown(Ek)) 
                    tank.GunRotation += 0.5f * Raylib.GetFrameTime();

                if (Raylib.IsKeyDown(Qk)) 
                    tank.GunRotation -= 0.5f * Raylib.GetFrameTime();

                tank.UpdateCenterAndBarrel();
            }


            private void Shoot(Tank tank)
            {
                if (!tank.FireReady) return;

                // projectile speed
                float projectileSpeed = 450f * Raylib.GetFrameTime();

                // laskee ampumis suunnan
                float angleRadians = tank.GunRotation + tank.Rotation * (MathF.PI / 180f);
                Vector2 direction = new Vector2(MathF.Sin(angleRadians), -MathF.Cos(angleRadians));

                // Luo uusi Projectile tykin päätyyn
                Projectile newProjectile = new Projectile(
                    tank.BarrelEnd, 
                    direction * projectileSpeed 
                );

                // Lisää projectile listalle
                projectiles.Add(newProjectile);

                tank.CooldownTimer.Start(1.5);
                tank.FireReady = false; 
            }

            private void FireReady(Tank tank)
            {
                if (!tank.FireReady)
                {
                    tank.FireReady = tank.CooldownTimer.IsDone();
                    if (tank.FireReady)
                    {
                        Console.WriteLine($"Tank {tank} cooldown complete!");
                    }
                }
            }


            public class Projectile
            {
                public Vector2 Position;
                public Vector2 Velocity;
                public bool Active;
                public float Radius = 10f; // size

                public Projectile(Vector2 position, Vector2 velocity)
                {
                    Position = position;
                    Velocity = velocity;
                    Active = true;
                }

                public void UpdatePrj()
                {
                    if (Active)
                    {
                        Position += Velocity;

                        if (Position.X < 0 || Position.X > WinWidth || Position.Y < 0 || Position.Y > WinHeight)
                        {
                            Active = false;
                        }
                    }
                }
            }

            private void Collisions()
            {
                ObsCollision(P1Tank, Level1.Obs1rec);
                ObsCollision(P1Tank, Level1.Obs2rec);

                ObsCollision(P2Tank, Level1.Obs1rec);
                ObsCollision(P2Tank, Level1.Obs2rec);

                EdgeCollision(P1Tank);
                EdgeCollision(P2Tank);

                PrjCollision();
            }

            private void ObsCollision(Tank tank, Rectangle obs)
            {
                Vector2[] baseOffRightPoints = GetRotatedRectanglePoints(tank.BaseOffRight, tank.Origin, tank.Rotation);
                Vector2[] baseOffLeftPoints = GetRotatedRectanglePoints(tank.BaseOffLeft, tank.Origin, tank.Rotation);
                Vector2[] basePoints = GetRotatedRectanglePoints(tank.Base, tank.Origin, tank.Rotation);

                Vector2[] obsPoints = GetRectanglePoints(obs);

                KeyboardKey Wk;
                KeyboardKey Sk;

                if (tank == P1Tank)
                {
                    Wk = KeyboardKey.W;
                    Sk = KeyboardKey.S;
                }
                else if (tank == P2Tank)
                {
                    Wk = KeyboardKey.Kp8;
                    Sk = KeyboardKey.Kp5;
                }
                else return;

                bool collided = false;
                bool sliding = false;
                foreach (var point in baseOffRightPoints)
                {
                    if (Raylib.CheckCollisionPointPoly(point, obsPoints))
                    {
                        collided = true; 
                        Console.WriteLine("Right track!");
                        if (Raylib.IsKeyDown(Wk) || Raylib.IsKeyDown(Sk))
                        {
                            sliding = true;
                            if (Raylib.IsKeyDown(Sk))
                            {
                                tank.Rotation -= rotationSpeed * Raylib.GetFrameTime();
                            }
                            else
                            {
                                tank.Rotation += rotationSpeed * Raylib.GetFrameTime();
                            }
                        }
                    }
                }
                foreach (var point in baseOffLeftPoints)
                {
                    if (Raylib.CheckCollisionPointPoly(point, obsPoints))
                    { 
                        collided = true; 
                        Console.WriteLine("Left track!");
                        if (Raylib.IsKeyDown(Wk) || Raylib.IsKeyDown(Sk))
                        {
                            sliding = true;
                            if (Raylib.IsKeyDown(Sk))
                            {
                                tank.Rotation += rotationSpeed * Raylib.GetFrameTime();
                            }
                            else
                            {
                                tank.Rotation -= rotationSpeed * Raylib.GetFrameTime();
                            }
                        }
                    }
                }
                foreach (var point in obsPoints)
                {
                    if (Raylib.CheckCollisionPointPoly(point, basePoints))
                        collided = true;
                }

                if (collided)
                {
                    tank.Center = tank.PreviousPosition;
                    if (!sliding)
                        tank.Rotation = tank.PreviousRotation;
                    else
                        Console.WriteLine("Sliding");
                    tank.UpdatePosition(tank.Center);
                    tank.UpdateCenterAndBarrel();
                }
            }

            private void PrjCollision()
            {
                foreach (var projectile in projectiles)
                {
                    if (!projectile.Active) continue;

                    if (IsProjectileCollidingWithTank(projectile, P1Tank))
                    {
                        P1Tank.Hp -= 1;
                        Console.WriteLine($"Player 1 hit! New HP:{P1Tank.Hp}");
                        projectile.Active = false;
                    }

                    // Check projectile collision with Player 2 tank
                    if (IsProjectileCollidingWithTank(projectile, P2Tank))
                    {
                        P2Tank.Hp -= 1;
                        Console.WriteLine($"Player 2 hit! New HP:{P2Tank.Hp}");
                        projectile.Active = false;
                    }

                    // Check collision with obstacles
                    if (Raylib.CheckCollisionCircleRec(projectile.Position, projectile.Radius, Level1.Obs1rec) ||
                        Raylib.CheckCollisionCircleRec(projectile.Position, projectile.Radius, Level1.Obs2rec))
                    {
                        projectile.Active = false;
                        Console.WriteLine("Projectile hit obstacle!");
                    }
                }
            }

            private void EdgeCollision(Tank tank)
            {
                Vector2[] baseOffRightPoints = GetRotatedRectanglePoints(tank.BaseOffRight, tank.Origin, tank.Rotation);
                Vector2[] baseOffLeftPoints = GetRotatedRectanglePoints(tank.BaseOffLeft, tank.Origin, tank.Rotation);

                KeyboardKey Wk;
                KeyboardKey Sk;

                if (tank == P1Tank)
                {
                    Wk = KeyboardKey.W;
                    Sk = KeyboardKey.S;
                }
                else if (tank == P2Tank)
                {
                    Wk = KeyboardKey.Kp8;
                    Sk = KeyboardKey.Kp5;
                }
                else return;


                bool collided = false;
                bool sliding = false;

                foreach (var point in baseOffRightPoints)
                {
                    if (Raylib.CheckCollisionPointLine(point, new Vector2(0, 0), new Vector2(WinWidth, 0), 10)
                    || Raylib.CheckCollisionPointLine(point, new Vector2(0, WinHeight), new Vector2(WinWidth, WinHeight), 10)
                    || Raylib.CheckCollisionPointLine(point, new Vector2(0, 0), new Vector2(0, WinHeight), 2)
                    || Raylib.CheckCollisionPointLine(point, new Vector2(WinWidth, 0), new Vector2(WinWidth, WinHeight), 2))
                    {
                        collided = true;
                        Console.WriteLine("Left track!");
                        if (Raylib.IsKeyDown(Wk) || Raylib.IsKeyDown(Sk))
                        {
                            sliding = true;
                            if (Raylib.IsKeyDown(Sk))
                            {
                                tank.Rotation -= rotationSpeed * Raylib.GetFrameTime();
                            }
                            else
                            {
                                tank.Rotation += rotationSpeed * Raylib.GetFrameTime();
                            }
                        }
                    }
                }

                foreach (var point in baseOffLeftPoints)
                {
                    if (Raylib.CheckCollisionPointLine(point, new Vector2(0, 0), new Vector2(WinWidth, 0), 10)
                    || Raylib.CheckCollisionPointLine(point, new Vector2(0, WinHeight), new Vector2(WinWidth, WinHeight), 10)
                    || Raylib.CheckCollisionPointLine(point, new Vector2(0, 0), new Vector2(0, WinHeight), 2)
                    || Raylib.CheckCollisionPointLine(point, new Vector2(WinWidth, 0), new Vector2(WinWidth, WinHeight), 2))
                    {
                        collided = true;
                        Console.WriteLine("Left track!");
                        if (Raylib.IsKeyDown(Wk) || Raylib.IsKeyDown(Sk))
                        {
                            sliding = true;
                            if (Raylib.IsKeyDown(Sk))
                            {
                                tank.Rotation += rotationSpeed * Raylib.GetFrameTime();
                            }
                            else
                            {
                                tank.Rotation -= rotationSpeed * Raylib.GetFrameTime();
                            }
                        }
                    }
                }

                if (collided)
                {
                    tank.Center = tank.PreviousPosition;
                    if (!sliding)
                        tank.Rotation = tank.PreviousRotation;
                    tank.UpdatePosition(tank.Center);
                    tank.UpdateCenterAndBarrel();
                }
            }

            private Vector2[] GetRectanglePoints(Rectangle rect)
            {
                return new Vector2[]
                {
                    new Vector2(rect.X, rect.Y),
                    new Vector2(rect.X + rect.Width, rect.Y),
                    new Vector2(rect.X + rect.Width, rect.Y + rect.Height),
                    new Vector2(rect.X, rect.Y + rect.Height)
                };
            }

            private Vector2[] GetRotatedRectanglePoints(Rectangle rect, Vector2 origin, float rotation)
            {
                float angleRad = rotation * (MathF.PI / 180f);

                Vector2 adjustedOrigin = new Vector2(rect.X, rect.Y);

                Vector2[] corners = new Vector2[4];
                corners[0] = new Vector2(rect.X - 50, rect.Y - 50);                           // Top-left
                corners[1] = new Vector2(rect.X + rect.Width - 50, rect.Y - 50);              // Top-right
                corners[2] = new Vector2(rect.X + rect.Width - 50, rect.Y + rect.Height - 50); // Bottom-right
                corners[3] = new Vector2(rect.X - 50, rect.Y + rect.Height - 50);             // Bottom-left

                for (int i = 0; i < corners.Length; i++)
                {
                    Vector2 relative = corners[i] - adjustedOrigin;
                    corners[i] = new Vector2(
                        relative.X * MathF.Cos(angleRad) - relative.Y * MathF.Sin(angleRad),
                        relative.X * MathF.Sin(angleRad) + relative.Y * MathF.Cos(angleRad)
                    ) + adjustedOrigin;
                }

                return corners;
            }

            private bool IsProjectileCollidingWithTank(Projectile projectile, Tank tank)
            {
                Vector2[] baseOffLeftPoints = GetRotatedRectanglePoints(tank.BaseOffLeft, tank.Origin, tank.Rotation);
                Vector2[] baseOffRightPoints = GetRotatedRectanglePoints(tank.BaseOffRight, tank.Origin, tank.Rotation);
                Vector2[] basePoints = GetRotatedRectanglePoints(tank.Base, tank.Origin, tank.Rotation);

                return IsCircleCollidingWithPolygon(projectile.Position, projectile.Radius, baseOffLeftPoints) ||
                       IsCircleCollidingWithPolygon(projectile.Position, projectile.Radius, baseOffRightPoints) ||
                       IsCircleCollidingWithPolygon(projectile.Position, projectile.Radius, basePoints);
            }

            private bool IsCircleCollidingWithPolygon(Vector2 circlePos, float radius, Vector2[] polygonPoints)
            {
                for (int i = 0; i < polygonPoints.Length; i++)
                {
                    Vector2 p1 = polygonPoints[i];
                    Vector2 p2 = polygonPoints[(i + 1) % polygonPoints.Length];

                    // Check the closest point on the edge to the circle center
                    Vector2 closest = ClosestPointOnLine(circlePos, p1, p2);
                    float distanceSquared = Vector2.DistanceSquared(closest, circlePos);

                    if (distanceSquared <= radius * radius)
                        return true;
                }

                return false;
            }

            private Vector2 ClosestPointOnLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
            {
                Vector2 lineDir = lineEnd - lineStart;
                float t = Vector2.Dot(point - lineStart, lineDir) / Vector2.Dot(lineDir, lineDir);

                t = Math.Clamp(t, 0, 1); // Clamp to segment
                return lineStart + t * lineDir;
            }

            private void DrawGame()
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DarkGreen);
                ///////////////////////////////////////////////////////////////////////////
                DrawObstacles(Level1);
                DrawProjectiles();
                DrawTank(P1Tank);
                DrawTank(P2Tank);
                if (Winner != "")
                {
                    int TxtSize = Raylib.MeasureText(Winner, WinHeight / 8);
                    Raylib.DrawText(Winner, WinWidth/2-TxtSize/2, WinHeight/2 - TxtSize / 8, WinHeight/8, Color.RayWhite);
                    Raylib.EndDrawing();
                    Raylib.WaitTime(4f);
                    gameRunning = false;
                }
                ///////////////////////////////////////////////////////////////////////////
                Raylib.EndDrawing();
            }

            public class Tank
            {
                public Timer CooldownTimer {  get; set; } = new Timer();
                public bool FireReady { get; set; }
                public int Hp {  get; set; }
                public int SizeXY { get; private set; }
                public Rectangle Base { get; set; }
                public Rectangle BaseOffLeft { get; set; }
                public Rectangle BaseOffRight { get; set; }
                public Color Color { get; set; }
                public Color TrackColor { get; set; }
                public Color HeadBot {  get; set; }
                public Color HeadTop { get; set; }
                public Vector2 Origin { get; private set; }
                public float Rotation { get; set; }
                public Vector2 Center { get; set; }
                public Vector2 PreviousPosition { get; set; }
                public float PreviousRotation { get; set; }
                public Vector2 BarrelEnd { get; set; }
                public float GunRotation { get; set; }

                public Tank(bool fireReady, int hp, int sizeXY, Rectangle baseRect, Color color, Color trackColor, Color headBot, Color headTop, Vector2 origin, float rotation, Vector2 center, Vector2 previousPosition, float previousRotation, Vector2 barrelEnd, float gunRotation, Rectangle baseOffLeft, Rectangle baseOffRight)
                {
                    FireReady = fireReady;
                    Hp = hp;
                    SizeXY = sizeXY;
                    Base = baseRect;
                    Color = color;
                    TrackColor = trackColor;
                    HeadBot = headBot;
                    HeadTop = headTop;
                    Origin = origin;
                    Rotation = rotation;
                    Center = center;
                    PreviousPosition = previousPosition;
                    PreviousRotation = PreviousRotation;
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

                public class Timer
                {
                    public double StartTime { get; private set; }
                    public double Duration { get; private set; }
                    public bool Active { get; private set; }

                    public void Start(double duration)
                    {
                        StartTime = Raylib.GetTime();
                        Duration = duration;
                        Active = true;
                    }

                    public bool IsDone()
                    {
                        if (!Active) return true;
                        return Raylib.GetTime() - StartTime >= Duration;
                    }

                    public void Reset()
                    {
                        Active = false;
                        StartTime = 0;
                        Duration = 0;
                    }
                }
            }

            private void DrawProjectiles()
            {
                foreach(var projectile in projectiles)
                {
                    Raylib.DrawCircleV(projectile.Position, 5f, Color.Yellow);
                }
            }

            private void DrawTank(Tank tank)
            {
                Raylib.DrawRectanglePro(tank.BaseOffLeft, tank.Origin, tank.Rotation, tank.TrackColor);
                Raylib.DrawRectanglePro(tank.BaseOffRight, tank.Origin, tank.Rotation, tank.TrackColor);
                Raylib.DrawRectanglePro(tank.Base, tank.Origin, tank.Rotation, tank.Color);

                Raylib.DrawLineEx(tank.Center, tank.BarrelEnd, WinWidth/150, Color.DarkGray);
                Raylib.DrawCircleV(tank.Center, tank.Base.Width / 3, tank.HeadBot);
                Raylib.DrawCircleV(tank.Center, tank.Base.Width / 4, tank.HeadTop);

                Vector2[] baseOffLeftPoints = GetRotatedRectanglePoints(tank.BaseOffLeft, tank.Origin, tank.Rotation);
                Vector2[] baseOffRightPoints = GetRotatedRectanglePoints(tank.BaseOffRight, tank.Origin, tank.Rotation);
                Vector2[] basePoints = GetRotatedRectanglePoints(tank.Base, tank.Origin, tank.Rotation);

                DrawPolygon(baseOffLeftPoints, Color.Red);
                DrawPolygon(baseOffRightPoints, Color.Red);
                DrawPolygon(basePoints, Color.Red);
            }

            private void DrawPolygon(Vector2[] points, Color color)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    Vector2 current = points[i];
                    Vector2 next = points[(i + 1) % points.Length];
                    Raylib.DrawLineV(current, next, color);
                }
            }

            public class Obstacles
            {
                public Rectangle Obs1rec { get; set; }
                public Rectangle Obs2rec { get; set; }
                public Color Color { get; set; }

                public Obstacles(Rectangle obs1rec, Rectangle obs2rec, Color color)
                {
                    Obs1rec = obs1rec;
                    Obs2rec = obs2rec;
                    Color = color;
                }
            }
            public void DrawObstacles(Obstacles obs) 
            {
                Raylib.DrawRectangleRec(obs.Obs1rec, obs.Color);
                Raylib.DrawRectangleRec(obs.Obs2rec, obs.Color);
            }
        }
    }
}