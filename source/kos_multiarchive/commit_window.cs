using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace kos_multiarchive
{
    partial class kos_multiarchive
    {
        private void CommitWindow(int windowId)
        {
            _repo.Index.Stage("*");
            _text = GUILayout.TextArea(_text);
            if (GUILayout.Button("Confirm") && _text != string.Empty)
            {
                Commit(_text);
                _showcommitdial = false;
            }
            if (GUILayout.Button("Cancel"))
            {
                _showcommitdial = false;
                return;
            }
            GUI.DragWindow();
        }
    }
}
