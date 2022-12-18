using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace d2
{
    public class d2Test : MonoBehaviour
    {
        public static d2Test Inst;

        public Text dmgTextTmp;
        public Queue<Text> dmgTextPool = new Queue<Text>();

        private void Awake() 
        {
            Inst = this;

            dmgTextTmp.gameObject.SetActive(false);
        }

        public void ShowDamageText(Unit target, int dmg)
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
            dmgText.gameObject.SetActive(true);
            if (dmg == 0)
                dmgText.text = "MISS";
            else
                dmgText.text = "+" + dmg.ToString();
            var spos = Camera.main.WorldToScreenPoint(target.transform.position);
            dmgText.GetComponent<RectTransform>().anchoredPosition = new Vector2(spos.x, spos.y) + new Vector2(Random.Range(-40f, 40f), 120f);
            dmgText.transform.DOLocalMoveY(dmgText.transform.position.y + 40f, 0.5f).OnComplete(() => {
                dmgText.gameObject.SetActive(false);
                dmgTextPool.Enqueue(dmgText);
            });
        }
    }
}

