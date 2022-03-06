using UnityEngine;

namespace kos_multiarchive
{
    partial class kos_multiarchive
    {
        private void NewBranchWindow(int windowId)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (GUILayout.Button("Use ship name"))
                {
                    NewArch(FlightGlobals.ActiveVessel.vesselName.Replace(" ", "_"));
                    _shownewbdial = false;
                }
            }
            _btext = GUILayout.TextArea(_btext);
            if (GUILayout.Button("Confirm") && _btext != string.Empty)
            {
                NewArch(_btext);
                _shownewbdial = false;
            }
            if (GUILayout.Button("Cancel"))
            {
                _shownewbdial = false;
                return;
            }
            GUI.DragWindow();
        }
    }
}
