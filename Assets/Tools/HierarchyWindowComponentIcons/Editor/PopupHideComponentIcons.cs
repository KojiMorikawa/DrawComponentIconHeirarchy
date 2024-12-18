#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace HandMaidEditorUtility
{
    public class PopupHideComponentIcons : PopupWindowContent
    {
        private const int _iconSize = 14;
        private const int _padding = 4;
        private const float _maxWindowWidth = 24f * 10f;

        private Component[] _components;
        private int _startIndex;
        private int _componentsCount;
        private int _totalCount;

        public PopupHideComponentIcons(Component[] components, int startIndex)
        {
            _components = components;
            _startIndex = startIndex;
        }

        public override Vector2 GetWindowSize()
        {
            float componentWidth = _iconSize + _padding;
            _componentsCount = Mathf.FloorToInt((_maxWindowWidth - _padding) / componentWidth);
            _totalCount = Mathf.CeilToInt((_components.Length - _startIndex) / (float)_componentsCount);

            float windowWidth = Mathf.Min(_maxWindowWidth, ((_components.Length - _startIndex) * componentWidth) + _padding);
            float windowHeight = _totalCount * (_iconSize + _padding) + _padding;
            return new Vector2(windowWidth, windowHeight);
        }

        public override void OnGUI(Rect rect)
        {
            int index = _startIndex;
            for (int row = 0; row < _totalCount; row++)
            {
                EditorGUILayout.BeginHorizontal();

                for (int col = 0; col < _componentsCount; col++)
                {
                    if (index >= _components.Length)
                        break;

                    GUIStyle style = new GUIStyle()
                    {
                        fixedWidth = _iconSize,
                        fixedHeight = _iconSize,
                        margin = new RectOffset(_padding, _padding, _padding, _padding)
                    };

                    Texture icon = AssetPreview.GetMiniThumbnail(_components[index]);
                    if (GUILayout.Button(icon, style)) { }

                    index++;
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
#endif