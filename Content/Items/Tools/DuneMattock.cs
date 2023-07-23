using ModdingTutorial.Content.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Tools
{
    internal class DuneMattock : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Can be used for mining and chopping wood");
        }
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.damage = 14;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(silver: 54);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 4;

            Item.pick = 100;
            Item.axe = 30;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DuneBar>(), 12);
            recipe.AddIngredient(ItemID.AntlionMandible, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
