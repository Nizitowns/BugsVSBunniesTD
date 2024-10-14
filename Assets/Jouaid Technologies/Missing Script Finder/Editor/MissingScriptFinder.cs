#region Copyright

// Copyright 2021, Jouaid Technologies, All rights reserved.
// Permission is hereby granted, to the person obtaining buying a copy of this software and associated documentation 
// files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, 
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

#if UNITY_EDITOR

namespace JT.Utils
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using UnityEngine.SceneManagement;

    public class MissingScriptFinder : EditorWindow
    {
        private readonly string[] dropOptions = new[] { "Scenes", "Project Prefabs" };
        private int selectedMode;
        private int oldSelectedMode;

        private Vector2 scrollPosition = Vector2.zero;

        private Scene currentScene;
        List<Object> objectsToRender = new List<Object>();

        [MenuItem("Tools/HJ/Missing Scripts Finder")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MissingScriptFinder), false, "Find GameObjects With Missing Scripts",
                true);
        }

        private void OnGUI()
        {
            if (Application.isPlaying)
                return;

            GUI.Label(new Rect(0, 0, 300, 50), GUI.tooltip);
            selectedMode = EditorGUI.Popup(new Rect(0, 10, position.width - 35, 30), selectedMode, dropOptions);
            if (selectedMode != oldSelectedMode)
            {
                oldSelectedMode = selectedMode;
                objectsToRender = GetMissingObjects();
            }

            GUIContent refreshIcon = EditorGUIUtility.IconContent("d_Refresh", "|Refresh");
            if (EditorGUI.DropdownButton(new Rect(position.width - 30, 10, 30, 30), refreshIcon,
                    FocusType.Passive, GUIStyle.none))
                objectsToRender = GetMissingObjects();

            EditorGUILayout.Space(40f);

            DisplayObjects(objectsToRender);
        }

        private List<Object> GetMissingObjects()
        {
            return selectedMode == 0 ? FindScriptsInScene() : FindScriptsInAssets();
        }

        /// <summary>
        /// Retrive The gameobjects in the current scene that have an script that does not exisit anymore
        /// </summary>
        /// <returns>List of objects with missing components</returns>
        private List<Object> FindScriptsInScene()
        {
            //Get the current scene and all top-level GameObjects in the scene hierarchy
            currentScene = SceneManager.GetActiveScene();
            List<Object> objectsWithDeadScripts = new List<Object>();

            EditorUtility.DisplayProgressBar("Processing", "Begin Search", 0);

            int totalObject = 0;
            foreach (GameObject go in currentScene.GetRootGameObjects())
            {
                totalObject = TotalObjectsMissingScripts(go, objectsWithDeadScripts);
                EditorUtility.DisplayProgressBar("Processing", go.name,
                    totalObject / (float)currentScene.GetRootGameObjects().Length);
            }

            if (objectsWithDeadScripts.Count <= 0)
                Debug.Log($"No GameObjects in '{currentScene.name}' have missing scripts!");

            EditorUtility.ClearProgressBar();
            return objectsWithDeadScripts;
        }


        /// <summary>
        /// Retrive The prefabs that have missing scripts in them from the enitre project
        /// </summary>
        /// <returns>List of objects with missing components</returns>
        private List<Object> FindScriptsInAssets()
        {
            string[] temp = AssetDatabase.GetAllAssetPaths();
            List<string> allPrefabs = new List<string>();
            if (temp != null)
                foreach (string s in temp)
                    if (s.Contains(".prefab"))
                        allPrefabs.Add(s);

            int count = 0;
            List<Object> objectsWithDeadScripts = new List<Object>();
            EditorUtility.DisplayProgressBar("Processing", "Begin Search", 0);
            foreach (string prefab in allPrefabs)
            {
                Object obj = AssetDatabase.LoadMainAssetAtPath(prefab);
                if (obj == null)
                {
                    Debug.Log($"prefab {prefab} is null");
                    continue;
                }

                GameObject go = obj as GameObject;
                if (go != null)
                {
                    EditorUtility.DisplayProgressBar("Processing...", go.name, ++count / (float)allPrefabs.Count);
                    TotalObjectsMissingScripts(go, objectsWithDeadScripts);
                }
            }

            EditorUtility.ClearProgressBar();
            return objectsWithDeadScripts;
        }


        /// <summary>
        /// Display missing scripts in the scene in a window for easy selection
        /// </summary>
        /// <param name="objects">List of objects that have a missing script</param>
        private void DisplayObjects(List<Object> objects)
        {
            if (objects.Count <= 0)
            {
                GUILayout.Label("No Missing Scripts / Components Found!");
                return;
            }

            EditorGUILayout.Space();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            foreach (Object obj in objects)
            {
                GUILayout.BeginHorizontal("box");
                EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
                GUILayout.EndHorizontal();
            }


            GUILayout.EndScrollView();
        }

        /// <summary>
        /// Populates the given list with all the children of the given game object that have missing Components
        /// </summary>
        /// <param name="go">Root game object</param>
        /// <param name="objectsWithDeadScripts">List of objects with missing components</param>
        /// <returns>Total number of missing components</returns>
        private int TotalObjectsMissingScripts(GameObject go, List<Object> objectsWithDeadScripts)
        {
            int totalObject = 0;
            // Get all the children of the root game object and search them to find missing scripts
            List<Transform> children = GetChildren(go.transform, true);
            foreach (Transform child in children)
            {
                totalObject++;
                Component[] childComps = child.GetComponents<Component>();
                foreach (Component currentComponent in childComps)
                {
                    if (currentComponent == null)
                    {
                        GameObject cGo = child.gameObject;
                        objectsWithDeadScripts.Add(cGo);
                        break;
                    }
                }
            }

            totalObject++;
            //Get all components on the GameObject, then loop through them 
            Component[] components = go.GetComponents<Component>();
            foreach (Component currentComponent in components)
            {
                if (currentComponent == null)
                {
                    objectsWithDeadScripts.Add(go);
                    break;
                }
            }

            return totalObject;
        }

        /// <summary>
        /// Get a list of children from a given parent, either the direct
        /// descendants or all recursively. 
        /// </summary>
        /// <param name="parent">Parent we are searching under</param>
        /// <param name="recursive">do we recursively search?</param>
        /// <returns>List of all the children transforms</returns>
        private List<Transform> GetChildren(Transform parent, bool recursive)
        {
            List<Transform> children = new List<Transform>();

            foreach (Transform child in parent)
            {
                children.Add(child);
                if (recursive)
                    children.AddRange(GetChildren(child, true));
            }

            return children;
        }
    }
}
#endif