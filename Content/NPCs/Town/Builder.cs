using System.Collections.Generic;
using AssortedAdditions.Content.Items.Placeables.Blocks;
using AssortedAdditions.Content.Items.Vanity;
using AssortedAdditions.Content.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.NPCs.Town
{
    [AutoloadHead]
    internal class Builder : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 26; // Total frames
            NPCID.Sets.ExtraFramesCount[Type] = 10; // NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
            NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
            NPCID.Sets.AttackType[Type] = 1; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
            NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            NPCID.Sets.AttackAverageChance[Type] = 30; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
            NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

            // Makes the NPC walk in the bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);


            // Which biome(s) and NPC(s) the BuilderNPC prefers
            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Hate)
                .SetNPCAffection(NPCID.Painter, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate);
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 15;
            NPC.defense = 15;
            NPC.lifeMax = 350;
            NPC.knockBackResist = 1f;

            NPC.townNPC = true;
            NPC.friendly = true;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AnimationType = NPCID.Guide;
        }

        // Set the spawn biome for bestiary and the info text
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("A Simple man looking for a place to call home. He knows all about building stuff and offers supplies in exchange for a few coins"),
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // Create gore when the NPC is killed.
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BuilderGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BuilderGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BuilderGore3").Type, 1f);
            }
        }

        // Can spawn after defeating the first boss
        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return NPC.downedBoss1;
        }

        // Some names that the NPC can have
        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Bob",
                "Frank",
                "Adam",
                "Chris",
                "Ben",
                "David",
                "George",
                "Henry",
                "Jacob"
            };
        }

        public override string GetChat()
        {
            NPC.FindFirstNPC(ModContent.NPCType<Builder>());

            switch (Main.rand.Next(4))
            {
                case 0:
                    return "If you like building things, you will find my wares useful";
                case 1:
                    return "You can find inspiration for new builds from the most unexcpected places";
                case 2:
                    return "The forest really is an excellent location for contructing something magnificent";
                case 3:
                    return "Looking for building materials? Look no further!";
                default:
                    return "Bla bla bla...";
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";
            button2 = "Vanilla Shop";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = "Shop";
            }
            else
            {
                shopName = "Vanilla Shop";
            }
        }

        // What the NPC sells
        // Currently 2 different shops
        public override void AddShops()
        {
            // Modded items get the price from their Item.value
            var moddedShop = new NPCShop(Type, "Shop")
                .Add<PaintBlock>()
                .Add<PaintBlock2>()
                .Add<CheckerBlock>()
                .Add<ArtDecoBlock>()
                .Add<HazardBlock>()
                .Add<RedCarpet>()
                .Add<BlueCarpet>()
                .Add<GreenCarpet>()

                .Add<BuilderHelmet>()
                .Add<BuilderJacket>()
                .Add<BuilderPants>();
            moddedShop.Register();

            // Shop for vanilla blocks
            var vanillaShop = new NPCShop(Type, "Vanilla Shop")
                .Add(new Item(ItemID.Wood) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.RichMahogany) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.Ebonwood) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.Shadewood) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.Pearlwood) { shopCustomPrice = Item.buyPrice(copper: 75) }, condition: Condition.Hardmode)
                .Add(new Item(ItemID.BorealWood) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.PalmWood) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.DynastyWood) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.AshWood) { shopCustomPrice = Item.buyPrice(copper: 75) })

                .Add(new Item(ItemID.StoneBlock) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.EbonstoneBlock) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.CrimstoneBlock) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.PearlstoneBlock) { shopCustomPrice = Item.buyPrice(copper: 75) }, condition: Condition.Hardmode)
                .Add(new Item(ItemID.Granite) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.Marble) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.Sandstone) { shopCustomPrice = Item.buyPrice(copper: 75) })

                .Add(new Item(ItemID.BlueBrick) { shopCustomPrice = Item.buyPrice(copper: 75) }, condition: Condition.DownedSkeletron)
                .Add(new Item(ItemID.GreenBrick) { shopCustomPrice = Item.buyPrice(copper: 75) }, condition: Condition.DownedSkeletron)
                .Add(new Item(ItemID.PinkBrick) { shopCustomPrice = Item.buyPrice(copper: 75) }, condition: Condition.DownedSkeletron)
                .Add(new Item(ItemID.LihzahrdBrick) { shopCustomPrice = Item.buyPrice(copper: 75) }, condition: Condition.DownedGolem)

                .Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.SandBlock) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.ClayBlock) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.MudBlock) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.SnowBlock) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.IceBlock) { shopCustomPrice = Item.buyPrice(copper: 75) })

                .Add(new Item(ItemID.Cloud) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.SunplateBlock) { shopCustomPrice = Item.buyPrice(copper: 75) })
                .Add(new Item(ItemID.LivingFireBlock) { shopCustomPrice = Item.buyPrice(copper: 75) }, condition: Condition.Hardmode)
                .Add(new Item(ItemID.RainbowBrick) { shopCustomPrice = Item.buyPrice(copper: 75) }, condition: Condition.Hardmode)

                .Add(new Item(ItemID.PaintSprayer) { shopCustomPrice = Item.buyPrice(gold: 7, silver: 50) })
                .Add(new Item(ItemID.ExtendoGrip) { shopCustomPrice = Item.buyPrice(gold: 7, silver: 50) })
                .Add(new Item(ItemID.PortableCementMixer) { shopCustomPrice = Item.buyPrice(gold: 7, silver: 50) })
                .Add(new Item(ItemID.BrickLayer) { shopCustomPrice = Item.buyPrice(gold: 7, silver: 50) })
                .Add(new Item(ItemID.Toolbox) { shopCustomPrice = Item.buyPrice(gold: 5) })
                ; // <-- Important remeber this
            vanillaShop.Register();
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 15;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
            randExtraCooldown = 10;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.WoodenArrowFriendly;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 15f;
        }

		public override void DrawTownAttackGun(ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset)
		{
            int itemType = ModContent.ItemType<SteelBow>();
            Main.GetItemDrawFrame(itemType, out item, out itemFrame);
            horizontalHoldoutOffset = (int) Main.DrawPlayerItemPos(1f, itemType).X - 12;
		}
	}
}
