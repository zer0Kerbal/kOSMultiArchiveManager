// -------------------------------------------------------------------------------------------------
// kos_multiarchive 0.2
//
// Simple KSP plugin to make kos have multiple archives (poor man style).
// Copyright (C) 2016 Iván Atienza
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// 
// Email: mecagoenbush at gmail dot com
// Freenode: hashashin
//
// -------------------------------------------------------------------------------------------------

using System.IO;
using System.Reflection;
using kOS.Module;
using UnityEngine;

namespace kos_multiarchive
{
    partial class kos_multiarchive 
    {
        private void LoadSettings()
        {
            KSPLog.print("[kos_ma.dll] Loading Config...");
            KSP.IO.PluginConfiguration configfile = KSP.IO.PluginConfiguration.CreateForType<kos_multiarchive>();
            configfile.load();

            _visible = configfile.GetValue<bool>("visible", false);
            _inusebranch = configfile.GetValue<string>("currentbranch", "master");

            _windowRect = configfile.GetValue<Rect>("windowpos", new Rect(50f, 25f, 200f, 260f));
            _windowRect2 = configfile.GetValue<Rect>("commitwindowpos", new Rect(Screen.width/2f, Screen.height/2f , 200f, 260f));
            _windowRect3 = configfile.GetValue<Rect>("newbwindowpos", new Rect(Screen.width / 2f, Screen.height / 2f, 200f, 260f));
            _windowRect4 = configfile.GetValue<Rect>("delwindowpos", new Rect(Screen.width / 2f, Screen.height / 2f, 300f, 120f));
            _keybind = configfile.GetValue<string>("keybind", "y");
            _versionlastrun = configfile.GetValue<string>("version");
            KSPLog.print("[kos_ma.dll] Config Loaded Successfully");
        }

        private void SaveSettings()
        {
            KSPLog.print("[kos_ma.dll] Saving Config...");
            KSP.IO.PluginConfiguration configfile = KSP.IO.PluginConfiguration.CreateForType<kos_multiarchive>();

            configfile.SetValue("visible", _visible);
            configfile.SetValue("currentbranch", _inusebranch);

            configfile.SetValue("windowpos", _windowRect);
            configfile.SetValue("commitwindowpos", _windowRect2);
            configfile.SetValue("newbwindowpos", _windowRect3);
            configfile.SetValue("delwindowpos", _windowRect4);
            configfile.SetValue("keybind", _keybind);
            configfile.SetValue("version", _version);

            configfile.save();
            KSPLog.print("[kos_ma.dll] Config Saved ");
        }

        public void Toggle()
        {
            if (ToolbarManager.ToolbarAvailable)
            {
                const string tooltipon = "Hide KOS-MA";
                const string btextureOn = "kos_ma/Textures/icon_on";
                if (_visible)
                {
                    _visible = false;
                    _button.TexturePath = BtextureOff;
                    _button.ToolTip = Tooltipoff;
                }
                else
                {
                    _visible = true;
                    _button.TexturePath = btextureOn;
                    _button.ToolTip = tooltipon;
                }
            }
            else
            {
                _visible = !_visible;
            }
        }

        private void VersionCheck()
        {
            _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            KSPLog.print("kos_ma.dll version: " + _version);
            if ((_version != _versionlastrun) && (KSP.IO.File.Exists<kos_multiarchive>("config.xml")))
            {
                KSP.IO.File.Delete<kos_multiarchive>("config.xml");
            }
#if DEBUG
            KSP.IO.File.Delete<kos_multiarchive>("config.xml");
#endif
        }

        private void LoadVersion()
        {
            KSP.IO.PluginConfiguration configfile = KSP.IO.PluginConfiguration.CreateForType<kos_multiarchive>();
            configfile.load();
            _versionlastrun = configfile.GetValue<string>("version");
        }

        private void ApplicationLauncherReady()
        {
            if ((ApplicationLauncher.Ready) && (_appbutton == null))
            {
                LoadTexture(ref _buttonTexture, "kos_ma_icon.png",
                    KSPUtil.ApplicationRootPath.Replace("\\", "/") + "GameData/kos_ma/Textures/");
                _appbutton = ApplicationLauncher.Instance.AddModApplication(Toggle, Toggle, null, null, null,
                    null, ApplicationLauncher.AppScenes.MAINMENU| ApplicationLauncher.AppScenes.FLIGHT|ApplicationLauncher.AppScenes.SPACECENTER|
                    ApplicationLauncher.AppScenes.SPH|ApplicationLauncher.AppScenes.VAB, (Texture) _buttonTexture);
            }
        }

        private void AppButtonRemove()
        {
            if (_appbutton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(_appbutton);
                _appbutton = null;
            }
        }

        private void Unreadifying(GameScenes scene)
        {
            AppButtonRemove();
        }
        
        private bool _isorig()
        {
            if (_inusebranch == "master")
            {
                return true;
            }
            return false;
        }

        private bool IsRepoDir()
        {
            return Directory.Exists(_scriptdir + Path.DirectorySeparatorChar + ".git");
        }

        private static void LoadTexture(ref Texture2D tex, string file, string folder)
        {
            //File Exists check
            if (File.Exists($"{folder}/{file}"))
            {
                tex.LoadImage(File.ReadAllBytes($"{folder}/{file}"));
            }
        }

        private void UpdateKOSBootlist()
        {
            var parts = EditorLogic.fetch.ship.Parts;
            foreach (var part in parts)
            {
                if (part.Modules.GetModules<kOSProcessor>().Count > 0)
                {
                    part.FindModuleImplementing<kOSProcessor>().OnStart(PartModule.StartState.Editor);
                }
            }
        }
    }
}
