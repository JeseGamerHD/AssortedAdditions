using System.Collections.Generic;
using AssortedAdditions.Content.Items.Consumables;
using AssortedAdditions.Content.NPCs.BossFireDragon;
using AssortedAdditions.Content.NPCs.BossTheHaunt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.Systems
{
	public class BossCheckListCompatibility : ModSystem
	{
		public override void PostSetupContent()
		{
			if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
			{
				return;
			}

			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"FireDragon",
				7.1f,
				() => DownedBossSystem.downedFireDragon,
				ModContent.NPCType<FireDragonHead>(),
				new Dictionary<string, object>()
				{
					["spawnItems"] = ModContent.ItemType<AncientToken>(),
					["spawnInfo"] = Language.GetText("Mods.AssortedAdditions.BossChecklist.FireDragon"),
					["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
					{
						Texture2D texture = ModContent.Request<Texture2D>("AssortedAdditions/Content/NPCs/BossFireDragon/FireDragon_Bestiary").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
						sb.Draw(texture, centered, color);
					}
				}
			);

			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"TheHaunt",
				11.1f,
				() => DownedBossSystem.downedTheHaunt,
				ModContent.NPCType<TheHaunt>(),
				new Dictionary<string, object>()
				{
					["spawnItems"] = ModContent.ItemType<GraveFlowers>(),
					["spawnInfo"] = Language.GetText("Mods.AssortedAdditions.BossChecklist.TheHaunt"),
				}
			);
		}
	}
}
