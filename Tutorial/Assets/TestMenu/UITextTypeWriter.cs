using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    [RequireComponent( typeof( Text ) )]
    public class UITextTypeWriter : MonoBehaviour
    {
        public static string[] richTextStartSymbols = new string[]{"<color=#", "<b>", "<size="};
        public static string[] richTextEndSymbols = new string[]{"</color>", "</b>", "</size>"};

        [SerializeField] private Text m_textUI = null;

        private string m_parsedText;
        private Action m_onComplete;
        private bool isPlaying = false;
        private int showCount = 0;
        private float showSpeed = 0f;
        private string richTextEndFlag = "";
        private float showCountTick = 0f;

        private void Awake()
        {
            m_textUI = GetComponent<Text>();
        }

        private void OnDestroy()
        {
            m_onComplete = null;
        }

        private void Update()
        {
            if (isPlaying)
            {
                showCountTick += showSpeed * Time.deltaTime;
                if (showCountTick >= 1f)
                {
                    showCount += 1;
                    showCountTick -= 1f;
                    OnUpdate();
                }
                if (showCount >= m_parsedText.Length)
                {
                    isPlaying = false;
                    OnComplete();
                }
            }
        }

        public void Play( string text, float speed, Action onComplete )
        {
            m_textUI.text = text; // create all text texture
            m_onComplete = onComplete;

            showSpeed = speed;
            m_parsedText = text;
            isPlaying = true;
            richTextEndFlag = "";

            showCount = 0;
            showCountTick = 0f;
            OnUpdate();
        }

        public void Skip( bool withCallbacks = true )
        {
            showCount = m_parsedText.Length ;
            richTextEndFlag = "";
            OnUpdate();

            if ( !withCallbacks ) return;

            if ( m_onComplete != null )
            {
                m_onComplete.Invoke();
            }

            m_onComplete = null;
        }

        public void Pause()
        {
            isPlaying = false;
        }

        public void Resume()
        {
            isPlaying = true;
        }

        private void OnUpdate()
        {
            var count = Mathf.Clamp(showCount, 0, m_parsedText.Length);
            if (count >= m_parsedText.Length)
                richTextEndFlag = "";

            int idx = Mathf.Max(0, count - 1);
            if (m_parsedText[idx] == '<')
            {
                bool isStartSymbol = false;
                for (int i = 0; i < richTextStartSymbols.Length; ++i)
                {
                    string symb = richTextStartSymbols[i];
                    int len = richTextStartSymbols[i].Length;
                    if (m_parsedText.Substring(idx, len).Equals(symb))
                    {
                        isStartSymbol = true;

                        int endIdx = m_parsedText.IndexOf('>', idx);
                        showCount += (endIdx - idx);
                        count += (endIdx - idx);

                        richTextEndFlag = richTextEndSymbols[i] + richTextEndFlag;
                        break;
                    }
                }

                if (!isStartSymbol)
                {
                    for (int i = 0; i < richTextEndSymbols.Length; ++i)
                    {
                        string symb = richTextEndSymbols[i];
                        int len = richTextEndSymbols[i].Length;
                        if (m_parsedText.Substring(idx, len).Equals(symb))
                        {
                            showCount += len;
                            count += len;
                            int fidx = richTextEndFlag.IndexOf(symb);
                            if (fidx != -1)
                            {
                                richTextEndFlag = richTextEndFlag.Substring(fidx + len);
                            }
                            break;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(richTextEndFlag) == false)
                m_textUI.text = m_parsedText.Substring(0, count) + richTextEndFlag;
            else
                m_textUI.text = m_parsedText.Substring(0, count);
        }

        private void OnComplete()
        {
            if ( m_onComplete != null )
            {
                m_onComplete.Invoke();
            }

            m_onComplete = null;
        }
    }
}