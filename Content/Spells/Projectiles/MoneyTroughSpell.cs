using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class MoneyTroughSpell : ProjectileSpell
    {
        public override void SetStaticDefaults()
        {
            UseType = SpellType.Invocation;
            projectileType = ProjectileID.FlyingPiggyBank;
            useSound = SoundID.Item59;
            canAutoReuse = false;
            useTimeMultiplier = 1f;
        }
    }
}
