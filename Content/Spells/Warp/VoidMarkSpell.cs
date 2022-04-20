﻿using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Extensions;
using Terraria;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Spells.Warp
{
    internal class VoidMarkSpell : TeleportationSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Invocation;
        }

        public override bool ConsumeReagents(Player player, int playerLevel, SpellData spellData)
        {
            if ((int)spellData.ExtraData == 1)
                return true;
            return base.ConsumeReagents(player, playerLevel, spellData);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            int action = (int)spellData.ExtraData;
            if (action == 0)
                return false;

            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            if (action == 1)
            {
                spellPlayer.VoidMarkPoint = player.position.ToGridPoint();
                return true;
            }
            else
            {
                var position = spellPlayer.VoidMarkPoint;
                var teleportPosition = position.ToWorldCoordinates();

                var collisionVec = teleportPosition + new Vector2(-player.width / 2 + 8, -player.height);
                if (Collision.SolidCollision(collisionVec, player.width, player.height))
                {
                    spellPlayer.VoidMarkPoint = Point.Zero;
                    return false;
                }

                Teleport(player, teleportPosition, true);

                spellPlayer.VoidMarkPoint = Point.Zero;
                return true;
            }
        }

        public override bool ProcessExtraData(Player player, SpellStructure structure, out object extraData)
        {
            string argument = structure.Argument;
            if (argument.Length == 0)
            {
                var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
                extraData = spellPlayer.VoidMarkPoint == Point.Zero ? 1 : 2;
                return true;
            }

            var lowerArgument = argument.ToLower();
            var localSet = GetTranslation("Set").Value.ToLower();
            if (lowerArgument == "set" || lowerArgument == localSet)
            {
                extraData = 1;
                return true;
            }

            extraData = 0;
            return false;
        }

        public override void SerializeExtraData(TagCompound tag, object extraData)
        {
            tag.Add("ExtraData", (int)extraData);
        }

        public override object DeserializeExtraData(TagCompound tag)
        {
            return tag.GetInt("ExtraData");
        }
    }
}