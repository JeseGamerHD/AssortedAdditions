using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Buffs
{
	internal class PhantasmicDaggerDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetDamage(DamageClass.Summon) -= 0.15f;
		}
	}
}
