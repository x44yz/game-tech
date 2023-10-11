using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Utility
{
    public class UtilityAIMonitor : MonoBehaviour
    {
        public static UtilityAIMonitor Inst;

        public PanelAgents panelAgents;
        public PanelActions panelActions;
        public PanelConsiderations panelConsiderations;

        private void Awake()
        {
            if (Inst != null)
            {
                Debug.LogError($"[UTILITY_AI]you cant add more than one monitor.");
                return;
            }

            Inst = this;
        }

        private void Start()
        {
            panelAgents.Init(this);
            panelActions.Init(this);
            panelConsiderations.Init(this);

            panelActions.Hide();
            panelConsiderations.Hide();

            var agents = GameObject.FindObjectsOfType<AgentAI>();
            if (agents != null)
            {
                panelAgents.Show(agents);
            }
        }
    }
}
