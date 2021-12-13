using ModernWpf;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

namespace AstroLab
{
    /// <summary>
    /// Main Window.
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.SetRequestedTheme(window, ElementTheme.Dark);
            ((App)App.Current).UpdateSkin(HandyControl.Data.SkinType.Dark);
            TabView.NewItemFactory = () =>
            {
                var newItem = new System.Windows.Controls.TabItem { Header = "New File" };
                TabItemHelper.SetIcon(newItem, new SymbolIcon(Symbol.Document));
                return newItem;
            };
        }
    }
}