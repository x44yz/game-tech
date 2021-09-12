
namespace AI.FSM
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class FSMStateClass : System.Attribute  
    {
        private string name; // fsm name
    
        public FSMStateClass(string name)
        {
            this.name = name;
        }
    }
}