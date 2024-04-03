using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AssortedAdditions.Content.Tiles.CraftingStations;
using AssortedAdditions.Content.Items.Misc;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using AssortedAdditions.Common.Systems;
using System.Collections.Generic;
using System.Linq;
using AssortedAdditions.Content.Buffs;
using AssortedAdditions.Helpers;
using Terraria.Audio;
using ReLogic.Content;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	internal class RuneOfHealth : RuneItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 1;
			Item.scale = 0.5f;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RuneOfHealthPlayer>().isWearingRuneOfHealth = true;

			// The player can heal, activates a cooldown
			if (CustomKeyBinds.RuneAbility.JustPressed && !player.HasBuff<RuneOfHealthCooldown>())
			{
				player.Heal(300);
				player.AddBuff(ModContent.BuffType<RuneOfHealthCooldown>(), HelperMethods.MinutesToTicks(2));

				SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact, player.position);
				for (int i = 0; i < 10; i++)
				{
					Gore.NewGoreDirect(player.GetSource_Accessory(Item), player.position, new Vector2(Main.rand.Next(-3, 4), Main.rand.Next(-2, 3)), 331);
				}
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Insert(tooltips.FindLastIndex(tip => tip.Name.StartsWith("Tooltip")) + 1,
				new(Mod, "Tooltip3", "Press " + CustomKeyBinds.RuneAbility.GetAssignedKeys().FirstOrDefault("<Unbound>") + " to heal"));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BlankRune>());
			recipe.AddIngredient(ModContent.ItemType<MagicEssence>(), 10);
			recipe.AddIngredient(ItemID.LifeCrystal, 3);
			recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
			recipe.Register();
		}

		// The rune icon would be too big when dropped and Item.scale did not fix this
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

	public class RuneOfHealthNPC : GlobalNPC
	{
		public override void OnKill(NPC npc)
		{
			Player player = Main.player[npc.lastInteraction];
			if (player.GetModPlayer<RuneOfHealthPlayer>().isWearingRuneOfHealth)
			{
				int heartDrop = 0;

				// 8.3% chance, now the rate is basically doubled since vanilla chance is 8.3% as well
				// only drop when health is not full as well
				if (Main.rand.NextBool(12) && player.statLife != player.statLifeMax2) 
				{
					heartDrop = Item.NewItem(npc.GetSource_DropAsItem(), npc.getRect(), ItemID.Heart);
				}
				
				if(Main.netMode == NetmodeID.MultiplayerClient && heartDrop >= 0)
				{
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, heartDrop, 1f);
				}
			}
		}
	}

	public class RuneOfHealthPlayer : ModPlayer
	{
		public bool isWearingRuneOfHealth;

		public override void ResetEffects()
		{
			isWearingRuneOfHealth = false;
		}
	}
}
