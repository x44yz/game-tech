using System.Collections.Generic;

namespace AI.FSMTool
{
    public class FTNodeCfg
    {
        // format: category/name
        public string path; // right-click menu path
        public FTNodeProperty[] defaultProperties = null;
    }

    public class FTNodePropertyCfg
    {
        public enum ValueType
        {
            StringEnum, // use , to split
        }

        public string name;
        public ValueType valueType;
        public string value;

        public string[] GetStringEnumValues() { return value.Split(','); }
    }

    public class BTEditorDefine
    {
        public static FTNodePropertyCfg GetFTNodePropertyCfg(string name, string category)
        {
            // NOTE:
            // hard code
            if (name == "bbKey")
            {
                name = category.ToLower() == BTDef.B3_CATEGORY_ACTION ? "setBBKey" : "checkBBKey";
            }
            return NodePropertyCfgs.Find(x => x.name.ToLower() == name.ToLower());
        }

        public static FTNodeCfg GetFTNodeCfg(string name)
        {
            return NodeCfgs.Find(x => x.path.ToLower().Contains(name.ToLower()));
        }

        private static string DefStringEnumVal(string propertyName)
        {
            return GetFTNodePropertyCfg(propertyName, "")?.GetStringEnumValues()[0];
        }

        public static List<FTNodePropertyCfg> NodePropertyCfgs = new List<FTNodePropertyCfg>()
        {
            new FTNodePropertyCfg(){ 
                name = "moveType",
                valueType = FTNodePropertyCfg.ValueType.StringEnum,
                value = "walk,run"
            },
            new FTNodePropertyCfg(){ 
                name = "locationType",
                valueType = FTNodePropertyCfg.ValueType.StringEnum,
                value = "normal,lootBox"
            },
            new FTNodePropertyCfg(){
                name = "pawnStatus",
                valueType = FTNodePropertyCfg.ValueType.StringEnum,
                value = "idle,follow,hit,assist,gohome"
            },
            new FTNodePropertyCfg(){
                name = "eventName",
                valueType = FTNodePropertyCfg.ValueType.StringEnum,
                value = "memberInHit"
            },
            new FTNodePropertyCfg(){
                name = "target",
                valueType = FTNodePropertyCfg.ValueType.StringEnum,
                value = "allMemberNotInHit"
            },
            new FTNodePropertyCfg(){
                name = "targetType",
                valueType = FTNodePropertyCfg.ValueType.StringEnum,
                value = "attack,assist,sound"
            },
            new FTNodePropertyCfg(){
                name = "source",
                valueType = FTNodePropertyCfg.ValueType.StringEnum,
                value = "event,soundTarget"
            },
            new FTNodePropertyCfg(){
                name = "eventArg",
                valueType = FTNodePropertyCfg.ValueType.StringEnum,
                value = "hitPawn,targetPawn"
            },
            new FTNodePropertyCfg(){
                name = "findStrategy",
                valueType = FTNodePropertyCfg.ValueType.StringEnum,
                value = "oneWay,round"
            },
            new FTNodePropertyCfg(){
                name = "checkBBKey",
                valueType = FTNodePropertyCfg.ValueType.StringEnum,
                value = "isInChaseBack,attackTarget,soundTarget,assistTarget,followTarget"
            },
            new FTNodePropertyCfg(){
                name = "setBBKey",
                valueType = FTNodePropertyCfg.ValueType.StringEnum,
                value = "isInChaseBack"
            },
            new FTNodePropertyCfg(){
                name = "compOperator",
                valueType = FTNodePropertyCfg.ValueType.StringEnum,
                value = "lessEqual,greaterEqual,greaterAndLess"
            },
        };

        public static List<FTNodeCfg> NodeCfgs = new List<FTNodeCfg>()
        {
            // Composite
            new FTNodeCfg(){ path = "Composite/Parallel" },
            new FTNodeCfg(){ path = "Composite/Selector" },
            new FTNodeCfg(){ path = "Composite/Sequence" },
            new FTNodeCfg(){ path = "Composite/Random" },
            new FTNodeCfg(){ path = "Composite/RandomSelector" },
            new FTNodeCfg(){ path = "Composite/RandomSequence" },

            // Decorator
            new FTNodeCfg(){ path = "Decorator/Inverter" },
            new FTNodeCfg(){ path = "Decorator/Success" },
            new FTNodeCfg(){ path = "Decorator/Failure" },
            new FTNodeCfg(){ path = "Decorator/UntilSuccess" },
            new FTNodeCfg(){ path = "Decorator/UntilFailure" },
            new FTNodeCfg(){ path = "Decorator/Loop",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("count", 1),
                    new FTNodeProperty("forever", 0),
                    new FTNodeProperty("endOnFailure", 0)
                },
            },

            // Action
            new FTNodeCfg(){ path = "Action/Idle" },
            new FTNodeCfg(){ path = "Action/Attack" },
            new FTNodeCfg(){ path = "Action/Crouch" },
            new FTNodeCfg(){ path = "Action/Reload" },
            new FTNodeCfg(){ path = "Action/Loot" },
            new FTNodeCfg(){ path = "Action/TurnToTarget",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("targetType", DefStringEnumVal("targetType")),
                    new FTNodeProperty("rotateSpeed", 720.0f)
                },
            },
            new FTNodeCfg(){ path = "Action/MoveToTarget",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("moveType", DefStringEnumVal("moveType"))
                },
            },
            new FTNodeCfg(){ path = "Action/MoveToLocation",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("moveType", DefStringEnumVal("moveType")),
                    new FTNodeProperty("locationType", DefStringEnumVal("locationType"))
                },
            },
            new FTNodeCfg(){ path = "Action/FindPathLocation",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("findStrategy", DefStringEnumVal("findStrategy"))
                },
            },
            new FTNodeCfg(){ path = "Action/FindTarget",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("viewRange", -1.0f),
                    new FTNodeProperty("viewSector", -1.0f),
                    new FTNodeProperty("useRaycast", 1)
                },
            },
            new FTNodeCfg(){ path = "Action/FindSafeLocation" },
            new FTNodeCfg(){ path = "Action/FindLootBox",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("range", 5f)
                },
            },
            new FTNodeCfg(){ path = "Action/FindFriendlyTarget",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("range", 50f),
                    new FTNodeProperty("radius", 5)
                }
            },
            new FTNodeCfg(){ path = "Action/FindHatredTarget" },
            new FTNodeCfg(){ path = "Action/FollowTarget" },
            new FTNodeCfg(){ path = "Action/Rotate",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("angle", 45),
                    new FTNodeProperty("rotateSpeed", 90)
                },
            },
            new FTNodeCfg(){ path = "Action/SetStatus",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("pawnStatus", DefStringEnumVal("pawnStatus"))
                }
            },
            new FTNodeCfg(){ path = "Action/Wait",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("waitTime", 1f), // float - seconds
                    new FTNodeProperty("randomWait", 0), // bool
                    new FTNodeProperty("randomWaitMin", 0f), // float
                    new FTNodeProperty("randomWaitMax", 0f), // float
                }
            },
            new FTNodeCfg(){ path = "Action/SendEvent",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("eventName", DefStringEnumVal("eventName")),
                    new FTNodeProperty("target", DefStringEnumVal("target"))
                }
            },
            new FTNodeCfg(){ path = "Action/SetTarget",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("targetType", DefStringEnumVal("targetType")),
                    new FTNodeProperty("source", DefStringEnumVal("source")),
                    new FTNodeProperty("eventName", DefStringEnumVal("eventName")),
                    new FTNodeProperty("eventArg", DefStringEnumVal("eventArg"))
                }
            },
            new FTNodeCfg(){ path = "Action/SetMemberStatus", // for all member
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("pawnStatus", DefStringEnumVal("pawnStatus"))
                }
            },
            new FTNodeCfg(){ path = "Action/MoveToDirection",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("dirX", 0f),
                    new FTNodeProperty("dirZ", 0f),
                    new FTNodeProperty("distMin", 0f),
                    new FTNodeProperty("distMax", 0f),
                    new FTNodeProperty("moveType", DefStringEnumVal("moveType"))
                }
            },
            new FTNodeCfg(){ path = "Action/GetSoundTarget" },
            new FTNodeCfg(){ path = "Action/SetBBValue",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("bbKey", DefStringEnumVal("setBBKey")),
                    new FTNodeProperty("isTrue", 1),
                }
            },
            new FTNodeCfg(){ path = "Action/PlayWarning" },
            new FTNodeCfg(){ path = "Action/ClearWarning" },
            new FTNodeCfg(){ path = "Action/GetBunkerLocation" },
            new FTNodeCfg(){ path = "Action/SetHatredTarget",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("order", 1),
                }
            },

            // Condition
            new FTNodeCfg(){ path = "Condition/IsTargetInAttackRange",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("useRaycast", 1)
                }
            },
            new FTNodeCfg(){ path = "Condition/IsTargetInSight",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("targetType", DefStringEnumVal("targetType")),
                    new FTNodeProperty("useRaycast", 1),
                    new FTNodeProperty("viewRange", -1.0f),
                    new FTNodeProperty("viewSector", -1.0f),
                }
            },
            new FTNodeCfg(){ path = "Condition/IsTargetInChaseRange",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("useRaycast", 1)
                }
            },
            new FTNodeCfg(){ path = "Condition/IsInSafeArea" },
            new FTNodeCfg(){ path = "Condition/IsInDangerous",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("duration", 5f)
                }
            },
            new FTNodeCfg(){ path = "Condition/IsLowAmmo" },
            new FTNodeCfg(){ path = "Condition/IsSelfLowHp",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("percent", 0.5f)
                }
            },
            new FTNodeCfg(){ path = "Condition/IsTargetLowHp",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("percent", 0.5f)
                }
            },
            new FTNodeCfg(){ path = "Condition/IsTargetInBunker",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("targetType", DefStringEnumVal("targetType")),
                    new FTNodeProperty("offset", -0.2f)
                }
            },
            new FTNodeCfg(){ path = "Condition/IsTargetFacesMe",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("angle", 30)
                }
            },
            new FTNodeCfg(){ path = "Condition/CanPathMove"},
            new FTNodeCfg(){ path = "Condition/IsStatus",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("pawnStatus", DefStringEnumVal("pawnStatus"))
                }
            },
            new FTNodeCfg(){ path = "Condition/IsMemberStatus",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("allMember", 0),
                    new FTNodeProperty("pawnStatus", DefStringEnumVal("pawnStatus"))
                }
            },
            new FTNodeCfg(){ path = "Condition/RandomProbability",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("successProbability", 0.5f),
                }
            },
            new FTNodeCfg(){ path = "Condition/HasReceivedEvent",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("eventName", DefStringEnumVal("eventName")),
                }
            },
            new FTNodeCfg(){ path = "Condition/CheckBBValue",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("bbKey", DefStringEnumVal("checkBBKey")),
                    new FTNodeProperty("isSet", 1),
                }
            },
            new FTNodeCfg(){ path = "Condition/CheckTargetHatred",
                defaultProperties = new FTNodeProperty[]
                {
                    new FTNodeProperty("order", 1),
                    new FTNodeProperty("compOperator", DefStringEnumVal("compOperator")),
                    new FTNodeProperty("value1", 0f),
                    new FTNodeProperty("value2", 1f)
                }
            },
            new FTNodeCfg(){ path = "Condition/HasPatrolPath" },
        };
    }
}