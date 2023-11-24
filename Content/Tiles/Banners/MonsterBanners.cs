using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Banners
{
    // This class is used as a base for other banners so there is no need to repeat the same code
    // Just set the Color and BuffNPC in the other banner's SetStaticDefaults() and keep the base.SetStaticDefaults();

    /// <summary>
    /// A Base class for easily creating banner tiles
    /// </summary>
    /// /// <remarks>
    /// Set these in SetStaticDefaults() and remember to leave the base:
    /// <list type="Attributes">
    /// <item>Color: color of the tile on the map. Use Color.name or new Color()</item>
    /// <item>BuffNPC: which npc the player gets a buff against. Use ModContent.NPCType or NPCID</item>
    /// </list>
    /// </remarks>
    public abstract class MonsterBanners : ModTile
    {
        public Color Color { get; set; } // What color the tile will be on the map
        public int BuffNPC { get; set; } // What npc does this banner give a buff against
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            DustType = -1;

            LocalizedText name = Language.GetText("Banner");
            AddMapEntry(Color, name); // Name will be just Banner like the vanilla ones (set in localisation)
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.hasBanner = true;
                Main.SceneMetrics.NPCBannerBuff[BuffNPC] = true;
            }
        }
    }
}
