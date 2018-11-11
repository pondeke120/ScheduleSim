using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class SidebarViewModel : BindableBase
    {
        private List<SidebarItemViewModel> _items;
        public List<SidebarItemViewModel> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private SidebarItemViewModel _selectedItem;
        public SidebarItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public SidebarViewModel()
        {
            InitItems();
        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitItems()
        {
            // 初期値
            this.Items = new List<SidebarItemViewModel>()
            {
                new SidebarItemViewModel()
                {
                    Name = "入力シート",
                    Children = new List<SidebarItemViewModel>()
                    {
                        new SidebarItemViewModel()
                        {
                            Name = "プロジェクト設定",
                            ViewName = "ProjectSettingPage"
                        },
                        new SidebarItemViewModel()
                        {
                            Name = "要員",
                            ViewName = "MemberPage"
                        },
                        new SidebarItemViewModel()
                        {
                            Name = "WBS",
                            ViewName = "WbsPage"
                        },
                        new SidebarItemViewModel()
                        {
                            Name = "工程間依存",
                            ViewName = "ProcessDependencyPage"
                        },
                        new SidebarItemViewModel()
                        {
                            Name = "機能間依存",
                            ViewName = "FunctionDependencyPage"
                        },
                        new SidebarItemViewModel()
                        {
                            Name = "PERT",
                            ViewName = "PertPage"
                        }
                    }
                },
                new SidebarItemViewModel()
                {
                    Name = "帳票",
                    Children = new List<SidebarItemViewModel>()
                    {
                        new SidebarItemViewModel()
                        {
                            Name = "全体スケジュール",
                            ViewName = "EntireSchedulePage"
                        },
                        new SidebarItemViewModel()
                        {
                            Name = "計画工数",
                            ViewName = "PlanValuePage"
                        },
                    }
                },
                new SidebarItemViewModel()
                {
                    Name = "グラフ",
                    Children = new List<SidebarItemViewModel>()
                    {
                        new SidebarItemViewModel()
                        {
                            Name = "全体ガントチャート",
                            ViewName = "GanttChartGraphPage"
                        },
                        new SidebarItemViewModel()
                        {
                            Name = "稼働スケジュール",
                            ViewName = "ActivityScheduleGraphPage"
                        },
                        new SidebarItemViewModel()
                        {
                            Name = "PERT図",
                            ViewName = "PertGraphPage"
                        },
                    }
                }
            };
        }
    }
}
