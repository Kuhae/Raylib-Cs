using Raylib_cs;
using System.Numerics;

namespace LunarLander
{
    internal class Lander
    {
        static void Main(string[] args)
        {
            Lander game = new Lander();
            game.Init();
            game.GameLoop();
        }

        Ship ship;
        public int screen_width = 720;
        public int screen_height = 480;
        Color bg_color = Color.Black;        
        bool LandingTextBool;
        string LandingText;
        Color LandingTextColor;
        Rectangle platformRec;
        Rectangle platformColRec;
        int PlatformTopY;

        void Init()
        {
            Raylib.InitWindow(screen_width, screen_height, "Lunar Lander");
            ship = new Ship();
            ship.screen_width = screen_width;
            ship.screen_height = screen_height;
        }

        void GameLoop()
        {
            ship.Init();

            while (Raylib.WindowShouldClose() == false)
            {
                Update();
                Draw();
            }

            // Sulje Raylib ikkuna
            Raylib.CloseWindow();
        }


        void Update()
        {
            if (!ship.shipHasLanded)
            {
                ship.Update();
                Collision();
            }
        }

        void Collision()
        {
            Vector2 velocity = ship.velocity;

            if (Raylib.CheckCollisionPointRec(ship.shipBottom, platformColRec) && !ship.shipHasLanded)
            {
                ship.shipHasLanded = true;
                ship.velocity = new Vector2(0, 0);

                if (velocity.Y > 20)
                {
                    LandingText = "Landing Failed!";
                    LandingTextColor = Color.Red;
                }
                else
                {
                    LandingText = "Landing Successful!";
                    LandingTextColor = Color.Green;
                }

                LandingTextBool = true;
            }
        }

        void Draw()
        {
            Raylib.BeginDrawing();

            Raylib.ClearBackground(bg_color);

            ship.Draw();
            DrawLandingPlatform();
            if (LandingTextBool)
            {
                float textHeight = screen_width * 0.05f;
                float textWidth = Raylib.MeasureText(LandingText, (int)textHeight);
                Raylib.DrawText(LandingText, (screen_width - (int)textWidth) / 2, screen_height / 2, (int)textHeight, LandingTextColor);
            }

            Raylib.EndDrawing();
        }

        void DrawLandingPlatform()
        {
            float platformWidth = screen_width * 0.25f;
            float platformHeight = screen_height * 0.05f;
            PlatformTopY = screen_height - (int)platformHeight;
            Vector2 platformOrigin = new Vector2(platformWidth / 2, platformHeight / 2);
            platformRec = new Rectangle(screen_width / 2, screen_height - platformHeight / 2, (int)platformWidth, (int)platformHeight);
            platformColRec = new Rectangle(0, PlatformTopY, screen_width, platformHeight);
            Raylib.DrawRectanglePro(platformRec, platformOrigin, 0, Color.LightGray);
        }
    }
}