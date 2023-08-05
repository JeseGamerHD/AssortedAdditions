using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace ModdingTutorial.Content.Items.Misc
{
    internal class ControlChip : ModItem // Sold by Cyborg after plantera, used for guided missile launcher
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Used for manufacturing weapons");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Lime;
        }
    }
}
