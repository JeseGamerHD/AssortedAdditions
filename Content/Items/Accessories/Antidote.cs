using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ModdingTutorial.Content.Items.Accessories
{
    internal class Antidote : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Rabies] = true;
        }
    }
}
