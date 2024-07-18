using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Buffs
{
	internal class ShellHornBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetDamage(DamageClass.Summon) += 0.1f;
			player.GetDamage(DamageClass.SummonMeleeSpeed) -= 0.1f; // Only buff minions, not whips
		}
	}
}
