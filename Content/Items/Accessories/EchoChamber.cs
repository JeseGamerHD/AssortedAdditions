using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Projectiles;
using ModdingTutorial.Content.Buffs;
using ModdingTutorial.Common.Players;

namespace ModdingTutorial.Content.Items.Accessories
{
    internal class EchoChamber : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.buffType = ModContent.BuffType<EchoChamberBuff>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Set a flag that detects when the accessory is being worn
            player.GetModPlayer<AccessoryFlags>().isWearingEchoChamber = true;

            // If cooldown is active, no can be buff active
            if (!player.HasBuff(ModContent.BuffType<EchoChamberDebuff>()))
            {
                player.AddBuff(Item.buffType, 2); // This keeps the buff alive

                // Spawn only one shield
                if (player.ownedProjectileCounts[ModContent.ProjectileType<EchoChamberProj>()] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<EchoChamberProj>(), 0, 0, player.whoAmI);
                }
                // The if statement spawning the shield needs to be inside the above if statement since otherwise the game would attempt to spawn the projectile constantly
                // creating weird issues with other projectiles. Dumb mistake but now its noted. 
            }
        }
    }
}
