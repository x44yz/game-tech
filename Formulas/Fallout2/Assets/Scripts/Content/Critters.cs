using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// typedef struct CritterProto {
//     int pid; // pid
//     int messageId; // message_num
//     int fid; // fid
//     int lightDistance; // light_distance
//     int lightIntensity; // light_intensity
//     int flags; // flags
//     int extendedFlags; // flags_ext
//     int sid; // sid
//     CritterProtoData data; // d
//     int headFid; // head_fid
//     int aiPacket; // ai_packet
//     int team; // team_num
// } CritterProto;

public class CiritterCfg : ICSVParser
{
    public int pid; // pid
    public int flags; // d.flags
  public int[] baseStats = new int[35]; // d.stat_base
        public int[] bonusStats = new int[35]; // d.stat_bonus
        public int[] skills = new int[18]; // d.stat_points
        public int bodyType; // d.body
        public int experience;
        public int killType;
        // Looks like this is the "native" damage type when critter is unarmed.
        public int damageType;

    public void ParseCSV(CSVLoader loader)
    {

    }
}

public static class Cirtters
{
    public static List<CiritterCfg> ciritterCfgs;

    public static void Init()
    {
        ciritterCfgs = CSVLoader.LoadCSV<CiritterCfg>("");
    }
}
