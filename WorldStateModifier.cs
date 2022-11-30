using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace WinterWonderland;

public class WorldStateModifier : ModSystem
{
    public override void PreUpdateWorld() {
        base.PreUpdateWorld();

        Filters.Scene["Sandstorm"].Active = false;
    }

    public override void PostUpdateWorld() {
        base.PostUpdateWorld();
        
        Main.xMas = true;
    }
    
    
}