using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Projectiles.RangedProj;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
    internal class EyeBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 36;
            Item.damage = 18;
            Item.knockBack = 4;
            Item.shootSpeed = 6;
            Item.useTime = 25;
            Item.useAnimation = 25;

            Item.noMelee = true;
            Item.autoReuse = true;

            Item.rare = ItemRarityID.Green;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item5;
            Item.value = Item.sellPrice(gold: 1);
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ModContent.ProjectileType<EyeBowProj>();
        }

        public override Vector2? HoldoutOffset() => new(-0.25f, 0); // Alligns the sprite properly

        // Always shoots custom projectile, without this arrow would determine the projectile
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<EyeBowProj>();
        }
    }
}
