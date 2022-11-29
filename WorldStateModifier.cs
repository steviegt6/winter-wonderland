using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace WinterWonderland;

public class WorldStateModifier : ModSystem
{
    public override void PreUpdateWorld() {
        base.PreUpdateWorld();

        Main.xMas = true;
        Main.forceXMasForToday = true;

        Filters.Scene["Sandstorm"].Active = false;
    }
}