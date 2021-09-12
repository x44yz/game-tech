
namespace AI.FSM
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class FSMTransitionMethod : System.Attribute  
    {
        private string name; // fsm name
    
        public FSMTransitionMethod(string name)
        {
            this.name = name;
        }
    }
}