using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Weapons.Ranged
{
    internal class SteelBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.autoReuse = true;
            Item.damage = 17;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 4;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 10;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item5;
            Item.value = Item.buyPrice(silver: 25);

            Item.useAmmo = AmmoID.Arrow; // Also determines which arrow projectile is fired
            Item.shoot = ProjectileID.WoodenArrowFriendly;
        }

        public override Vector2? HoldoutOffset() => new(-2, 0); // Used for alligning the sprite

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SteelBar>(), 9);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
