using AssortedAdditions.Content.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Helpers;
using Terraria.Audio;
using AssortedAdditions.Content.Items.Placeables.Ores;

namespace AssortedAdditions.Content.Items.Weapons.Summon
{
	internal class AncientHorn : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.useTime = 90;
			Item.useAnimation = 90;
			Item.mana = 80;

			Item.noMelee = true;

			Item.rare = ItemRarityID.Lime;
			Item.DamageType = DamageClass.Summon;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.UseSound = new SoundStyle("AssortedAdditions/Assets/Sounds/WeaponSound/AncientHornSound") with { Pitch = -0.2f };
			Item.value = Item.sellPrice(gold: 8);
			Item.buffType = ModContent.BuffType<AncientHornBuff>();
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				player.AddBuff(Item.buffType, HelperMethods.SecondsToTicks(45));
			}

			return true;
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

	public class AtestPlayer : ModPlayer
	{
		public override void ModifyHurt(ref Player.HurtModifiers modifiers)
		{
			if (Player.HasBuff(ModContent.BuffType<AncientHornBuff>()))
			{
				modifiers.IncomingDamageMultiplier *= 1.33f;
			}
		}

		public override void PostUpdateMiscEffects()
		{
			if (Player.HasBuff(ModContent.BuffType<AncientHornBuff>()))
			{
				if(Main.rand.NextBool(3))
				{
					Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.FlameBurst);
					dust.noGravity = true;
				}
			}
		}
	}
}
