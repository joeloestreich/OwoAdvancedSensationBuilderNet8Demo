using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWOGame;

namespace OwoAdvancedSensationBuilder.builder
{
    public class AdvancedSensationBuilder {

        AdvancedStreamingSensation advanced = null;
        Muscle[] muscles = null;

        public AdvancedSensationBuilder(Sensation sensation, Muscle[] muscles = null) {

            this.muscles = muscles;

            MicroSensation micro = analyzeSensation(sensation);
            if (advanced == null) {
                // Not null when Sensation is SensationsSequence
                advanced = AdvancedSensationService.splitSensation(micro, this.muscles);
            }
        }

        public AdvancedSensationBuilder(List<int> intensities, Muscle[] muscles = null) {
            advanced = AdvancedSensationService.createSensationCurve(100, intensities, muscles);
        }

        private MicroSensation analyzeSensation(Sensation sensation) {
            if (sensation is AdvancedStreamingSensation) {
                advanced = sensation as AdvancedStreamingSensation;
            } else if (sensation is MicroSensation) {
                return sensation as MicroSensation;
            } else if (sensation is SensationWithMuscles) {
                SensationWithMuscles withMuscles = sensation as SensationWithMuscles;
                if (muscles == null) {
                    muscles = withMuscles.muscles;
                }
                return analyzeSensation(withMuscles.reference);
            } else if (sensation is SensationsSequence) {
                advanced = new AdvancedStreamingSensation();
                SensationsSequence sequence = sensation as SensationsSequence;
                foreach (Sensation s in sequence.sensations) {
                    advanced.addSensation(s);
                }
            } else if (sensation is BakedSensation) {
                BakedSensation baked = sensation as BakedSensation;
                return analyzeSensation(baked.reference);
            }
            return null;
        }

        public Sensation getSensationForSend() {
            Console.WriteLine("getSensationForSend() IS NOT YET WORKING CORRECTLY. SCALING IS MISSING");
            if (advanced == null) {
                advanced = new AdvancedStreamingSensation();
            }
            return advanced.transformForSend();
        }

        public AdvancedStreamingSensation getSensationForStream() {
            if (advanced == null) {
                advanced = new AdvancedStreamingSensation();
            }
            return advanced;
        }

        public AdvancedSensationBuilder merge(Sensation s, AdvancedSensationBuilderMergeOptions mergeOptions) {

            AdvancedStreamingSensation newAdvanced = new AdvancedSensationBuilder(s).getSensationForStream();
            if (newAdvanced == null || newAdvanced.isEmpty()) {
                // noting to merge
                return this;
            }
            advanced = AdvancedSensationService.actualMerge(advanced, newAdvanced, mergeOptions);
            return this;
        }

        public AdvancedSensationBuilder appendNow(params Sensation[] sensations) {

            AdvancedSensationBuilderMergeOptions forMerge = null;

            foreach (Sensation s in sensations) {
                AdvancedStreamingSensation newAdvanced = new AdvancedSensationBuilder(s).getSensationForStream();
                if (newAdvanced == null || newAdvanced.isEmpty()) {
                    continue;
                }

                if (forMerge == null) {
                    float delay = (float)Math.Round(advanced.getSnippets().Count * 0.1f, 2);
                    advanced.addSensation(newAdvanced);
                    forMerge = new AdvancedSensationBuilderMergeOptions();
                    forMerge.withDelay(delay);
                } else {
                    advanced = AdvancedSensationService.actualMerge(advanced, newAdvanced, forMerge);
                }
            }

            return this;
        }

        public AdvancedSensationBuilder cutAtTime(float cutAtSecond, bool keepFirstHalf) {
            int cutAt = AdvancedSensationService.float2snippets(cutAtSecond);
            if (keepFirstHalf) {
                advanced = AdvancedSensationService.cutSensation(advanced, 0, cutAt);
            } else {
                advanced = AdvancedSensationService.cutSensation(advanced, cutAt, advanced.sensations.Count);
            }
            return this;
        }

        public AdvancedSensationBuilder cutAtPercent(int cutAtPercent, bool keepFirstHalf) {
            int cutAt = (int)Math.Round(((float)advanced.sensations.Count) / 100 * cutAtPercent);
            if (keepFirstHalf) {
                advanced = AdvancedSensationService.cutSensation(advanced, 0, cutAt);
            } else {
                advanced = AdvancedSensationService.cutSensation(advanced, cutAt, advanced.sensations.Count);
            }
            return this;
        }

        public AdvancedSensationBuilder cutBetweenTime(float fromSecond, float tillSecond) {
            advanced = AdvancedSensationService.cutSensation(advanced,
                AdvancedSensationService.float2snippets(fromSecond),
                AdvancedSensationService.float2snippets(tillSecond));
            return this;
        }

        public AdvancedSensationBuilder cutBetweenPercent(int fromPercent, int tillPercent) {
            advanced = AdvancedSensationService.cutSensation(advanced,
                (int)Math.Round(((float)advanced.sensations.Count) / 100 * fromPercent),
                (int)Math.Round(((float)advanced.sensations.Count) / 100 * tillPercent));
            return this;
        }
    }
}
