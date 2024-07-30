using AssortedAdditions.Content.Buffs;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Helpers;

namespace AssortedAdditions.Content.Items.Weapons.Summon
{
	internal class ShellHorn : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 24;
			Item.useTime = 90;
			Item.useAnimation = 90;
			Item.mana = 50;

			Item.noMelee = true;

			Item.rare = ItemRarityID.Green;
			Item.DamageType = DamageClass.Summon;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = new SoundStyle("AssortedAdditions/Assets/Sounds/WeaponSound/HornBlow");
			Item.value = Item.sellPrice(silver: 75);
			Item.buffType = ModContent.BuffType<ShellHornBuff>();
		}

		public override bool CanUseItem(Player player)
		{
			return !player.HasBuff(ModContent.BuffType<AncientHornBuff>());
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
			recipe.AddIngredient(ItemID.ShellPileBlock, 8);
			recipe.AddIngredient(ItemID.Coral, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}

	public class ShellHornPlayer : ModPlayer
	{
		public override void ModifyHurt(ref Player.HurtModifiers modifiers)
		{
			if (Player.HasBuff(ModContent.BuffType<ShellHornBuff>()))
			{
				modifiers.IncomingDamageMultiplier *= 1.20f;
			}
		}

		public override void PostUpdateMiscEffects()
		{
			if (Player.HasBuff(ModContent.BuffType<ShellHornBuff>()))
			{
				if (Main.rand.NextBool(3))
				{
					Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.WaterCandle);
					dust.noGravity = true;
				}
			}
		}
	}
}
