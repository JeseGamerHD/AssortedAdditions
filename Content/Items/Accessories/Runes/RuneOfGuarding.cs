using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedAdditions.Common.Systems;
using AssortedAdditions.Content.Projectiles.SummonProj;
using Terraria.Audio;
using System.Collections.Generic;
using System.Linq;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	internal class RuneOfGuarding : RuneItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 7));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 30;
			Item.value = Item.sellPrice(gold: 6);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RuneOfGuardingPlayer>().isWearingRuneOfGuarding = true;
			player.maxMinions += 2;

			// Spawn the sentry if one is not already active
			if (CustomKeyBinds.RuneAbility.JustPressed)
			{
				if (player.ownedProjectileCounts[ModContent.ProjectileType<RuneSentryController>()] < 1 && Main.myPlayer == player.whoAmI)
				{
					Projectile.NewProjectile(player.GetSource_Accessory(Item), new Vector2(player.position.X, player.position.Y - 20),
						Vector2.Zero, ModContent.ProjectileType<RuneSentryController>(), 50, 4, player.whoAmI);

					for (int i = 0; i < 30; i++)
					{
						Vector2 speed = Main.rand.NextVector2CircularEdge(1.5f, 1.5f); // Creates a circle of dust
						Dust dust = Dust.NewDustPerfect(player.Center, DustID.IchorTorch, speed * 6f, 75, default, 2f);
						dust.noGravity = true;
					}

					SoundEngine.PlaySound(SoundID.Item82, player.position);
				}
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips); // Call base so RuneItem can replace the "Equipable" tip with a custom one

			// Unsure if this is the best way to do this, but I couldn't get replacing the placeholder to work
			// by just doing public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(keybindhere)
			// Probably because these runes inherit RuneItem and that messes it up somehow
			int index = tooltips.FindIndex(static line => line.Text.Contains("{0}"));
			if(index >= 0)
			{
				ref string text = ref tooltips[index].Text;
				text = text.Replace("{0}", CustomKeyBinds.RuneAbility.GetAssignedKeys().FirstOrDefault("<Unbound>"));
			}
		}
	}

	public class RuneOfGuardingPlayer : ModPlayer
	{
		public bool isWearingRuneOfGuarding;

		public override void ResetEffects()
		{
			isWearingRuneOfGuarding = false;
		}
	}
}
