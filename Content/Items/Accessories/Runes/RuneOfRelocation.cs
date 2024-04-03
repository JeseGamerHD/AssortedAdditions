using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AssortedAdditions.Common.Systems;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using AssortedAdditions.Content.Buffs;
using AssortedAdditions.Helpers;
using ReLogic.Content;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	internal class RuneOfRelocation : RuneItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(7, 8));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 46;
			Item.value = Item.sellPrice(gold: 4);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (CustomKeyBinds.RuneAbility.JustPressed && !player.HasBuff<RuneOfRelocationCooldown>())
			{
				AttemptToTeleportToMouse(player);
			}
		}

		private static void AttemptToTeleportToMouse(Player player)
		{
			Vector2 teleportHere = new(Main.MouseWorld.X, Main.MouseWorld.Y);

			int xCheck = (int)(teleportHere.X / 16f);
			int yCheck = (int)(teleportHere.Y / 16f);
			if ((Main.tile[xCheck, yCheck].WallType == WallID.LihzahrdBrickUnsafe && !NPC.downedPlantBoss && (Main.remixWorld || yCheck > Main.worldSurface)) || Collision.SolidCollision(teleportHere, player.width, player.height))
			{ // Disable teleporting into the jungle temple early or inside tiles
				return; 
			}

			player.Teleport(teleportHere, 1000); // TODO: better system for custom ids
			
			if (Main.CurrentFrameFlags.AnyActiveBossNPC) // Longer cooldown when fighting a boss
			{
				player.AddBuff(ModContent.BuffType<RuneOfRelocationCooldown>(), HelperMethods.SecondsToTicks(15));
			}
			else
			{
				player.AddBuff(ModContent.BuffType<RuneOfRelocationCooldown>(), HelperMethods.SecondsToTicks(5));
			}

			NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, teleportHere.X, teleportHere.Y, 1);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Insert(tooltips.FindLastIndex(tip => tip.Name.StartsWith("Tooltip")) + 1,
				new(Mod, "Tooltip3", "Press " + CustomKeyBinds.RuneAbility.GetAssignedKeys().FirstOrDefault("<Unbound>") + " to teleport to mouse position"));
		}

		// The rune icon be too big when dropped and Item.scale did not fix this
		// If the sprite's size is reduced then the icon is too small in the inventory so either way drawing would be required..
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Asset<Texture2D> texture = TextureAssets.Item[Item.type];

			Rectangle? source = null;
			if (Main.itemAnimations[Type] != null)
			{
				// The current frame of the animation, null check for items that have one frame
				source = Main.itemAnimations[Type].GetFrame(texture.Value);
			}

			// Draw item with 0.5 scale
			Main.spriteBatch.Draw(texture.Value, Item.position - Main.screenPosition, source, lightColor, 0, Vector2.Zero, scale * 0.5f, SpriteEffects.None, 0f);

			return false;
		}
	}

	public class RuneOfRelocationDetour : ModSystem
	{
		public override void Load()
		{
			Terraria.On_Main.TeleportEffect += CustomTeleportEffect;
		}

		private static void CustomTeleportEffect(On_Main.orig_TeleportEffect orig, Rectangle effectRect, int Style, int extraInfo, float dustCountMult, TeleportationSide side, Vector2 otherPosition)
		{
			orig(effectRect, Style, extraInfo, dustCountMult, side, otherPosition);

			if (Style == 1000) // TODO: better system for custom ids
			{
				Vector2 position = new Vector2(effectRect.X, effectRect.Y);
				for (int i = 0; i < 60; i++)
				{
					Dust dust = Dust.NewDustDirect(position, effectRect.Width, effectRect.Height, DustID.JungleTorch, 0, 0, 75, default, 2f);
					dust.velocity *= 1.4f;
				}

				SoundStyle sound = new SoundStyle("AssortedAdditions/Assets/Sounds/Misc/Teleportation");
				sound = sound with { Volume = 1.5f };
				SoundEngine.PlaySound(sound, position);
			}
		}
	}
}
