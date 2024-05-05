using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AssortedAdditions.Content.Projectiles;
using AssortedAdditions.Content.Items.Placeables.Ores;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Items.Tools
{
    internal class FrostFishingRod : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodFishingPole); // Most defaults can be copied
            Item.width = 46;
            Item.height = 42;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 3);
            Item.fishingPole = 50; // Fishing power
            Item.shootSpeed = 14f; // Speed at which the bobbers are launched.

            Item.shoot = ModContent.ProjectileType<FrostBobber>(); // Has an unique bobber
        }

		public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
		{
			lineOriginOffset = new Vector2(44, -40); // Where the line is drawn from
			lineColor = Color.LightGray; // Sets the fishing line's color.
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<FrostBar>(), 8);
            recipe.AddTile(TileID.IceMachine);
            recipe.Register();
        }
    }
}
