using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AI.Utility
{
    public class PanelAgents : MonoBehaviour
    {
        public WidgetAgent tmpWidgetAgent;
        public Button btnClose;

        private UtilityAIMonitor monitor;
        private WidgetAgent selectedWidgetAgent;

        private void Start()
        {
            btnClose.onClick.AddListener(()=>{
                Hide();
            });
        }

        public void Init(UtilityAIMonitor monitor)
        {
            this.monitor = monitor;
            selectedWidgetAgent = null;
            tmpWidgetAgent.Hide();
        }

        public void Show(AgentAI[] agents)
        {
            gameObject.SetActive(true);

            UIUtils.HandleListAllWidgets<WidgetAgent>(tmpWidgetAgent, (wgt) =>
            {
                wgt.Hide();
            });

            if (agents == null)
                return;

            for (int i = 0; i < agents.Length; ++i)
            {
                var agent = agents[i];
                var wgt = UIUtils.GetListValidWidget<WidgetAgent>(i, tmpWidgetAgent);
                wgt.Show(agent);
                wgt.onWidgetClick = OnWidgetAgentClick;
            }
        }

        public void Hide()
        {
            monitor.panelActions.Hide();
            UIUtils.HandleListAllWidgets<WidgetAgent>(tmpWidgetAgent, (wgt) =>
            {
                wgt.Hide();
            });

            DeselectWidgetAgent();
            gameObject.SetActive(false);
        }

        private void OnWidgetAgentClick(WidgetAgent wgt, AgentAI agent)
        {
            DeselectWidgetAgent();
            selectedWidgetAgent = wgt;
            selectedWidgetAgent.Select();
            monitor.panelActions.Show(agent.actionObjs);
        }

        public void DeselectWidgetAgent()
        {
            if (selectedWidgetAgent != null)
                selectedWidgetAgent.Deselect();
            selectedWidgetAgent = null;
        }
    }
}
