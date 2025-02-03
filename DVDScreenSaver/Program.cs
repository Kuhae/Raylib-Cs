using Raylib_cs;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography;

namespace DVDScreenSaver
{
    internal class Program
    {
        public static int width = 1280;
        public static int height = 720;

        public static string Txt = "DVD";
        public static int fs = 100;

        public static bool isSpeedNaN;

        public static float speed = 150.0f;
        public static bool DecreaseSpeed;

        public static Vector2 TxLc;
        public static Vector2 TxD = new(1, 1);

        public static int TxW;
        public static Raylib_cs.Color TxColor = Raylib_cs.Color.Blue;

        public static void Main()
        {
            var random = new Random();
            Raylib.InitWindow(width, height, "DVD");
            TxLc = new(random.Next(0, width), random.Next(0, height)); // Random aloitus pos

            while (!Raylib.WindowShouldClose())
            {
                TxW = Raylib.MeasureText(Txt, fs);
                TxLc += TxD * speed * Raylib.GetFrameTime();
                TxLc.X = Math.Clamp(TxLc.X, 0, Raylib.GetScreenWidth() - TxW);
                TxLc.Y = Math.Clamp(TxLc.Y, 0, Raylib.GetScreenHeight() - fs);

                if ((TxLc.X <= 0
                || TxLc.X + TxW >= Raylib.GetScreenWidth())
                || (TxLc.Y <= 0
                || TxLc.Y + fs >= Raylib.GetScreenHeight()))
                {
                    if (TxLc.X <= 0 || TxLc.X + TxW >= Raylib.GetScreenWidth())
                    { TxD.X *= -1.0f; }
                    if (TxLc.Y <= 0 || TxLc.Y + fs >= Raylib.GetScreenHeight())
                    { TxD.Y *= -1.0f; }
                    if (!DecreaseSpeed)
                    { speed *= 1.25f; if (speed >= float.MaxValue) DecreaseSpeed = true; }
                    else
                    { speed *= 0.75f; if (speed <= 150.0f) DecreaseSpeed = false; }
                    speed = Math.Clamp(speed, 150, float.MaxValue);
                    TxColor = ToggleColor(TxColor);
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib_cs.Color.Black);
                //////////////////////////////////////////////////////////////////////////////////
                Raylib.DrawText(Txt, (int)TxLc.X, (int)TxLc.Y, fs, TxColor);
                Raylib.DrawText($"X={TxLc.X.ToString()}", 0, 0, 20, Raylib_cs.Color.White);
                Raylib.DrawText($"Y={TxLc.Y.ToString()}", 0, 25, 20, Raylib_cs.Color.White);
                Raylib.DrawText($"Speed={speed.ToString()}", 0, 50, 20, Raylib_cs.Color.White);
                //////////////////////////////////////////////////////////////////////////////////
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        // Vaihtaa sinisen ja punaisen välillä
        public static Raylib_cs.Color ToggleColor(Raylib_cs.Color currentColor)
        {
            if
            (currentColor.R == Raylib_cs.Color.Blue.R &&
            currentColor.G == Raylib_cs.Color.Blue.G &&
            currentColor.B == Raylib_cs.Color.Blue.B &&
            currentColor.A == Raylib_cs.Color.Blue.A)
            { return Raylib_cs.Color.Red; }
            else
            { return Raylib_cs.Color.Blue; }
        }
    }
}