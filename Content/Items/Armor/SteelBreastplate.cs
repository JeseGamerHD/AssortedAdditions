using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using ModdingTutorial.Content.Items.Placeables;

namespace ModdingTutorial.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    internal class SteelBreastplate : ModItem
    {
        public override void SetStaticDefaults() {
            
            Tooltip.SetDefault("A breastplate made out of steel");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 7;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SteelBar>(), 30);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
