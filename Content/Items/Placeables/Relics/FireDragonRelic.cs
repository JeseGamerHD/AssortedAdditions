using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.Tiles.Relics;

namespace AssortedAdditions.Content.Items.Placeables.Relics
{
    internal class FireDragonRelic : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 48;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Master;
            Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
            Item.value = Item.sellPrice(gold: 5);

            Item.DefaultToPlaceableTile(ModContent.TileType<FireDragonRelicTile>(), 0);
        }
    }
}
