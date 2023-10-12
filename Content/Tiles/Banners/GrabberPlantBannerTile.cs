﻿using Microsoft.Xna.Framework;
using ModdingTutorial.Content.NPCs;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Tiles.Banners
{
    internal class GrabberPlantBannerTile : MonsterBanners
    {
        public override void SetStaticDefaults()
        {
            Color = Color.Green;
            BuffNPC = ModContent.NPCType<GrabberPlant>();
            base.SetStaticDefaults();
        }
    }
}
