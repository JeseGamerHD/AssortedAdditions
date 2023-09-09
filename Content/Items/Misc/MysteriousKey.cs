using Microsoft.Xna.Framework;
using ModdingTutorial.Common.Players;
using ModdingTutorial.Common.Systems;
using ModdingTutorial.Content.Tiles.CraftingStations;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Misc
{
    internal class MysteriousKey : ModItem
    {
        public override void SetStaticDefaults()
        {

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GoldenKey);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item8;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool? UseItem(Player player)
        {
            if(Main.myPlayer == player.whoAmI)
            {
                // Remember to multiply by 16
                Vector2 roomPos = new((Main.dungeonX + 3) * 16, (Main.dungeonY + 16) * 16);
                player.Teleport(roomPos, TeleportationStyleID.TeleportationPotion);

                // After this the wizard will begin to sell items from the mysterious chest
                // Otherwise items would be obtainable only once per world
                ModContent.GetInstance<ItemFlags>().mysteriousKeyWasUsed = true;
            }

            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SoulofMight, 15);
            recipe.AddIngredient(ItemID.SoulofSight, 15);
            recipe.AddIngredient(ItemID.SoulofFright, 15);
            recipe.AddIngredient(ItemID.ShadowKey, 1);
            recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            recipe.Register();
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.Purple.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }
    }
}
