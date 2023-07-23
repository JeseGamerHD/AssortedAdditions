using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using ModdingTutorial.Content.Items.Placeables.Ores;

namespace ModdingTutorial.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    internal class SteelGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Greaves made out of steel");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(silver: 75);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SteelBar>(), 25);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
