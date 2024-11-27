using OwoAdvancedSensationBuilder.builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using OWOGame;
using OwoAdvancedSensationBuilderNet8.manager;

namespace OwoAdvancedSensationBuilder.manager
{
    public class AdvancedSensationManager {

        private enum ProcessState { ADD, REMOVE, UPDATE }

        private static AdvancedSensationManager managerInstance;

        private System.Timers.Timer timer;

        private Dictionary<string, AdvancedSensationStreamInstance> playSensations;
        private Dictionary<AdvancedSensationStreamInstance, ProcessState> processSensation;
        private List<string> _priorityList;
        public List<string> priorityList { get { return _priorityList; } }

        private int tick = 0;
        private bool calculating = false;
        private Sensation calculatedSensation = null;


        Stopwatch watch;

        private AdvancedSensationManager() {
            timer = new System.Timers.Timer(100);
            timer.Elapsed += streamSensation;
            timer.Elapsed += calcManagerTick;
            timer.AutoReset = true;
            timer.Enabled = false;

            playSensations = new Dictionary<string, AdvancedSensationStreamInstance>();
            processSensation = new Dictionary<AdvancedSensationStreamInstance, ProcessState>();
            _priorityList = new List<string>();
        }

        public static AdvancedSensationManager getInstance() {
            if (managerInstance == null) {
                managerInstance = new AdvancedSensationManager();
            }
            return managerInstance;
        }

        private void streamSensation(Object source, ElapsedEventArgs e) {
            OWO.Send(calculatedSensation);
            tick++;
            Debug.WriteLine(tick + " / " + watch.ElapsedMilliseconds);
        }

        private void calcManagerTick(Object source, ElapsedEventArgs e) {
            if (calculating) {
                return;
            }
            try {
                calculating = true;
                processRemove(false);
                processUpdate();
                processAdd();
                calcSensation();
                processRemove(true);
            } finally {
                calculating = false;
            }
        }

        private void processUpdate() {
            foreach (var process in new Dictionary<AdvancedSensationStreamInstance, ProcessState>(processSensation)) {

                if (process.Value != ProcessState.UPDATE) {
                    continue;
                }
                AdvancedSensationStreamInstance instance = process.Key;
                AdvancedSensationStreamInstance oldInstance = null;

                if (playSensations.Keys.Contains(instance.name)) {
                    // Update Playing Sensation
                    oldInstance = playSensations[instance.name];
                } else {
                    foreach (KeyValuePair<AdvancedSensationStreamInstance, ProcessState> processInstance in processSensation.ToArray()) {
                        if (processInstance.Value == ProcessState.ADD && processInstance.Key.name == instance.name) {
                            // Update to be Added Sensation
                            oldInstance = processInstance.Key;
                            break;
                        }
                    }
                }

                if (oldInstance != null) {
                    oldInstance.updateSensation(instance.sensation);
                }

                processSensation.Remove(process.Key);
            }
        }

        private void processAdd() {
            foreach (var process in new Dictionary<AdvancedSensationStreamInstance, ProcessState>(processSensation)) {

                if (process.Value != ProcessState.ADD) {
                    continue;
                }
                AdvancedSensationStreamInstance instance = process.Key;
                instance.firstTick = tick;

                playSensations[instance.name] = instance;

                processSensation.Remove(process.Key);
            }
        }

        private void processRemove(bool endOfCylce) {
            foreach (var process in new Dictionary<AdvancedSensationStreamInstance, ProcessState>(processSensation)) {

                if (process.Value != ProcessState.REMOVE) {
                    continue;
                }

                AdvancedSensationStreamInstance instance = process.Key;

                if (playSensations.ContainsKey(instance.name)) {
                    playSensations.Remove(instance.name);
                }

                processSensation.Remove(process.Key);
            }

            if (playSensations.Count == 0 && endOfCylce) {
                // Only allow to stop manager at end of cycle.
                // Else race time conditions might stop manager while something to add just got inserted.
                bool toAdd = false;
                foreach (var processInstance in processSensation) {
                    if (processInstance.Value == ProcessState.ADD) {
                        toAdd = true;
                        break;
                    }
                }
                if (!toAdd) {
                    resetManagerState();
                }
            }
        }

        private void calcSensation() {
            int calcTick = tick;
            AdvancedSensationBuilder builder = null;

            AdvancedSensationBuilderMergeOptions mergeOptions = new AdvancedSensationBuilderMergeOptions();
            mergeOptions.mode = AdvancedSensationBuilderMergeOptions.MuscleMergeMode.MAX;
            mergeOptions.overwriteBaseSensation = true;


            List<string> reversPrio = new List<string>(priorityList);
            reversPrio.Reverse();
            Dictionary<string, AdvancedSensationStreamInstance> snapshot = new Dictionary<string, AdvancedSensationStreamInstance>(playSensations);

            foreach (var priority in reversPrio) {
                if (!snapshot.ContainsKey(priority)) {
                    continue;
                }

                AdvancedSensationStreamInstance sensationInstance = snapshot[priority];

                Sensation sensationTick = sensationInstance.getSensationAtTick(calcTick);
                if (sensationTick == null) {
                    continue;
                }

                if (builder == null) {
                    builder = new AdvancedSensationBuilder(sensationTick);
                } else {
                    builder.merge(sensationTick, mergeOptions);
                }

                if (sensationInstance.isLastTickOfCycle(calcTick) && !sensationInstance.loop) {
                    playSensations.Remove(priority);
                }
            }

            mergeOptions.overwriteBaseSensation = false;
            foreach (var entry in snapshot) {
                if (reversPrio.Contains(entry.Key)) {
                    continue;
                }
                AdvancedSensationStreamInstance sensationInstance = entry.Value;

                Sensation sensationTick = sensationInstance.getSensationAtTick(calcTick);
                if (sensationTick == null) {
                    continue;
                }

                if (builder == null) {
                    builder = new AdvancedSensationBuilder(sensationTick);
                } else {
                    builder.merge(sensationTick, mergeOptions);
                }

                if (sensationInstance.isLastTickOfCycle(calcTick) && !sensationInstance.loop) {
                    playSensations.Remove(entry.Key);
                }
            }
            if (builder != null) {
                // May be null due to racetime condition, on last Sensation remove
                calculatedSensation = builder.getSensationForStream();
            }
        }

        public void playOnce(Sensation sensation) {
            addSensationInstance(new AdvancedSensationStreamInstance(analyzeSensation(sensation).name, sensation, false));
        }

        public void playLoop(Sensation sensation) {
            addSensationInstance(new AdvancedSensationStreamInstance(analyzeSensation(sensation).name, sensation, true));
        }

        public void play(AdvancedSensationStreamInstance instance) {
            addSensationInstance(instance);
        }

        public void updateSensation(Sensation sensation, String name = null) {
            if (name == null) {
                name = analyzeSensation(sensation).name;
            }
            processSensation[new AdvancedSensationStreamInstance(name, sensation)] = ProcessState.UPDATE;
        }

        public void stopSensation(string sensationInstanceName) {
            AdvancedSensationStreamInstance instance = new AdvancedSensationStreamInstance(sensationInstanceName);
            instance.overwriteManagerProcessList = true;
            RemoveInstanceFromManager(instance);
        }

        private void RemoveInstanceFromManager(AdvancedSensationStreamInstance instance) {
            if (instance.name != null && (!processSensation.ContainsKey(instance) || instance.overwriteManagerProcessList)) {
                processSensation[instance] = ProcessState.REMOVE;
            }
        }

        private void addSensationInstance(AdvancedSensationStreamInstance instance) {
            if (!processSensation.ContainsKey(instance) || instance.overwriteManagerProcessList) {
                processSensation[instance] = ProcessState.ADD;
            }

            if (!timer.Enabled) {
                calcManagerTick(null, null);
                watch = Stopwatch.StartNew();
                timer.Start();
            }
        }

        public void stopAll() {
            resetManagerState();
            playSensations.Clear();
            processSensation.Clear();
        }

        private void resetManagerState() {
            timer.Stop();
            tick = 0;
            // cancel the last sensation
            OWO.Send(SensationsFactory.Create(0, 0, 0, 0, 0, 1));
        }

        public Dictionary<string, AdvancedSensationStreamInstance> getPlayingSensationInstances(bool addPlanned = true) {
            Dictionary<string, AdvancedSensationStreamInstance> returnInstances = new Dictionary<string, AdvancedSensationStreamInstance>();
            foreach (var playInstance in playSensations) {
                returnInstances[playInstance.Key] = playInstance.Value;
            }
            if (addPlanned) {
                foreach (var processInstance in processSensation) {
                    if (processInstance.Value == ProcessState.ADD) {
                        returnInstances[processInstance.Key.name] = processInstance.Key;
                    }
                }
            }
            return returnInstances;
        }

        private MicroSensation analyzeSensation(Sensation sensation) {
            if (sensation is MicroSensation) {
                return sensation as MicroSensation;
            } else if (sensation is SensationWithMuscles) {
                SensationWithMuscles withMuscles = sensation as SensationWithMuscles;
                return analyzeSensation(withMuscles.reference);
            } else if (sensation is SensationsSequence) {
                SensationsSequence sequence = sensation as SensationsSequence;
                foreach (Sensation s in sequence.sensations) {
                    // just take first
                    return analyzeSensation(s);
                }
            } else if (sensation is BakedSensation) {
                BakedSensation baked = sensation as BakedSensation;
                return analyzeSensation(baked.reference);
            }
            return null;
        }

    }
}
