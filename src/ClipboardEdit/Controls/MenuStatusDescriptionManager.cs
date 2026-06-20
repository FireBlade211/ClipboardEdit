using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipboardEdit.Controls
{
    [ProvideProperty("StatusDescription", typeof(MenuItem))]
    public partial class MenuStatusDescriptionManager : Component, IExtenderProvider
    {
        private Dictionary<MenuItem, string> _descriptions;
        private StatusBar _bar;

        public StatusBar StatusBar
        {
            get => _bar;
            set => _bar = value;
        }

        private bool _hideDisabled;

        // hide descriptions for disabled menu items
        public bool HideDisabled
        {
            get => _hideDisabled;
            set => _hideDisabled = value;
        }

        public MenuStatusDescriptionManager()
        {
            InitializeComponent();
            _descriptions = new();
        }

        public MenuStatusDescriptionManager(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            _descriptions = new();
        }

        public bool CanExtend(object extendee) => extendee is MenuItem mi && !mi.Text.Trim().Equals("-", StringComparison.Ordinal);

        [Category("Data")]
        //[DefaultValue(null)]
        public string GetStatusDescription(MenuItem mi) => _descriptions.TryGetValue(mi, out string val) ? val : null;

        public void SetStatusDescription(MenuItem mi, string value)
        {
            if (_descriptions.ContainsKey(mi))
            {
                mi.Select -= OnMenuItemSelect;
                _descriptions.Remove(mi);
            }

            if (!string.IsNullOrEmpty(value))
            {
                _descriptions.Add(mi, value);
                mi.Select += OnMenuItemSelect;

                ApplyItemSelectHandlersRecursive(mi.GetMainMenu());
            }
        }

        private HashSet<MenuItem> _eventMenus = new();

        private void ApplyItemSelectHandlersRecursive(Menu m)
        {
            foreach (MenuItem mi in m.MenuItems)
            {
                if (!_eventMenus.Contains(mi))
                {
                    mi.Select += OnMenuItemSelect;
                    _eventMenus.Add(mi);
                }

                ApplyItemSelectHandlersRecursive(mi);
            }
        }

        private void OnMenuItemSelect(object sender, EventArgs e)
        {
            if (sender is MenuItem mi)
                if (!mi.Enabled && _hideDisabled)
                    this.StatusBar.Text = string.Empty;
                else
                    if (_descriptions.TryGetValue(mi, out string desc))
                        this.StatusBar.Text = desc;
                    else
                        this.StatusBar.Text = string.Empty;
            else
                this.StatusBar.Text = string.Empty;
        }
    }
}
