using OwoAdvancedSensationBuilder.builder;
using OwoAdvancedSensationBuilder.Demo.experience;
using OwoAdvancedSensationBuilder.manager;
using OWOGame;
using static OwoAdvancedSensationBuilder.manager.AdvancedSensationManager;
using TrackBar = System.Windows.Forms.TrackBar;

namespace OwoAdvancedSensationBuilder {
    public partial class AdvancedDemoForm : Form {
        public AdvancedDemoForm() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            basicSensation = basicParse();
            advancedSensation = new AdvancedSensationBuilder(basicSensation).getSensationForSend();

            //Here we declare two baked sensations, Ball(0) and Dart(1)
            var auth = GameAuth.Parse("0~Ball~100,1,100,0,0,0,Impact|0%100,1%100,2%100,3%100,4%100,5%100~impact-0~#1~Dart~12,1,100,0,0,0,Impact|5%100~impact-1~");
            OWO.Configure(auth);

            updateVisualisation();
            OWO.AutoConnect();

            BakedSensation baked = basicSensation.Bake(3, "manual bake");

            managableSensations.Add("initial basic", basicSensation);
            managableSensations.Add("initial advanced", advancedSensation);
            managableSensations.Add("ball basic", basicSensation2);
            //managableSensations.Add("baked Ball", "0");
            //managableSensations.Add("baked Dart", "1");

            lbSensations.Items.Add("initial basic");
            lbSensations.Items.Add("initial advanced");
            lbSensations.Items.Add("ball basic");
            //lbSensations.Items.Add("baked Ball");
            //lbSensations.Items.Add("baked Dart");
            lbSensations.SelectedIndex = 0;

            lblSelectedExperience.Text = "Warbringers: Azshara";
            pbThumbnail.Load("https://img.youtube.com/vi/hndyTy3uiZM/0.jpg");
            videoUrl = "https://www.youtube.com/watch?v=hndyTy3uiZM";
        }

        private void btnDebug_Click(object sender, EventArgs e) {

            Sensation none = SensationsFactory.Create(100, 5, 0, 0, 0, 0);
            Sensation windRising1 = SensationsFactory.Create(100, 5, 12, 2, 0, 0).WithMuscles(Muscle.All);
            Sensation windConstant2 = SensationsFactory.Create(90, 4f, 12, 0, 0, 0).WithMuscles(Muscle.All);
            Sensation windRising2 = SensationsFactory.Create(90, 4f, 17, 2, 0, 0).WithMuscles(Muscle.All);
            Sensation windConstant3 = SensationsFactory.Create(80, 3.5f, 17, 0, 0, 0).WithMuscles(Muscle.All);
            Sensation windRising3 = SensationsFactory.Create(80, 3.5f, 25, 2, 0, 0).WithMuscles(Muscle.All);
            Sensation windConstant4 = SensationsFactory.Create(70, 3.5f, 25, 0, 0, 0).WithMuscles(Muscle.All);
            Sensation windRising4 = SensationsFactory.Create(70, 3.5f, 35, 2, 0, 0).WithMuscles(Muscle.All);
            Sensation boomingWave = SensationsFactory.Create(45, 3, 50, 0.2f, 0, 0).WithMuscles(Muscle.All);

            AdvancedSensationMergeOptions options = new AdvancedSensationMergeOptions();
            Sensation windBaseline = new AdvancedSensationBuilder(none)
                .appendNow(windRising1)
                .appendNow(windRising2, windConstant2)
                .appendNow(windRising3, windConstant3)
                .appendNow(windRising4, windConstant4)
                .getSensationForStream();

            Sensation blow1 = SensationsFactory.Create(80, 0.4f, 35, 0.1f, 0.2f, 0);
            Sensation blow2 = SensationsFactory.Create(80, 1.4f, 25, 0.2f, 0.2f, 0);

            Sensation windLeft = new AdvancedSensationBuilder(windBaseline)
                .merge(blow1.WithMuscles(Muscle.Arm_L), options.withDelay(3f))
                .merge(blow1.WithMuscles(Muscle.Lumbar_L, Muscle.Abdominal_L, Muscle.Dorsal_L, Muscle.Pectoral_L), options.afterDelay(0.1f))
                .merge(blow1.WithMuscles(Muscle.Lumbar_R, Muscle.Abdominal_R, Muscle.Dorsal_R, Muscle.Pectoral_R), options.afterDelay(0.1f).copy().withIntensityScale(70))
                .merge(blow1.WithMuscles(Muscle.Arm_R), options.afterDelay(0.1f).copy().withIntensityScale(50))
                .merge(blow2.WithMuscles(Muscle.Arm_L), options.withDelay(3.5f))
                .merge(blow2.WithMuscles(Muscle.Lumbar_L, Muscle.Abdominal_L, Muscle.Dorsal_L, Muscle.Pectoral_L), options.afterDelay(0.1f))
                .merge(blow2.WithMuscles(Muscle.Lumbar_R, Muscle.Abdominal_R, Muscle.Dorsal_R, Muscle.Pectoral_R), options.afterDelay(0.1f).copy().withIntensityScale(90))
                .merge(blow2.WithMuscles(Muscle.Arm_R), options.afterDelay(0.1f).copy().withIntensityScale(80))
                .merge(blow1.WithMuscles(Muscle.Arm_L), options.withDelay(4.7f).withIntensityScale(150))
                .merge(blow1.WithMuscles(Muscle.Lumbar_L, Muscle.Abdominal_L, Muscle.Dorsal_L, Muscle.Pectoral_L), options.afterDelay(0.1f).withIntensityScale(150))
                .merge(blow1.WithMuscles(Muscle.Lumbar_R, Muscle.Abdominal_R, Muscle.Dorsal_R, Muscle.Pectoral_R), options.afterDelay(0.1f).withIntensityScale(130))
                .merge(blow1.WithMuscles(Muscle.Arm_R), options.afterDelay(0.1f).withIntensityScale(110))
                .getSensationForStream();

            AdvancedSensationManager.getInstance().playOnce(windBaseline);

            //ExperienceHelper.getInstance().startAzshara();
        }

        private void btnToggleRain_Click(object sender, EventArgs e) {
            AdvancedSensationManager manager = AdvancedSensationManager.getInstance();
            if (manager.getPlayingSensationInstances().Keys.Contains("Rain Snippet")) {
                manager.stopSensation("Rain Snippet");
            } else {
                addRainRandom();
            }
        }

        private void addRainRandom() {
            AdvancedSensationManager manager = AdvancedSensationManager.getInstance();
            AdvancedSensationStreamInstance instance = new AdvancedSensationStreamInstance("Rain Snippet", getRandomRainSensation()).setLoop(true);
            instance.LastCalculationOfCycle += Instance_LastCalculationOfCycle;
            instance.AfterRemove += (_, _) => updateVisualisationManager();

            manager.play(instance);
            updateVisualisationManager();
        }

        private Sensation getRandomRainSensation() {
            Random r = new Random();
            return SensationsFactory.Create(20, 0.1f, 60, 0, 0, 0.3f).WithMuscles(Muscle.All[r.Next(0, Muscle.All.Length)]);
        }

        private void Instance_LastCalculationOfCycle(AdvancedSensationStreamInstance instance) {
            AdvancedSensationManager manager = AdvancedSensationManager.getInstance();
            manager.updateSensation(getRandomRainSensation(), "Rain Snippet");
        }

        private void btn20_Click(object sender, EventArgs e) {
            Sensation s = SensationsFactory.Create(100, 1, 20, 0, 0, 0).WithMuscles(Muscle.All.WithIntensity(100));
            OWO.Send(s);
        }

        private void btn20Adv_Click(object sender, EventArgs e) {
            Sensation s = new AdvancedSensationBuilder(SensationsFactory.Create(100, 1, 20, 0, 0, 0).WithMuscles(Muscle.All.WithIntensity(100))).getSensationForSend();
            OWO.Send(s);
        }

        private void btn120_Click(object sender, EventArgs e) {
            Sensation s = SensationsFactory.Create(100, 1, 100, 0, 0, 0).WithMuscles(Muscle.All.WithIntensity(20));
            OWO.Send(s);
        }

        private void btn120adv_Click(object sender, EventArgs e) {
            Sensation s = new AdvancedSensationBuilder(SensationsFactory.Create(100, 1, 100, 0, 0, 0).WithMuscles(Muscle.All.WithIntensity(20))).getSensationForSend();
            OWO.Send(s);
        }

        /*
         * CREATOR
         */

        Sensation basicSensation = null;
        Sensation basicSensation2 = Sensation.Ball;
        Sensation advancedSensation = null;

        Boolean intoS1 = true;

        private void btnConnect_Click(object sender, EventArgs e) {
            OWO.AutoConnect();
        }

        private void btnDisconnect_Click(object sender, EventArgs e) {
            OWO.Disconnect();
        }

        private void btnBasic_Click(object sender, EventArgs e) {
            OWO.Send(basicSensation);
        }

        private void btnBasic2_Click(object sender, EventArgs e) {
            OWO.Send(basicSensation2);
        }

        private void btnAdvanced_Click(object sender, EventArgs e) {
            OWO.Send(advancedSensation);
        }

        private void btnAdvancedMuscle_Click(object sender, EventArgs e) {
            Sensation advancedSensation = new AdvancedSensationBuilder(basicSensation, Muscle.Front).getSensationForSend();
            OWO.Send(advancedSensation);
        }

        private void btnUpdate_Click(object sender, EventArgs e) {
            if (intoS1) {
                basicSensation = basicParse();
                advancedSensation = new AdvancedSensationBuilder(basicSensation).getSensationForSend();
            } else {
                basicSensation2 = basicParse();
                advancedSensation = new AdvancedSensationBuilder(basicSensation2).getSensationForSend();
            }
            updateVisualisation();
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (intoS1) {
                basicSensation = basicSensation.Append(basicParse());
                advancedSensation = new AdvancedSensationBuilder(basicSensation).getSensationForSend();
            } else {
                basicSensation2 = basicSensation2.Append(basicParse());
                advancedSensation = new AdvancedSensationBuilder(basicSensation2).getSensationForSend();
            }
            updateVisualisation();
        }

        private void btnMuscleNew_Click(object sender, EventArgs e) {
            Random r = new Random();
            if (intoS1) {
                basicSensation = basicParse().WithMuscles(Muscle.All[r.Next(0, Muscle.All.Length)]);
                advancedSensation = new AdvancedSensationBuilder(basicSensation).getSensationForSend();
            } else {
                basicSensation2 = basicParse().WithMuscles(Muscle.All[r.Next(0, Muscle.All.Length)]);
                advancedSensation = new AdvancedSensationBuilder(basicSensation2).getSensationForSend();
            }
            updateVisualisation();
        }

        private void btnMuscleAdd_Click(object sender, EventArgs e) {
            Random r = new Random();
            if (intoS1) {
                basicSensation = basicSensation.Append(basicParse().WithMuscles(Muscle.All[r.Next(0, Muscle.All.Length)]));
                advancedSensation = new AdvancedSensationBuilder(basicSensation).getSensationForSend();
            } else {
                basicSensation2 = basicSensation2.Append(basicParse().WithMuscles(Muscle.All[r.Next(0, Muscle.All.Length)]));
                advancedSensation = new AdvancedSensationBuilder(basicSensation2).getSensationForSend();
            }
            updateVisualisation();
        }

        private MicroSensation basicParse() {
            return SensationsFactory.Create(
                Int32.Parse(txtFrequency.Text),
                float.Parse(txtDuration.Text),
                Int32.Parse(txtIntensity.Text),
                float.Parse(txtRampUp.Text),
                float.Parse(txtRampDown.Text),
                float.Parse(txtExit.Text));
        }

        private void btnManualBuild_Click(object sender, EventArgs e) {
            basicSensation = Sensation.Parse(txtBasic1.Text);
            basicSensation2 = Sensation.Parse(txtBasic2.Text); ;
            if (intoS1) {
                advancedSensation = new AdvancedSensationBuilder(basicSensation).getSensationForSend();
            } else {
                advancedSensation = new AdvancedSensationBuilder(basicSensation2).getSensationForSend();
            }
            updateVisualisation();
        }

        private void updateVisualisation() {
            txtBasic1.Text = basicSensation.ToString();
            txtBasic2.Text = basicSensation2.ToString();
            txtAdvanced.Text = advancedSensation.ToString();

            if (intoS1) {
                lblToggle.Text = "1";
            } else {
                lblToggle.Text = "2";
            }
        }

        private void btnToggle_Click(object sender, EventArgs e) {
            intoS1 = !intoS1;
            updateVisualisation();
        }

        private void btnMerge_Click(object sender, EventArgs e) {
            AdvancedSensationMergeOptions mergeOptions = new AdvancedSensationMergeOptions();
            mergeOptions.mode = AdvancedSensationMergeOptions.MuscleMergeMode.MAX;

            advancedSensation = new AdvancedSensationBuilder(basicSensation).merge(basicSensation2, mergeOptions).getSensationForSend();
            updateVisualisation();
        }

        private void btnMergeDelayed_Click(object sender, EventArgs e) {
            AdvancedSensationMergeOptions mergeOptions = new AdvancedSensationMergeOptions();
            mergeOptions.mode = AdvancedSensationMergeOptions.MuscleMergeMode.MAX;
            mergeOptions.delaySeconds = 1.5f;

            advancedSensation = new AdvancedSensationBuilder(basicSensation).merge(basicSensation2, mergeOptions).getSensationForSend();
            updateVisualisation();
        }

        private void btnAddToManager_Click(object sender, EventArgs e) {

            string name = txtName.Text;
            int count = managableSensations.Count;
            managableSensations.Add(name, advancedSensation);
            if (managableSensations.Count > count) {
                // new
                lbSensations.Items.Add(name);
            }
        }

        /*
         * MANAGER
         */

        Dictionary<string, Sensation> managableSensations = new Dictionary<string, Sensation>();

        private void lbSensations_SelectedIndexChanged(object sender, EventArgs e) {
            ListBox lb = sender as ListBox;
            string selected = lb.SelectedItem as string;
            Sensation s = managableSensations[selected];

            lblName.Text = selected;
            txtAdvanced2.Text = s.ToString();
        }

        private void lbManager_SelectedIndexChanged(object sender, EventArgs e) {
            ListBox lb = sender as ListBox;
            string selected = lb.SelectedItem as string;

            lblName.Text = selected;
        }

        private void tbInensityMultiply_Scroll(object sender, EventArgs e) {

            string selected = lbSensations.SelectedItem as string;
            Sensation s = managableSensations[selected];

            TrackBar tb = sender as TrackBar;

            AdvancedSensationManager manager = AdvancedSensationManager.getInstance();
            manager.updateSensation(s.MultiplyIntensityBy(tb.Value), selected);
            updateVisualisationManager();
        }

        private void btnPlayNow_Click(object sender, EventArgs e) {
            string selected = lbSensations.SelectedItem as string;
            Sensation s = managableSensations[selected];

            AdvancedSensationManager manager = AdvancedSensationManager.getInstance();
            AdvancedSensationStreamInstance instance = new AdvancedSensationStreamInstance(selected, s);
            instance.AfterRemove += (_, _) => updateVisualisationManager();
            manager.play(instance);
            updateVisualisationManager();
        }

        private void btnLoopNow_Click(object sender, EventArgs e) {
            string selected = lbSensations.SelectedItem as string;
            Sensation s = managableSensations[selected];

            AdvancedSensationManager manager = AdvancedSensationManager.getInstance();
            AdvancedSensationStreamInstance instance = new AdvancedSensationStreamInstance(selected, s).setLoop(true);
            instance.AfterRemove += (_, _) => updateVisualisationManager();
            manager.play(instance);
            updateVisualisationManager();
        }

        private void btnStopNow_Click(object sender, EventArgs e) {
            string selected = lbManager.SelectedItem as string;

            AdvancedSensationManager manager = AdvancedSensationManager.getInstance();
            manager.stopSensation(selected);
        }

        private void btnRemoveAfter_Click(object sender, EventArgs e) {
            string selected = lbManager.SelectedItem as string;

            AdvancedSensationManager manager = AdvancedSensationManager.getInstance();
            Dictionary<string, AdvancedSensationStreamInstance> instances = manager.getPlayingSensationInstances();
            if (selected != null && instances.ContainsKey(selected)) {
                instances[selected].loop = false;
            }
        }

        private void updateVisualisationManager() {

            if (lbManager.InvokeRequired) {
                Action doInvoke = delegate { updateVisualisationManager(); };
                lbManager.Invoke(doInvoke);
                return;
            }

            AdvancedSensationManager manager = AdvancedSensationManager.getInstance();

            List<string> names = new List<string>(manager.getPlayingSensationInstances().Keys);

            lbManager.Items.Clear();
            foreach (string name in names) {
                lbManager.Items.Add(name);
            }

            lblIntensityMultiplier.Text = tbInensityMultiply.Value.ToString() + "%";

            if (names.Count == 0) {
                tbInensityMultiply.Enabled = false;
                btnRemoveNow.Enabled = false;
                btnRemoveAfter.Enabled = false;
            } else {
                tbInensityMultiply.Enabled = true;
                btnRemoveNow.Enabled = true;
                btnRemoveAfter.Enabled = true;
            }

        }

        /*
         * MANAGER
         */

        string videoUrl = "";

        private void btnAzshara_Click(object sender, EventArgs e) {
            lblSelectedExperience.Text = "Warbringers: Azshara";
            pbThumbnail.Load("https://img.youtube.com/vi/hndyTy3uiZM/0.jpg");
            videoUrl = "https://www.youtube.com/watch?v=hndyTy3uiZM";
        }

        private void btnOpenVideo_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start(videoUrl);
        }

        private void btnStartVideoSensations_Click(object sender, EventArgs e) {
            switch (lblSelectedExperience.Text) {
                case "Warbringers: Azshara":
                    ExperienceHelper.getInstance().startAzshara();
                    break;
                default:
                    break;
            }
        }

        private void btnStopExperience_Click(object sender, EventArgs e) {
            ExperienceHelper.getInstance().reset();
        }
    }
}
