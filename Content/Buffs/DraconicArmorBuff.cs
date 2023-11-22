using AssortedAdditions.Common.Players;
using AssortedAdditions.Content.Items.Armor;
using AssortedAdditions.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Buffs
{
    internal class DraconicArmorBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // If not wearing the full set, remove the set bonus buff
            // There is probably a better way to do this but this works for now...
            // Tried making a bool in ArmorSetBuff (ModPlayer) and setting it to true inside the head pieces,
            // however, it did not work. The bool got reset to false before anything could happen and if resetEffects
            // was not used the buff would keep going when removing armor pieces
            if (player.armor[0].type != ModContent.ItemType<DraconicHelmet>()
                && player.armor[0].type != ModContent.ItemType<DraconicHood>()
                && player.armor[0].type != ModContent.ItemType<DraconicHat>()
                && player.armor[0].type != ModContent.ItemType<DraconicMask>()
                || player.armor[1].type != ModContent.ItemType<DraconicChestplate>()
                || player.armor[2].type != ModContent.ItemType<DraconicGreaves>())
            {
                player.GetModPlayer<ArmorSetBuffs>().DraconicArmorBuff = 0; // Reset timer so that new projectiles can spawn if armor gets put on again
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            // If player has the full set, keep buff going
            else
            {
                player.buffTime[buffIndex] = 18000;

                // Buff spawns five projectiles that circle around the player
                player.GetModPlayer<ArmorSetBuffs>().DraconicArmorBuff++;
                if (player.GetModPlayer<ArmorSetBuffs>().DraconicArmorBuff % 25 == 0
                    && player.GetModPlayer<ArmorSetBuffs>().DraconicArmorBuff <= 125)
                {
                    if (Main.myPlayer == player.whoAmI)
                    {
                        Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<DraconicArmorProj>(), 20, 4f, player.whoAmI);
                    }
                }

                // Add some lighting
                Lighting.AddLight(player.Center, TorchID.Orange);
            }
        }
    }
}
