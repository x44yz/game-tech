using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Tutorial
{
    public static class TutorialUtils
    {
        public static GameObject GetTarget(string targetName)
        {
            // avoid exception cause tutorial block
            try
            {
                var tt = GetTargetByPath(targetName);
                return tt;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[TUT]Tutorial: GetTarget exception > {ex.ToString()}");
                return null;
            }
        }

        public static GameObject GetTargetByPath(string targetPath)
        {
            return null;
            // if (string.IsNullOrEmpty(targetPath))
            //     return null;

            // var menu = targetPath;
            // var idx = targetPath.IndexOf('/');
            // if (idx != -1)
            // {
            //     menu = targetPath.Substring(0, idx);
            // }

            // Test.UIBaseController uiCtrl = null;
            // if (uiCtrl == null)
            //     uiCtrl = UIBaseScene.Instance.FindUIByObjectName(menu);

            // if (uiCtrl != null && idx != -1)
            // {
            //     var subpath = targetPath.Substring(idx + 1, targetPath.Length - idx - 1);
            //     var tf = uiCtrl.transform.Find(subpath);
            //     if (tf != null)
            //         return tf.gameObject;
            // }

            // return uiCtrl != null ? uiCtrl.gameObject : null;
        }

        private static StringBuilder sb = new StringBuilder();
        public static string FindParentRelativePath(this Transform tf, string parentName, bool withParentName)
        {
            sb.Clear();
            var parent = tf;
            while (parent)
            {
                if (parent.name == parentName)
                {
                    if (withParentName)
                    {
                        sb.Insert(0, parent);
                    }
                    break;
                }
                else
                {
                    sb.Insert(0, parent);
                }
                parent = parent.parent;
            }
            return sb.ToString();
        }
    }
}