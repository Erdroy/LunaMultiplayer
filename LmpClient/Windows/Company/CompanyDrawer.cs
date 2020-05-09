using System.Linq;
using LmpClient.Systems.Companies;
using LmpClient.Systems.SettingsSys;
using LmpClient.Systems.Warp;
using UnityEngine;

namespace LmpClient.Windows.Company
{
    public partial class CompanyWindow
    {
        private Vector2 _scrollView;
        protected override void DrawWindowContent(int windowId)
        {
            GUI.DragWindow(MoveRect);

            ScrollPos = GUILayout.BeginScrollView(ScrollPos, GUILayout.Width(WindowWidth), GUILayout.Height(WindowHeight));
            Draw();
            GUILayout.EndScrollView();
        }

        private void Draw()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Create"))
            {
                CompanySystem.Singleton.CreateCompany("TestGroup");
            }

            if (GUILayout.Button("Remove"))
            {
                CompanySystem.Singleton.RemoveCompany("TestGroup");
            }

            GUILayout.EndHorizontal();

            var ownPlayerName = SettingsSystem.CurrentSettings.PlayerName;
            var ownGroup = CompanySystem.Singleton.Companies.FirstOrDefault(x=>x.Value.Owner == ownPlayerName).Value;

            if (ownGroup != null)
            {
                _scrollView = GUILayout.BeginScrollView(_scrollView);
                var players = WarpSystem.Singleton.WarpEntryDisplay.GetSubspaceDisplayEntries().SelectMany(x => x.Players);
                foreach (var player in players)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(player);
                    GUILayout.Button("Add to company");
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("You are not owner of any company!");
            }
            GUILayout.EndVertical();
        }
    }
}
