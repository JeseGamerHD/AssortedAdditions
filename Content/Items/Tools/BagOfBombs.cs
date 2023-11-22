using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Tools
{
    internal class BagOfBombs : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 36;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.shootSpeed = 5f;

            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.shoot = ProjectileID.Bomb;
        }
    }
}
