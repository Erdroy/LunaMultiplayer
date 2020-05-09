using LmpClient.Base;
using LmpClient.Localization;
using LmpCommon.Enums;
using UnityEngine;

namespace LmpClient.Windows.Company
{
    public partial class CompanyWindow : Window<CompanyWindow>
    {
        #region Fields

        private static bool _display;
        public override bool Display
        {
            get => base.Display && _display && MainSystem.NetworkState >= ClientState.Running && HighLogic.LoadedScene >= GameScenes.SPACECENTER;
            set => base.Display = _display = value;
        }

        private const float WindowHeight = 300;
        private const float WindowWidth = 400;

        #endregion

        #region Base overrides

        protected override bool Resizable => true;

        public override void SetStyles()
        {
            // ReSharper disable once PossibleLossOfFraction
            WindowRect = new Rect(Screen.width / 10, Screen.height / 2f - WindowHeight / 2f, WindowWidth, WindowHeight);
            MoveRect = new Rect(0, 0, int.MaxValue, TitleHeight);
        }

        public override void RemoveWindowLock()
        {
            if (IsWindowLocked)
            {
                IsWindowLocked = false;
                InputLockManager.RemoveControlLock("LMP_CompanyLock");
            }
        }

        public override void Update()
        {
            base.Update();
            if (Display)
            {
            }
        }

        protected override void DrawGui()
        {
            WindowRect = FixWindowPos(GUILayout.Window(6704 + MainSystem.WindowOffset, WindowRect, DrawContent,
                LocalizationContainer.CompanyWindowText.Title));
        }

        #endregion

        #region Private methods

        public override void CheckWindowLock()
        {
            if (Display)
            {
                if (MainSystem.NetworkState < ClientState.Running || HighLogic.LoadedSceneIsFlight)
                {
                    RemoveWindowLock();
                    return;
                }

                Vector2 mousePos = Input.mousePosition;
                mousePos.y = Screen.height - mousePos.y;

                var shouldLock = WindowRect.Contains(mousePos);

                if (shouldLock && !IsWindowLocked)
                {
                    InputLockManager.SetControlLock(ControlTypes.ALLBUTCAMERAS, "LMP_CompanyLock");
                    IsWindowLocked = true;
                }
                if (!shouldLock && IsWindowLocked)
                    RemoveWindowLock();
            }

            if (!Display && IsWindowLocked)
                RemoveWindowLock();
        }

        #endregion
    }
}
