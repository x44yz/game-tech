using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MagicTower
{
    public class mtTest : MonoBehaviour
    {
        public static mtTest Inst;

        public Text dmgTextTmp;
        
        public Queue<Text> dmgTextPool = new Queue<Text>();

        private void Awake() 
        {
            Inst = this;

            dmgTextTmp.gameObject.SetActive(false);
        }

        private void Start() 
        {

        }

        private void Update() 
        {
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

