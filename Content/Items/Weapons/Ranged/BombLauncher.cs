using ModdingTutorial.Content.Projectiles.RangedProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Items.Tools;

namespace ModdingTutorial.Content.Items.Weapons.Ranged
{
    internal class BombLauncher : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 32;
            Item.damage = 36;
            Item.knockBack = 8f;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.shootSpeed = 11f;
            Item.crit = 4;

            Item.noMelee = true;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item61;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<BombLauncherProj>();
        }

        public override Vector2? HoldoutOffset() => new(-10, 0); // Alligns the sprite properly

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset; // Projectiles come out of the muzzle properly using this
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SnowballCannon)
                .AddIngredient(ItemID.IllegalGunParts)
                .AddIngredient(ModContent.ItemType<BagOfBombs>())
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
