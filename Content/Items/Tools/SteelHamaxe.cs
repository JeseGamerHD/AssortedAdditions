using AssortedAdditions.Content.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Tools
{
    internal class SteelHamaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 32;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.damage = 25;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(silver: 75);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 4;

            Item.axe = 14; // 5 * power = actual power
            Item.hammer = 70;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SteelBar>(), 25);
            recipe.AddRecipeGroup("Wood", 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
