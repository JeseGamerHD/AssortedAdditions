﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Buffs.Whips
{
    internal class BlossomLashDebuff : ModBuff
    {
        public override string Texture => "AssortedAdditions/Content/Buffs/Whips/MotivatorDebuff";

        public static readonly int TagDamage = 10;

        public override void SetStaticDefaults()
        {
            // This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
            // Other mods may check it for different purposes.
            BuffID.Sets.IsATagBuff[Type] = true;
            Main.debuff[Type] = true;
        }
    }

    public class BlossomLashDebuffNPC : GlobalNPC
    {
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            // Only player attacks should benefit from this buff, hence the NPC and trap checks.
            if (projectile.npcProj || projectile.trap || !projectile.IsMinionOrSentryRelated)
                return;

            // SummonTagDamageMultiplier scales down tag damage for some specific minion and sentry projectiles for balance purposes.
            var projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
            if (npc.HasBuff<BlossomLashDebuff>())
            {
                // Apply a flat bonus to every hit
                modifiers.FlatBonusDamage += BlossomLashDebuff.TagDamage * projTagMultiplier;
            }
        }
    }
}
