using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Buffs
{
	internal class RuneOfHealthCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true; // Important to remember for debuffs
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}
	}
}
