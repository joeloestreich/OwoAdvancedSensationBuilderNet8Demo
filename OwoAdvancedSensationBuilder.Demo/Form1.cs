using OwoAdvancedSensationBuilder.builder;
using OwoAdvancedSensationBuilder.Demo.experience;
using OwoAdvancedSensationBuilder.manager;
using OWOGame;
using System.Diagnostics;

namespace OwoAdvancedSensationBuilder.Demo {
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OWO.AutoConnect();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var ps = new ProcessStartInfo("https://www.youtube.com/watch?v=hndyTy3uiZM")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ExperienceHelper.getInstance().startAzshara();
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            Muscle[] barrier = { Muscle.Pectoral_L.WithIntensity(80), Muscle.Arm_L, Muscle.Dorsal_L,
                Muscle.Pectoral_R.WithIntensity(80), Muscle.Arm_R, Muscle.Dorsal_R };

            Sensation s = new AdvancedSensationBuilder(AdvancedSensationService.createSensationRamp(75, 45, 20, 60, 3)) //38
                .appendNow(AdvancedSensationService.createSensationRamp(45, 90, 60, 88, 1.5f, barrier)) // 39.5
                .appendNow(AdvancedSensationService.createSensationRamp(90, 75, 88, 55, 2.5f, barrier)) // 42
                .appendNow(AdvancedSensationService.createSensationRamp(75, 70, 55, 55, 2f, barrier)) // 44
                /* .appendNow(AdvancedSensationService.createSensationRamp(70, 70, 55, 55, 9)) // 53
                 .appendNow(AdvancedSensationService.createSensationRamp(70, 65, 55, 65, 10)) // 73
                 .appendNow(AdvancedSensationService.createSensationRamp(65, 50, 65, 75, 3)) // 76
                 .appendNow(AdvancedSensationService.createSensationRamp(50, 50, 75, 75, 9)) // 85*/
                .getSensationForStream();
            AdvancedSensationManager.getInstance().playOnce(s);
        }
    }
}
