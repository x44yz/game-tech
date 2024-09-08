using UnityEngine;
using UnityEngine.UI;

namespace AI.Utility
{
    public class PanelActions : MonoBehaviour
    {
        public WidgetAction tmpWidgetAction;
        public Button btnClose;

        private UtilityAIMonitor monitor;
        private WidgetAction selectedWidgetAction;

        private void Start()
        {
            btnClose.onClick.AddListener(()=>{
                Hide();
                monitor.panelAgents.DeselectWidgetAgent();
            });
        }

        public void Init(UtilityAIMonitor monitor)
        {
            this.monitor = monitor;
            selectedWidgetAction = null;
            tmpWidgetAction.Hide();
        }

        public void Show(ActionObj[] actions)
        {
            gameObject.SetActive(true);

            UIUtils.HandleListAllWidgets<WidgetAction>(tmpWidgetAction, (wgt) =>
            {
                wgt.Hide();
            });

            if (actions == null)
                return;

            for (int i = 0; i < actions.Length; ++i)
            {
                var act = actions[i];
                var wgt = UIUtils.GetListValidWidget<WidgetAction>(i, tmpWidgetAction);
                wgt.Show(act);
                wgt.onWidgetClick = OnWidgetActionClick;
                wgt.onWidgetRefresh = OnWidgetActionRefresh;
            }
        }

        public void Hide()
        {
            monitor.panelConsiderations.Hide();
            UIUtils.HandleListAllWidgets<WidgetAction>(tmpWidgetAction, (wgt) =>
            {
                wgt.Hide();
            });

            DeselectWidgetAction();
            gameObject.SetActive(false);
        }

        private void OnWidgetActionClick(WidgetAction wgt, ActionObj act)
        {
            DeselectWidgetAction();
            selectedWidgetAction = wgt;
            selectedWidgetAction.Select();
            monitor.panelConsiderations.Show(act);
        }

        private void OnWidgetActionRefresh(WidgetAction wgt)
        {
            if (wgt != selectedWidgetAction)
                return;
            monitor.panelConsiderations.Refresh();
        }

        public void DeselectWidgetAction()
        {
            if (selectedWidgetAction != null)
                selectedWidgetAction.Deselect();
            selectedWidgetAction = null;
        }
    }
}
