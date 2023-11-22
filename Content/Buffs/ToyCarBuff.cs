﻿using AssortedAdditions.Content.Projectiles.PetProj;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Buffs
{
    internal class ToyCarBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        { // This method gets called every frame your buff is active on your player.
            bool unused = false;
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<ToyCar>());
        }
    }
}
