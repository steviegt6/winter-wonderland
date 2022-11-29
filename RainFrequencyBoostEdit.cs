using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace WinterWonderland;

public class RainFrequencyBoostEdit : ModSystem
{
    public override void OnModLoad() {
        base.OnModLoad();

        IL.Terraria.Main.UpdateTime += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(x => x.MatchCall<Main>("StartRain"));
            c.GotoPrev(x => x.MatchLdcR8(5.75f));
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_R8, 2.5f);
            c.GotoNext(x => x.MatchLdcR8(4.25f));
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_R8, 1.25f);
        };
    }
}