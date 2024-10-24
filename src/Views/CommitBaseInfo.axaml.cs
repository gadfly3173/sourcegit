using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SourceGit.Views
{
    public partial class CommitBaseInfo : UserControl
    {
        public static readonly StyledProperty<string> MessageProperty =
            AvaloniaProperty.Register<CommitBaseInfo, string>(nameof(Message), string.Empty);

        public string Message
        {
            get => GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public static readonly StyledProperty<AvaloniaList<Models.CommitLink>> WebLinksProperty =
            AvaloniaProperty.Register<CommitBaseInfo, AvaloniaList<Models.CommitLink>>(nameof(WebLinks));

        public AvaloniaList<Models.CommitLink> WebLinks
        {
            get => GetValue(WebLinksProperty);
            set => SetValue(WebLinksProperty, value);
        }

        public static readonly StyledProperty<AvaloniaList<Models.IssueTrackerRule>> IssueTrackerRulesProperty =
            AvaloniaProperty.Register<CommitBaseInfo, AvaloniaList<Models.IssueTrackerRule>>(nameof(IssueTrackerRules));

        public AvaloniaList<Models.IssueTrackerRule> IssueTrackerRules
        {
            get => GetValue(IssueTrackerRulesProperty);
            set => SetValue(IssueTrackerRulesProperty, value);
        }

        public CommitBaseInfo()
        {
            InitializeComponent();
        }

        private void OnOpenWebLink(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.CommitDetail detail)
            {
                var links = WebLinks;
                if (links.Count > 1)
                {
                    var menu = new ContextMenu();
                    
                    foreach (var link in links)
                    {
                        var url = link.URLTemplate.Replace("SOURCEGIT_COMMIT_HASH_CODE", detail.Commit.SHA);
                        var item = new MenuItem() { Header = link.Name };
                        item.Click += (_, ev) =>
                        {
                            Native.OS.OpenBrowser(url);
                            ev.Handled = true;
                        };

                        menu.Items.Add(item);
                    }

                    (sender as Control)?.OpenContextMenu(menu);
                }
                else if (links.Count == 1)
                {
                    var url = links[0].URLTemplate.Replace("SOURCEGIT_COMMIT_HASH_CODE", detail.Commit.SHA);
                    Native.OS.OpenBrowser(url);
                }
            }            

            e.Handled = true;
        }

        private void OnParentSHAPressed(object sender, PointerPressedEventArgs e)
        {
            if (DataContext is ViewModels.CommitDetail detail && sender is Control { DataContext: string sha })
            {
                detail.NavigateTo(sha);
            }

            e.Handled = true;
        }
    }
}
