using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spellwright.Common.Players;
using Spellwright.Content.Items;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Projectiles;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.Bestiary;
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

            // Biomes
            NPC.Happiness.LikeBiome(PrimaryBiomeID.Forest);
            NPC.Happiness.LoveBiome(PrimaryBiomeID.Hallow);
            NPC.Happiness.DislikeBiome(PrimaryBiomeID.Snow);

            // NPCs
            NPC.Happiness.HateNPC(NPCID.Stylist);
            NPC.Happiness.DislikeNPC(NPCID.Golfer);
            NPC.Happiness.LikeNPC(NPCID.Guide);
            NPC.Happiness.LoveNPC(NPCID.Wizard);
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
                new FlavorTextBestiaryInfoElement("Mysterious practiotioner of a an unusual brand of magic, one that does not rely on mana, but on reagens and incantations. Looks remarkably similar to wizard, but both claim that they are not related."),
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
                if (!player.active)
                    continue;

                if (player.inventory.Any(item => item.type == ItemID.FallenStar))
                    return true;
            }

            return false;
        }

        public override string TownNPCName()
        {
            switch (WorldGen.genRand.Next(8))
            {
                case 0:
                    return "Atarum";

                case 1:
                    return "Adisorin";

                case 2:
                    return "Igoquam";

                case 3:
                    return "Ilubin";

                case 4:
                    return "Elliro";

                case 5:
                    return "Kolenor";

                case 6:
                    return "Ezith";

                default:
                    return "Gilleas";
            }
        }

        public override string GetChat()
        {
            var chat = new WeightedRandom<string>();

            int wizardId = NPC.FindFirstNPC(NPCID.Wizard);
            if (wizardId >= 0 && Main.rand.NextBool(4))
                chat.Add("No, me and " + Main.npc[wizardId].GivenName + " are not twins, we are not even related.");

            chat.Add("Let me teach you the secrets of my art.");
            chat.Add("Words have power, a lot of power.");
            chat.Add("Don't speak my name lightly!");
            chat.Add("There is yet much to be learned.");
            return chat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");

            Player player = Main.LocalPlayer;
            var modPlayer = player.GetModPlayer<SpellwrightPlayer>();
            if (modPlayer.PlayerLevel == 0)
                button2 = "Learn spellcraft";
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
                var modPlayer = player.GetModPlayer<SpellwrightPlayer>();

                if (modPlayer.PlayerLevel == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item37); // Reforge/Anvil sound

                    Main.npcChatText = $"You seems to be interested in my art. Take this {Lang.GetItemNameValue(ItemID.HiveBackpack)} and make use of it if you want to learn it.";

                    //int hiveBackpackItemIndex = Main.LocalPlayer.FindItem(ItemID.HiveBackpack);
                    //Main.LocalPlayer.inventory[hiveBackpackItemIndex].TurnToAir();

                    player.QuickSpawnItem(ModContent.ItemType<SilverMirror>());

                    return;
                }
            }

        }

        // Not completely finished, but below is what the NPC will sell

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<GoldenBook>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<SilverMirror>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<CommonSpellReagent>());

            if (!Main.dayTime)
                shop.item[nextSlot++].SetDefaults(ModContent.ItemType<RareSpellReagent>());
            if (Main.moonPhase == (int)MoonPhase.Full)
                shop.item[nextSlot++].SetDefaults(ModContent.ItemType<MythicalSpellReagent>());
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

        // todo: implement
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