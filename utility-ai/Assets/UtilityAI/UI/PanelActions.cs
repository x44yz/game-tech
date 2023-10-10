using UnityEngine;

namespace AI.Utility
{
    public class PanelActions : MonoBehaviour
    {
        public WidgetAction tmpWidgetAction;

        private UtilityAIMonitor monitor;
        private WidgetAction selectedWidgetAction;

        public void Init(UtilityAIMonitor monitor)
        {
            this.monitor = monitor;
            selectedWidgetAction = null;
            tmpWidgetAction.Hide();
        }

        public void Show(AgentAI ai)
        {
            gameObject.SetActive(true);

            UIUtils.HandleListAllWidgets<WidgetAction>(tmpWidgetAction, (wgt) =>
            {
                wgt.Hide();
            });

            if (ai.actions == null)
                return;

            for (int i = 0; i < ai.actions.Length; ++i)
            {
                var act = ai.actions[i];
                var wgt = UIUtils.GetListValidWidget<WidgetAction>(i, tmpWidgetAction);
                wgt.Show(act);
                wgt.onWidgetClick = OnWidgetActionClick;
                wgt.onWidgetRefresh = OnWidgetActionRefresh;
            }
        }

        public void Hide()
        {
            UIUtils.HandleListAllWidgets<WidgetAction>(tmpWidgetAction, (wgt) =>
            {
                wgt.Hide();
            });

            gameObject.SetActive(false);
        }

        private void OnWidgetActionClick(WidgetAction wgt, Action act)
        {
            selectedWidgetAction = wgt;
            monitor.panelConsiderations.Show(act);
        }

        private void OnWidgetActionRefresh(WidgetAction wgt)
        {
            if (wgt != selectedWidgetAction)
                return;
            monitor.panelConsiderations.Refresh();
        }
    }
}
