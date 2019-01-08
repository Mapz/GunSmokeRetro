using System.Collections.Generic;

public class LevelConfig {
    public string levelNum;
    public string levelName;
    public List<string> levelLoops;
    public string initLoop;
    public string bossLoop;
    public string bossUnitName;
    public string bossName;
    public string bossWeaponName;
}
public static class LevelsConfig {
    public static Dictionary<int, LevelConfig> Levels = new Dictionary<int, LevelConfig> ();

    public static void Init () {
        // Level1
        LevelConfig level1 = new LevelConfig ();
        level1.levelNum = "1";
        level1.levelName = "TOWN OF HICKSVILLE";
        level1.bossName = "BANDIT BILL";
        level1.bossWeaponName = "RIFLE";
        level1.bossLoop = "Level_1_Loop_1";
        level1.initLoop = "Level_1_Loop_1";
        level1.bossUnitName = "Level_1_Boss_1";
        level1.levelLoops = new List<string> ();
        level1.levelLoops.Add ("Level_1_Loop_1");
        Levels.Add (1, level1);
    }

    public static LevelConfig GetCurLevelConfig () {
        LevelConfig retLevel = null;
        Levels.TryGetValue (GameVars.CurLevel, out retLevel);
        return retLevel;
    }
}