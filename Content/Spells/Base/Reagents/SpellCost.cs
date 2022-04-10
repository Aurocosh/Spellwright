using Microsoft.Xna.Framework;
using Terraria;

namespace Spellwright.Content.Spells.Base.Reagents
{
    public abstract class SpellCost
    {
        public string LastError { get; protected set; }
        public Color ErrorColor { get; protected set; }

        protected SpellCost()
        {
            ErrorColor = new Color(255, 140, 40, 255);
        }

        public abstract bool Consume(Player player, int playerLevel, float costModifier, SpellData spellData);
    }
}
