using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    internal class SporeNecklace : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 6);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SporeNecklacePlayer>().isWearingSporeNecklace = true;
        }
    }

    internal class SporeNecklacePlayer : ModPlayer
    {
        public bool isWearingSporeNecklace;

        public override void OnHurt(Player.HurtInfo info)
        {
            if(isWearingSporeNecklace && Player.whoAmI == Main.myPlayer)
            {
                if (Player.ownedProjectileCounts[ModContent.ProjectileType<SporeNecklaceProj>()] < 25)
                {
                    // Spawn 5 projectiles with random upwards velocity
                    for(int i = 0; i <= 5; i++)
                    {
                        Vector2 launchVelocity = new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-4, -2));
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, launchVelocity, ModContent.ProjectileType<SporeNecklaceProj>(), 45, 2, Main.myPlayer);
                    }
                }
            }
        }

        public override void ResetEffects()
        {
            isWearingSporeNecklace = false;
        }
    }
}
