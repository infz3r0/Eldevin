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
using System.Windows.Shapes;

namespace Eldevin
{
    /// <summary>
    /// Interaction logic for wSelectQuest.xaml
    /// </summary>
    public partial class wSelectQuest : Window
    {
        private List<skill> skills;

        public wSelectQuest()
        {
            InitializeComponent();
            GetSkills();
            GetSkillQuest();
        }

        private void GetSkills()
        {
            using (EldevinEntities db = new EldevinEntities())
            {
                skills = db.skills.ToList();
            }
        }

        private void GetSkillQuest()
        {
            foreach (skill s in skills)
            {
                ucSkillQuest ucSkillQuest = new ucSkillQuest(s.skill_id);
                ucSkillQuest.Width = 150;
                ucSkillQuest.Height = 360;

                wrpSkillQuest.Children.Add(ucSkillQuest);
            }
        }

        //end
    }
}
