
namespace AI.FSM
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class FSMAttrStateClass : System.Attribute  
    {
        private string name; // fsm name
    
        public FSMAttrStateClass(string name)
        {
            this.name = name;
        }
    }
}