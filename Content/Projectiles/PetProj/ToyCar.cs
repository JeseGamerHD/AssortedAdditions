using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using AssortedAdditions.Content.Buffs;

namespace AssortedAdditions.Content.Projectiles.PetProj
{
    internal class ToyCar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 11;
            Main.projPet[Projectile.type] = true;

            // This code is needed to customize the vanity pet display in the player select screen.
            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
                .WithOffset(-10, 0) // Offset the sprite so it looks good
                .WithSpriteDirection(-1) // Direction it faces
                .WhenNotSelected(0, 0) // Stops animation when not hovering over the icon
                .WhenSelected(1, 5) // Only play these frames when hovering over the icon
                .WithCode(DelegateMethods.CharacterPreview.Float);
        }

        public override void SetDefaults()
        {
            // Just clone default AI since making one is annoying
            Projectile.CloneDefaults(ProjectileID.BlackCat);
            Projectile.width = 40;
            Projectile.height = 24;
            AIType = ProjectileID.BlackCat;
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
            if (!player.dead && player.HasBuff(ModContent.BuffType<ToyCarBuff>()))
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
