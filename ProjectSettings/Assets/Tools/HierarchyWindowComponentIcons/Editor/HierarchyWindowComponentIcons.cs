#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

// Site URL of the referenced code
// http://anchan828.hatenablog.jp/entry/2013/05/12/021914

namespace HandMaidEditorUtility
{
    [InitializeOnLoad]
    public class HierarchyWindowComponentIcons : Editor
    {
        private const int _hideIconSize = 14;
        private const int _iconSize = 16;
        private const float _padding = 6f;

        static HierarchyWindowComponentIcons()
        {
            EditorApplication.hierarchyWindowItemOnGUI += (instanceID, rect) =>
            {
                GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
                if (obj != null)
                {
                    DrawComponentIcons(instanceID, rect, obj);
                }
            };
        }
        static void DrawComponentIcons(int instanceID, Rect rect, GameObject obj)
        {
            bool isPrefab = PrefabUtility.GetPrefabAssetType(obj) != PrefabAssetType.NotAPrefab;
            Rect iconRect = new Rect(rect.x + rect.width, rect.y, _iconSize, rect.height);
            int maxIconsToShow = Mathf.FloorToInt(CalculateWidthForIcons(obj.name, rect, isPrefab) / (_iconSize + _padding));

            Component[] components = obj.GetComponents<Component>();
            List<Texture> textures = new List<Texture>();

            foreach (Component component in components)
            {
                Texture icon = AssetPreview.GetMiniThumbnail(component);
                if (icon != null)
                {
                    textures.Add(AssetPreview.GetMiniThumbnail(component));
                }
            }

            int showIcons = 0;
            iconRect.x += _iconSize;

            if (isPrefab)
            {
                iconRect.x -= _iconSize;
                GUI.Button(iconRect, EditorGUIUtility.IconContent("ArrowNavigationRight"), GUIStyle.none);
            }

            foreach (Texture texture in textures)
            {
                if (showIcons > maxIconsToShow)
                    break;

                if (texture != null)
                {
                    iconRect.x -= _iconSize;
                    GUI.DrawTexture(iconRect, texture);
                    ++showIcons;
                }
            }

            if (textures.Count > showIcons)
            {
                iconRect.x -= _iconSize;
                if (GUI.Button(iconRect, (Texture)EditorGUIUtility.Load("Icons/hm_CollabChangesDeleted.png"), GUIStyle.none))
                    PopupWindow.Show(iconRect, new PopupHideComponentIcons(obj.GetComponents<Component>(), showIcons));
            }
        }

        private static int CalculateWidthForIcons(string objName, Rect selectionRect, bool isPrefab)
        {
            return (int)(selectionRect.width - (GUI.skin.label.CalcSize(new GUIContent(objName)).x + _padding) -
                         (isPrefab ? _iconSize : 0) - (float)_iconSize);
        }
    }
}
#endif