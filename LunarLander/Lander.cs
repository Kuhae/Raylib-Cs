using Raylib_cs;

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

        int screen_width = 720;
        int screen_height = 480;
        Color bg_color = Color.Yellow;

        // Laskeutumisalustan katon sijainti y-akselilla. Y kasvaa alaspäin

        // Ruudunpäivitykseen menevä aika

        // Painovoiman voimakkuus

        // Ikkunan leveys ja korkeus

        void Init()
        {
            Raylib.InitWindow(screen_width, screen_height, "Lunar Lander");
        }

        void GameLoop()
        {
            // Pyöritä gamelooppia niin kauan
            // kun Raylibin ikkuna on auki
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
            // Kysy Raylibiltä miten pitkään yksi ruudunpäivitys kesti
            float frameTime = Raylib.GetFrameTime();
            // Päivitä aluksen tilaa

            // Lisää painovoiman vaikutus


        }

        void Draw()
        {
            // Aloita piirtäminen
            Raylib.BeginDrawing();

            // Tyhjennä ruutu tausta väriksi
            Raylib.ClearBackground(bg_color);

            // Piirrä laskeutumisalusta suorakulmiona

            // Piirrä alus

            // Piirrä debug tietoja tarvittaessa, kuten nopeus

            //Lopeta piirtäminen
            Raylib.EndDrawing();
        }
    }
}