using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace kos_multiarchive
{
    partial class kos_multiarchive
    {
        private void NewArch(string branchname)
        {
            try
            {
                _repo.CreateBranch(branchname);
                _repo.Checkout(branchname);
                _inusebranch = branchname;
                GetBranches();
                if (!HighLogic.LoadedSceneIsEditor) return;
                UpdateKOSBootlist();
            }
            catch (LibGit2SharpException e)
            {
                ScreenMessages.PostScreenMessage(e.Message,
                    4f, ScreenMessageStyle.UPPER_CENTER);
            }
        }

        private void DelArch(string branchname)
        {
            try
            {
                _repo.Branches.Remove(branchname);
                GetBranches();
            }

            catch (LibGit2SharpException e)
            {
                ScreenMessages.PostScreenMessage(e.Message,
                    4f, ScreenMessageStyle.UPPER_CENTER);
            }
        }

        private void ChangeArch()
        {
            var newbranch = _branchNameList[_selectionGridInt];
            try
            {
                _repo.Checkout(newbranch);
                _inusebranch = newbranch;
                if (!HighLogic.LoadedSceneIsEditor) return;
                UpdateKOSBootlist();
            }
            catch (LibGit2SharpException e)
            {
                ScreenMessages.PostScreenMessage(e.Message,
                    4f, ScreenMessageStyle.UPPER_CENTER);
            }
        }

        private void RestoreOrig()
        {
            try
            {
                _repo.Checkout("master");
                _inusebranch = _repo.Head.Name;
                if (!HighLogic.LoadedSceneIsEditor) return;
                UpdateKOSBootlist();
            }
            catch (LibGit2SharpException e)
            {
                ScreenMessages.PostScreenMessage(e.Message,
                    4f, ScreenMessageStyle.UPPER_CENTER);
            }
        }

        private void GetBranches()
        {
            if (_repo.Branches == null) return;
            _branchNameList = new List<string>();
            foreach (Branch b in _repo.Branches.Where(b => !b.IsRemote))
            {
                _branchNameList.Add(b.Name);
            }
        }

        private void Commit(string commitmsg)
        {
            try
            {
                _repo.Commit(commitmsg);
            }
            catch (LibGit2SharpException e)
            {
                ScreenMessages.PostScreenMessage(e.Message,
                    4f, ScreenMessageStyle.UPPER_CENTER);
            }

        }
    }
}
