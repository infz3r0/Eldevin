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
using System.IO;

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

            LoadLastSelection();
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
                ucSkillQuest.Height = 340;

                wrpSkillQuest.Children.Add(ucSkillQuest);
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            string quest_id;
            char[] seperator = { '_' };
            List<string> quest_ids = new List<string>();
            //Get checked quests
            foreach (ucSkillQuest uc in wrpSkillQuest.Children)
            {
                foreach (CheckBox ckb in uc.stpQuests.Children)
                {
                    if (ckb.IsChecked == true)
                    {
                        string[] split = ckb.Name.Split(seperator);
                        quest_id = split[1] + "_" + split[2];
                        quest_ids.Add(quest_id);
                    }                    
                }
            }

            MainWindow.selected_quests_id = quest_ids;

            SaveLastSelection();

            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void SaveLastSelection()
        {
            List<string> ckbNames = new List<string>();
            //Get checked quests
            foreach (ucSkillQuest uc in wrpSkillQuest.Children)
            {
                foreach (CheckBox ckb in uc.stpQuests.Children)
                {
                    if (ckb.IsChecked == true)
                    {
                        ckbNames.Add(ckb.Name);
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(File.Open("userdat.txt", System.IO.FileMode.Create)))
            {
                foreach (string s in ckbNames)
                {
                    sw.WriteLine(s);
                }
            }
        }

        private void LoadLastSelection()
        {
            List<string> selected = new List<string>();
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader("userdat.txt"))
                {
                    string line;

                    // Read and display lines from the file until 
                    // the end of the file is reached. 
                    while ((line = sr.ReadLine()) != null)
                    {
                        selected.Add(line);
                    }
                }

                foreach (ucSkillQuest uc in wrpSkillQuest.Children)
                {
                    foreach (CheckBox ckb in uc.stpQuests.Children)
                    {
                        if (selected.Contains(ckb.Name))
                        {
                            ckb.IsChecked = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        //end
    }
}
