using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.Audio;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Tiles.CraftingStations;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	internal class RuneOfMovement : RuneItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(30, 2));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation
		}

		public override void SetDefaults()
		{
			Item.width = 28; 
			Item.height = 32;
			Item.value = Item.sellPrice(gold: 1, silver: 22);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RuneOfMovementPlayer>().isWearingRuneOfMovement = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BlankRune>());
			recipe.AddIngredient(ItemID.EoCShield);
			recipe.AddIngredient(ItemID.SoulofFlight, 5);
			recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
			recipe.Register();
		}

		// The rune icon would be too big when dropped and Item.scale did not fix this
		// If the sprite's size is reduced then the icon is too small in the inventory so either way drawing would be required..
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = TextureAssets.Item[Type].Value; // Item's spritesheet

			Rectangle? source = null;
			if (Main.itemAnimations[Type] != null)
			{
				// The current frame of the animation, null check for items that have one frame
				source = Main.itemAnimations[Type].GetFrame(texture);
			}

			// Draw item with 0.5 scale
			Main.spriteBatch.Draw(texture, Item.position - Main.screenPosition, source, lightColor, 0, Vector2.Zero, scale * 0.5f, SpriteEffects.None, 0f);

			return false;
		}
	}

	public class RuneOfMovementPlayer : ModPlayer
	{
		public bool isWearingRuneOfMovement; // Flag for accessory

		private const int DashRight = 0;
		private const int DashLeft = 1;
		private const int DashUp = 2;
		private const int DashDown = 3;

		private const int DashCooldown = 70; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
		private const int DashDuration = 35; // Duration of the dash afterimage effect in frames

		private const float DashVelocity = 11f; // The initial velocity.
		private int DashDir = -1; // The direction the player has double tapped. Defaults to -1 for no dash double tap
		private int DashDelay = 0; // frames remaining till we can dash again
		private int DashTimer = 0; // frames remaining in the dash

		public override void PreUpdateMovement()
		{
			if (CanDash() && DashDir != -1 && DashDelay == 0)
			{
				Vector2 newVelocity = Player.velocity;
				float dashDirection;

				// Adjust velocity accordingly depending on if the player is moving left or right
				// or up/down
				if (DashDir == 0 && Player.velocity.X < DashVelocity) // Right
				{
					dashDirection = DashDir == DashRight ? 1 : -1;
					newVelocity.X = dashDirection * DashVelocity;
				}
				else if (DashDir == 1 && Player.velocity.X > -DashVelocity) // Left
				{
					dashDirection = DashDir == DashLeft ? -1 : 1;
					newVelocity.X = dashDirection * DashVelocity;
				}
				else if (DashDir == 2 && Player.velocity.Y > -DashVelocity) // Up
				{
					dashDirection = DashDir == DashUp ? -1 : 1.3f; // 1.3 to account for gravity
					newVelocity.Y = dashDirection * DashVelocity;
				}
				else if (DashDir == 3 && Player.velocity.Y < DashVelocity) // Down
				{
					dashDirection = DashDir == DashDown ? 1 : -1.3f;
					newVelocity.Y = dashDirection * DashVelocity;
				}
				else
				{
					return; // Else cant dash
				}

				DashDelay = DashCooldown;
				DashTimer = DashDuration;
				Player.velocity = newVelocity;

				// Play a sound effect
				SoundStyle dashSound = new SoundStyle("AssortedAdditions/Assets/Sounds/Misc/MimicsTongueDash");
				SoundEngine.PlaySound(dashSound, Player.position);
			}

			if (DashDelay > 0)
			{
				DashDelay--;
			}

			if (DashTimer > 0)
			{
				// TODO figure out how to sync the visual effect
				Player.eocDash = DashTimer;
				Player.armorEffectDrawShadowEOCShield = true;
				DashTimer--;

				if (Main.rand.NextBool())
				{
					Dust dust = Dust.NewDustDirect(Player.position - Player.velocity, Player.width, Player.height, 307);
					dust.noGravity = true;
				}
			}
		}

		private bool CanDash()
		{
			return isWearingRuneOfMovement
				&& Player.dashType == 0
				&& !Player.setSolar
				&& !Player.mount.Active;
		}

		public override void ResetEffects()
		{
			isWearingRuneOfMovement = false;

			// [0] = double tap down
			// [1] = double tap up
			// [2] = double tap right
			// [3] = double tap left

			if (Main.myPlayer == Player.whoAmI)
			{
				if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[2] < 15)
				{
					DashDir = DashRight;
				}
				else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[3] < 15)
				{
					DashDir = DashLeft;
				}
				else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[1] < 15)
				{
					DashDir = DashUp;
				}
				else if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[0] < 15)
				{
					DashDir = DashDown;
				}
				else
				{
					DashDir = -1; // No dash
				}
			}
		}
	}
}
