using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.NPCs
{
    internal class IceGuardianShield : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            // Immune to:
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 26;
            NPC.height = 26;
            NPC.damage = 40;
            NPC.defense = 0;
            NPC.lifeMax = 50;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.Item27;
            NPC.value = 0;

            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.aiStyle = 0;
        }

        private bool doOnce;
        public override void AI()
        {
            // If the parent NPC dies, so does this one
            if (Main.npc[(int)NPC.ai[0]].type != ModContent.NPCType<IceGuardian>() 
                || !Main.npc[(int)NPC.ai[0]].active)
            {
                NPC.active = false;
            }

            if (!doOnce)
            {
                for(int i = 0; i < 15; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height,
                        DustID.Frost, 0, 0, 150, default, 1f);
                    dust.noGravity = true;
                }
                doOnce = true;
            }

            // Circles around the parent NPC
            NPC.ai[1] += 0.05f;
            Vector2 Orbit = Main.npc[(int)NPC.ai[0]].Center + new Vector2(0, 40).RotatedBy(NPC.ai[1]);
            NPC.Center = Orbit;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void OnKill()
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height,
                    DustID.Frost, 0, 0, 150, default, 1f);
                dust.noGravity = true;
                dust.velocity *= 4f;
            }
        }
    }
}
