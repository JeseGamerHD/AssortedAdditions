using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;

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
			TileID.Sets.MultiTileSway[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom | AnchorType.PlanterBox, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.newTile.DrawYOffset = -2;
			
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
            TileObjectData.addAlternate(0);

			TileObjectData.addTile(Type);

			DustType = -1;
            AddMapEntry(Color, Language.GetText("MapObject.Banner"));
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (!closer)
            {
                int tileStyle = TileObjectData.GetTileStyle(Main.tile[i, j]);
                int itemType = TileLoader.GetItemDropFromTypeAndStyle(Type, tileStyle);

                if (BuffNPC != 0)
                {
                    if (ItemID.Sets.BannerStrength.IndexInRange(itemType) && ItemID.Sets.BannerStrength[itemType].Enabled)
                    {
                        Main.SceneMetrics.NPCBannerBuff[BuffNPC] = true;
                        Main.SceneMetrics.hasBanner = true;
                    }
                }
            }
        }

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			// Due to MultiTileVine rendering the tile 2 pixels higher than expected for modded tiles using TileObjectData.DrawYOffset, we need to add 2 to fix the math for correct drawing
			offsetY += 2;
        }

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
            Tile tile = Main.tile[i, j];
            if (TileObjectData.IsTopLeft(tile))
            {
                Main.instance.TilesRenderer.AddSpecialPoint(i, j, TileDrawing.TileCounterType.MultiTileVine);
            }
			return false;
		}
	}
}
