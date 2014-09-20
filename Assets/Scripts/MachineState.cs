using System;
using System.Collections;
using System.Collections.Generic;
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
        public float MoveToCenterDuration = 0.95f;

        private List<MachineState> _outgoingStates;

        public void Setup()
        {
            _outgoingStates = new List<MachineState>();

            var angleToRotate = 2*Mathf.PI/State.OutgoingStates.Count;

            for (int i = 0, n = State.OutgoingStates.Count; i < n; i++)
            {
                var outStateGameObject = (GameObject) Instantiate(OtherStatesPrefab, transform.position, Quaternion.identity);

                var dX = Mathf.Cos(i*angleToRotate)*Radius;
                var dY = Mathf.Sin(i*angleToRotate)*Radius;

                outStateGameObject.transform.position += new Vector3(dX, dY, 0);
                outStateGameObject.renderer.material.color = new Color(outStateGameObject.renderer.material.color.r, outStateGameObject.renderer.material.color.g, outStateGameObject.renderer.material.color.b, 0);

                var outState = outStateGameObject.GetComponent<MachineState>();
                outState.UpdateState(State.OutgoingStates[i]);
                outState.OtherStatesPrefab = OtherStatesPrefab;
                outState.LineDrawer = LineDrawer;
                outState.FadeIn();

                _outgoingStates.Add(outState);
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
                        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hitInfo;

                        Physics.Raycast(ray, out hitInfo, 100f);
                        if (hitInfo.collider)
                        {
                            var ms = hitInfo.collider.GetComponent<MachineState>();

                            if (ms != null)
                            {
                                // Move it HERE!
                                ms.ToCenterAndSetup();

                                HideOutgoinExcept(ms);

                                FadeOut();
                            }
                        }

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

        public void FadeIn()
        {
            StartCoroutine(LerpAlph(1, MoveToCenterDuration * 1.2f));
        }


        public void FadeOut()
        {
            StartCoroutine(LerpFadeOut(0, MoveToCenterDuration*1.2f));
        }

        public void ToCenterAndSetup()
        {
            StartCoroutine(LerpPosition(Vector3.zero, MoveToCenterDuration));
        }


        public void MoveAway()
        {
            StartCoroutine(LerpPosition(2*transform.position, MoveToCenterDuration));
        }


        public void HideOutgoinExcept(MachineState exception)
        {
            for (int i = 0, n = _outgoingStates.Count; i < n; i++)
            {
                if (_outgoingStates[i] != exception)
                {
                    _outgoingStates[i].FadeOut();
                    _outgoingStates[i].MoveAway();
                }
            }
        }

        private IEnumerator LerpPosition(Vector3 target, float duration)
        {
            Vector3 start = transform.position;
            float elapsedTime = 0.0f;

            while (transform.position != target)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(start, target, elapsedTime/duration);
                yield return null;
            }

            Setup();
        }

        private IEnumerator LerpAlph(float targetAlpha, float duration)
        {
            Color targetColor = renderer.material.color;
            targetColor.a = targetAlpha;

            float elapsedTime = 0.0f;

            while (Math.Abs(renderer.material.color.a - targetAlpha) > 0.0001f)
            {
                elapsedTime += Time.deltaTime;
                renderer.material.color = Color.Lerp(renderer.material.color, targetColor, elapsedTime / duration);

                yield return null;
            }

            renderer.material.color = targetColor;
        }

        private IEnumerator LerpFadeOut(float targetAlpha, float duration)
        {
            Color targetColor = renderer.material.color;
            targetColor.a = targetAlpha;

            float elapsedTime = 0.0f;

            while (Math.Abs(renderer.material.color.a - targetAlpha) > 0.0001f)
            {
                elapsedTime += Time.deltaTime;
                renderer.material.color = Color.Lerp(renderer.material.color, targetColor, elapsedTime/duration);

                yield return null;
            }

            renderer.material.color = targetColor;
            gameObject.SetActive(false);
        }
    }
}