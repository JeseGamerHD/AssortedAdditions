using AssortedAdditions.Content.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
    internal class SteelBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 34;
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

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SteelBar>(), 9);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
