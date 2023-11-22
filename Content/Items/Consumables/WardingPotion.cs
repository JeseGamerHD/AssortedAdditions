using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.Items.Placeables.Ores;
using AssortedAdditions.Content.Buffs;

namespace AssortedAdditions.Content.Items.Consumables
{
    internal class WardingPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 20;

            // Dust that will appear in these colors when the item with ItemUseStyleID.DrinkLiquid is used
            ItemID.Sets.DrinkParticleColors[Type] = new Color[3] {
                new Color(82, 76, 71),
                new Color(71, 70, 69),
                new Color(117, 84, 52)
            };
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.maxStack = 9999;

            Item.consumable = true;
            Item.useTurn = true;

            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item3;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 6);
            Item.buffType = ModContent.BuffType<WardingPotionBuff>();
            Item.buffTime = 28800; // 8 minutes
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.ArmoredCavefish);
            recipe.AddIngredient(ModContent.ItemType<SteelBar>());
            recipe.AddTile(TileID.Bottles);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.BottledWater);
            recipe2.AddIngredient(ItemID.ArmoredCavefish);
            recipe2.AddIngredient(ModContent.ItemType<SteelBar>());
            recipe2.AddTile(TileID.AlchemyTable);
            recipe2.Register();
        }
    }
}
