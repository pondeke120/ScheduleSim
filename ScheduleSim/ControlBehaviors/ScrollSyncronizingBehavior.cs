using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace ScheduleSim.ControlBehaviors
{
    /// <summary>
    /// スクロールの動機制御を行うbehaviorクラス
    /// ref:https://days-of-programming.blogspot.com/2015/01/wpfscrollviewerscrollbar.html
    /// </summary>
    public class ScrollSyncronizingBehavior : Behavior<Control>
    {
        static Dictionary<string, List<Control>> SyncGroups = new Dictionary<string, List<Control>>();

        protected override void OnAttached()
        {
            base.OnAttached();

            AddSyncGroup(ScrollGroup);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            RemoveSyncGroup(ScrollGroup);
        }

        /// <summary>
        /// スクロールグループ
        /// </summary>
        public string ScrollGroup
        {
            get { return (string)this.GetValue(ScrollGroupProperty); }
            set { this.SetValue(ScrollGroupProperty, value); }
        }

        private static readonly DependencyProperty ScrollGroupProperty = DependencyProperty.Register(
            "ScrollGroup", typeof(string), typeof(ScrollSyncronizingBehavior), new FrameworkPropertyMetadata((d, e) => {
                ScrollSyncronizingBehavior me = (ScrollSyncronizingBehavior)d;

                me.RemoveSyncGroup((string)e.OldValue);
                me.AddSyncGroup((string)e.NewValue);
            })
        );

        /// <summary>
        /// スクロールの向き
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)this.GetValue(OrientationProperty); }
            set { this.SetValue(OrientationProperty, value); }
        }

        private static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(ScrollSyncronizingBehavior), new FrameworkPropertyMetadata()
        );

        /// <summary>
        /// 同期グループに追加するメソッド
        /// </summary>
        /// <param name="GroupName">グループ名</param>
        /// <returns>成功したかどうか</returns>
        bool AddSyncGroup(string GroupName)
        {
            if (!string.IsNullOrEmpty(ScrollGroup) && (this.AssociatedObject is ScrollViewer || this.AssociatedObject is ScrollBar || this.AssociatedObject is DataGrid))
            {
                if (!SyncGroups.ContainsKey(GroupName))
                    SyncGroups.Add(GroupName, new List<Control>());

                var associatedObject = this.AssociatedObject;

                ScrollViewer sv = this.AssociatedObject as ScrollViewer;
                ScrollBar sb = this.AssociatedObject as ScrollBar;
                DataGrid dg = this.AssociatedObject as DataGrid;

                if (sv != null)
                    sv.ScrollChanged += ScrollViewerScrolled;
                if (sb != null)
                    sb.ValueChanged += ScrollBarScrolled;
                if (dg != null)
                {
                    dg.Loaded += Dg_Loaded;
                    return true;
                }
                
                SyncGroups[GroupName].Add(associatedObject);

                return true;
            }
            else
                return false;
        }

        private void Dg_Loaded(object sender, RoutedEventArgs e)
        {
            var dg = sender as DataGrid;
            var behavior = System.Windows.Interactivity.Interaction.GetBehaviors(dg).FirstOrDefault(x => x is ScrollSyncronizingBehavior);
            var groupName = behavior.GetValue(ScrollGroupProperty).ToString();

            var scrollViewer = GetScrollViewer(dg);
            scrollViewer.ScrollChanged += ScrollViewerScrolled;

            SyncGroups[groupName].Add(dg);

            dg.Loaded -= Dg_Loaded;
        }

        private ScrollViewer GetScrollViewer(UIElement element)
        {
            if (element == null) return null;
            
            ScrollViewer retour = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element) && retour == null; i++)
            {
                if (VisualTreeHelper.GetChild(element, i) is ScrollViewer)
                {
                    retour = (ScrollViewer)(VisualTreeHelper.GetChild(element, i));
                }
                else
                {
                    retour = GetScrollViewer(VisualTreeHelper.GetChild(element, i) as UIElement);
                }
            }
            return retour;
        }

        /// <summary>
        /// 同期グループから削除するメソッド
        /// </summary>
        /// <param name="GroupName">グループ名</param>
        /// <returns>成功したかどうか</returns>
        bool RemoveSyncGroup(string GroupName)
        {
            if (!string.IsNullOrEmpty(ScrollGroup) && (this.AssociatedObject is ScrollViewer || this.AssociatedObject is ScrollBar || this.AssociatedObject is DataGrid))
            {
                ScrollViewer sv = this.AssociatedObject as ScrollViewer;
                ScrollBar sb = this.AssociatedObject as ScrollBar;
                DataGrid dg = this.AssociatedObject as DataGrid;

                var associatedObject = this.AssociatedObject;

                if (sv != null)
                    sv.ScrollChanged -= ScrollViewerScrolled;
                if (sb != null)
                    sb.ValueChanged -= ScrollBarScrolled;
                if (dg != null)
                {
                    dg.Unloaded += Dg_Unloaded;
                    return true;
                }
                
                SyncGroups[GroupName].Remove(associatedObject);
                if (SyncGroups[GroupName].Count == 0)
                    SyncGroups.Remove(GroupName);

                return true;
            }
            else
                return false;
        }

        private void Dg_Unloaded(object sender, RoutedEventArgs e)
        {
            var dg = sender as DataGrid;
            var behavior = System.Windows.Interactivity.Interaction.GetBehaviors(dg).FirstOrDefault(x => x is ScrollSyncronizingBehavior);
            var groupName = behavior.GetValue(ScrollGroupProperty).ToString();

            var scrollViewer = GetScrollViewer(dg);
            scrollViewer.ScrollChanged -= ScrollViewerScrolled;

            SyncGroups[groupName].Remove(dg);
            if (SyncGroups[groupName].Count == 0)
                SyncGroups.Remove(groupName);

            dg.Loaded -= Dg_Unloaded;
        }

        /// <summary>
        /// ScrollViewerの場合の変更通知イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ScrollViewerScrolled(object sender, ScrollChangedEventArgs e)
        {
            UpdateScrollValue(sender, Orientation == Orientation.Horizontal ? e.HorizontalOffset : e.VerticalOffset);
        }

        /// <summary>
        /// ScrollBarの場合の変更通知イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ScrollBarScrolled(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateScrollValue(sender, e.NewValue);
        }

        /// <summary>
        /// スクロール値を設定するメソッド
        /// </summary>
        /// <param name="sender">スクロール値を更新してきたコントロール</param>
        /// <param name="NewValue">新しいスクロール値</param>
        void UpdateScrollValue(object sender, double NewValue)
        {
            IEnumerable<Control> others = SyncGroups[ScrollGroup].Where(p => p != sender);

            foreach (ScrollBar sb in others.OfType<ScrollBar>().Where(p => p.Orientation == Orientation))
                sb.Value = NewValue;
            foreach (ScrollViewer sv in others.OfType<ScrollViewer>())
            {
                if (Orientation == Orientation.Horizontal)
                    sv.ScrollToHorizontalOffset(NewValue);
                else
                    sv.ScrollToVerticalOffset(NewValue);
            }
        }
    }
}
