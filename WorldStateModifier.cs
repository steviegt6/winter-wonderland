using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace WinterWonderland;

public class WorldStateModifier : ModSystem
{
    public override void Load() {
        base.Load();
        
        IL.Terraria.Main.UpdateTime += il =>
        {
            var c = new ILCursor(il);
            c.GotoNext(MoveType.After, x => x.MatchStsfld<Main>("eclipse"));
            c.Emit(OpCodes.Call, typeof(Main).GetMethod("UpdateTime_SpawnTownNPCs", BindingFlags.Static | BindingFlags.NonPublic));
        };
    }

    public override void PreUpdateWorld() {
        base.PreUpdateWorld();

        Filters.Scene["Sandstorm"].Active = false;
    }

    public override void PostUpdateWorld() {
        base.PostUpdateWorld();
        
        Main.xMas = true;
    }
}