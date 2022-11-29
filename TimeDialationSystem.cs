using Terraria;
using Terraria.ModLoader;

namespace WinterWonderland;

public class TimeDialationSystem : ModSystem
{
    public override void ModifyTimeRate(ref double timeRate, ref double tileUpdateRate, ref double eventUpdateRate) {
        base.ModifyTimeRate(ref timeRate, ref tileUpdateRate, ref eventUpdateRate);

        timeRate = Main.dayTime ? 2d : 0.5d;
    }
}