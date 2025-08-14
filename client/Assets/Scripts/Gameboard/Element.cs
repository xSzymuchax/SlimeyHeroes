using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class Element : MonoBehaviour
    {
        private int _x;
        private int _y;
        private float _fallingAnimationDuration = 0.3f;
        public ElementType elementType;
        private Effect _effect;


        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public Effect Effect { get => _effect; protected set => _effect = value; }

        public void SetPosition(Position2D position2D)
        {
            X = position2D.X;
            Y = position2D.Y;
        }

        public void SetEffect(Effect effect)
        {
            Effect = effect;
        }

        private void OnMouseDown()
        {
            Position2D position2D = new Position2D(X, Y);
            GameController.Instance.ElementPressed(position2D);
        }

        public void SetFallingAnimationTime(float time)
        {
            _fallingAnimationDuration = time;
        }

        public void FallToPosition(Vector3 targetPosition)
        {
            StopAllCoroutines();
            StartCoroutine(FallAnimation(targetPosition));
        }

        private IEnumerator FallAnimation(Vector3 targetPosition)
        {
            Vector3 startPosition = transform.position;
            float elapsed = 0f;

            while (elapsed < _fallingAnimationDuration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / _fallingAnimationDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
        }
    }
}
