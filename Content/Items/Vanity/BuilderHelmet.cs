using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AssortedAdditions.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    internal class BuilderHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 18;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 25);
            Item.vanity = true;
        }
    }
}
