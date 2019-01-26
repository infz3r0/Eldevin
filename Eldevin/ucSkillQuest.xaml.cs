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

namespace Eldevin
{
    /// <summary>
    /// Interaction logic for ucSkillQuest.xaml
    /// </summary>
    public partial class ucSkillQuest : UserControl
    {
        private string skill_id;
        private skill skill;
        private List<quest> quests;

        public ucSkillQuest()
        {
            InitializeComponent();
        }

        public ucSkillQuest(string skill_id)
        {
            InitializeComponent();
            this.skill_id = skill_id;

            if (GetSkill())
            {
                GetQuests();
                AddCheckbox();
            }
        }

        private bool GetSkill()
        {
            skill s = null;
            using (EldevinEntities db = new EldevinEntities())
            {
                s = db.skills.Find(skill_id);
            }
            if (s == null)
            {
                MessageBox.Show("Skill null");
                return false;
            }
            this.skill = s;
            return true;
        }

        private void GetQuests()
        {
            using (EldevinEntities db = new EldevinEntities())
            {
                this.quests = db.quests.Where(x => x.skill_id.Equals(skill_id)).ToList();
            }
        }

        private void AddCheckbox()
        {
            string ckbName = "";
            string questName = "";

            foreach (quest q in quests)
            {
                ckbName = "ckbQuest_" + q.quest_id;
                ckbName = ckbName.Replace("-", "_");
                questName = q.quest_name;

                CheckBox checkBox = new CheckBox();
                checkBox.Style = (Style)FindResource("CheckBox");
                checkBox.Name = ckbName;
                checkBox.Content = questName;

                stpQuests.Children.Add(checkBox);
            }
        }

        //end
    }
}
