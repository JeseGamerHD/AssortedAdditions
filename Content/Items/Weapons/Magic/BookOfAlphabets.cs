using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Tiles.CraftingStations;
using AssortedAdditions.Content.Projectiles.MagicProj;

namespace AssortedAdditions.Content.Items.Weapons.Magic
{
    internal class BookOfAlphabets : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.damage = 45;
            Item.knockBack = 4f;
            Item.shootSpeed = 6;
            Item.mana = 6;

            Item.noMelee = true; // Don't want the book to deal damage
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.UseSound = SoundID.Item17;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 4);
            Item.shoot = ModContent.ProjectileType<BookOfAlphabetsProj>();
        }

        // Makes the projectiles have a spread
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));
        }

        public override void AddRecipes()
        {
            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.SpellTome, 1);
            recipe2.AddIngredient(ItemID.Book, 25);
            recipe2.AddIngredient(ItemID.PixieDust, 5);
            recipe2.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            recipe2.Register();
        }
    }
}
