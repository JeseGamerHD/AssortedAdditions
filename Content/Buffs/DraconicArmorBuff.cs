using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Items.Armor;
using ModdingTutorial.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Buffs
{
    internal class DraconicArmorBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
        }

        int timer;
        public override void Update(Player player, ref int buffIndex)
        {
            // If not wearing the full set, remove the set bonus buff
            // There is probably a better way to do this but this works for now
            if ((player.armor[0].type != ModContent.ItemType<DraconicHelmet>() 
                && player.armor[0].type != ModContent.ItemType<DraconicHood>() 
                && player.armor[0].type != ModContent.ItemType<DraconicHat>()  )
                || player.armor[1].type != ModContent.ItemType<DraconicChestplate>()
                || player.armor[2].type != ModContent.ItemType<DraconicGreaves>())
            {
                timer = 0; // Reset timer so that new projectiles can spawn if armor gets put on again
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            // If player has the full set, keep buff going
            else 
            {
                player.buffTime[buffIndex] = 18000;

                // Buff spawns five projectiles that circle around the player
                timer++;
                if(timer % 25 == 0 && timer <= 125) 
                {
                    Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<DraconicArmorProj>(), 20, 4f, player.whoAmI);
                }

                // Add some lighting
                Lighting.AddLight(player.Center, TorchID.Orange); 
            }
        }
    }
}
