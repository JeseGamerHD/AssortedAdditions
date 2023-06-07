using Terraria;
using Terraria.ModLoader;
using ModdingTutorial.Content.Projectiles.SummonProj;

namespace ModdingTutorial.Content.Buffs;

public class DragonStaffBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("It's a dragon!");
        Description.SetDefault("The dragon will protect you.");
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        if (player.ownedProjectileCounts[ModContent.ProjectileType<DragonMinionProj>()] > 0)
        {
            player.buffTime[buffIndex] = 18000;
        }
        else
        {
            player.DelBuff(buffIndex);
            buffIndex--;
        }
    }
}