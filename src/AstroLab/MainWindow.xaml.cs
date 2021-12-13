using ModernWpf;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;

namespace AstroLab
{
    /// <summary>
    /// Main Window.
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        public CompletionWindow completionWindow;
        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.SetRequestedTheme(window, ElementTheme.Dark);
            ((App)App.Current).UpdateSkin(HandyControl.Data.SkinType.Dark);
            TabView.NewItemFactory = () =>
            {
                var newItem = new System.Windows.Controls.TabItem { Header = "New File" };
                TabItemHelper.SetIcon(newItem, new SymbolIcon(Symbol.Document));
                ICSharpCode.AvalonEdit.TextEditor Editor = new() {
                    Margin = new Thickness(10),
                    FontSize = 16,
                    Background = Brushes.Transparent,
                    Foreground = Brushes.White,
                    FontFamily = new FontFamily("Consolas"),
                    SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("C#")
                    
                };
                Editor.TextArea.TextEntering += Editor_TextArea_TextEntering;
                Editor.TextArea.TextEntered += Editor_TextArea_TextEntered;
                FoldingManager foldingManager = new FoldingManager(Editor.Document);
                XmlFoldingStrategy foldingStrategy = new XmlFoldingStrategy();
                foldingManager = FoldingManager.Install(Editor.TextArea);
                foldingStrategy = new XmlFoldingStrategy();
                foldingStrategy.UpdateFoldings(foldingManager, Editor.Document);
                newItem.Content = Editor;
                return newItem;
            };
        }

        private void Editor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {

            if (e.Text == ".")
            {
                TextArea Editor = (TextArea)sender;
                // Open code completion after the user has pressed dot:
                completionWindow = new CompletionWindow(Editor);
                IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                data.Add(new SyntaxCompletionData("Item1"));
                data.Add(new SyntaxCompletionData("Item2"));
                data.Add(new SyntaxCompletionData("Item3"));
                completionWindow.Show();
                completionWindow.Closed += delegate {
                    completionWindow = null;
                };
            }
        }

        private void Editor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }
    }
}