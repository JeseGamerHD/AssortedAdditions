using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ModdingTutorial.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Legs)]
    internal class BuilderPants : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 25);
            Item.vanity = true;
        }
    }
}
