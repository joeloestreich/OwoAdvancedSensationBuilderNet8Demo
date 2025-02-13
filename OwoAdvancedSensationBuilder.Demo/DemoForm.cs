using OwoAdvancedSensationBuilder.builder;
using OwoAdvancedSensationBuilder.Demo.DemoSections;
using OwoAdvancedSensationBuilder.manager;
using OWOGame;
using System.Collections.Specialized;
using static OwoAdvancedSensationBuilder.manager.AdvancedSensationStreamInstance;

namespace OwoAdvancedSensationBuilder.Demo {
    public partial class DemoForm : Form {
        public DemoForm() {
            InitializeComponent();
        }

        OrderedDictionary sections = new OrderedDictionary();

        private void Form1_Load(object sender, EventArgs e) {
            OWO.AutoConnect();

            initIntro();
            initFeatures();
            initBuilder();
            initCompare();
            initExamples();
        }

        private void initIntro() {
            flowIntro.Controls.Clear();

            flowIntro.Controls.Add(new HeaderSection("Level up the Capabilities of the OWO"));
            flowIntro.Controls.Add(new TextSection("Hello everyone. I would like to present you a way to use the OWO more effectivly. " +
                "I worked on a solution to solve a few of the limitations that the OWO has."));
            flowIntro.Controls.Add(new TextSection("First, that the OWO doesn't allow to play multiple Sensations at once."));
            flowIntro.Controls.Add(new TextSection("Second, that the OWO doesn't have have a feature to update Sensations at runtime."));
            flowIntro.Controls.Add(new TextSection("Third, that the OWO doesn't have the most options to create more complex Sensations."));
            flowIntro.Controls.Add(new TextSection("My Solution to these problems are the the AdvancedSensationBuilder, " +
                "the AdvancedSensationManager and the AdvancedSensationService (in the following just \"Builder\", \"Manager\" or \"Service\"). But first..."));

            flowIntro.Controls.Add(new HeaderSection("What are advanced Sensations"));
            flowIntro.Controls.Add(new TextSection("Advanced Sensations (or in code AdvancedStreamingSensation) are a special new Type of SensationSequences and have two main Characteristics."));
            flowIntro.Controls.Add(new TextSection("They are split into very short snippets which each represent a timeframe of 0.1 Seconds (even though the " +
             "duration of each snippet is slightly longer)."));
            flowIntro.Controls.Add(new TextSection("Every Sensation Snippet additionally has no Ramp and a 100% intensity, while the actual intensity is set in a " +
                "per Muscle basis. This unified Model allows to compare Sensations and treat each Muscle on its own. The only information besides the Muscles that " +
                "is relevant is the frequency (and maybe the name)."));

            flowIntro.Controls.Add(new HeaderSection("The Builder"));
            flowIntro.Controls.Add(new TextSection("The Builder is the part that transforms a Sensation into an advanced Sensation. This includes " +
                "transforming the actual intensity and the per Muscle intensity into the per muscle intensity as well as calculating the intensity in case of " +
                "RampUp and RampDown."));
            flowIntro.Controls.Add(new TextSection("Additionally it also allows to modify and combine Sensations."));

            flowIntro.Controls.Add(new HeaderSection("The Manager"));
            flowIntro.Controls.Add(new TextSection("The Manager is the part that plays the Sensations. It keeps a reference to all Sensations and streams " +
                " every 0.1 seconds the calculated Sensation based on all managed Sensations."));
            flowIntro.Controls.Add(new TextSection("The manager plays a combination of all Sensations, but can only calculate the intensities. " +
                "To decide the overall frequency it respects the Priority of the Sensations. In case of the same Priority, " +
                "the oldest inserted Sensation has priority."));
            flowIntro.Controls.Add(new TextSection("The Manager also allows to update Sensations. This allows to change Sensations at runtime and the " +
                "Manager will simply continue to play the Sensation with the updated Values, without the need to start the whole Sensation from the beginning. " +
                "This allows to dynamically factor in Variables. A simple event management is also provided to help keep Track of Sensations."));
            flowIntro.Controls.Add(new TextSection("All Sensations in the Manager are AdvancedStreamingSensations, but its possible to call the Manager with " +
                "regular OWO Sensations. The Manager transforms them if needed by itself."));

            flowIntro.Controls.Add(new HeaderSection("The Service"));
            flowIntro.Controls.Add(new TextSection("The Service is the part that actually does all the math. This is where Sensations actually get combined, cut " +
                "or what else. Additionally the Service has a few Methods that allow to create a new Builder in ways a normal Sensation can't do, like e.g. " +
                "a Ramp up that doesn't start at 0."));

            flowIntro.Controls.Add(new HeaderSection("Drawbacks"));
            flowIntro.Controls.Add(new TextSection("The Main drawback is that by using the Manager it is (currently) not really possible to use " +
                "Baked Sensations. OWO doesn't allow to load Baked Sensations by Code at the moment. Once that is a feature, this should be resolvable. " +
                "There already is Code that would be working with Baked Sensations (as the Class already exists), " +
                "but there is no inbuilt way to fill this Class with the User calibrated Values."));
            flowIntro.Controls.Add(new TextSection("You may say now \"Why not use the Baked Sensations the regular way and the advanced Sensations " +
                "the new Way?\". Well that is in theory possible, but leads to the minor issue that generally the two Methods don't have great compatibility. " +
                "The regular way sends the Sensation once while the Manager sends a Sensation every 0.1 seconds while processing Sensations, " +
                "which would then cancel the non-managed Sensations. This could be designed and worked around, but would require some work."));
            flowIntro.Controls.Add(new TextSection("Lastly there is minor issue number two. As mentioned above the Manager works in 0.1 Second Ticks. " +
                "This means two things. First that there could be a delay of up to 0.1 Seconds if you try to add a new Sensation to a running Manager. " +
                "This also applies to adding two Sensations right after each other to a non running manager, as the first one starts playing " +
                "while the second one isnt added yet. Second (especially very short) RampUps or RampDowns may feel slightly stiffer. " +
                "I have no information on how smoothly OWO ramps its Sensations, but the advanced Sensations doesn't use the OWO ramping. " +
                "Instead the ramping steps are getting calculated in 0.1 second steps."));

            flowIntro.Controls.Add(new HeaderSection("Regarding this Demo"));
            flowIntro.Controls.Add(new TextSection("As this is a Demo for OWO Code, there are multiple opportunities to feel stuff yourself. " +
                "There are multiple Triggers such as this Buttons here, often accompanied by a code example to recreate the effect."));
            flowIntro.Controls.Add(
                new InteractiveSection("Something to feel")
                    .addControl(generateButton("Feel", (_, _) =>
                         interaction_playSensation(new AdvancedSensationStreamInstance("Tap on back", SensationsFactory.Create(100, 0.1f, 15, 0.1f, 0, 0.2f).WithMuscles(Muscle.Dorsal_R)))))
                    .addCode(this, flowIntro, "" +
                        "// You could create this Sensation like this\r\n" +
                        "Sensation tap = SensationsFactory.Create(100, 0.1f, 15, 0.1f, 0, 0.2f)\r\n" +
                        "    .WithName(\"Tap on back\")\r\n" +
                        "    .WithMuscles(Muscle.Dorsal_R);\r\n" +
                        "\r\n" +
                        "AdvancedSensationManager manager = AdvancedSensationManager.getInstance();\r\n" +
                        "manager.playOnce(tap);"));

            flowIntro.Controls.Add(new TextSection("You probably also already noticed the \"Managed Sensations\" list on the right. " +
                "This list displays the Sensations in the Manager and hopefully makes a few things easier to understand. " +
                "Also If there is a Sensation you want to stop, such as a looping Sensation started from a different Tab or a Sensation where you forgot " +
                "how you started it, simply double click it at any time."));
            flowIntro.Controls.Add(new TextSection("I also reccomend to check out the \"Examples\" Tab once you finished trying everything out. "));
        }


        private void initFeatures() {
            flowFeatures.Controls.Clear();

            flowFeatures.Controls.Add(new HeaderSection("Upgrading to advanced Sensations can be simple"));
            flowFeatures.Controls.Add(new TextSection("Triggering a Sensation can be about as simple as it was regular OWO way. But simply switching out " +
                "the calls to Send() and Stop() will allow you to play multiple Sensations at once. There are tons of other interesting features, " +
                "but if this is all you want this is all you need."));
            flowFeatures.Controls.Add(new CodeSection(
                "Sensation sensation = SensationsFactory.Create(100, 1.0f, 20, 0.5f, 0.5f, 0)\r\n" +
                "    .WithMuscles(Muscle.Pectoral_L);\r\n" +
                "\r\n" +
                "// Regular OWO\r\n" +
                "OWO.Send(sensation);\r\n" +
                "OWO.Stop();\r\n" +
                "\r\n" +
                "// Advanced\r\n" +
                "AdvancedSensationManager.getInstance().playOnce(sensation);\r\n" +
                "AdvancedSensationManager.getInstance().stopAll();"));

            flowFeatures.Controls.Add(new TextSection("Do keep in mind though, that OWO (currently) wont allow to load BakedSensations by Code and thus can't " +
                "play those by the Manager. If your usecase requires BakedSensations the Manager may require heavy planning. Should OWO ever decide to make " +
                "BakedSensations loadable by Code this probably wouldn't be a problem anymore."));

            flowFeatures.Controls.Add(new HeaderSection("Play multiple Sensations at the same time"));
            flowFeatures.Controls.Add(new TextSection("Using the Manager lets you play multiple Sensations at the same Time."));
            flowFeatures.Controls.Add(new TextSection("If the Sensation affects different muscles both Sensations just play."));
            flowFeatures.Controls.Add(
                new InteractiveSection("Left and Right")
                    .addControl(generateButton("Feel", (_, _) =>
                        interaction_playSensation(
                            new AdvancedSensationStreamInstance("Left", SensationsFactory.Create(100, 1.0f, 20, 0.5f, 0.5f, 0).WithMuscles(Muscle.Pectoral_L)),
                            new AdvancedSensationStreamInstance("Right", SensationsFactory.Create(100, 1.5f, 20, 0.5f, 0.5f, 0).WithMuscles(Muscle.Pectoral_R)))))
                    .addCode(this, flowFeatures,
                        "Sensation left = SensationsFactory.Create(100, 1.0f, 20, 0.5f, 0.5f, 0)\r\n" +
                        "    .WithName(\"Left\")\r\n" +
                        "    .WithMuscles(Muscle.Pectoral_L);\r\n" +
                        "Sensation right = SensationsFactory.Create(100, 1.5f, 20, 0.5f, 0.5f, 0)\r\n" +
                        "    .WithName(\"Right\")\r\n" +
                        "    .WithMuscles(Muscle.Pectoral_R);\r\n" +
                        "\r\n" +
                        "AdvancedSensationManager manager = AdvancedSensationManager.getInstance();\r\n" +
                        "manager.playOnce(left);\r\n" +
                        "manager.playOnce(right);"));


            flowFeatures.Controls.Add(new TextSection("If the Sensation affects the same muscles both Sensations get merged, meaning that (by default) " +
                "only the highest intensity per Muscle gets played."));
            flowFeatures.Controls.Add(
                new InteractiveSection("Same Muscles")
                    .addControl(generateButton("Feel", (_, _) =>
                        interaction_playSensation(
                            new AdvancedSensationStreamInstance("Constant", SensationsFactory.Create(100, 2, 20, 0, 0, 0)
                                .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R)),
                            new AdvancedSensationStreamInstance("Rising", SensationsFactory.Create(100, 2, 50, 2, 0, 0)
                                .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R)))))
                    .addCode(this, flowFeatures,
                        "Sensation constant = SensationsFactory.Create(100, 2, 20, 0, 0, 0)\r\n" +
                        "    .WithName(\"Constant\")\r\n" +
                        "    .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R);\r\n" +
                        "Sensation rising = SensationsFactory.Create(100, 2, 50, 2, 0, 0)\r\n" +
                        "    .WithName(\"Rising\")\r\n" +
                        "    .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R);\r\n" +
                        "\r\n" +
                        "AdvancedSensationManager manager = AdvancedSensationManager.getInstance();\r\n" +
                        "manager.playOnce(constant);\r\n" +
                        "manager.playOnce(rising);"));

            flowFeatures.Controls.Add(new TextSection("Something to keep in mind though, is that only intensity can be merged. The Frequency cannot!"));
            flowFeatures.Controls.Add(new TextSection("By default the Sensation thats added last defines the Frequency, " +
                "but once this Sensation is over the Frequency might change to that of an earlier Sensation. "));
            flowFeatures.Controls.Add(
                new InteractiveSection("(!) Different Frequencies")
                    .addControl(generateButton("Feel", (_, _) =>
                        interaction_playSensation(
                            new AdvancedSensationStreamInstance("Constant Chest", SensationsFactory.Create(100, 3, 20, 0, 0, 0)
                                .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R)),
                            new AdvancedSensationStreamInstance("Rumbling Stomach", SensationsFactory.Create(20, 1.5f, 20, 0, 0, 0)
                                .WithMuscles(Muscle.Abdominal_L, Muscle.Abdominal_R)))))
                    .addCode(this, flowFeatures,
                        "Sensation constant = SensationsFactory.Create(100, 3, 20, 0, 0, 0)\r\n" +
                        "    .WithName(\"Constant Chest\")\r\n" +
                        "    .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R);\r\n" +
                        "Sensation rumbling = SensationsFactory.Create(20, 1.5f, 20, 0, 0, 0)\r\n" +
                        "    .WithName(\"Rumbling Stomach\")\r\n" +
                        "    .WithMuscles(Muscle.Abdominal_L, Muscle.Abdominal_R);\r\n" +
                        "\r\n" +
                        "AdvancedSensationManager manager = AdvancedSensationManager.getInstance();\r\n" +
                        "manager.playOnce(constant);\r\n" +
                        "manager.playOnce(rumbling);"));

            flowFeatures.Controls.Add(new HeaderSection("New Priority handling"));
            flowFeatures.Controls.Add(new TextSection("The default OWO priority won't block other sensations with lower priority anymore, " +
                "but rather controlls which frequency has priority."));
            flowFeatures.Controls.Add(
                new InteractiveSection("With Priority")
                    .addControl(generateButton("Feel", (_, _) =>
                        interaction_playSensation(
                            new AdvancedSensationStreamInstance("Constant Chest (prio)", SensationsFactory.Create(100, 1.5f, 20, 0, 0, 0)
                                .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R).WithPriority(1)),
                            new AdvancedSensationStreamInstance("Rumbling Stomach", SensationsFactory.Create(20, 3, 20, 0, 0, 0)
                                .WithMuscles(Muscle.Abdominal_L, Muscle.Abdominal_R)))))
                    .addCode(this, flowFeatures,
                        "Sensation constant = SensationsFactory.Create(100, 3, 20, 0, 0, 0)\r\n" +
                        "    .WithName(\"Constant Chest (prio)\")\r\n" +
                        "    .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R)\r\n" +
                        "    .WithPriority(1);\r\n" +
                        "\r\n" +
                        "Sensation rumbling = SensationsFactory.Create(20, 1.5f, 20, 0, 0, 0)\r\n" +
                        "    .WithName(\"Rumbling Stomach\")\r\n" +
                        "    .WithMuscles(Muscle.Abdominal_L, Muscle.Abdominal_R);\r\n" +
                        "\r\n" +
                        "AdvancedSensationManager manager = AdvancedSensationManager.getInstance();\r\n" +
                        "manager.playOnce(constant);\r\n" +
                        "manager.playOnce(rumbling);"));

            flowFeatures.Controls.Add(new TextSection("If a Sensation should restrict other Sensations that would be seen as less priority by the Manager " +
                "(meaning eg. same priority, but played earlier) thats possible to do too, by setting that on the AdvancedSensationStreamInstance."));
            flowFeatures.Controls.Add(new TextSection("This is especially useful when there is a short high impact Sensation, with very different frequencies " +
                "as the other running Sensations."));
            flowFeatures.Controls.Add(
                new InteractiveSection("With Blocking Priority")
                    .addControl(generateButton("Feel", (_, _) =>
                        interaction_playSensation(
                            new AdvancedSensationStreamInstance("Constant Chest (prio)", SensationsFactory.Create(100, 1.5f, 20, 0, 0, 0)
                                .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R).WithPriority(1)).setBlockLowerPrio(true),
                            new AdvancedSensationStreamInstance("Rumbling Stomach", SensationsFactory.Create(20, 3, 30, 0, 0, 0)
                                .WithMuscles(Muscle.Abdominal_L, Muscle.Abdominal_R)))))
                    .addCode(this, flowFeatures,
                        "AdvancedSensationStreamInstance constant = \r\n" +
                        "    new AdvancedSensationStreamInstance(\"Constant Chest (prio)\", \r\n" +
                        "        SensationsFactory.Create(100, 1.5f, 20, 0, 0, 0)\r\n" +
                        "            .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R)\r\n" +
                        "            .WithPriority(1))\r\n" +
                        "    .setBlockLowerPrio(true);\r\n" +
                        "\r\n" +
                        "Sensation rumbling = SensationsFactory.Create(20, 1.5f, 20, 0, 0, 0)\r\n" +
                        "    .WithName(\"Rumbling Stomach\")\r\n" +
                        "    .WithMuscles(Muscle.Abdominal_L, Muscle.Abdominal_R);\r\n" +
                        "\r\n" +
                        "AdvancedSensationManager manager = AdvancedSensationManager.getInstance();\r\n" +
                        "manager.play(constant);\r\n" +
                        "manager.playOnce(rumbling);"));

            flowFeatures.Controls.Add(new HeaderSection("Working with the Manager"));
            flowFeatures.Controls.Add(new TextSection("The previous example made it so, that the Sensations with lower Priority wouldn't play while " +
                "the higher Priority one plays, but it continued afterwards as it still was added."));
            flowFeatures.Controls.Add(new TextSection("Let's say that in the last example we also want to block adding new Sensations of lower Priority. " +
                "This is currently not a feature and I can't possibly anticipate every feature that one might need. But don't worry, because at least " +
                "for the basic usecases there are ways to implemented and manage these yourself."));
            flowFeatures.Controls.Add(new TextSection("This mainly gets down to getPlayingSensationInstances() to get every managed Sensation. " +
                "The return value can be used to simply check if there are Sensations that affect the behaviour such as blocking a potential new Sensation. "));
            flowFeatures.Controls.Add(
                new InteractiveSection("With Blocking Priority")
                    .addControl(generateButton("Feel", (_, _) =>
                        interaction_playSensation(
                            new AdvancedSensationStreamInstance("Constant Chest (prio)", SensationsFactory.Create(100, 1.5f, 20, 0, 0, 0)
                                .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R).WithPriority(1)).setBlockLowerPrio(true))))
                    .addCode(this, flowFeatures,
                        "public void sendTestSensations() {\r\n" +
                        "    AdvancedSensationStreamInstance constant = new AdvancedSensationStreamInstance(\"Constant Chest (prio)\",\r\n" +
                        "        SensationsFactory.Create(100, 1.5f, 20, 0, 0, 0).WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R).WithPriority(1))\r\n" +
                        "    .setBlockLowerPrio(true);\r\n" +
                        "\r\n" +
                        "    AdvancedSensationStreamInstance rumbling = new AdvancedSensationStreamInstance(\"Rumbling Stomach\",\r\n" +
                        "        SensationsFactory.Create(20, 3f, 20, 0, 0, 0).WithMuscles(Muscle.Abdominal_L, Muscle.Abdominal_R));\r\n" +
                        "\r\n" +
                        "    customAddSensation(constant);\r\n" +
                        "    customAddSensation(rumbling);\r\n" +
                        "}\r\n" +
                        "\r\n" +
                        "public void customAddSensation(AdvancedSensationStreamInstance instance) {\r\n" +
                        "    AdvancedSensationManager manager = AdvancedSensationManager.getInstance();\r\n" +
                        "    Dictionary<string, AdvancedSensationStreamInstance> managedSensations = manager.getPlayingSensationInstances();\r\n" +
                        "    bool blocked = managedSensations.Values.Any(inst => inst.blockLowerPrio && inst.sensation.Priority > instance.sensation.Priority);\r\n" +
                        "    if (!blocked) {\r\n" +
                        "        manager.play(instance);\r\n" +
                        "    }\r\n" +
                        "}"));

            flowFeatures.Controls.Add(new TextSection("Now that we solved \"not adding new Sensations that we dont allow\", what is with potentially long running " +
                "Sensations that already run? Well depending on the usecase we can stop all managed Sensations or use a similar Logic as above to just stop " +
                "specific ones. "));
            flowFeatures.Controls.Add(new CodeSection(
                "// To stop every single running Sensation call\r\n" +
                "AdvancedSensationManager.getInstance().stopAll();"));
            flowFeatures.Controls.Add(new CodeSection(
                "// To stop only specific Sensation call\r\n" +
                "AdvancedSensationManager.getInstance().stopSensation(\"SensationInstanceName\");"));

            flowFeatures.Controls.Add(new TextSection("The Method getPlayingSensationInstances() returns the actual Instances in the Manager. Meaning that " +
                "changes you do to the AdvancedSensationStreamInstance will reflect in the Manager."));

            flowFeatures.Controls.Add(new HeaderSection("Build in support for looping Sensations"));
            flowFeatures.Controls.Add(new TextSection("The Manager was build up to support multiple Sensations at once. This gives more design freedom to " +
                "eg. design looping Sensations that are always there, in addition to the impactful ones. These could be weather, vehicles or other " +
                "environmental influences. Do keep in mind though, that the frequencies of different Sensations still can't be merged, so depending on " +
                "how the frequencies were kept in mind when designing the Sensations it may be a good idea to exclude these Sensations on certain high impact " +
                "Sensations."));
            flowFeatures.Controls.Add(new TextSection("An important thing to keep in mind is, that looped Sensations play till they are stopped manually. " +
                "So as long as you don't plan to always call stopAll() or plan to query the running Sensations it is reccomended that " +
                "the AdvancedSensationStreamInstance has a name you defined. To assure that, you have to either call playLoop() with a named Sensation " +
                "or call play() with the named Instance."));
            flowFeatures.Controls.Add(
                new InteractiveSection("Loop Constant")
                    .addControl(generateButton("Feel (toggle)", (_, _) =>
                        interaction_toggleSensation(
                            new AdvancedSensationStreamInstance("Loop Constant",
                                SensationsFactory.Create(100, 0.3f, 20, 0, 0, 0).WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R))
                            .setLoop(true))))
                    .addCode(this, flowFeatures,
                        "// Start call\r\n" +
                        "Sensation loop = SensationsFactory.Create(100, 0.3f, 20, 0, 0, 0).WithName(\"Loop Constant\").WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R);\r\n" +
                        "AdvancedSensationManager.getInstance().playLoop(loop);\r\n" +
                        "\r\n" +
                        "// Stop call\r\n" +
                        "AdvancedSensationManager.getInstance().stopSensation(\"Loop Constant\");"));


            flowFeatures.Controls.Add(new TextSection("Sometimes you have not so random patterns which you dont want to stop in the middle. In these cases you " +
                "shouldn't call one of the stop Methods, but rather remove the looping property from the Instance, so it finishes its current loop and then stops."));
            flowFeatures.Controls.Add(
                new InteractiveSection("Loop Pulse")
                    .addControl(generateButton("Feel (toggle)", (_, _) =>
                        interaction_toggleSensationLoopFinish(
                            new AdvancedSensationStreamInstance("Loop Pulse",
                                SensationsFactory.Create(100, 1, 20, 0.3f, 0.3f, 0.2f).WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R))
                            .setLoop(true))))
                    .addCode(this, flowFeatures,
                        "// Start call\r\n" +
                        "Sensation loop = SensationsFactory.Create(100, 1, 20, 0.3f, 0.3f, 0.2f).WithName(\"Loop Pulse\").WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R);\r\n" +
                        "AdvancedSensationManager.getInstance().playLoop(loop);\r\n" +
                        "\r\n" +
                        "// Stop call\r\n" +
                        "if (AdvancedSensationManager.getInstance().getPlayingSensationInstances().TryGetValue(\"Loop Pulse\", out var loopingInstance)) {\r\n" +
                        "    loopingInstance.loop = false;\r\n" +
                        "}"));

            flowFeatures.Controls.Add(new HeaderSection("Updating Sensations"));
            flowFeatures.Controls.Add(new TextSection("By calling one of the play methods on the Manager we can replace a Sensation with a different one. " +
                "When doing this, the new Sensation starts from the beginning though."));
            flowFeatures.Controls.Add(new CodeSection(
                "AdvancedSensationManager.getInstance().playOnce(sensation);\r\n" +
                "AdvancedSensationManager.getInstance().playLoop(sensation);\r\n" +
                "AdvancedSensationManager.getInstance().play(instance);"));

            flowFeatures.Controls.Add(new TextSection("If we now have a Sensation thats tied to changing parameters, we dont always want to restart the Sensation " +
                "or wait till the last loop is finished, but edit the running Sensation for immediate feedback. " +
                "For these and other cases the manager provides the updateSensation() Method."));

            TrackBar slider = generateSlider(0, 200, 100, 10, (sender, _) => interaction_sliderUpdate((TrackBar) sender,
                        new AdvancedSensationStreamInstance("Slider Pulse", SensationsFactory.Create(100, 1, 20, 0.3f, 0.3f, 0.2f)
                            .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R))
                            .setLoop(true)));
            flowFeatures.Controls.Add(
                new InteractiveSection("Loop Pulse (slider)")
                    .addControl(generateButton("Feel (toggle)", (_, _) => interaction_toggleSensationSliderLoop(
                        SensationsFactory.Create(100, 1, 20, 0.3f, 0.3f, 0.2f)
                            .WithMuscles(Muscle.Pectoral_L, Muscle.Pectoral_R), "Slider Pulse", slider)))
                    .addControl(slider)
                    .addCode(this, flowFeatures,
                        "private void sliderUpdate(TrackBar sender, Sensation origSensation, string name) {\r\n" +
                        "    // sender.Value in this example is an int between 0 and 200 where 100 is the original intensity, 200 is double and 0 is off.\r\n" +
                        "    Sensation updated = origSensation.MultiplyIntensityBy(sender.Value);\r\n" +
                        "    AdvancedSensationManager.getInstance().updateSensation(updated, name);\r\n" +
                        "}"));

            flowFeatures.Controls.Add(new TextSection("Updating Sensations doesn't only have to be for a change of intensity though. Updating Sensations can also " +
                "be used to eg. transition into a different Sensation or do a fade out. When used to fade out a looped Sensation, the loop has to be disabled manually."));
            flowFeatures.Controls.Add(new TextSection("When doing this kind of update I reccomend, that the Sensation starts with the looped Sensation or one that " +
                "feels very similar and then has the transitional / fading Sensation appended."));
            flowFeatures.Controls.Add(new TextSection("But I am getting a little ahead of myself. To more easily achieve these effects I would like to " +
                "introduce a few more features first."));

            flowFeatures.Controls.Add(new HeaderSection("Event handling"));
            flowFeatures.Controls.Add(new TextSection("The Instance offeres multiple Events, which can eg. help to modify the Sensation or Sync up the Sensations " +
                "with external Code more easily."));
            flowFeatures.Controls.Add(new TextSection("AfterAdd is called after a Sensation is added. If the manager is already running it could take up to 0.1 " +
                "Seconds. If you add multiple Instances with the same name at once it could happen that you wont get the same count of events as play calls, " +
                "as you can't add multiple Sensations of the same name in the same manager tick. Following Instances will either get ignored or overwrite the " +
                "queued Instance. AddInfo contains information of if the Instance got newly added or replaced an existing Instance."));
            flowFeatures.Controls.Add(new TextSection("AfterUpdate is called after a Sensation is updated."));
            flowFeatures.Controls.Add(new TextSection("AfterRemove is called after a Sensation is removed. RemoveInfo contains information of if the Instance " +
                "got removed manually, finished playing or if it got replaced by a new play call"));
            flowFeatures.Controls.Add(new TextSection("LastCalculationOfCycle is called right before the last part of the Sensation is played. For looping " +
                "Sensations this gets called on every loop. It could for example trigger an update for randomized Sensations like rain. " +
                "In case of looped Sensation it takes about 0.1 seconds before the loop starts from the beginning and calling an update or a remove wont affect " +
                "this last Sensation part, but the next one to be played when it restarts."));
            flowFeatures.Controls.Add(new CodeSection(
                "public void prepareInstanceEvents(AdvancedSensationStreamInstance instance) {\r\n" +
                "    instance.AfterAdd += Instance_AfterAdd;\r\n" +
                "    instance.AfterUpdate += Instance_AfterUpdate;\r\n" +
                "    instance.AfterRemove += Instance_AfterRemove;\r\n" +
                "    instance.LastCalculationOfCycle += Instance_LastCalculationOfCycle;\r\n" +
                "}\r\n" +
                "\r\n" +
                "private void Instance_AfterAdd(AdvancedSensationStreamInstance instance, AddInfo info) {\r\n" +
                "    // AddInfo can bei either NEW or REPLACE\r\n" +
                "    throw new NotImplementedException();\r\n" +
                "}\r\n" +
                "\r\n" +
                "private void Instance_AfterUpdate(AdvancedSensationStreamInstance instance) {\r\n" +
                "    throw new NotImplementedException();\r\n" +
                "}\r\n" +
                "\r\n" +
                "private void Instance_AfterRemove(AdvancedSensationStreamInstance instance, RemoveInfo info) {\r\n" +
                "    // RemoveInfo can bei either MANUAL, FINISHED or REPLACED\r\n" +
                "    throw new NotImplementedException();\r\n" +
                "}\r\n" +
                "\r\n" +
                "private void Instance_LastCalculationOfCycle(AdvancedSensationStreamInstance instance) {\r\n" +
                "    throw new NotImplementedException();\r\n" +
                "}"));

            flowFeatures.Controls.Add(new TextSection("Events can help you to schedule changes in the Sensation. In these cases it is likley that you don't " +
                "want these events to keep occuring, but only get called once. To do that you can simply unsubscribe the event when its called, as all " +
                "events provide the instance."));
            flowFeatures.Controls.Add(new CodeSection(
                "public void prepareLoopedInstance(AdvancedSensationStreamInstance loopedInstance) {\r\n" +
                "    loopedInstance.LastCalculationOfCycle += doStuffOnce;\r\n" +
                "}\r\n" +
                "\r\n" +
                "private void doStuffOnce(AdvancedSensationStreamInstance loopedInstance) {\r\n" +
                "    loopedInstance.LastCalculationOfCycle -= doStuffOnce;\r\n" +
                "    throw new NotImplementedException();\r\n" +
                "}"));

            flowFeatures.Controls.Add(new HeaderSection("Building Advanced Sensations yourself"));
        }

        private void initBuilder() {
            flowBuilder.Controls.Clear();
        }

        private void initCompare() {
            flowComparisson.Controls.Clear();

            flowComparisson.Controls.Add(new HeaderSection("Comparissions"));
            flowComparisson.Controls.Add(new TextSection("In this Section of the Demo I present to you a few Comparissons of official prebuild Sensations " +
                "by OWO and how these can be enhanced with the Builder or even just by sending these through the Manager."));
            flowComparisson.Controls.Add(new TextSection("Simpler Sensations, such as \"Dart\" are designed to be simple, so there are no advanced alternatives, " +
                "but even in these cases the Manager can help to create additional benefits. By sending them through the manager, we can play multiple " +
                "Sensations at once. Depending on how we name the Sensation Instance we can decide if the same Sensation can be played multiple times or " +
                "replaces itself. Unique names stack on top of each other, while universal names cancel each other."));
            flowComparisson.Controls.Add(new TextSection("The \"Original\" Button sends the Original Sensation in the normal OWO way."));
            flowComparisson.Controls.Add(new TextSection("The \"Manager\" Buttons send the Original Sensation through the Manager, which in isolation should " +
                "feel very similar (if not identical) to the OWO way, but allows to play multiple Sensations at once."));
            flowComparisson.Controls.Add(new TextSection("The \"Advanced\" Button, if available, sends an alternative Sensation that utilizes Features " +
                "of the Builder."));

            Sensation hugOrig = SensationsFactory.Create(1, 1, 1, 0, 0, 0);
            Sensation hugAdvanced = SensationsFactory.Create(1, 1, 1, 0, 0, 0);
            flowComparisson.Controls.Add(new CompareSection(this, "hug", hugOrig, null));

            flowComparisson.Controls.Add(new CompareSection(this, "hug", hugOrig, null));
            flowComparisson.Controls.Add(new CompareSection(this, "hug", hugOrig, null));
            flowComparisson.Controls.Add(new HeaderSection("Comparissions"));
            flowComparisson.Controls.Add(new CompareSection(this, "hug", hugOrig, hugAdvanced));
            flowComparisson.Controls.Add(new CompareSection(this, "hug", hugOrig, hugAdvanced));
            flowComparisson.Controls.Add(new CompareSection(this, "hug", hugOrig, hugAdvanced));
        }

        private void initExamples() {
            flowExamples.Controls.Clear();
            flowExamples.Controls.Add(new ExperienceSection());
        }




        public Button generateButton(string name, EventHandler handler) {
            Button btn = new Button();
            btn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn.Size = new Size(136, 40);
            btn.Text = name;
            btn.UseVisualStyleBackColor = true;
            btn.Click += handler;

            return btn;
        }

        public CodeSection generateCodePanel(string code) {
            CodeSection cs = new CodeSection(code);
            cs.Hide();
            cs.Location = new Point(0, 50);

            return cs;
        }
        public TrackBar generateSlider(int min, int max, int val, int interval, EventHandler handler) {
            TrackBar slider = new TrackBar();
            slider.Size = new Size(136, 40);
            slider.Minimum = min;
            slider.Maximum = max;
            slider.TickFrequency = interval;
            slider.Value = val;
            slider.ValueChanged += handler;

            return slider;
        }

        private void interaction_playSensation(params AdvancedSensationStreamInstance[] instances) {
            foreach (AdvancedSensationStreamInstance instance in instances) {
                instance.AfterRemove += Instance_AfterRemove;
                AdvancedSensationManager.getInstance().play(instance);
                Instance_AfterAdd(instance, AddInfo.NEW);
            }
        }

        private void Instance_AfterAdd(AdvancedSensationStreamInstance instance, AddInfo info) {
            if (lbManager.InvokeRequired) {
                Action doInvoke = delegate { Instance_AfterAdd(instance, info); };
                lbManager.Invoke(doInvoke);
                return;
            }

            lbManager.Items.Add(instance.name);
        }

        private void Instance_AfterRemove(AdvancedSensationStreamInstance instance, RemoveInfo info) {
            if (lbManager.InvokeRequired) {
                Action doInvoke = delegate { Instance_AfterRemove(instance, info); };
                lbManager.Invoke(doInvoke);
                return;
            }

            lbManager.Items.Remove(instance.name);
        }

        private void interaction_stopLooping(string name) {
            if (AdvancedSensationManager.getInstance().getPlayingSensationInstances().TryGetValue(name, out var loopingInstance)) {
                loopingInstance.loop = false;
            }
        }

        public void interaction_toggleCode(Button sender, CodeSection cs, FlowLayoutPanel flow) {
            cs.Visible = !cs.Visible;
            if (cs.Visible) {
                sender.Text = "Hide Code";
                flow.ScrollControlIntoView(cs);
            } else {
                sender.Text = "Show Code";
            }
        }

        private void interaction_toggleSensation(AdvancedSensationStreamInstance instance) {
            if (AdvancedSensationManager.getInstance().getPlayingSensationInstances().ContainsKey(instance.name)) {
                AdvancedSensationManager.getInstance().stopSensation(instance.name);
            } else {
                interaction_playSensation(instance);
            }
        }

        private void interaction_toggleSensationLoopFinish(AdvancedSensationStreamInstance instance) {
            if (AdvancedSensationManager.getInstance().getPlayingSensationInstances().ContainsKey(instance.name)) {
                interaction_stopLooping(instance.name);
            } else {
                interaction_playSensation(instance);
            }
        }

        private void interaction_toggleSensationSliderLoop(Sensation sensation, string name, TrackBar slider) {
            if (AdvancedSensationManager.getInstance().getPlayingSensationInstances().ContainsKey(name)) {
                interaction_stopLooping(name);
            } else {
                AdvancedSensationStreamInstance instance = new AdvancedSensationStreamInstance(name, sensation)
                    .setLoop(true);
                interaction_playSensation(instance);
            }
        }

        private void interaction_sliderUpdate(TrackBar sender, AdvancedSensationStreamInstance origInstance) {
            AdvancedSensationBuilder builder = new AdvancedSensationBuilder(origInstance.sensation)
                .multiplyIntensityBy(sender.Value);
            AdvancedSensationManager.getInstance().updateSensation(builder.getSensationForStream(), origInstance.name);
        }





        private void lbManager_MouseDoubleClick(object sender, MouseEventArgs e) {
            int index = lbManager.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches) {
                AdvancedSensationManager.getInstance().stopSensation(lbManager.Items[index].ToString());
            }
        }

        private void btnDebug_Click(object sender, EventArgs e) {
            if (AdvancedSensationManager.getInstance().getPlayingSensationInstances().TryGetValue("Update Check", out var loopingInstance)) {
                AdvancedSensationManager.getInstance().updateSensation(SensationsFactory.Create(1, 1, 1, 1, 1, 1), "Update Check");
            } else {
                AdvancedSensationStreamInstance inst = new AdvancedSensationStreamInstance("Update Check", SensationsFactory.Create(1, 1, 1, 1, 1, 1));
                inst.loop = true;
                inst.AfterUpdate += Inst_AfterUpdate;
                interaction_playSensation(inst);
            }
        }

        private void Inst_AfterUpdate(AdvancedSensationStreamInstance instance) {
            if (lbManager.InvokeRequired) {
                Action doInvoke = delegate { Inst_AfterUpdate(instance); };
                lbManager.Invoke(doInvoke);
                return;
            }
            instance.AfterUpdate -= Inst_AfterUpdate;
            lbManager.Items.Add("Update");
        }
    }
}
