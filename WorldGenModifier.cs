using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace WinterWonderland;

public class WorldGenModifier : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) {
        void replaceTask(string name, GenPass pass) {
            int index = tasks.FindIndex(x => x.Name == name);
            if (index != -1) tasks[index] = pass;
        }

        void wrapTask(string name, GenPass? before = null, GenPass? after = null) {
            int index = tasks.FindIndex(x => x.Name == name);
            if (index == -1) return;
            var pass = tasks[index];
            tasks[index] = new PassLegacy(
                "Wrap: " + name,
                (progress, configuration) =>
                {
                    before?.Apply(progress, configuration);
                    pass.Apply(progress, configuration);
                    after?.Apply(progress, configuration);
                }
            );
        }

        base.ModifyWorldGenTasks(tasks, ref totalWeight);

        replaceTask("Guide", replaceGuideGenPass());
        replaceTask("Generate Ice Biome", generateIceBiomeGenPass());
        wrapTask("Generate Ice Biome", before: extendIceBiomeGenPass());
        replaceTask("Slush", slushGenPass());
        replaceTask("Gems In Ice Biome", gemsInIceBiomeGenPass());
    }

    private static GenPass replaceGuideGenPass() {
        return new PassLegacy(
            "Guide",
            (progress, _) =>
            {
                progress.Set(1f);
                var npc = NPC.NewNPCDirect(new EntitySource_WorldGen(), Main.spawnTileX * 16, Main.spawnTileY * 16, NPCID.Merchant);
                npc.homeTileX = Main.spawnTileX;
                npc.homeTileY = Main.spawnTileY;
                npc.direction = 1;
                npc.homeless = true;
            }
        );
    }

    private static GenPass generateIceBiomeGenPass() {
        return new PassLegacy(
            "Generate Ice Biome",
            (progress, _) =>
            {
                progress.Message = Lang.gen[56].Value;
                WorldGen.snowTop = (int) Main.worldSurface;
                int num826 = WorldGen.lavaLine - WorldGen.genRand.Next(160, 200);
                int num827 = WorldGen.snowOriginLeft;
                int num828 = WorldGen.snowOriginRight;
                int num829 = 10;
                for (int num830 = 0; num830 <= WorldGen.lavaLine - 140; num830++) {
                    progress.Set(num830 / (float) (WorldGen.lavaLine - 140));
                    num827 += WorldGen.genRand.Next(-4, 4);
                    num828 += WorldGen.genRand.Next(-3, 5);
                    if (num830 > 0) {
                        num827 = (num827 + WorldGen.snowMinX[num830 - 1]) / 2;
                        num828 = (num828 + WorldGen.snowMaxX[num830 - 1]) / 2;
                    }

                    if (WorldGen.dungeonSide > 0) {
                        if (WorldGen.genRand.NextBool(4)) {
                            num827++;
                            num828++;
                        }
                    }
                    else if (WorldGen.genRand.NextBool(4)) {
                        num827--;
                        num828--;
                    }

                    WorldGen.snowMinX[num830] = num827;
                    WorldGen.snowMaxX[num830] = num828;
                    for (int num831 = num827; num831 < num828; num831++) {
                        if (num830 < num826) {
                            if (Framing.GetTileSafely(num831, num830).WallType == 2) {
                                Framing.GetTileSafely(num831, num830).WallType = 40;
                            }

                            switch (Framing.GetTileSafely(num831, num830).TileType) {
                                case 0:
                                case 2:
                                case 23:
                                case 40:
                                case 53:
                                    Framing.GetTileSafely(num831, num830).TileType = 147;
                                    break;
                                case 1:
                                    Framing.GetTileSafely(num831, num830).TileType = 161;
                                    break;
                            }
                        }
                        else {
                            num829 += WorldGen.genRand.Next(-3, 4);
                            if (WorldGen.genRand.NextBool(3)) {
                                num829 += WorldGen.genRand.Next(-4, 5);
                                if (WorldGen.genRand.NextBool(3)) {
                                    num829 += WorldGen.genRand.Next(-6, 7);
                                }
                            }

                            if (num829 < 0) {
                                num829 = WorldGen.genRand.Next(3);
                            }
                            else if (num829 > 50) {
                                num829 = 50 - WorldGen.genRand.Next(3);
                            }

                            for (int num832 = num830; num832 < num830 + num829; num832++) {
                                if (Framing.GetTileSafely(num831, num832).WallType == 2) {
                                    Framing.GetTileSafely(num831, num832).WallType = 40;
                                }

                                switch (Framing.GetTileSafely(num831, num832).TileType) {
                                    case 0:
                                    case 2:
                                    case 23:
                                    case 40:
                                    case 53:
                                        Framing.GetTileSafely(num831, num832).TileType = 147;
                                        break;
                                    case 1:
                                        Framing.GetTileSafely(num831, num832).TileType = 161;
                                        break;
                                }
                            }
                        }
                    }

                    if (WorldGen.snowBottom < num830) {
                        WorldGen.snowBottom = num830;
                    }
                }
            }
        );
    }

    private static GenPass extendIceBiomeGenPass() {
        return new PassLegacy(
            "Extend Ice Biome",
            (progress, _) =>
            {
                progress.Set(1f);
                WorldGen.snowOriginLeft = 0;
                WorldGen.snowOriginRight = Main.maxTilesX;
            }
        );
    }

    private static GenPass slushGenPass() {
        return new PassLegacy(
            "Slush",
            (_, _) =>
            {
                for (int num645 = WorldGen.snowTop; num645 < WorldGen.snowBottom; num645++) {
                    for (int num646 = WorldGen.snowMinX[num645]; num646 < WorldGen.snowMaxX[num645]; num646++) {
                        switch (Framing.GetTileSafely(num646, num645).TileType) {
                            case 123:
                                Framing.GetTileSafely(num646, num645).TileType = 224;
                                break;
                            case 59:
                            {
                                bool flag43 = true;
                                int num647 = 3;
                                for (int num648 = num646 - num647; num648 <= num646 + num647; num648++) {
                                    for (int num649 = num645 - num647; num649 <= num645 + num647; num649++) {
                                        if (Framing.GetTileSafely(num648, num649).TileType is 60 or 70 or 71 or 72) {
                                            flag43 = false;
                                            break;
                                        }
                                    }
                                }

                                if (flag43) {
                                    Framing.GetTileSafely(num646, num645).TileType = 224;
                                }

                                break;
                            }
                            case 1:
                                Framing.GetTileSafely(num646, num645).TileType = 161;
                                break;
                        }
                    }
                }
            }
        );
    }

    private static GenPass gemsInIceBiomeGenPass() {
        return new PassLegacy(
            "Gems In Ice Biome",
            (progress, _) =>
            {
                progress.Set(1f);
                for (int num159 = 0; (double)num159 < (double)Main.maxTilesX * 0.25; num159++)
                {
                    int num160 = WorldGen.genRand.Next((int)(Main.worldSurface + Main.rockLayer) / 2, WorldGen.lavaLine);
                    int num161 = WorldGen.genRand.Next(WorldGen.snowMinX[num160], WorldGen.snowMaxX[num160]);
                    if (Framing.GetTileSafely(num161, num160).HasTile && (Framing.GetTileSafely(num161, num160).TileType == 147 || Framing.GetTileSafely(num161, num160).TileType == 161 || Framing.GetTileSafely(num161, num160).TileType == 162 || Framing.GetTileSafely(num161, num160).TileType == 224))
                    {
                        int num162 = WorldGen.genRand.Next(1, 4);
                        int num163 = WorldGen.genRand.Next(1, 4);
                        int num164 = WorldGen.genRand.Next(1, 4);
                        int num165 = WorldGen.genRand.Next(1, 4);
                        int num166 = WorldGen.genRand.Next(12);
                        int num167 = 0;
                        num167 = ((num166 >= 3) ? ((num166 < 6) ? 1 : ((num166 < 8) ? 2 : ((num166 < 10) ? 3 : ((num166 >= 11) ? 5 : 4)))) : 0);
                        for (int num168 = num161 - num162; num168 < num161 + num163; num168++)
                        {
                            for (int num169 = num160 - num164; num169 < num160 + num165; num169++)
                            {
                                if (!Framing.GetTileSafely(num168, num169).HasTile && WorldGen.InWorld(num168, num169))
                                {
                                    WorldGen.PlaceTile(num168, num169, 178, mute: true, forced: false, -1, num167);
                                }
                            }
                        }
                    }
                }
            }
        );
    }
}