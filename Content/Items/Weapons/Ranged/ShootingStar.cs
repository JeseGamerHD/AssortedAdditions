using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Projectiles.RangedProj;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
    internal class ShootingStar : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 76;
            Item.damage = 70;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.knockBack = 6f;
            Item.shootSpeed = 12;

            Item.noMelee = true;
            Item.autoReuse = true;

            Item.rare = ItemRarityID.Lime;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item9;
            Item.value = Item.sellPrice(gold: 10);
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ModContent.ProjectileType<ShootingStarProj>();
        }

        public override Vector2? HoldoutOffset() => new(-10, 2); // Alligns the sprite properly

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Always shoots custom projectile no matter which arrow was used
            type = ModContent.ProjectileType<ShootingStarProj>();
        }
    }
}
