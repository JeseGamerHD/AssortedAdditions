using AssortedAdditions.Common.Configs;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.GlobalItems
{
    internal class WingFlightLean : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            // This can be toggled on/off from the configs
            if(ModContent.GetInstance<ClientSidedToggles>().FancyWingFlightToggle)
            {
                if (player.wingsLogic > 0)
                {
                    player.GetModPlayer<WingPlayer>().isWearingWings = true;
                }

                // Player is flying
                if (player.wingsLogic > 0 && player.wingTime > 0 && (player.controlJump || player.TryingToHoverUp || player.TryingToHoverDown))
                {
                    player.fullRotation = player.velocity.X * 0.01f;
                }
                // Player is gliding
                else if (player.wingsLogic > 0 && player.controlJump && player.wingTime == 0 && player.velocity.Y != 0)
                {
                    player.fullRotation = player.velocity.X * 0.01f;
                }
                // Wings equipped on the ground or when falling
                else if (player.wingsLogic > 0)
                {
                    player.fullRotation = default;
                }

                // If player had wings equipped previously but takes them off mid flight reset the rotation to normal
                if (player.GetModPlayer<WingPlayer>().wasWearingWings && !player.GetModPlayer<WingPlayer>().isWearingWings)
                {
                    player.GetModPlayer<WingPlayer>().wasWearingWings = false;
                    player.fullRotation = default;
                }
            }
        }
    }

    public class WingPlayer : ModPlayer
    {
        public bool wasWearingWings;
        public bool isWearingWings;

        public override void PostUpdateEquips()
        {
            if (ModContent.GetInstance<ClientSidedToggles>().FancyWingFlightToggle)
            {
                if (isWearingWings)
                {
                    // Since this is called after UpdateAccessory
                    // if the player "isWearingWings" then the player "wasWearingWings"
                    wasWearingWings = true;
                }
            }
        }

        public override void ResetEffects()
        {
            isWearingWings = false; // Keep resetting this since it it set by a tick update
        }
    }
}
