
namespace AI.FSM
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class FSMAttrTransitionMethod : System.Attribute  
    {
        private string name; // fsm name
    
        public FSMAttrTransitionMethod(string name)
        {
            this.name = name;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class FSMAttrTransitionClass : System.Attribute  
    {
        private string name; // fsm name
    
        public FSMAttrTransitionClass(string name)
        {
            this.name = name;
        }
    }
}