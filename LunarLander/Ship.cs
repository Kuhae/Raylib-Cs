using Raylib_cs;
using System.Numerics;

namespace LunarLander
{
    internal class Ship
    {
        // MUUTTUJAT: 
        // Pelaajan sijainti
        Vector2 position;
        Vector2 velocity;
        // Onko moottori päällä ?
        bool engineRunning;
        // Moottorin voimakkuus
        float engineAcceleration;
        // Pelaajan nopeus, polttoaine ja kuinka nopeasti polttoaine kuluu
        float shipSpeed;
        float engineFuel = 10000; // Fueltank capacity, start with 10 000 Litres of fuel 
        float engineFuelConsumption; // Consumption per second
        void Update()
        {
            // Kun pelaaja painaa nappia (esim nuoli ylös)
            // ja polttoainetta on jäljellä, lisää
            // kiihtyvyys nopeuteen

            // Liikuta alusta
        }

        void Draw()
        {
            // Piirrä alus: käytä kolmiota tai muita muotoja

            // Piirrä moottorin liekki

            // Piirrä polttoaineen tilanne
        }
    }
}