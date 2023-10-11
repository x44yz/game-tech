using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AI.Utility
{
    public class PanelConsiderations : MonoBehaviour
    {
        public WidgetConsideration tmpWidgetConsideration;
        public Button btnClose;

        private UtilityAIMonitor monitor;
        private List<WidgetConsideration> widgets = new List<WidgetConsideration>();

        private void Start()
        {
            btnClose.onClick.AddListener(()=>{
                Hide();
                monitor.panelActions.DeselectWidgetAction();
            });
        }

        public void Init(UtilityAIMonitor monitor)
        {
            this.monitor = monitor;
            tmpWidgetConsideration.Hide();
        }

        public void Show(ActionObj act)
        {
            gameObject.SetActive(true);

            UIUtils.HandleListAllWidgets<WidgetConsideration>(tmpWidgetConsideration, (wgt) =>
            {
                wgt.Hide();
            });

            if (act.considerations == null)
                return;

            widgets.Clear();
            for (int i = 0; i < act.considerations.Length; ++i)
            {
                var wgt = UIUtils.GetListValidWidget<WidgetConsideration>(i, tmpWidgetConsideration);
                widgets.Add(wgt);
                wgt.Show(act, i);
            }
        }

        public void Hide()
        {
            UIUtils.HandleListAllWidgets<WidgetConsideration>(tmpWidgetConsideration, (wgt) =>
            {
                wgt.Hide();
            });

            gameObject.SetActive(false);
        }

        public void Refresh()
        {
            foreach (var wgt in widgets)
            {
                wgt.Refresh();
            }
        }
    }
}