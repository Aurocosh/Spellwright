using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spellwright.Common.Players;
using Spellwright.Content.Items;
using Spellwright.Content.Items.Mirrors;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Items.SpellTomes;
using Spellwright.Content.Projectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Spellwright.Content.NPCs
{
    [AutoloadHead]
    public class SpellwrightNPC : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
            DisplayName.SetDefault("Spellwright");
            Main.npcFrameCount[Type] = 25; // The amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the npc that it tries to attack enemies.
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

            var drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f,
                Direction = 1
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
                .SetBiomeAffection<HallowBiome>(AffectionLevel.Love)
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Stylist, AffectionLevel.Hate)
                .SetNPCAffection(NPCID.Golfer, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Wizard, AffectionLevel.Like);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Wizard;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement(Spellwright.GetTranslation("SpellwrightNpc","BestiaryDescription").Value),
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(Type, out NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers))
            {
                drawModifiers.Rotation += 0.001f;

                NPCID.Sets.NPCBestiaryDrawOffset.Remove(Type);
                NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            }

            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            int num = NPC.life > 0 ? 1 : 5;

            for (int k = 0; k < num; k++)
            {
                var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.SilverCoin);
                dust.noLightEmittence = true;
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (player.active && player.statManaMax >= 100)
                    return true;
            }

            return false;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() { "Atarum", "Adisorin", "Igoquam", "Ilubin", "Elliro", "Kolenor", "Ezith", "Gilleas" };
        }

        public override string GetChat()
        {
            var chat = new WeightedRandom<string>();

            int wizardId = NPC.FindFirstNPC(NPCID.Wizard);
            if (wizardId >= 0 && Main.rand.NextBool(4))
                chat.Add(Spellwright.GetTranslation("SpellwrightNpc", "Chatting", "WizardTwin").Format(Main.npc[wizardId].GivenName));

            if (wizardId >= 0 && Main.rand.NextBool(4))
                chat.Add(Spellwright.GetTranslation("SpellwrightNpc", "Chatting", "WizardEvilTwin").Format(Main.npc[wizardId].GivenName));

            chat.Add(Spellwright.GetTranslation("SpellwrightNpc", "Chatting", "ArtSecrets").Value);
            chat.Add(Spellwright.GetTranslation("SpellwrightNpc", "Chatting", "WordPower").Value);
            chat.Add(Spellwright.GetTranslation("SpellwrightNpc", "Chatting", "NoSpeakName").Value);
            chat.Add(Spellwright.GetTranslation("SpellwrightNpc", "Chatting", "MuchToLearn").Value);

            return chat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");

            Player player = Main.LocalPlayer;
            var modPlayer = player.GetModPlayer<SpellwrightPlayer>();

            if (!modPlayer.LearnedBasics)
            {
                bool hasTeachings = player.HasItem(ModContent.ItemType<SpellwrightTeachings>());
                if (!hasTeachings)
                    button2 = Spellwright.GetTranslation("SpellwrightNpc", "AskSpellcraft").Value;
            }
            else if (!modPlayer.CanCastSpells)
            {
                bool hasFruit = player.HasItem(ModContent.ItemType<BizarreFruit>());
                if (!hasFruit)
                    button2 = Spellwright.GetTranslation("SpellwrightNpc", "AskForMysticFruit").Value;
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
            else
            {
                Player player = Main.LocalPlayer;
                var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();


                if (!spellPlayer.LearnedBasics)
                {
                    SoundEngine.PlaySound(SoundID.Item25);

                    int teachingsId = ModContent.ItemType<SpellwrightTeachings>();
                    Main.npcChatText = Spellwright.GetTranslation("SpellwrightNpc", "TeachingsOffer").ToString();
                    player.QuickSpawnItem(new EntitySource_Gift(NPC), teachingsId);
                }
                else if (!spellPlayer.CanCastSpells)
                {
                    SoundEngine.PlaySound(SoundID.Item25);

                    int bizarreFruitId = ModContent.ItemType<BizarreFruit>();
                    int purifiedFruitId = ModContent.ItemType<PurifiedFruit>();
                    Main.npcChatText = Spellwright.GetTranslation("SpellwrightNpc", "FruitOffer").Format(Lang.GetItemNameValue(bizarreFruitId), Lang.GetItemNameValue(purifiedFruitId));
                    player.QuickSpawnItem(new EntitySource_Gift(NPC), bizarreFruitId);
                }
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<SpellResonator>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<SilverMirror>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<CommonSpellReagent>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<RareSpellReagent>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<MythicalSpellReagent>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<BeginnerSpellTome>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<AdvancedSpellTome>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<SupremeSpellTome>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<PurifiedFruit>());
        }

        public override bool CanGoToStatue(bool toKingStatue) => true;

        // Create a square of pixels around the NPC on teleport.
        public void StatueTeleport()
        {
            for (int i = 0; i < 30; i++)
            {
                Vector2 position = Main.rand.NextVector2Square(-20, 21);
                if (Math.Abs(position.X) > Math.Abs(position.Y))
                    position.X = Math.Sign(position.X) * 20;
                else
                    position.Y = Math.Sign(position.Y) * 20;

                Dust.NewDustPerfect(NPC.Center + position, DustID.SilverCoin, Vector2.Zero).noLightEmittence = true;
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<BoltOfConfusionProjectile>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }
    }
}