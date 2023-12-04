using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.Tiles;

namespace AssortedAdditions.Content.Items.Placeables.Ores
{
    internal class FrostBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 58;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.value = Item.sellPrice(silver: 25);
            Item.rare = ItemRarityID.Orange;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.createTile = ModContent.TileType<FrostBarTile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Permafrost>(), 4);
            recipe.AddTile(TileID.IceMachine);
            recipe.Register();
        }
    }
}
