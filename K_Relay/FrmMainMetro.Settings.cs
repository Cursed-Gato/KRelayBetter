using System;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Forms;
using Lib_K_Relay.Utilities;

namespace K_Relay
{
    partial class FrmMainMetro
    {
        private FixedStyleManager _mThemeManager;

        private void InitSettings()
        {
            Invoke((MethodInvoker)delegate
            {
                _mThemeManager = new FixedStyleManager(this);
                themeCombobox.Items.AddRange(Enum.GetNames(typeof(MetroThemeStyle)));
                styleCombobox.Items.AddRange(Enum.GetNames(typeof(MetroColorStyle)));

                themeCombobox.SelectedValueChanged += themeCombobox_SelectedValueChanged;
                styleCombobox.SelectedValueChanged += styleCombobox_SelectedValueChanged;

                themeCombobox.SelectedItem = Config.Default.Theme.ToString();
                styleCombobox.SelectedItem = Config.Default.Style.ToString();

                tglStartByDefault.Checked = Config.Default.StartProxyByDefault;
                lstServers.SelectedItem = Config.Default.DefaultServerName;

                _mThemeManager.OnStyleChanged += m_themeManager_OnStyleChanged;
                m_themeManager_OnStyleChanged(null, null);
            });
        }

        private void styleCombobox_SelectedValueChanged(object sender, EventArgs e)
        {
            _mThemeManager.Style =
                (MetroColorStyle)Enum.Parse(typeof(MetroColorStyle), (string)styleCombobox.SelectedItem, true);
        }

        private void themeCombobox_SelectedValueChanged(object sender, EventArgs e)
        {
            _mThemeManager.Theme =
                (MetroThemeStyle)Enum.Parse(typeof(MetroThemeStyle), (string)themeCombobox.SelectedItem, true);
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            Config.Default.StartProxyByDefault = tglStartByDefault.Checked;
            Config.Default.DefaultServerName = lstServers.SelectedItem.ToString();
            Config.Default.ProxyServerName = ProxyServerNameTxtBox.Text;
            Config.Default.Save();
            Config.Default.Theme =
                (MetroThemeStyle)Enum.Parse(typeof(MetroThemeStyle), (string)themeCombobox.SelectedItem, true);
            Config.Default.Style =
                (MetroColorStyle)Enum.Parse(typeof(MetroColorStyle), (string)styleCombobox.SelectedItem, true);
            Config.Default.Save();

            MetroMessageBox.Show(this, "\nYour settings have been saved.", "Save Settings", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private class FixedStyleManager
        {
            private readonly MetroStyleManager _mManager;
            private MetroColorStyle _mColorStyle;
            private MetroThemeStyle _mThemeStyle;


            public FixedStyleManager(MetroForm form)
            {
                _mManager = new MetroStyleManager(form.Container);
                _mManager.Owner = form;
            }

            public MetroColorStyle Style
            {
                get => _mColorStyle;
                set
                {
                    _mColorStyle = value;
                    Update();
                    if (OnStyleChanged != null) OnStyleChanged(this, new EventArgs());
                }
            }

            public MetroThemeStyle Theme
            {
                get => _mThemeStyle;
                set
                {
                    _mThemeStyle = value;
                    Update();
                    if (OnThemeChanged != null) OnThemeChanged(this, new EventArgs());
                }
            }

            public event EventHandler OnThemeChanged;
            public event EventHandler OnStyleChanged;

            public void Update()
            {
                (_mManager.Owner as MetroForm).Theme = _mThemeStyle;
                (_mManager.Owner as MetroForm).Style = _mColorStyle;

                _mManager.Theme = _mThemeStyle;
                _mManager.Style = _mColorStyle;
            }
        }
    }
}