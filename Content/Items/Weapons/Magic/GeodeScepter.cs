using AssortedAdditions.Content.Projectiles.MagicProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.DataStructures;
using AssortedAdditions.Content.Items.Misc;

namespace AssortedAdditions.Content.Items.Weapons.Magic
{
	internal class GeodeScepter : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.staff[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 29;
			Item.height = 28;
			Item.damage = 13;
			Item.knockBack = 6f;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 14;
			Item.mana = 8;
			Item.scale = 1.75f;

			Item.autoReuse = true;
			Item.noMelee = true;

			Item.value = Item.sellPrice(silver: 70);
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.DamageType = DamageClass.Magic;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item88;
			Item.shoot = ModContent.ProjectileType<GeodeScepterProj>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(10);

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}

			return false;
		}

		// Draw item with increased scale when dropped
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = TextureAssets.Item[Type].Value; // Item's spritesheet

			Rectangle? source = null;
			if (Main.itemAnimations[Type] != null)
			{
				// The current frame of the animation, null check for items that have one frame
				source = Main.itemAnimations[Type].GetFrame(texture);
			}

			Main.spriteBatch.Draw(texture, new Vector2(Item.position.X, Item.position.Y - 17) - Main.screenPosition, 
				source, lightColor, 0, Vector2.Zero, scale * 1.5f, SpriteEffects.None, 0f);

			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Geode);
			recipe.AddIngredient(ModContent.ItemType<GraniteArmorShard>(), 4);
			recipe.AddIngredient(ItemID.StoneBlock, 30);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
