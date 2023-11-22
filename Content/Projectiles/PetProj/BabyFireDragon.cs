using System.Collections.Generic;
using AssortedAdditions.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.PetProj
{
    internal class BabyFireDragon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 10;
            Main.projPet[Projectile.type] = true;

            // This code is needed to customize the vanity pet display in the player select screen.
            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type] - 4, 6) // - 4 to frames since I only want the pet to walk here
                .WithOffset(-10, -20f) // Offset the sprite so it looks good
                .WithSpriteDirection(-1) // Direction it faces
                .WhenNotSelected(0, 0) // Stops animation when not hovering over the icon
                .WithCode(DelegateMethods.CharacterPreview.Float);
        }
        public override void SetDefaults()
        {
            // Just clone default AI since making one is annoying
            Projectile.CloneDefaults(ProjectileID.PetLizard);
            AIType = ProjectileID.PetLizard;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            player.lizard = false; // Relic from AIType

            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Keep the projectile from disappearing as long as the player isn't dead and has the pet buff.
            if (!player.dead && player.HasBuff(ModContent.BuffType<BabyFireDragonBuff>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            // Pet can now walk in front of the player's draw layer
            overPlayers.Add(index);
        }
    }
}
