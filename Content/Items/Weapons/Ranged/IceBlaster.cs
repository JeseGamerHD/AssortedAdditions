using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
    internal class IceBlaster : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 28;
            Item.scale = 0.90f;
            Item.damage = 44;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.shootSpeed = 15;

            Item.noMelee = true;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item12;
            Item.value = Item.sellPrice(gold: 4);
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.PurificationPowder; // Why this? No clue... 
        }

        public override Vector2? HoldoutOffset() => new(-2, 3); // Used for alligning the sprite

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset; // Projectiles come out of the muzzle properly using this
            }

            type = ProjectileID.CrystalBullet; // Converts all bullets into Crystal Bullets
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddIngredient(ItemID.PhoenixBlaster, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
