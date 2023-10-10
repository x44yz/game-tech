using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Utility
{
    public class UtilityAIMonitor : MonoBehaviour
    {
        public static UtilityAIMonitor Inst;

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
            panelActions.Init(this);
            panelConsiderations.Init(this);

            panelActions.Hide();
            panelConsiderations.Hide();

            var agent = GameObject.FindObjectOfType<Agent>();
            if (agent != null)
            {
                panelActions.Show(agent.ai);
            }
        }
    }
}
