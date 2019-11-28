
namespace FSM
{
	public class State
	{
		public string Type { get { return GetType().ToString(); } }

		public virtual void OnEnter(string from)
		{
		}

		public virtual void OnExit(string to)
		{
		}

		public virtual void Update()
		{
		}
	}
}