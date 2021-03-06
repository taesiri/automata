﻿using UnityEngine;

namespace Assets.Scripts
{
    public class Controller : MonoBehaviour
    {
        public GameObject StatePrefab;
        public MachineState CurrentState;
        public LineRenderer LineDrawer;

        public void AutomataSetup()
        {
            var stateA = new State {StateName = "A"};
            var stateB = new State {StateName = "B"};
            var stateC = new State {StateName = "C"};
            var stateD = new State {StateName = "D"};
            var stateE = new State {StateName = "E"};
            var stateF = new State {StateName = "F"};

            stateA.OutgoingStates.Add(stateA);
            stateA.OutgoingStates.Add(stateB);
            stateA.OutgoingStates.Add(stateC);
            stateA.OutgoingStates.Add(stateE);
            stateA.OutgoingStates.Add(stateF);

            stateB.OutgoingStates.Add(stateC);

            stateC.OutgoingStates.Add(stateD);


            stateD.OutgoingStates.Add(stateA);

            stateE.OutgoingStates.Add(stateA);

            stateF.OutgoingStates.Add(stateA);


            var mA = (GameObject) Instantiate(StatePrefab, Vector3.zero, Quaternion.identity);
            var mAState = mA.GetComponent<MachineState>();
            mAState.LineDrawer = LineDrawer;
            mAState.OtherStatesPrefab = StatePrefab;
            mAState.UpdateRelations(stateA);
        }

        private void Start()
        {
            AutomataSetup();
        }

        private void Update()
        {
        }
    }
}