using System;
using OWOGame;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwoAdvancedSensationBuilder.builder
{
    public class AdvancedSensationBuilderMergeOptions {
        public enum MuscleMergeMode { MAX, KEEP, OVERRIDE, MIN };

        public MuscleMergeMode mode { get; set; }
        public float delaySeconds { get; set; }
        public Multiplier intensityScale { get; set; }
        public bool overwriteBaseSensation { get; set; }

        public AdvancedSensationBuilderMergeOptions() {
            mode = MuscleMergeMode.MAX;
            delaySeconds = 0.0f;
            intensityScale = 100;
            overwriteBaseSensation = false;
        }

        public AdvancedSensationBuilderMergeOptions copy() {
            AdvancedSensationBuilderMergeOptions copy = new AdvancedSensationBuilderMergeOptions();
            copy.mode = mode;
            copy.delaySeconds = delaySeconds;
            return copy;
        }

        public AdvancedSensationBuilderMergeOptions withMode(MuscleMergeMode mode) {
            this.mode = mode;
            return this;
        }

        public AdvancedSensationBuilderMergeOptions afterDelay(float delay) {
            delaySeconds = delaySeconds + delay;
            return this;
        }

        public AdvancedSensationBuilderMergeOptions withDelay(float delaySeconds) {
            this.delaySeconds = delaySeconds;
            return this;
        }

        public AdvancedSensationBuilderMergeOptions withIntensityScale(Multiplier intensityScale) {
            this.intensityScale = intensityScale;
            return this;
        }

        public AdvancedSensationBuilderMergeOptions withOverwriteBaseSensation(bool overwriteBaseSensation) {
            this.overwriteBaseSensation = overwriteBaseSensation;
            return this;
        }
    }
}