using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Inst;

        public TutorialConfigs tutorialConfigs;

        [Header("RUNTIME")]
        public ITutUIController curTutUIController;
        public Tutorial[] tutorials = null;
        public bool waitForShow = false;
        public Tutorial curTutorial = null;
        public List<int> finishTutorialIds = new List<int>();
        public Dictionary<TutMenuType, ITutUIController> menuUIControllers = new Dictionary<TutMenuType, ITutUIController>();

        private void Awake() 
        {
            Inst = this;

            tutorials = tutorialConfigs.tutorials;
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            if (curTutorial != null)
            {
                if (curTutorial.IsDone)
                    StopTutorial();
                else
                    curTutorial.Tick(dt);
            }
        }

        public ITutUIController ShowTutUIController(TutMenuType menuType)
        {
            ITutUIController v = null;
            if (menuUIControllers.TryGetValue(menuType, out v) == false)
            {
                Debug.LogError($"[TUT]cant find {menuType} ui controller");
                return null;
            }
            if (v == null)
            {
                Debug.LogError($"[TUT]{menuType} ui controller is null");
                return null;
            }
            curTutUIController = v;
            curTutUIController.Show();
            return curTutUIController;
        }

        public void HideTutUIController(TutMenuType menuType)
        {
            ITutUIController v = null;
            if (menuUIControllers.TryGetValue(menuType, out v) == false)
            {
                Debug.LogError($"[TUT]cant find {menuType} ui controller");
                return;
            }
            if (v == null)
            {
                Debug.LogError($"[TUT]{menuType} ui controller is null");
                return;
            }
            v.Hide();
        }

        public void SetTutUIController(TutMenuType menuType, ITutUIController ui)
        {
            menuUIControllers[menuType] = ui;
        }

        public ITutUIController GetTutUIController()
        {
            return curTutUIController;
        }

        public bool HasTutorialFinished(int tutorialId)
        {
            return finishTutorialIds.Contains(tutorialId);
        }

#region TRIGGERS
        public void OnTriggerOpenMenu(string menu)
        {
            TryStartTutorial(TutorialTriggerType.OpenMenu, menu);
        }
#endregion // TRIGGERS

        private void TryStartTutorial(TutorialTriggerType triggerType, params object[] datas)
        {
            if (curTutorial != null)
                return;

            for (int i = 0; i < tutorials.Length; ++i)
            {
                var tutorial = tutorials[i];
                if (tutorial == null)
                    continue;

                if (HasTutorialFinished(tutorial.id))
                    continue;

                if (tutorial.CheckConditions(triggerType, datas))
                {
                    StartTutorial(tutorial);
                    break;
                }
            }
        }

        private void StartTutorial(int tutorialId)
        {
            for (int i = 0; i < tutorials.Length; ++i)
            {
                if (tutorials[i].id == tutorialId)
                {
                    StartTutorial(tutorials[i]);
                    break;
                }
            }
        }

        public void StartTutorial(Tutorial tutorial)
        {
            Debug.Log($"[TUT]Tutorial: starttutorial > {tutorial.id}");
            curTutorial = tutorial;
            curTutorial.Start();
        }

        private void StopTutorial()
        {
            if (curTutorial != null)
            {
                int tutorialId = curTutorial.id;
                curTutorial = null;
                StopTutorial(tutorialId);
            }
        }

        public void StopTutorial(int tutorialId)
        {
            Debug.Log($"[TUT]Tutorial: stoptutorial > {tutorialId}");
            if (!finishTutorialIds.Contains(tutorialId))
            {
                finishTutorialIds.Add(tutorialId);
            }

            curTutorial = null;
            TryStartTutorial(TutorialTriggerType.Tutorial, tutorialId);
        }
    }
}