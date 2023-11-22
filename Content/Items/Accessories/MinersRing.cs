using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace AssortedAdditions.Content.Items.Accessories
{
    internal class MinersRing : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.pickSpeed += 0.25f; // Increases mining speed by 25%
        }
    }
}
