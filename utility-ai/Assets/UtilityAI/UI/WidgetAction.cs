using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WidgetAction : MonoBehaviour
{
    public TMP_Text txtName;
    public TMP_Text txtScore;
    public Button btn;

    private Action action;
    public System.Action<WidgetAction, Action> onWidgetClick;
    public System.Action<WidgetAction> onWidgetRefresh;

    private void Start()
    {
        btn.onClick.AddListener(()=>{
            onWidgetClick?.Invoke(this, action);
        });
    }

    public void Show(Action act)
    {
        gameObject.SetActive(true);

        this.action = act;
        act.onScoreChanged += OnActionScoreChanged;

        txtName.text = act.name;
        txtScore.text = act.CurScore.ToString("F2");
    }

    public void Hide()
    {
        if (action != null)
        {
            action.onScoreChanged -= OnActionScoreChanged;
            action = null;
        }

        gameObject.SetActive(false);
    }

    private void OnActionScoreChanged(float v)
    {
        txtScore.text = v.ToString("F2");
        onWidgetRefresh?.Invoke(this);
    }
}
