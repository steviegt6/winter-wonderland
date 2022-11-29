using Terraria;
using Terraria.ModLoader;

namespace WinterWonderland;

public class ChristmasToggleSystem : ModSystem
{
    public override void PreUpdateWorld() {
        base.PreUpdateWorld();

        Main.xMas = true;
        Main.forceXMasForToday = true;
    }
}