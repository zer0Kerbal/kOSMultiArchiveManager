using UnityEngine;

namespace kos_multiarchive
{
    partial class kos_multiarchive
    {
        private void DelWindow(int windowId)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Are you sure want to delete: {_branchNameList[_selectionGridInt]}?");
            GUILayout.EndHorizontal();
            if (GUILayout.Button("YES"))
            {
                DelArch(_branchNameList[_selectionGridInt]);
                _showdeldial = false;
            }
            if (GUILayout.Button("NO"))
            {
                _showdeldial = false;
            }
            GUI.DragWindow();
        }
    }
}
