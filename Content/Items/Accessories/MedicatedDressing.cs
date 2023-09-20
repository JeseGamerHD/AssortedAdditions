using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Accessories
{
    internal class MedicatedDressing : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Dressing>());
            recipe.AddIngredient(ItemID.Bezoar);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
