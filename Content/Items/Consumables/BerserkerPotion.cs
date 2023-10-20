using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ModdingTutorial.Content.Items.Consumables
{
    internal class BerserkerPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 20;

            // Dust that will appear in these colors when the item with ItemUseStyleID.DrinkLiquid is used
            ItemID.Sets.DrinkParticleColors[Type] = new Color[3] {
                new Color(212, 84, 38),
                new Color(227, 97, 77),
                new Color(242, 70, 44)
            };
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.maxStack = 9999;

            Item.consumable = true;
            Item.useTurn = true;

            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item3;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 6);
            Item.buffType = ModContent.BuffType<BerserkerBuff>();
            Item.buffTime = 18000; // 5 minutes
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.Mushroom);
            recipe.AddIngredient(ItemID.Blinkroot);
            recipe.AddIngredient(ItemID.NeonTetra);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.BottledWater);
            recipe2.AddIngredient(ItemID.Mushroom);
            recipe2.AddIngredient(ItemID.Blinkroot);
            recipe2.AddIngredient(ItemID.NeonTetra);
            recipe2.AddTile(TileID.AlchemyTable);
            recipe2.Register();
        }
    }
}
