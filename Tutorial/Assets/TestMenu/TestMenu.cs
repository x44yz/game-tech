using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tutorial;

namespace Test
{
    public class TestMenu : MonoBehaviour
    {
        public GameObject menuHUD;
        public GameObject menuShop;
        public GameObject menuTutorial;

        void Awake()
        {
            menuHUD.SetActive(true);
            menuShop.SetActive(false);
            menuTutorial.SetActive(false);
        }

        void Start()
        {
            TutorialManager.Inst.SetTutUIController(TutMenuType.UIMenuGuide, menuTutorial.GetComponent<UITutorialMenuController>());
        }

        void Update()
        {
            
        }
    }
}
