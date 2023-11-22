using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;

namespace AssortedAdditions.Content.Items.Misc
{
    internal class DragonScale : ModItem // Dropped from Fire Dragon boss
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(silver: 40);
        }
    }
}
