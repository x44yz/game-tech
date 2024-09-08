// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace FSM
// {
// 	public class StateMachine
// 	{
// 		private State m_prevState = null;
// 		private State m_currState = null;
// 		private Dictionary<string, State> states = new Dictionary<string, State>();

// 		public State CurrState { get { return m_currState; } }
// 		public State PrevState { get { return m_prevState; } }

// 		public void Update()
// 		{
// 			if (m_currState != null)
// 				m_currState.Execute();
// 		}

// 		public void AddState(State state)
// 		{
// 			AddState(state.GetType().ToString(), state);
// 		}

// 		public void AddState(string stype, State state)
// 		{
// 			if (states.ContainsKey(stype))
// 			{
// 				Debug.LogError("exist same state > " + stype);
// 				return;
// 			}

// 			states[stype] = state;
// 		}

// 		public State ChangeState(string stype)
// 		{
// 			if (m_currState != null && 
// 					m_currState.Type == stype)
// 			{
// 				Debug.LogWarning("current state is still running.");
// 				return m_currState;
// 			}

// 			if (!states.ContainsKey(stype))
// 			{
// 				Debug.LogError("failed to find state > " + stype);
// 				return null;
// 			}

// 			if (m_currState != null)
// 				m_currState.Exit(stype);
// 			m_prevState = m_currState;

// 			m_currState = states[stype];
// 			m_currState.Enter(m_prevState.Type);

// 			return m_currState;
// 		}

// 		public void ChangeState(State newState)
// 		{
// 			string stype = newState.Type;
// 			if (!states.ContainsKey(stype))
// 			{
// 				AddState(stype, newState);
// 			}

// 			this.ChangeState(stype);
// 		}

// 		public bool IsInState(string stype)
// 		{
// 			return m_currState != null && m_currState.Type == stype;
// 		}
// 	}
// }
