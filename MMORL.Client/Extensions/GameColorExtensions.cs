using Microsoft.Xna.Framework;
using MMORL.Client.Util;
using MMORL.Shared.Util;
using System;

namespace MMORL.Client.Extensions
{
    public static class GameColorExtensions
    {
        public static Color ParseColor(this GameColor color)
        {
            switch (color)
            {
                case GameColor.Dark: return Swatch.Dark;
                case GameColor.OldBlood: return Swatch.OldBlood;
                case GameColor.DeepWater: return Swatch.DeepWater;
                case GameColor.OldStone: return Swatch.OldStone;
                case GameColor.Wood: return Swatch.Wood;
                case GameColor.Vegetation: return Swatch.Vegetation;
                case GameColor.Blood: return Swatch.Blood;
                case GameColor.Stone: return Swatch.Stone;
                case GameColor.Water: return Swatch.Water;
                case GameColor.BrightWood: return Swatch.BrightWood;
                case GameColor.Metal: return Swatch.Metal;
                case GameColor.Grass: return Swatch.Grass;
                case GameColor.Skin: return Swatch.Skin;
                case GameColor.Sky: return Swatch.Sky;
                case GameColor.Sun: return Swatch.Sun;
                case GameColor.Light: return Swatch.Light;
                default:
                    throw new InvalidOperationException($"Unknown color: {color}");
            }
        }
    }
}
