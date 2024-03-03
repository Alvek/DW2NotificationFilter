using DistantWorlds.Types;
using DistantWorlds.UI;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace NotificationFilter
{
    internal class UiManager
    {
        private DWButton _btnFilterCurrent;
        private DWButton _btnOpenSettings;
        private DWPanel _panelBackground;
        private bool _settingsShouldBeVisible = false;
        private readonly fFilterSettings _fFilterSettings;

        public UiManager(EventHandler<DWEventArgs> createFilterRule, nint gameWindowHandle)
        {
            _fFilterSettings = new fFilterSettings(gameWindowHandle);
            _btnFilterCurrent = CreateButton();
            _btnOpenSettings = CreateButton();


            DWControl.SetupButton(ref _btnFilterCurrent, "FilterCurrent", new Stride.Core.Mathematics.Vector2(90, 90), new Stride.Core.Mathematics.Vector2(90, 90), "Filter current", createFilterRule, true);
            DWControl.SetupButton(ref _btnOpenSettings, "OpenFilterSettings", new Stride.Core.Mathematics.Vector2(90, 90), new Stride.Core.Mathematics.Vector2(90, 90), "Open filter settings", ShowFilterSettings, true);

            _btnFilterCurrent.Visible = false;
            _btnOpenSettings.Visible = false;

            _panelBackground = CreateSettingsPanel(_fFilterSettings.Size);
            _fFilterSettings.Hiden += fFilterSettings_Hiden;
        }

        public void ShowEmpireMessageFilterCurrentBtn()
        {
            _btnFilterCurrent.Visible = true;
            _btnOpenSettings.Visible = true;

            //var test = Process.GetCurrentProcess().MainWindowHandle;
            //_test.AssignHandle(test);
        }
        public void HideEmpireMessageFilterCurrentBtn()
        {
            _btnFilterCurrent.Visible = false;
            _btnOpenSettings.Visible = false;
        }

        public void UpdatePosition(Stride.Core.Mathematics.Vector2 pos)
        {
            bool test = UserInterfaceController.CustomControls.ContainsKey("FilterCurrent");
            _btnFilterCurrent.Position = new Stride.Core.Mathematics.Vector2(UserInterfaceController.EmpireMessageDialog.TargetPosition.X,
                UserInterfaceController.EmpireMessageDialog.TargetPosition.Y + UserInterfaceController.EmpireMessageDialog.Size.Y);
            _btnFilterCurrent.TargetPosition = new Stride.Core.Mathematics.Vector2(UserInterfaceController.EmpireMessageDialog.TargetPosition.X,
                UserInterfaceController.EmpireMessageDialog.TargetPosition.Y + UserInterfaceController.EmpireMessageDialog.Size.Y);

            _btnOpenSettings.Position = new Stride.Core.Mathematics.Vector2(UserInterfaceController.EmpireMessageDialog.TargetPosition.X + _btnFilterCurrent.Size.X,
                UserInterfaceController.EmpireMessageDialog.TargetPosition.Y + UserInterfaceController.EmpireMessageDialog.Size.Y);
            _btnOpenSettings.TargetPosition = new Stride.Core.Mathematics.Vector2(UserInterfaceController.EmpireMessageDialog.TargetPosition.X + _btnFilterCurrent.Size.X,
                UserInterfaceController.EmpireMessageDialog.TargetPosition.Y + UserInterfaceController.EmpireMessageDialog.Size.Y);
        }

        public void ManageActivation()
        {
            if(_settingsShouldBeVisible)
            {
                _panelBackground.Visible = true;
                UserInterfaceController.AddCustomControl(_panelBackground);
                _fFilterSettings.Show();
            }
        }
        public void ManageDeactivation()
        {
            _panelBackground.Visible = false;
            _fFilterSettings.Hide();            
        }

        public nint GetSettingFormHandle()
        {
            return _fFilterSettings.Handle;
        }
        private void ShowFilterSettings(object? sender, DWEventArgs e)
        {
            _panelBackground.SetSizeAndPosition(new Stride.Core.Mathematics.Vector2(_fFilterSettings.Width, _fFilterSettings.Height + _panelBackground.HeaderHeight), new Stride.Core.Mathematics.Vector2());
            _panelBackground.Position = new Stride.Core.Mathematics.Vector2(UserInterfaceController.ScreenWidth / 2 - _fFilterSettings.Width / 2, UserInterfaceController.ScreenHeight / 2 - _fFilterSettings.Height / 2 - _panelBackground.HeaderHeight);
            _panelBackground.TargetPosition = new Stride.Core.Mathematics.Vector2(UserInterfaceController.ScreenWidth / 2 - _fFilterSettings.Width / 2, UserInterfaceController.ScreenHeight / 2 - _fFilterSettings.Height / 2 - _panelBackground.HeaderHeight);
            UserInterfaceController.AddCustomControl(_panelBackground);
            _fFilterSettings.Location = new System.Drawing.Point(UserInterfaceController.ScreenWidth / 2 - _fFilterSettings.Width / 2, UserInterfaceController.ScreenHeight / 2 - _fFilterSettings.Height / 2);
            _fFilterSettings.Show();
            _settingsShouldBeVisible = true;
        }
        private void CloseFilterSettings(object? sender, DWEventArgs e)
        {
            _settingsShouldBeVisible = false;
            _fFilterSettings.Hide();
        }

        private void fFilterSettings_Hiden(object? sender, EventArgs e)
        {
            UserInterfaceController.RemoveCustomControl(_panelBackground);
        }

        private DWPanel CreateSettingsPanel(Size size)
        {
            DWPanel res = new DWPanel();
            res.BorderColor = new Stride.Core.Mathematics.Color(128, 128, 128, 48);
            res.BackgroundImageTintColor = new Stride.Core.Mathematics.Color(255, 255, 255, 255);
            res.DarkColor = new Stride.Core.Mathematics.Color(96, 96, 96, 255);
            res.ForeColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
            res.ListSelectionColor = new Stride.Core.Mathematics.Color(255, 0, 0, 255);
            res.ShadowColor = new Stride.Core.Mathematics.Color(0, 0, 0, 255);

            res.Name = "TestPanel";
            res.HeaderText = "Filter settings";

            //UserInterfaceController.HoverInfo.Visible = false;
            res.Visible = true;
            res.SetFeatureDefaults();

            res.CloseButton = CreateExitButton(res);
            res.CloseButton.ClickEvent = CloseFilterSettings;

            return res;
        }
        private DWButton CreateButton()
        {
            DWButton res = new()
            {
                PostScene = true,
                ButtonColorDisabled = new Stride.Core.Mathematics.Color(1, 1, 1, 255),
                ButtonColorGradient = new Stride.Core.Mathematics.Color(48, 48, 48, 255),
                ButtonColorHover = new Stride.Core.Mathematics.Color(120, 120, 120, 255),
                ButtonColorToggle = new Stride.Core.Mathematics.Color(245, 180, 96, 255),
                BackColor = new Stride.Core.Mathematics.Color(96, 96, 96, 48),
                BorderColor = new Stride.Core.Mathematics.Color(128, 128, 128, 48),
                DarkColor = new Stride.Core.Mathematics.Color(96, 96, 96, 255),
                ForeColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255),
                ForeColorHover = new Stride.Core.Mathematics.Color(255, 255, 255, 255),
                ListSelectionColor = new Stride.Core.Mathematics.Color(255, 0, 0, 255),
                ShadowColor = new Stride.Core.Mathematics.Color(0, 0, 0, 255),
                DropDownSelectorBackgroundColor = new Stride.Core.Mathematics.Color(160, 160, 160, 40),
                DropDownSelectorBackgroundColorHover = new Stride.Core.Mathematics.Color(64, 64, 64, 128)
            };
            return res;
        }

        private DWButton CreateExitButton(DWPanel panel)
        {
            DWButton res = new DWButton();
            string btnName = "SettingsCloseButton";
            string text = "x";
            float num = Math.Max(UserInterfaceHelper.LineHeightLarge, panel.HeaderHeight - UserInterfaceHelper.Margin * 2f);
            Vector2 vector = new Vector2(num, num);
            Vector2 vector2 = new Vector2(panel.TargetSize.X - (vector.X + UserInterfaceHelper.Margin + panel.ContentPaddingRight), panel.ContentPaddingTop + UserInterfaceHelper.Margin);
            DWControl.SetupButton(ref res, btnName, vector, vector2, text, null, false);
            res.Margin = 0f;
            res.Visible = true;
            res.UseBoldFont = true;
            panel.AddControl(res);
            //res.Layer += CloseButtonExtraLayer;

            float num2 = Math.Max(UserInterfaceHelper.LineHeightLarge, panel.HeaderHeight - UserInterfaceHelper.Margin * 2f);
            Vector2 vector3 = new Vector2(num2, num2);
            Vector2 vector4 = new Vector2(panel.TargetSize.X - (vector3.X + UserInterfaceHelper.Margin + panel.ContentPaddingRight), panel.ContentPaddingTop + UserInterfaceHelper.Margin);
            res.SetSizeAndPosition(vector3, vector4);

            return res;
        }



        //    public static void PanelGen()
        //    {
        //        DWPanel panel = new DWPanel();

        //        panel.BorderColor = new Stride.Core.Mathematics.Color(128, 128, 128, 48);
        //        panel.BackgroundImageTintColor = new Stride.Core.Mathematics.Color(255, 255, 255, 255);
        //        panel.DarkColor = new Stride.Core.Mathematics.Color(96, 96, 96, 255);
        //        panel.ForeColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
        //        panel.ListSelectionColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
        //        panel.ListSelectionColor = new Stride.Core.Mathematics.Color(255, 0, 0, 255);
        //        panel.ShadowColor = new Stride.Core.Mathematics.Color(0, 0, 0, 255);

        //        panel.Name = "TestPanel";
        //        panel.HeaderText = "TestPanel";
        //        panel.SetSizeAndPosition(new Stride.Core.Mathematics.Vector2(500, 400), new Stride.Core.Mathematics.Vector2(500, 500));
        //        UserInterfaceController.HoverInfo.Visible = false;
        //        panel.Visible = true;
        //        panel.AddCloseButtonToHeader = true;
        //        panel.SetFeatureDefaults();
        //        DWButton button = new DWButton();
        //        button.PostScene = true;

        //        button.ButtonColorDisabled = new Stride.Core.Mathematics.Color(1, 1, 1, 255);
        //        button.ButtonColorGradient = new Stride.Core.Mathematics.Color(48, 48, 48, 255);
        //        button.ButtonColorHover = new Stride.Core.Mathematics.Color(120, 120, 120, 255);
        //        button.ButtonColorToggle = new Stride.Core.Mathematics.Color(245, 180, 96, 255);
        //        button.BackColor = new Stride.Core.Mathematics.Color(96, 96, 96, 48);
        //        button.BorderColor = new Stride.Core.Mathematics.Color(128, 128, 128, 48);
        //        button.DarkColor = new Stride.Core.Mathematics.Color(96, 96, 96, 255);
        //        button.ForeColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
        //        button.ForeColorHover = new Stride.Core.Mathematics.Color(255, 255, 255, 255);
        //        button.ListSelectionColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
        //        button.ListSelectionColor = new Stride.Core.Mathematics.Color(255, 0, 0, 255);
        //        button.ShadowColor = new Stride.Core.Mathematics.Color(0, 0, 0, 255);
        //        button.DropDownSelectorBackgroundColor = new Stride.Core.Mathematics.Color(160, 160, 160, 40);
        //        button.DropDownSelectorBackgroundColorHover = new Stride.Core.Mathematics.Color(64, 64, 64, 128);

        //        DWControl.SetupButton(ref button, "TestBtn", new Stride.Core.Mathematics.Vector2(90, 90), new Stride.Core.Mathematics.Vector2(90, 90), "TestBtn", null, false);
        //        panel.AddControl(button);

        //        panel.Visible = true;
        //        button.Visible = true;
        //        panel.CloseButton.ClickEvent += ExitClick;
        //        _panel = panel;
        //    }
    }
}
