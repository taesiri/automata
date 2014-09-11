using UnityEngine;

namespace Assets.Scripts
{
    public class MachineState : MonoBehaviour
    {
        public LineRenderer LineDrawer;
        public GameObject OtherStatesPrefab;
        public State State;
        public TextMesh Text;
        public const int Radius = 4;
        public bool Ghost = true;
        private bool _isDown = false;

        public void Setup()
        {
            var angleToRotate = 2*Mathf.PI/State.OutgoingStates.Count;

            for (int i = 0, n = State.OutgoingStates.Count; i < n; i++)
            {
                var outStateGameObject = (GameObject) Instantiate(OtherStatesPrefab, transform.position, Quaternion.identity);

                var dX = Mathf.Cos(i*angleToRotate)*Radius;
                var dY = Mathf.Sin(i*angleToRotate)*Radius;

                outStateGameObject.transform.position += new Vector3(dX, dY, 0);

                var outState = outStateGameObject.GetComponent<MachineState>();
                outState.UpdateState(State.OutgoingStates[i]);
            }

            LineDrawer.renderer.enabled = false;
            var tPos = transform.position;
            LineDrawer.SetPosition(0, tPos);

            Ghost = false;
        }


        public void Update()
        {
            if (!Ghost)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _isDown = true;

                    LineDrawer.renderer.enabled = true;
                }

                if (_isDown)
                {
                    var endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    endPos.z = 0;
                    LineDrawer.SetPosition(1, endPos);
                }


                if (Input.GetMouseButtonUp(0))
                {
                    if (_isDown)
                    {
                        LineDrawer.renderer.enabled = false;

                        _isDown = false;
                    }
                }
            }
        }


        public void UpdateRelations(State newState)
        {
            UpdateState(newState);
            Setup();
        }

        public void UpdateState(State newState)
        {
            State = newState;
            if (Text)
            {
                Text.text = State.StateName;
            }
        }
    }
}