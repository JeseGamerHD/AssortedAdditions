using AssortedAdditions.Content.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Weapons.Melee
{
    internal class SteelSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.damage = 35;
            Item.knockBack = 6;
            Item.crit = 2;

            Item.autoReuse = true;
            // Shoot is set so the player will change direction based on their cursors location when swinging
            // similar to Excalibur or swords that shoot projectiles
            Item.ChangePlayerDirectionOnShoot = true;
            Item.shoot = ProjectileID.PurificationPowder;

            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false; // False so it wont actually shoot anything
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SteelBar>(), 15);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
