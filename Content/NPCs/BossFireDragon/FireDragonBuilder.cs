using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.NPCs.BossFireDragon /// This is a copy of ExampleMod's Worm.cs, modified for a custom Fire Dragon boss
{ // https://github.com/tModLoader/tModLoader/blob/1.4.3-MigrationPreparation/ExampleMod/Content/NPCs/Worm.cs

    public enum WormSegmentType
    {
        Head,
        Body,
        Tail
    }

    // Abstract class can be used as a framework that other classes can build upon
    // This abstract class is for making Wormlike NPCs
    public abstract class FireDragonBuilder : ModNPC
    {
        // These values are set in other classes that make the actual NPCs
        public abstract WormSegmentType SegmentType { get; } // Which type of segment this NPC is considered to be
        public float MoveSpeed { get; set; } // The maximum velocity for the NPC
        public NPC HeadSegment => Main.npc[NPC.realLife]; // The NPC instance of the head segment for this worm.
        public NPC FollowingNPC => SegmentType == WormSegmentType.Head ? null : Main.npc[(int)NPC.ai[1]]; // The NPC instance of the segment that this segment is following (ai[1])
        public NPC FollowerNPC => SegmentType == WormSegmentType.Tail ? null : Main.npc[(int)NPC.ai[0]]; // The NPC instance of the segment that is following this segment (ai[0])

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return SegmentType == WormSegmentType.Head ? null : false;
        }

        public sealed override bool PreAI()
        {
            if (NPC.localAI[1] == 0)
            {
                NPC.localAI[1] = 1f;
                Init();
            }

            if (SegmentType == WormSegmentType.Head)
            {
                HeadAI();

                if (!NPC.HasValidTarget)
                {
                    NPC.TargetClosest(true);

                    // If the NPC is a boss and it has no target, force it to fall to the underworld quickly
                    // This part handeled in FireDragon.cs
/*                    if (!NPC.HasValidTarget && NPC.boss)
                    {
                        NPC.velocity.Y += 8f;

                        MoveSpeed = 1000f;
                    }*/
                }
            }
            else
                BodyTailAI();

            return true;
        }

        // Prevents parts from despawning when they are "far" away from the player
        public override bool CheckActive()
        {
            return false;
        }

        // Not visible to public API, but is used to indicate what AI to run
        internal virtual void HeadAI() { }
        internal virtual void BodyTailAI() { }
        public abstract void Init();
    }

    public abstract class WormHead : FireDragonBuilder
    {
        public sealed override WormSegmentType SegmentType => WormSegmentType.Head;
        public abstract int BodyType { get; } // The NPCID or ModContent.NPCType for the body segment NPCs.
                                             // This property is only used if HasCustomBodySegments = false

        public abstract int TailType { get; } // The NPCID or ModContent.NPCType for the tail segment NPC.
                                             // This property is only used if HasCustomBodySegments = false
        public int MinSegmentLength { get; set; } // The minimum amount of segments expected, including the head and tail segments

        public int MaxSegmentLength { get; set; } // The maximum amount of segments expected, including the head and tail segments

        public virtual bool HasCustomBodySegments => false; // Whether the NPC uses custom segments such as Legs like in the wyvern

        public virtual int SpawnBodySegments(int segmentCount) // Override this method to use custom body-spawning code. This method only runs if HasCustomBodySegments = true
                                                              // The whoAmI of the most-recently spawned NPC
        {
            // Defaults to just returning this NPC's whoAmI, since the tail segment uses the return value as its "following" NPC index
            return NPC.whoAmI;
        }

        // Spawns a body or tail segment of the worm.
        protected int SpawnSegment(IEntitySource source, int type, int latestNPC)
        {
            // We spawn a new NPC, setting latestNPC to the newer NPC, whilst also using that same variable
            // to set the parent of this new NPC. The parent of the new NPC (may it be a tail or body part)
            // will determine the movement of this new NPC.
            // Under there, we also set the realLife value of the new NPC, because of what is explained above.
            int oldLatest = latestNPC;
            latestNPC = NPC.NewNPC(source, (int)NPC.Center.X, (int)NPC.Center.Y, type, NPC.whoAmI, 0, latestNPC);

            Main.npc[oldLatest].ai[0] = latestNPC;

            NPC latest = Main.npc[latestNPC];
            // NPC.realLife is the whoAmI of the NPC that the spawned NPC will share its health with
            latest.realLife = NPC.whoAmI;
            return latestNPC;
        }

        // Proper AI is handeled in FireDragon.cs, this just spawns the segments and orients them correctly
        internal sealed override void HeadAI()
        {
            HeadAI_SpawnSegments(); // Spawns the rest of the dragon
            HeadAI_SetRotation(); // Rotates the head towards where it is going
        }

        private void HeadAI_SpawnSegments()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // So, we start the AI off by checking if NPC.ai[0] (the following NPC's whoAmI) is 0.
                // This is practically ALWAYS the case with a freshly spawned NPC, so this means this is the first update.
                // Since this is the first update, we can safely assume we need to spawn the rest of the worm (bodies + tail).
                bool hasFollower = NPC.ai[0] > 0;
                if (!hasFollower)
                {
                    // So, here we assign the NPC.realLife value.
                    // The NPC.realLife value is mainly used to determine which NPC loses life when we hit this NPC.
                    // We don't want every single piece of the worm to have its own HP pool, so this is a neat way to fix that.
                    NPC.realLife = NPC.whoAmI;
                    // latestNPC is going to be used in SpawnSegment() and I'll explain it there.
                    int latestNPC = NPC.whoAmI;

                    // Here we determine the length of the Dragon.
                    int randomWormLength = Main.rand.Next(MinSegmentLength, MaxSegmentLength + 1);

                    int distance = randomWormLength - 2;

                    IEntitySource source = NPC.GetSource_FromAI();

                    // Call the method that'll handle spawning the body segments if there are custom ones
                    if (HasCustomBodySegments)
                    {
                        latestNPC = SpawnBodySegments(distance);
                    }
                    else
                    {
                        // If not, spawn the body segments like usual
                        while (distance > 0)
                        {
                            latestNPC = SpawnSegment(source, BodyType, latestNPC);
                            distance--;
                        }
                    }

                    // Spawn the tail segment
                    SpawnSegment(source, TailType, latestNPC);

                    NPC.netUpdate = true;

                    // Ensure that all of the segments could spawn.  
                    int count = 0; // Count the amount of segments
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC n = Main.npc[i];

                        if (n.active && (n.type == Type || n.type == BodyType || n.type == TailType) && n.realLife == NPC.whoAmI)
                            count++;
                    }
                    // If count doesn't match, despawn the dragon entirely
                    if (count != randomWormLength)
                    {
                        // Unable to spawn all of the segments... kill the dragon
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC n = Main.npc[i];

                            if (n.active && (n.type == Type || n.type == BodyType || n.type == TailType) && n.realLife == NPC.whoAmI)
                            {
                                n.active = false;
                                n.netUpdate = true;
                            }
                        }
                    }
                    // Set the player target for good measure
                    NPC.TargetClosest(true);
                }
            }
        }

        // Used for making the head look in the direction it is going
        private void HeadAI_SetRotation()
        {
            // Rotate the head / sprite
            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
            NPC.spriteDirection = NPC.direction = -1 * (NPC.velocity.X > 0).ToDirectionInt();

            // Force a netupdate if the NPC's velocity changed sign and it was not "just hit" by a player
            if ((NPC.velocity.X > 0 && NPC.oldVelocity.X < 0 || NPC.velocity.X < 0 && NPC.oldVelocity.X > 0 || NPC.velocity.Y > 0 && NPC.oldVelocity.Y < 0 || NPC.velocity.Y < 0 && NPC.oldVelocity.Y > 0) && !NPC.justHit)
                NPC.netUpdate = true;
        }
    }

    public abstract class WormBody : FireDragonBuilder
    {
        public sealed override WormSegmentType SegmentType => WormSegmentType.Body;

        internal override void BodyTailAI()
        {
            CommonAI_BodyTail(this);
        }

        internal static void CommonAI_BodyTail(FireDragonBuilder worm)
        {
            if (!worm.NPC.HasValidTarget)
                worm.NPC.TargetClosest(true);

            NPC following = worm.NPC.ai[1] >= Main.maxNPCs ? null : worm.FollowingNPC;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Kill the segment if the head dies
                if (Main.npc[following.realLife].active == false)
                {
                    worm.NPC.life = 0;
                    worm.NPC.HitEffect(0, 10);
                    worm.NPC.active = false;
                }
            }

            if (following is not null)
            {
                // Follow behind the segment "in front" of this NPC
                // Use the current NPC.Center to calculate the direction towards the "parent NPC" of this NPC.
                float dirX = following.Center.X - worm.NPC.Center.X;
                float dirY = following.Center.Y - worm.NPC.Center.Y;

                // We then use Atan2 to get a correct rotation towards that parent NPC.
                // Assumes the sprite for the NPC points upward.
                worm.NPC.rotation = (float)Math.Atan2(dirY, dirX) + MathHelper.PiOver2;
                // Makes the body flip when the head flips
                worm.NPC.spriteDirection = Main.npc[following.realLife].direction = -1 * (Main.npc[following.realLife].velocity.X > 0).ToDirectionInt();

                // We also get the length of the direction vector.
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                // We calculate a new, correct distance.
                float dist = (length - worm.NPC.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;

                // Reset the velocity of this NPC, because we don't want it to move on its own
                worm.NPC.velocity = Vector2.Zero;
                // And set this NPCs position accordingly to that of this NPCs parent NPC.
                worm.NPC.position.X += posX;
                worm.NPC.position.Y += posY;
            }
        }
    }

    // Since the body and tail segments share the same AI copy it
    public abstract class WormTail : FireDragonBuilder
    {
        public sealed override WormSegmentType SegmentType => WormSegmentType.Tail;

        internal override void BodyTailAI()
        {
            WormBody.CommonAI_BodyTail(this);
        }
    }
}
