using AssortedAdditions.Content.Items.Misc;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.GlobalTiles
{
	internal class PotLootDrops : GlobalTile
	{

		public override void Drop(int i, int j, int type)
		{
			if(type == TileID.Pots)
			{
				if(Main.rand.NextBool(125))
				{
					Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), new Vector2(i * 16, j * 16), ModContent.ItemType<BlankRune>(), 1);
				}	
			}
		}
	}
}
