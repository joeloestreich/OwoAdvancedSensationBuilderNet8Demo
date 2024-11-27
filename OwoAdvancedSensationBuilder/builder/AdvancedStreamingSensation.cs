using OWOGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwoAdvancedSensationBuilder.builder
{
    public class AdvancedStreamingSensation : SensationsSequence {

        public static AdvancedStreamingSensation createByAdvancedMicro(SensationWithMuscles advancedMicro) {
            if (advancedMicro.Duration > 0.3f) {
                Console.WriteLine("AdvancedStreamingSensation - createByAdvancedMicro() - Invalid Advanced Micro");
                return null;
            }
            return new AdvancedStreamingSensation(advancedMicro);
        }

        private AdvancedStreamingSensation(SensationWithMuscles advancedMicro) : base() {
            // Dont make this public to avoid missuse, as this wont do a check or transform
            this.sensations.Add(advancedMicro);
        }

        public AdvancedStreamingSensation(params Sensation[] sensations) : base() {
            foreach (Sensation s in sensations) {
                List<SensationWithMuscles> snippets = new AdvancedSensationBuilder(s).getSensationForStream().getSnippets();
                foreach (SensationWithMuscles swm in snippets) {
                    this.sensations.Add(swm);
                }
            }
        }

        public void addSensation(Sensation addSensation) {
            List<SensationWithMuscles> snippets = new AdvancedSensationBuilder(addSensation).getSensationForStream().getSnippets();
            foreach (SensationWithMuscles swm in snippets) {
                sensations.Add(swm);
            }
        }

        public List<SensationWithMuscles> getSnippets() {
            List<SensationWithMuscles> snippets = new List<SensationWithMuscles>();

            foreach (Sensation sensation in sensations) {
                if (sensation is SensationWithMuscles) {
                    snippets.Add(sensation as SensationWithMuscles);
                } else {
                    // shouldnt happen, as long as devs dont manually mess with the sensations list
                    Console.WriteLine("AdvancedStreamingSensation - getSnippets() - sensations contains invalid Sensations: " + sensation.GetType());
                    snippets.AddRange(new AdvancedSensationBuilder(sensation).getSensationForStream().getSnippets());
                }
            }

            return snippets;
        }

        public bool isEmpty() {
            return sensations.Count == 0;
        }

        public Sensation transformForSend() {
            List<SensationWithMuscles> snippets = new List<SensationWithMuscles>();

            foreach (Sensation sensation in sensations) {
                if (sensation is SensationWithMuscles) {
                    snippets.Add(sensation as SensationWithMuscles);
                } else {
                    // shouldnt happen, as long as devs dont manually mess with the sensations list
                    Console.WriteLine("AdvancedStreamingSensation - getSnippets() - sensations contains invalid Sensations: " + sensation.GetType());
                    // Dont call "for send", as this is just a shortcut, to this method.
                    snippets.AddRange(new AdvancedSensationBuilder(sensation).getSensationForStream().getSnippets());
                }
            }

            Sensation transformed = null;
            foreach (SensationWithMuscles withMuscles in snippets) {
                foreach (MicroSensation micro in analyzeSensation(withMuscles)) {
                    Sensation current = SensationsFactory.Create(micro.frequency, 0.1f, micro.intensity, micro.rampUp, micro.rampDown, micro.exitDelay);
                    current = current.WithMuscles(withMuscles.muscles);
                    if (transformed == null) {
                        transformed = current;
                    } else {
                        transformed = transformed.Append(current);
                    }
                }
            }
            return transformed;
        }
        private List<MicroSensation> analyzeSensation(Sensation sensation) {
            List<MicroSensation> list = new List<MicroSensation>();
            if (sensation is MicroSensation) {
                list.Add(sensation as MicroSensation);
            } else if (sensation is SensationWithMuscles) {
                SensationWithMuscles withMuscles = sensation as SensationWithMuscles;
                list.AddRange(analyzeSensation(withMuscles.reference));
            } else if (sensation is SensationsSequence) {
                SensationsSequence sequence = sensation as SensationsSequence;
                foreach (Sensation s in sequence.sensations) {
                    list.AddRange(analyzeSensation(s));
                }
            } else if (sensation is BakedSensation) {
                BakedSensation baked = sensation as BakedSensation;
                list.AddRange(analyzeSensation(baked.reference));
            }
            return list;
        }

    }
}
