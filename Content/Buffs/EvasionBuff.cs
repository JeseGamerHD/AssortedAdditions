using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Buffs
{
	internal class EvasionBuff : ModBuff
	{
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<EvasionBuffPlayer>().hasEvasionBuff = true;
		}
	}

	internal class EvasionBuffPlayer : ModPlayer 
	{ 
		public bool hasEvasionBuff = false;

		public override bool FreeDodge(Player.HurtInfo info)
		{
			if(hasEvasionBuff)
			{
				if (Main.rand.NextBool(20))
				{
					Player.immune = true;
					Player.immuneTime = 75;
					Player.immuneNoBlink = false;

					for(int i = 0; i < 30; i++)
					{
						Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.RainCloud);
						dust.noGravity = true;
						dust.velocity *= 1.2f;
					}

					NetMessage.SendData(MessageID.Dodge, -1, -1, null, Player.whoAmI, 1f, 0f, 0f, 0, 0, 0);

					return true;
				}
			}


			return base.FreeDodge(info);
		}

		public override void ResetEffects()
		{
			hasEvasionBuff = false;

		}
	}
}
