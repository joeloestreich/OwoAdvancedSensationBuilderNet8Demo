using OwoAdvancedSensationBuilder.builder;
using OwoAdvancedSensationBuilder.manager;
using OwoAdvancedSensationBuilderNet8.manager;
using OWOGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwoAdvancedSensationBuilder.Demo.experience
{
    internal class ExperienceAzshara {

        Dictionary<double, List<AdvancedSensationStreamInstance>> queue = new Dictionary<double, List<AdvancedSensationStreamInstance>>();
        int count = 0;

        public Dictionary<double, List<AdvancedSensationStreamInstance>> getAzshara() {
            List<string> prio = AdvancedSensationManager.getInstance().priorityList;
            prio.Add("boomingWave");
            prio.Add("wind");

            queue = new Dictionary<double, List<AdvancedSensationStreamInstance>>();
            count = 0;

            add(5, getWindRising());
            add(21.7, getBoomingWave());
            add(27, getBoomingWave());
            add(31, getCrashingWave());
            return queue;
        }

        private AdvancedSensationStreamInstance getWindRising() {
            Sensation windRising1 = SensationsFactory.Create(100, 5, 12, 2, 0, 0).WithMuscles(Muscle.All);

            Sensation windConstant2 = SensationsFactory.Create(90, 4.5f, 12, 0, 0, 0).WithMuscles(Muscle.All);
            Sensation windRising2 = SensationsFactory.Create(90, 4.5f, 17, 2, 0, 0).WithMuscles(Muscle.All);

            Sensation windConstant3 = SensationsFactory.Create(80, 2.5f, 17, 0, 0, 0).WithMuscles(Muscle.All);
            Sensation windRising3 = SensationsFactory.Create(80, 2.5f, 25, 2, 0, 0).WithMuscles(Muscle.All);

            Sensation windConstant4 = SensationsFactory.Create(70, 2f, 25, 0, 0, 0).WithMuscles(Muscle.All);
            Sensation windRising4 = SensationsFactory.Create(70, 2f, 35, 2, 0, 0).WithMuscles(Muscle.All);

            Sensation windConstant5 = SensationsFactory.Create(70, 2f, 35, 0, 0, 0).WithMuscles(Muscle.All);
            Sensation windRising5 = SensationsFactory.Create(70, 2f, 47, 2, 0, 0).WithMuscles(Muscle.All);

            Sensation windConstant6 = SensationsFactory.Create(70, 5, 47, 0, 0, 0).WithMuscles(Muscle.All);

            AdvancedSensationBuilderMergeOptions options = new AdvancedSensationBuilderMergeOptions();
            Sensation windBaseline = new AdvancedSensationBuilder(windRising1)  // 10
                .appendNow(windRising2, windConstant2)                          // 14.5
                .appendNow(windRising3, windConstant3)                          // 17
                .appendNow(windRising4, windConstant4)                          // 19
                .appendNow(windRising5, windConstant5)                          // 21
                .appendNow(windConstant6)                                       // 26
                .appendNow(windConstant6)                                       // 31
                .getSensationForStream();

            return new AdvancedSensationStreamInstance("wind", windBaseline);
        }

        private AdvancedSensationStreamInstance getBoomingWave() {
            Sensation boomingWave = SensationsFactory.Create(45, 2, 58, 0.2f, 0, 0).WithMuscles(Muscle.All);
            return new AdvancedSensationStreamInstance("boomingWave", boomingWave);
        }

        private AdvancedSensationStreamInstance getCrashingWave() {
            Sensation boomingWave = SensationsFactory.Create(45, 4, 70, 0.3f, 0, 0).WithMuscles(Muscle.All);
            return new AdvancedSensationStreamInstance("boomingWave", boomingWave);
        }

        private AdvancedSensationStreamInstance getBlockWave() {
            Sensation boomingWave = SensationsFactory.Create(80, 3, 70, 0.3f, 0, 0).WithMuscles(Muscle.All);
            //AdvancedSensationService.createSensationCurve
            return new AdvancedSensationStreamInstance("blockWave", boomingWave);
        }

        private string getName() {
            count++;
            return count.ToString();
        }


        private void add(double time, AdvancedSensationStreamInstance sensation) {
            if (queue.ContainsKey(time)) {
                queue[time].Add(sensation);
            } else {
                List<AdvancedSensationStreamInstance> list = new List<AdvancedSensationStreamInstance>();
                list.Add(sensation);
                queue.Add(time, list);
            }
        }
    }
}
