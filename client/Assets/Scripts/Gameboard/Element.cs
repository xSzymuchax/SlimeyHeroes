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
    /// <summary>
    /// Class containing informations, about elements on gameboard. 
    /// </summary>
    public class Element : MonoBehaviour
    {
        private int _x;
        private int _y;
        private float _fallingAnimationDuration = 0.3f;
        public ElementType elementType;
        public Effect _effect;

        /// <summary>
        /// X position of element
        /// </summary>
        public int X { get => _x; set => _x = value; }
        
        /// <summary>
        /// Y position of element
        /// </summary>
        public int Y { get => _y; set => _y = value; }

        /// <summary>
        /// Effect assigned to element. Can be null.
        /// </summary>
        public Effect Effect { get => _effect; protected set => _effect = value; }

        /// <summary>
        /// Sets position of the element - informs it, where it is on gameboard.
        /// </summary>
        /// <param name="position2D">position in gameboard</param>
        public void SetPosition(Position2D position2D)
        {
            X = position2D.X;
            Y = position2D.Y;

            _effect?.SetPosition(position2D);
        }

        /// <summary>
        /// Sets collecting effect of element.
        /// </summary>
        /// <param name="effect">added effect</param>
        public void SetEffect(Effect effect)
        {
            Effect = effect;
            //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        /// <summary>
        /// If element is clicked, it should report that to gamecontroller.
        /// </summary>
        private void OnMouseDown()
        {
            Position2D position2D = new Position2D(X, Y);
            GameController.Instance.ElementPressed(position2D);
        }

        /// <summary>
        /// Sets how long element should be falling.
        /// </summary>
        /// <param name="time">duration of fall</param>

        public void SetFallingAnimationTime(float time)
        {
            _fallingAnimationDuration = time;
        }

        /// <summary>
        /// Starts falling animation of the element.
        /// </summary>
        /// <param name="targetPosition">position in scene, where element should fall</param>

        public void FallToPosition(Vector3 targetPosition)
        {
            StopAllCoroutines();
            StartCoroutine(FallAnimation(targetPosition));
        }

        /// <summary>
        /// Falling animation of element.
        /// </summary>
        /// <param name="targetPosition">position in scene, where element should fall</param>
        /// <returns>Coroutine enumerator.</returns>
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
