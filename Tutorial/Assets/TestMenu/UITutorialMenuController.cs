using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Reflection;
using Tutorial;

namespace Test
{
    // because UIRaycastFilter, so IPointerUpHandler dont work
    public class UITutorialMenuController : UIBaseController, ITutUIController
    {
        private const float TEXTWRITER_SPEED = 17f;

        public GameObject cutRect;
        public GameObject textAreaRoot;
        public Text textTitle;
        public Text textArea;
        public GameObject frame;
        public GameObject arrowRoot;
        public GameObject arrow;
        public GameObject maskPanel;

        [Header("RUNTIME")]
        public float UnitsPerPixel;
        private bool isAcceptClickEnable;
        private System.Action areaClickCallback = null;
        private bool eligibleForClick = false;
        private UITextTypeWriter textWriter = null;
        private UIRaycastFilter uiRaycastFilter = null;
        private bool isTouchBeginInHA = false; // HA - HighlightArea
        private bool isTouchEndInHA = false;
        private GameObject hightligtAreaTarget = null;
        private GameObject clickArrowTarget = null;

        private bool _highlightAreaAcceptClick = false;
        private bool highlightAreaAcceptClick
        {
            get { return _highlightAreaAcceptClick; }
            set {
                _highlightAreaAcceptClick = value;
                CheckHighlightAreaRaycast();
            }
        }

        private bool _isTextWriting = false;
        private bool isTextWriting
        {
            get { return _isTextWriting; }
            set {
                _isTextWriting = value;
                CheckHighlightAreaRaycast();
            }
        }

        private void Awake()
        {
            cutRect.SetActive(false);
            textAreaRoot.SetActive(false);
            arrowRoot.SetActive(false);

            textWriter = textArea.GetComponent<UITextTypeWriter>();
            uiRaycastFilter = maskPanel.GetComponent<UIRaycastFilter>();
            uiRaycastFilter.enable = false;
        }

        private void OnDestroy()
        {
            hightligtAreaTarget = null;
            clickArrowTarget = null;
        }

        private void Update() 
        {
            if (highlightAreaAcceptClick && !isTextWriting)
            {
                UpdateHighlightAreaClick();
            }

            if (isTextWriting && Input.GetMouseButtonUp(0))
            {
                textWriter.Skip();
            }

            // because ui layout delay, use some performance to get right pos
            if (hightligtAreaTarget != null && Time.frameCount % 10 == 0)
            {
                cutRect.transform.position = GetTargetUIWPos(hightligtAreaTarget);
            }

            if (clickArrowTarget != null && Time.frameCount % 10 == 0)
            {
                arrowRoot.transform.position = GetTargetUIWPos(clickArrowTarget);
            }
        }

        private void UpdateHighlightAreaClick()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                isTouchBeginInHA = IsScreenPointInHighlightArea(Input.mousePosition);
            }
            if (Input.GetMouseButtonUp(0))
            {
                isTouchEndInHA = IsScreenPointInHighlightArea(Input.mousePosition);
            }
#else
            for (int i = 0; i < Input.touchCount; ++i)
            {
                var th = Input.touches[i];
                if (!isTouchBeginInHA && th.phase == TouchPhase.Began)
                {
                    isTouchBeginInHA = IsScreenPointInHighlightArea(th.position);
                }
                if (!isTouchEndInHA && th.phase == TouchPhase.Ended)
                {
                    isTouchEndInHA = IsScreenPointInHighlightArea(th.position);
                }
            }
#endif

            if (Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                // if scrollview + button, drag scrollview, shouldn't finish tutAction
                var inputModule = EventSystem.current.currentInputModule;
                if (inputModule != null)
                {
                    var method = inputModule.GetType().GetMethod("GetLastPointerEventData", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (method != null)
                    {
#if UNITY_EDITOR
                        object[] pp = {-1};
#else
                        object[] pp = {0};
#endif
                        var eventdata = method.Invoke(inputModule, pp) as PointerEventData;
                        if (eventdata != null)
                        {
                            // Debugger.Log("xx-- elig click 1 > " + eventdata.eligibleForClick);
                            eligibleForClick = eventdata.eligibleForClick;
                        }
                    }
                }
            }

            if (isTouchBeginInHA && isTouchEndInHA && eligibleForClick)
            {
                isTouchBeginInHA = false;
                isTouchEndInHA = false;
                eligibleForClick = false;
                highlightAreaAcceptClick = false;
                if (areaClickCallback != null)
                    areaClickCallback.Invoke();
                areaClickCallback = null;
            }

#if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0))
#else
            if (Input.touchCount == 0)
#endif
            {
                isTouchBeginInHA = false;
                isTouchEndInHA = false;
            }
        }

        private Vector3 GetTargetUIWPos(GameObject targetObj)
        {
            if (targetObj == null) return Vector3.zero;
            var targetRT = targetObj.GetComponent<RectTransform>();
            var poffset = new Vector2(0.5f, 0.5f) - targetRT.pivot;
            var woffset = new Vector3(targetRT.sizeDelta.x * poffset.x, targetRT.sizeDelta.y * poffset.y, 0f) * UnitsPerPixel;
            return targetObj.transform.position + woffset;
        }

        private bool IsScreenPointInHighlightArea(Vector2 screenPoint)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(cutRect.GetComponent<RectTransform>(), screenPoint);
        }

#region ITutUIController
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public bool IsAcceptClickEnable => isAcceptClickEnable;

        public void ShowHighlightArea(Rect rect, Vector2 anchorMin, Vector2 anchorMax, 
                                    bool acceptInput, string targetName)
        {
            hightligtAreaTarget = null;
            cutRect.SetActive(true);

            var rectTF = cutRect.GetComponent<RectTransform>();
            var targetObj = TutorialUtils.GetTarget(targetName);
            if (targetObj != null)
            {
                hightligtAreaTarget = targetObj;
                rectTF.anchorMin = new Vector2(0.5f, 0.5f);
                rectTF.anchorMax = new Vector2(0.5f, 0.5f);
                rectTF.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.width);
                rectTF.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.height);
                rectTF.transform.position = GetTargetUIWPos(hightligtAreaTarget);
            }
            else
            {
                float minX = 0;
                float maxX = 0;
                if (Mathf.Approximately(anchorMin.x, anchorMax.x))
                {
                    minX = rect.x - rect.width * 0.5f;
                    maxX = rect.x + rect.width * 0.5f;
                }
                else
                {
                    minX = rect.x;
                    maxX = -rect.width;
                }

                float minY = 0;
                float maxY = 0;
                if (Mathf.Approximately(anchorMin.y, anchorMax.y))
                {
                    minY = rect.y - rect.height * 0.5f;
                    maxY = rect.y + rect.height * 0.5f;
                }
                else
                {
                    minY = rect.y;
                    maxY = -rect.height;
                }

                rectTF.offsetMin = new Vector2(minX, minY);
                rectTF.offsetMax = new Vector2(maxX, maxY);

                rectTF.anchorMin = anchorMin;
                rectTF.anchorMax = anchorMax;
            }

            highlightAreaAcceptClick = acceptInput;
        }

        public void HideHighlightArea()
        {
            hightligtAreaTarget = null;
            highlightAreaAcceptClick = false;
            cutRect.SetActive(false);
        }

        public void ShowTextArea(string title, string text, System.Action typeEndCallback)
        {
            textAreaRoot.SetActive(true);

            textTitle.text = title;
            textArea.text = text;
            if (textWriter != null)
            {
                isTextWriting = true;
                isAcceptClickEnable = false;

                textWriter.Play(textArea.text, TEXTWRITER_SPEED, ()=>{
                    isTextWriting = false;
                    isAcceptClickEnable = true;
                    typeEndCallback?.Invoke();
                });
            }
        }

        public void HideTextArea()
        {
            textAreaRoot.SetActive(false);
        }

        public void SetHighlightAreaAcceptClick(bool enable, System.Action clickCallback)
        {
            highlightAreaAcceptClick = true;
            areaClickCallback = clickCallback;
        }

        public void ShowClickArrow(TutActionShowClickArrow.ArrowType arrowType, 
            Vector2 pos, Vector2 anchorMin, Vector2 anchorMax, Vector2 scale, string targetName)
        {
            clickArrowTarget = null;
            arrowRoot.SetActive(true);

            var arrowRootTF = arrowRoot.GetComponent<RectTransform>();
            var targetObj = TutorialUtils.GetTarget(targetName);
            if (targetObj != null)
            {
                clickArrowTarget = targetObj;
                arrowRootTF.anchorMin = new Vector2(0.5f, 0.5f);
                arrowRootTF.anchorMax = new Vector2(0.5f, 0.5f);
                arrowRootTF.transform.position = GetTargetUIWPos(clickArrowTarget);
            }
            else
            {
                arrowRootTF.anchorMin = anchorMin;
                arrowRootTF.anchorMax = anchorMax;
                arrowRootTF.anchoredPosition = new Vector2(pos.x, pos.y);
            }
            arrowRootTF.localScale = scale;

            var arrowTF = arrow.GetComponent<RectTransform>();
            if (arrowType == TutActionShowClickArrow.ArrowType.ArrowUp)
                arrowTF.localRotation = Quaternion.identity;
            else if (arrowType == TutActionShowClickArrow.ArrowType.ArrowDown)
                arrowTF.localRotation = Quaternion.AngleAxis(180, Vector3.forward);
            else if (arrowType == TutActionShowClickArrow.ArrowType.ArrowLeft)
                arrowTF.localRotation = Quaternion.AngleAxis(90, Vector3.forward);
             else if (arrowType == TutActionShowClickArrow.ArrowType.ArrowRight)
                arrowTF.localRotation = Quaternion.AngleAxis(270, Vector3.forward);
        }

        public void HideClickArrow()
        {
            clickArrowTarget = null;
            arrowRoot.SetActive(false);
        }
#endregion // ITutUIController
    
        private void CheckHighlightAreaRaycast()
        {
            if (uiRaycastFilter != null)
                uiRaycastFilter.enable = highlightAreaAcceptClick && !isTextWriting;
        }
    }
}