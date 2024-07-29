using AssortedAdditions.Content.Projectiles.MagicProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Items.Placeables.Ores;

namespace AssortedAdditions.Content.Items.Weapons.Magic
{
	internal class AncientGauntlet : ModItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 2));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.damage = 80;
			Item.knockBack = 4f;
			Item.shootSpeed = 14f;
			Item.mana = 5;

			Item.autoReuse = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			Item.DamageType = DamageClass.Magic;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = SoundID.Item34;
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(gold: 8);
			Item.shoot = ModContent.ProjectileType<AncientGauntletProj>();
		}
	
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float radiusX = (Main.screenWidth / 2) - 20f;
			float radiusY = (Main.screenHeight / 2) - 10f;
			float rotation = Main.rand.NextFloat(0, 6.28318531f); 

			Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY); // Target is the mouse
			Vector2 spawnPosition = Main.MouseWorld + new Vector2(radiusX, radiusY).RotatedBy(rotation); // Spawn in a random position along a circular shape
			Vector2 heading = target - spawnPosition;

			heading.Normalize();
			heading *= velocity.Length();

			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(spawnPosition, 32, 32, DustID.FlameBurst);
				dust.noGravity = true;
				dust.velocity *= 1.4f;
			}

			Projectile.NewProjectile(source, spawnPosition, heading, type, damage, knockback, player.whoAmI);

			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarTabletFragment, 6);
			recipe.AddIngredient(ModContent.ItemType<CoalChunk>(), 6);
			recipe.AddIngredient(ItemID.MeteoriteBar, 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	public class AncientGauntletPlayer : ModPlayer
	{
		public override void PostUpdateMiscEffects()
		{
			// This is how the dust effect in the Player's hand when holding Nebula Blaze is created:
			if(Player.inventory[Player.selectedItem].type == ModContent.ItemType<AncientGauntlet>() && Player.HandPosition.HasValue)
			{
				Vector2 handPos = Player.HandPosition.Value - Player.velocity;
				for (int i = 0; i < 4; i++)
				{
					Dust dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.Flare, Player.direction * 2, 0f, 150, default, 1f);
					dust.position = handPos;
					dust.velocity *= 0f;
					dust.noGravity = true;
					dust.fadeIn = 1f;
					dust.velocity += Player.velocity;
					
					if (Main.rand.NextBool(2))
					{
						dust.position += Utils.RandomVector2(Main.rand, -4f, 4f);
						dust.scale += Main.rand.NextFloat();
						if (Main.rand.NextBool(2))
						{
							dust.customData = this;
						}
					}
				}
			}
		}
	}
}
