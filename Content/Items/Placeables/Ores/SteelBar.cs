using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Placeables.Ores
{
    internal class SteelBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 60;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.value = Item.buyPrice(silver: 6);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;

            Item.createTile = ModContent.TileType<Tiles.SteelBar>();
            Item.placeStyle = 0;

        }

        // 2 recipes because lead sometimes replaces iron
        // Could have used recipegroup "IronBar"
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronOre, 3);
            recipe.AddIngredient(ModContent.ItemType<CoalChunk>(), 2);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();

            Recipe recipe_alt = CreateRecipe();
            recipe_alt.AddIngredient(ItemID.LeadOre, 3);
            recipe_alt.AddIngredient(ModContent.ItemType<CoalChunk>(), 2);
            recipe_alt.AddTile(TileID.Furnaces);
            recipe_alt.Register();
        }
    }
}
