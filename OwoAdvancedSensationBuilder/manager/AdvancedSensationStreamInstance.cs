using OwoAdvancedSensationBuilder.builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWOGame;

namespace OwoAdvancedSensationBuilderNet8.manager {
    public class AdvancedSensationStreamInstance {

        public delegate void SensationStreamInstanceEvent(AdvancedSensationStreamInstance instance);

        public event SensationStreamInstanceEvent LastCalculationOfCycle;

        public string name { get; }
        public int firstTick { get; set; }
        public bool overwriteManagerProcessList { get; set; }
        public bool loop { get; set; }

        private AdvancedStreamingSensation _sensation;
        public AdvancedStreamingSensation sensation { get { return _sensation; } }

        public AdvancedSensationStreamInstance(string name, Sensation sensation = null, bool loop = false) {
            this.name = name;
            this.loop = loop;
            firstTick = 0;
            overwriteManagerProcessList = false;
            if (sensation != null) {
                _sensation = new AdvancedSensationBuilder(sensation).getSensationForStream();
            }
        }

        public Sensation getSensationAtTick(int tick) {
            if (sensation.isEmpty()) {
                return null;
            }

            int playedSensation = (tick - firstTick) % sensation.sensations.Count;

            if (isLastTickOfCycle(tick)) {
                // trigger events for the last calculation
                LastCalculationOfCycle?.Invoke(this);
            }

            return sensation.sensations[playedSensation];
        }

        public bool isLastTickOfCycle(int tick) {
            int playedSensation = (tick - firstTick) % sensation.sensations.Count;
            return sensation.sensations.Count - 1 == playedSensation;
        }

        public void updateSensation(Sensation newSensation) {
            _sensation = new AdvancedSensationBuilder(newSensation).getSensationForStream();
        }

    }
}
