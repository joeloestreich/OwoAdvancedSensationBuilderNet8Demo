using OWOGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwoAdvancedSensationBuilder.builder
{
    public class AdvancedSensationService {


        public static AdvancedStreamingSensation splitSensation(MicroSensation micro, Muscle[] muscles) {

            AdvancedStreamingSensation advanced = new AdvancedStreamingSensation();
            if (micro == null) {
                return advanced;
            }

            float time = 0.1f;

            float rampUp = micro.rampUp;
            float exitDelay = (float)Math.Round(micro.Duration - micro.exitDelay, 2);
            float rampDown = (float)Math.Round(micro.Duration - micro.exitDelay - micro.rampDown, 2);

            while (micro.Duration >= time) {
                if (rampUp >= time) {
                    float by = 1f / rampUp * time;
                    advanced.addSensation(createAdvancedMicro(micro.frequency, lerp(0, micro.intensity, by), micro.duration <= 0.2, muscles));
                } else if (exitDelay < time) {
                    advanced.addSensation(createAdvancedMicro(micro.frequency, 0, micro.duration <= 0.2, muscles));
                } else if (rampDown < time) {
                    float by = 1f / micro.rampDown * (time - rampDown);
                    advanced.addSensation(createAdvancedMicro(micro.frequency, lerp(micro.intensity, 0, by), micro.duration <= 0.2, muscles));
                } else {
                    advanced.addSensation(createAdvancedMicro(micro.frequency, micro.intensity, micro.duration <= 0.2, muscles));
                }
                time = (float)Math.Round(time + 0.1f, 2);
            }

            return advanced;
        }

        private static int lerp(float firstFloat, float secondFloat, float by) {
            return (int)(firstFloat * (1 - by) + secondFloat * by);
        }

        private static AdvancedStreamingSensation createAdvancedMicro(int frequency, int intensity, bool isShortSensation, Muscle[] muscles) {

            if (muscles == null || muscles.Length == 0) {
                muscles = Muscle.All;
            }

            Muscle[] modifiedMuscle = new Muscle[muscles.Length];
            for (int i = 0; i < modifiedMuscle.Length; i++) {
                Muscle m = muscles[i];
                if (intensity == 0) {
                    modifiedMuscle[i] = m.WithIntensity(0);
                } else {
                    float modifiedIntensity = ((float)m.intensity) / 100 * intensity;
                    modifiedMuscle[i] = m.WithIntensity((int)Math.Round(modifiedIntensity));
                }
            }

            float duration = 0.3f;
            if (isShortSensation) {
                // Sensations that are 0.1 or 0.2 will get an additional 20% Scaling fixed by owo
                duration = 0.2f;
            }

            Sensation s = SensationsFactory.Create(frequency, duration, 100, 0, 0, 0);
            return AdvancedStreamingSensation.createByAdvancedMicro(new SensationWithMuscles(s, modifiedMuscle));
        }

        public static AdvancedStreamingSensation createSensationCurve(int frequency, List<int> intensities, Muscle[] muscles = null) {
            AdvancedStreamingSensation curve = new AdvancedStreamingSensation();
            if (intensities == null) {
                return curve;
            }

            foreach (int intensity in intensities) {
                curve.addSensation(createAdvancedMicro(frequency, intensity, false, muscles));
            }

            return curve;
        }

        public static AdvancedStreamingSensation createSensationCurve(List<int> frequencies, int intensity, Muscle[] muscles = null) {
            AdvancedStreamingSensation curve = new AdvancedStreamingSensation();
            if (frequencies == null) {
                return curve;
            }

            foreach (int frequency in frequencies) {
                curve.addSensation(createAdvancedMicro(frequency, intensity, false, muscles));
            }

            return curve;
        }

        public static AdvancedStreamingSensation createSensationRamp(int frequencyStart, int frequencyEnd, int intensityStart, int intensityEnd,
                float duration, Muscle[] muscles = null) {

            AdvancedStreamingSensation ramp = new AdvancedStreamingSensation();
            float time = 0.1f;
            int snippets = float2snippets(duration);

            for (int i = 0; i <= snippets; i++) {
                int frequency = lerp(frequencyStart, frequencyEnd, 1f / duration * time);
                int intensity = lerp(intensityStart, intensityEnd, 1f / duration * time); ;
                ramp.addSensation(createAdvancedMicro(frequency, intensity, false, muscles));
                    time = (float)Math.Round(time + 0.1f, 2);
                }

            return ramp;
        }

        public static int float2snippets(float seconds) {
            return (int)Math.Round(seconds * 10);
        }

        public static AdvancedStreamingSensation actualMerge(AdvancedStreamingSensation origAdvanced, AdvancedStreamingSensation newAdvanced,
            AdvancedSensationBuilderMergeOptions mergeOptions) {

            List<SensationWithMuscles> origSnippets = origAdvanced.getSnippets();
            List<SensationWithMuscles> newSnippets = newAdvanced.getSnippets();

            AdvancedStreamingSensation mergedSensation = new AdvancedStreamingSensation();

            int delaySnippets = float2snippets(mergeOptions.delaySeconds);

            for (int i = 0; i < delaySnippets; i++) {
                newSnippets.Insert(0, null);
            }

            while (newSnippets.Count < origSnippets.Count) {
                newSnippets.Add(null);
            }

            for (int i = 0; i < newSnippets.Count; i++) {
                SensationWithMuscles origSensation = null;
                if (origSnippets.Count > i) {
                    origSensation = origSnippets[i];
                }
                SensationWithMuscles newSensation = newSnippets[i];

                if (origSensation == null && newSensation == null) {
                    mergedSensation.addSensation(createAdvancedMicro(0, 0, false, Muscle.All));
                } else if (origSensation == null) {
                    mergedSensation.addSensation(AdvancedStreamingSensation.createByAdvancedMicro((SensationWithMuscles)newSensation.MultiplyIntensityBy(mergeOptions.intensityScale)));
                } else if (newSensation == null) {
                    mergedSensation.addSensation(AdvancedStreamingSensation.createByAdvancedMicro(origSensation));
                } else {
                    Muscle[] newMuscles = newSensation.muscles.MultiplyIntensityBy(mergeOptions.intensityScale);
                    Muscle[] origMuscles = origSensation.muscles;
                    Muscle[] mergedMuscles = actualMuscleMerge(newMuscles, origMuscles, mergeOptions.mode);

                    SensationWithMuscles baseSensation = origSensation;
                    if (mergeOptions.overwriteBaseSensation) {
                        baseSensation = newSensation;
                    }

                    mergedSensation.addSensation(AdvancedStreamingSensation.createByAdvancedMicro(new SensationWithMuscles(baseSensation.reference, mergedMuscles)));
                }
            }

            return mergedSensation;
        }

        private static Muscle[] actualMuscleMerge(Muscle[] newMuscles, Muscle[] origMuscles, AdvancedSensationBuilderMergeOptions.MuscleMergeMode mode) {
            switch (mode) {
                case AdvancedSensationBuilderMergeOptions.MuscleMergeMode.MAX:
                    return actualMuscleMergeMax(newMuscles, origMuscles);
                case AdvancedSensationBuilderMergeOptions.MuscleMergeMode.MIN:
                    return actualMuscleMergeMin(newMuscles, origMuscles);
                case AdvancedSensationBuilderMergeOptions.MuscleMergeMode.KEEP:
                    return actualMuscleMergeKeep(newMuscles, origMuscles);
                case AdvancedSensationBuilderMergeOptions.MuscleMergeMode.OVERRIDE:
                    return actualMuscleMergeOverride(newMuscles, origMuscles);
                default:
                    throw new Exception("Unknown Merge Type");
            }
        }

        private static Muscle[] actualMuscleMergeMax(Muscle[] newMuscles, Muscle[] origMuscles) {
            List<Muscle> mergedMuscles = new List<Muscle>();
            mergedMuscles.AddRange(origMuscles);
            foreach (Muscle m in newMuscles) {
                if (!mergedMuscles.Any(origM => origM.id == m.id)) {
                    mergedMuscles.Add(m);
                } else {
                    Muscle existing = mergedMuscles.Find(origM => origM.id == m.id);
                    if (existing.intensity < m.intensity) {
                        mergedMuscles.Remove(existing);
                        mergedMuscles.Add(m);
                    }
                }
            }
            return mergedMuscles.ToArray();
        }

        private static Muscle[] actualMuscleMergeMin(Muscle[] newMuscles, Muscle[] origMuscles) {
            List<Muscle> mergedMuscles = new List<Muscle>();
            mergedMuscles.AddRange(origMuscles);
            foreach (Muscle m in newMuscles) {
                if (!mergedMuscles.Any(origM => origM.id == m.id)) {
                    mergedMuscles.Add(m);
                } else {
                    Muscle existing = mergedMuscles.Find(origM => origM.id == m.id);
                    if (existing.intensity > m.intensity) {
                        mergedMuscles.Remove(existing);
                        mergedMuscles.Add(m);
                    }
                }
            }
            return mergedMuscles.ToArray();
        }

        private static Muscle[] actualMuscleMergeKeep(Muscle[] newMuscles, Muscle[] origMuscles) {
            List<Muscle> mergedMuscles = new List<Muscle>();
            mergedMuscles.AddRange(origMuscles);
            foreach (Muscle m in newMuscles) {
                if (!mergedMuscles.Any(origM => origM.id == m.id)) {
                    mergedMuscles.Add(m);
                }
            }
            return mergedMuscles.ToArray();
        }

        private static Muscle[] actualMuscleMergeOverride(Muscle[] newMuscles, Muscle[] origMuscles) {
            List<Muscle> mergedMuscles = new List<Muscle>();
            mergedMuscles.AddRange(newMuscles);
            foreach (Muscle m in origMuscles) {
                if (!mergedMuscles.Any(newM => newM.id == m.id)) {
                    mergedMuscles.Add(m);
                }
            }
            return mergedMuscles.ToArray();
        }

        public static AdvancedStreamingSensation cutSensation(AdvancedStreamingSensation origSensation, int from, int till) {
            AdvancedStreamingSensation cutSensation = new AdvancedStreamingSensation();

            for (int i = 0; i < origSensation.sensations.Count; i++) {
                if (i >= from && i <= till) {
                    cutSensation.addSensation(origSensation.sensations[i]);
                }
            }

            return cutSensation;
        }

    }
}