using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace df
{
    public class dfTest : MonoBehaviour
    {
        public enum TestMode
        {
            PLAYER_2_MONSTER,
            PLAYER_2_PLAYER,
        }

        public static dfTest Inst;

        public TestMode testMode;
        public Text dmgTextTmp;
        public dfPlayer leftPlayer;
        public dfPlayer rightPlayer;
        public dfMonster rightMonster;
        
        public Queue<Text> dmgTextPool = new Queue<Text>();

        private void Awake() 
        {
            Inst = this;

            dmgTextTmp.gameObject.SetActive(false);

            leftPlayer.gameObject.SetActive(false);
            rightPlayer.gameObject.SetActive(false);
            rightMonster.gameObject.SetActive(false);
            if (testMode == TestMode.PLAYER_2_MONSTER)
            {
                leftPlayer.gameObject.SetActive(true);
                rightMonster.gameObject.SetActive(true);
            }
            else if (testMode == TestMode.PLAYER_2_PLAYER)
            {
                leftPlayer.gameObject.SetActive(true);
                rightPlayer.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("not implement test mode > " + testMode);
            }
        }

        private void Update() 
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (testMode == TestMode.PLAYER_2_MONSTER)
                    leftPlayer.AttackMonster(rightMonster);
                else if (testMode == TestMode.PLAYER_2_PLAYER)
                    leftPlayer.AttackPlayer(rightPlayer);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                if (testMode == TestMode.PLAYER_2_MONSTER)
                    rightMonster.AttackPlayer(leftPlayer);
                else if (testMode == TestMode.PLAYER_2_PLAYER)
                    rightPlayer.AttackPlayer(leftPlayer);
            }
        }

        private Text GetFreeDmgText()
        {
            Text dmgText = null;
            if (dmgTextPool.Count > 0)
            {
                dmgText = dmgTextPool.Dequeue();
            }
            else
            {
                dmgText = Instantiate(dmgTextTmp);
                dmgText.transform.SetParent(dmgTextTmp.transform.parent);
            }
            return dmgText;
        }

        public void ShowUnitText(Unit target, string info)
        {
            var dmgText = GetFreeDmgText();
            dmgText.gameObject.SetActive(true);
            dmgText.text = info;
            var spos = Camera.main.WorldToScreenPoint(target.transform.position);
            dmgText.GetComponent<RectTransform>().anchoredPosition = new Vector2(spos.x, spos.y) + new Vector2(Random.Range(-40f, 40f), 120f);
            dmgText.transform.DOLocalMoveY(dmgText.transform.position.y + 60f, 0.5f).OnComplete(() => {
                dmgText.gameObject.SetActive(false);
                dmgTextPool.Enqueue(dmgText);
            });
        }

        public void ShowDamageText(Unit target, int dmg)
        {
            if (dmg == 0)
                ShowUnitText(target, "MISS");
            else
                ShowUnitText(target, "+" + dmg.ToString());
        }

        public void ShowMiss(Unit target)
        {
            ShowUnitText(target, "MISS");
        }
    }
}

