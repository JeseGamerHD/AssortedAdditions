using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ModdingTutorial.Content.Tiles;

namespace ModdingTutorial.Content.Items.Placeables.Ores
{
    internal class Permafrost : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
            ItemID.Sets.SortingPriorityMaterials[Type] = 58;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.useAnimation = 15;
            Item.useTime = 10;

            Item.consumable = true;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(silver: 15);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.createTile = ModContent.TileType<PermafrostTile>();
        }
    }
}
