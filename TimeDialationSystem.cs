using Terraria;
using Terraria.ModLoader;

namespace WinterWonderland;

public class TimeDialationSystem : ModSystem
{
    public override void ModifyTimeRate(ref double timeRate, ref double tileUpdateRate, ref double eventUpdateRate) {
        base.ModifyTimeRate(ref timeRate, ref tileUpdateRate, ref eventUpdateRate);

        if (Main.time > 32400d) timeRate = 2d;
        if (Main.time > 54000d) timeRate = 0.5d;
    }
}