using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    internal class ShroomHat : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 25);
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 1;
            player.moveSpeed += 0.08f;
        }
    }
}
