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

        ////////////// MUUTTUJAT ///////////////////////

        public int screen_width = 720;
        public int screen_height = 480;
        Color bg_color = Color.Black;
        
        Ship ship;

        // Laskeutumisalustan katon sijainti y-akselilla. Y kasvaa alaspäin

        // Ruudunpäivitykseen menevä aika

        // Painovoiman voimakkuus

        // Ikkunan leveys ja korkeus

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
            // Päivitä aluksen tilaa
            ship.Update();

            // Lisää painovoiman vaikutus


        }

        void Draw()
        {
            Raylib.BeginDrawing();

            Raylib.ClearBackground(bg_color);

            DrawLandingPlatform();

            // Piirrä alus
            ship.Draw();

            // Piirrä debug tietoja tarvittaessa, kuten nopeus

            //Lopeta piirtäminen
            Raylib.EndDrawing();
        }

        void DrawLandingPlatform()
        {
            float platformWidth = screen_width * 0.25f;
            float platformHeight = screen_height * 0.05f;
            Vector2 platformOrigin = new Vector2(platformWidth / 2, platformHeight / 2);
            Rectangle platformRec = new Rectangle(screen_width / 2, screen_height - platformHeight, (int)platformWidth, (int)platformHeight);
            Raylib.DrawRectanglePro(platformRec, platformOrigin, 0, Color.LightGray);
        }
    }
}