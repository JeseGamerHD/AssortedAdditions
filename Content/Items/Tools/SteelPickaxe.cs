using ModdingTutorial.Content.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Tools
{
    internal class SteelPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Can mine Meteorite");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.damage = 14;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(silver: 45);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 4;

            Item.pick = 63;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SteelBar>(), 12);
            recipe.AddRecipeGroup("Wood", 4);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
