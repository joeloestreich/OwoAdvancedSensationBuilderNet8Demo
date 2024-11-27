using OwoAdvancedSensationBuilder.manager;
using OwoAdvancedSensationBuilderNet8.manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OwoAdvancedSensationBuilder.Demo.experience
{
    internal class ExperienceHelper {

        private static ExperienceHelper managerInstance;

        private System.Timers.Timer timer;

        private double time = 0;

        private Dictionary<double, List<AdvancedSensationStreamInstance>> queue;

        private ExperienceHelper() {
            timer = new System.Timers.Timer(100);
            timer.Elapsed += tick;
            timer.Elapsed += addSensation;
            timer.AutoReset = true;
            timer.Enabled = false;
        }

        public static ExperienceHelper getInstance() {
            if (managerInstance == null) {
                managerInstance = new ExperienceHelper();
            }
            return managerInstance;
        }

        private void tick(Object source, ElapsedEventArgs e) {
            time += 0.1;
            time = Math.Round(time, 1);
        }
        private void addSensation(Object source, ElapsedEventArgs e) {

            if (!queue.ContainsKey(time)) {
                return;
            }

            AdvancedSensationManager manager = AdvancedSensationManager.getInstance();
            foreach (AdvancedSensationStreamInstance i in queue[time]) {
                manager.play(i);
            }
        }
        public void reset() {
            AdvancedSensationManager manager = AdvancedSensationManager.getInstance();
            timer.Stop();
            manager.stopAll();
            time = 0;
            manager.priorityList.Clear();
        }

        public void startAzshara() {
            reset();
            queue = new ExperienceAzshara().getAzshara();
            timer.Start();
        }

    }
}