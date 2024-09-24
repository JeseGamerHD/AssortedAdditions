using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.Players
{
	internal class PlayerNotifications : ModPlayer
	{
		public override void OnEnterWorld()
		{
			// Checking if the player is using the translation mod, but their game language is not in Chinese.
			// Then notifying about it incase the player did not realise that this mod is originally in English.
			if (ModLoader.TryGetMod("AssortedAdditionsChinese", out Mod AssortedAdditionsChinese))
			{
				if (!LanguageManager.Instance.ActiveCulture.Name.Equals("zh-Hans"))
				{
					Main.NewText("Assorted Additions: You are using the Chinese translation mod, but your game is not set to Chinese.", Color.Yellow);
					Main.NewText("If you want the mod to be in English, disable: AssortedAdditions各种增补--简体中文汉化 inside the 'Manage Mods' menu.", Color.Yellow);
				}
			}
		}
	}
}
