using AssortedAdditions.Content.Projectiles.SummonProj;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Buffs
{
    internal class SunScepterBuff : ModBuff
    {
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SunScepterMinion>()] < 1)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}
