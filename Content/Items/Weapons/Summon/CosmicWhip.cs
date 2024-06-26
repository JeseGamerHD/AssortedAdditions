﻿using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.Projectiles.SummonProj;

namespace AssortedAdditions.Content.Items.Weapons.Summon
{
    internal class CosmicWhip : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<CosmicWhipProj>(), 90, 5, 10);
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Lime;
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}
