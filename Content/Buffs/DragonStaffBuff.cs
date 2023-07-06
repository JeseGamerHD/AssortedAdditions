using Terraria;
using Terraria.ModLoader;
using ModdingTutorial.Content.Projectiles.SummonProj;

namespace ModdingTutorial.Content.Buffs;

public class DragonStaffBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        if (player.ownedProjectileCounts[ModContent.ProjectileType<DragonMinionProj>()] < 1)
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