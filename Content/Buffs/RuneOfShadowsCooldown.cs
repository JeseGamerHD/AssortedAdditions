using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Buffs
{
	internal class RuneOfShadowsCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true; // Important to remember for debuffs
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}
	}
}
