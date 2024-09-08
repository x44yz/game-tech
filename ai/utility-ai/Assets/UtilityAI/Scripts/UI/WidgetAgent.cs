using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace AI.Utility
{
    public class WidgetAgent : MonoBehaviour
    {
        public TMP_Text txtName;
        public TMP_Text txtAction;
        public Button btn;
        public Image imgBG;
        public Color selectedBgColor;
        public Color normalBgColor;

        private AgentAI agent;
        public System.Action<WidgetAgent, AgentAI> onWidgetClick;

        private void Start()
        {
            btn.onClick.AddListener(() =>
            {
                onWidgetClick?.Invoke(this, agent);
            });
        }

        public void Show(AgentAI agent)
        {
            gameObject.SetActive(true);

            this.agent = agent;
            agent.onActionChanged += OnActionChanged;

            txtName.text = agent.name;
            SetAction(agent.curActionObj);
        }

        public void Hide()
        {
            if (agent != null)
            {
                agent.onActionChanged -= OnActionChanged;
                agent = null;
            }

            gameObject.SetActive(false);
        }

        private void OnActionChanged(ActionObj act)
        {
            SetAction(act);
        }

        public void Select()
        {
            imgBG.color = selectedBgColor;
        }

        public void Deselect()
        {
            imgBG.color = normalBgColor;
        }

        private void SetAction(ActionObj act)
        {
            if (act != null)
                txtAction.text = act.dbgName;
            else
                txtAction.text = "null";
        }
    }
}