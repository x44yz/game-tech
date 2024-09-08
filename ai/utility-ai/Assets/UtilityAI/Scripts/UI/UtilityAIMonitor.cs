using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AI.Utility
{
    public class UtilityAIMonitor : MonoBehaviour
    {
        // public static UtilityAIMonitor Inst;

        public PanelAgents panelAgents;
        public PanelActions panelActions;
        public PanelConsiderations panelConsiderations;
        public Button btnMonitor;
        public bool autoShowAgents;

        // private void Awake()
        // {
        //     if (Inst != null)
        //     {
        //         Debug.LogError($"[UTILITY_AI]you cant add more than one monitor.");
        //         return;
        //     }

        //     Inst = this;
        // }

        private void Start()
        {
            panelAgents.Init(this);
            panelActions.Init(this);
            panelConsiderations.Init(this);

            panelAgents.Hide();
            panelActions.Hide();
            panelConsiderations.Hide();

            btnMonitor.onClick.AddListener(()=>{
                if (panelAgents.gameObject.activeSelf)
                    panelAgents.Hide();
                else
                    ShowPanelAgents();
            });

            if (autoShowAgents)
                ShowPanelAgents();
        }

        private void ShowPanelAgents()
        {
            var agents = GameObject.FindObjectsOfType<AgentAI>();
            panelAgents.Show(agents);
        }
    }
}
