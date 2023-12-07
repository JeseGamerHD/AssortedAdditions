using AssortedAdditions.Content.Projectiles.SummonProj;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Buffs
{
    internal class SnowDwellerBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
            Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SnowDwellerMinion>()] < 1)
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
