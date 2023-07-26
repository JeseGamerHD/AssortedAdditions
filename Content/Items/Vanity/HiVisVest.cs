using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ModdingTutorial.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    internal class HiVisVest : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 25);
            Item.vanity = true;
        }
    }
}
