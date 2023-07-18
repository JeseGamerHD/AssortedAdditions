using ModdingTutorial.Content.Projectiles.MeleeProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ModdingTutorial.Content.Items.Weapons.Melee
{
    internal class DesertsFury : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 55;
            Item.height = 55;
            Item.useAnimation = 23;
            Item.useTime = 23;
            Item.value = Item.sellPrice(gold: 5);

            Item.DamageType = DamageClass.Melee;
            Item.damage = 48;
            Item.knockBack = 5f;
            Item.noMelee = true;

            Item.noUseGraphic = true; // Spawns a projectile that spins so hide item
            Item.channel = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Pink;

            Item.shoot = ModContent.ProjectileType<DesertsFuryProj>();
            Item.shootSpeed = 5f;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }
}
