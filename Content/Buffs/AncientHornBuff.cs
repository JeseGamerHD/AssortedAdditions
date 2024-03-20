using Terraria.ModLoader;
using Terraria;

namespace AssortedAdditions.Content.Buffs
{
	public class AncientHornBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetDamage(DamageClass.Summon) += 0.2f;
			player.GetDamage(DamageClass.SummonMeleeSpeed) -= 0.2f;
		}
	}
}
