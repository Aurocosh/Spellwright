using Microsoft.Xna.Framework;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Spells.Warp
{
    internal class OceanGateSpell : TeleportationSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            UseType = SpellType.Invocation;
            castSound = SoundID.Item6;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.Coral, 10)
                .WithCost(ItemID.RecallPotion, 1);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 1);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            bool isOnLeftSide = player.position.X / 16f < Main.maxTilesX / 2;

            int teleportDestination = 0;
            if (spellData.ExtraData is OceanGateData oceanStepData)
                teleportDestination = oceanStepData.TeleportDestination;

            bool teleportToRightSide;
            if (teleportDestination == 0)
                teleportToRightSide = isOnLeftSide;
            else
                teleportToRightSide = teleportDestination == 2;

            Vector2 vector = Vector2.Zero;
            int crawlOffsetX = teleportToRightSide.ToDirectionInt();
            int startX = teleportToRightSide ? Main.maxTilesX - 50 : 50;
            bool canTeleport = true;
            if (!TeleportHelpers.RequestMagicConchTeleportPosition(player, -crawlOffsetX, startX, out Point landingPoint))
            {
                canTeleport = false;
                startX = !teleportToRightSide ? Main.maxTilesX - 50 : 50;
                if (TeleportHelpers.RequestMagicConchTeleportPosition(player, crawlOffsetX, startX, out landingPoint))
                    canTeleport = true;
            }

            if (canTeleport)
                vector = landingPoint.ToWorldCoordinates(8f, 16f) - new Vector2(player.width / 2, player.height);

            Teleport(player, vector, canTeleport);
            return canTeleport;
        }

        public override bool ProcessExtraData(Player player, SpellStructure structure, out object extraData)
        {
            int teleportDestination = 0;
            if (structure.Argument.Length > 0)
            {
                var destination = structure.Argument.ToLower();
                if (destination == "west" || destination == "left")
                    teleportDestination = 1;
                else if (destination == "east" || destination == "right")
                    teleportDestination = 2;
                else
                {
                    extraData = null;
                    return false;
                }
            }

            extraData = new OceanGateData(teleportDestination);
            return true;
        }

        public override void SerializeExtraData(TagCompound tag, object extraData)
        {
            if (extraData is OceanGateData oceanGateData)
                tag.Add("TeleportDestination", oceanGateData.TeleportDestination);
        }

        public override object DeserializeExtraData(TagCompound tag)
        {
            int teleportDestination = tag.GetInt("TeleportDestination");
            return new OceanGateData(teleportDestination);
        }
    }

    internal class OceanGateData
    {
        public int TeleportDestination { get; } // 0 - not set, 1 - left, 2 - right

        public OceanGateData(int teleportDestination)
        {
            TeleportDestination = teleportDestination;
        }
    }
}