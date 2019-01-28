using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Eldevin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<string> selected_quests_id = new List<string>();
        private List<quest> quests;
        private List<skill> skills;

        public MainWindow()
        {
            InitializeComponent();
            if (!System.IO.File.Exists("userdat.txt"))
            {
                using (File.Open("userdat.txt", System.IO.FileMode.Create))
                { }
            }

        }

        private void New()
        {
            //Open window select quests
            wSelectQuest wSelectQuest = new wSelectQuest();
            wSelectQuest.ShowDialog();
            if (selected_quests_id.Count <= 0)
            {
                return;
            }
            //Handle selected quests    
            dgAllItem.ItemsSource = GetTotalQuantity(selected_quests_id).OrderBy(x => x.skill_id);

            //Add tab for each skill
            using (EldevinEntities db = new EldevinEntities())
            {
                quests = db.quests.Include("skill").Where(x => selected_quests_id.Contains(x.quest_id)).ToList();
            }
            skills = new List<skill>();
            foreach (quest q in quests)
            {
                skills.Add(q.skill);
            }
            skills = skills.Distinct().ToList();

            foreach (skill s in skills)
            {
                TabItem tab = new TabItem();
                tab.Name = "tab_" + s.skill_id;
                tab.Header = s.skill_name;
                DataGrid dataGrid = new DataGrid();
                dataGrid.Name = "dg_" + s.skill_id;
                tab.Content = dataGrid;

                tabControl.Items.Add(tab);

                List<string> param = new List<string>();
                List<quest> temp;
                using (EldevinEntities db = new EldevinEntities())
                {
                    temp = db.quests.Where(x => selected_quests_id.Contains(x.quest_id) && x.skill_id.Equals(s.skill_id)).ToList();
                }
                foreach (quest q in temp)
                {
                    param.Add(q.quest_id);
                }

                dataGrid.ItemsSource = GetTotalQuantity(param).OrderBy(x => x.skill_id);

                TabItem tab2 = new TabItem();
                tab2.Name = "tab_req_" + s.skill_id;
                tab2.Header = s.skill_name + "REQ";
                DataGrid dataGrid2 = new DataGrid();
                dataGrid2.Name = "dg_req_" + s.skill_id;
                tab2.Content = dataGrid2;

                tabControl.Items.Add(tab2);

                List<quest_req> reqs;
                using (EldevinEntities db = new EldevinEntities())
                {
                    reqs = db.quest_req.Where(x => selected_quests_id.Contains(x.quest_id) && x.quest.skill_id.Equals(s.skill_id)).ToList();
                }

                dataGrid2.ItemsSource = reqs;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            New();
        }

        private List<f_getTotalQtyByQuestID_Result2> Union2List(List<f_getTotalQtyByQuestID_Result2> list1, List<f_getTotalQtyByQuestID_Result2> list2)
        {
            return list1.Union(list2).GroupBy(x => new { x.item, x.skill_id }).Select(y => new f_getTotalQtyByQuestID_Result2 { item = y.Key.item, total_quantity = y.Sum(x => x.total_quantity), skill_id = y.Key.skill_id }).ToList();
        }

        private List<f_getTotalQtyByQuestID_Result2> GetTotalQuantity(List<string> quests_id)
        {
            List<f_getTotalQtyByQuestID_Result2> result = new List<f_getTotalQtyByQuestID_Result2>();
            using (EldevinEntities db = new EldevinEntities())
            {
                foreach (string qid in quests_id)
                {

                    List<f_getTotalQtyByQuestID_Result2> temp = db.f_getTotalQtyByQuestID(qid).ToList();
                    result = Union2List(result, temp);
                }
                
            }
            result = result.OrderBy(x => x.item).ToList();
            return result;
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            New();
        }

        //end
    }
}
