using System.Collections.Generic;
using UnityEngine;

namespace AI.Utility
{
    public class PanelAgents : MonoBehaviour
    {
        public WidgetAgent tmpWidgetAgent;

        private UtilityAIMonitor monitor;
        private WidgetAgent selectedWidgetAgent;

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
            UIUtils.HandleListAllWidgets<WidgetAgent>(tmpWidgetAgent, (wgt) =>
            {
                wgt.Hide();
            });

            if (selectedWidgetAgent != null)
                selectedWidgetAgent.Deselect();
            selectedWidgetAgent = null;
            gameObject.SetActive(false);
        }

        private void OnWidgetAgentClick(WidgetAgent wgt, AgentAI agent)
        {
            if (selectedWidgetAgent != null)
                selectedWidgetAgent.Deselect();
            selectedWidgetAgent = wgt;
            selectedWidgetAgent.Select();
            monitor.panelActions.Show(agent.config.actions);
        }
    }
}
