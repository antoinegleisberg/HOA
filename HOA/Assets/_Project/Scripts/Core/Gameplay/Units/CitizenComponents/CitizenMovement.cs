using antoinegleisberg.Types;
using antoinegleisberg.Animation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Citizen))]
    public class CitizenMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private bool _isInBuilding = false;
        private Building _currentBuilding;

        // ToDo: Move fading to another class handling citizen UI
        private SpriteRenderer _spriteRenderer => GetComponent<SpriteRenderer>();

        public IEnumerator MoveToPosition(Vector3 targetPosition)
        {
            yield return StartCoroutine(MoveToClosestPosition(new List<Vector3> { targetPosition }));
        }

        public IEnumerator MoveToBuilding(Building target)
        {
            List<RoadConnection> roadConnections = target.GetComponentsInChildren<RoadConnection>().ToList();
            List<Vector3> roadConnectionsPositions = roadConnections.Select((RoadConnection rc) => rc.transform.position).ToList();

            yield return StartCoroutine(MoveToClosestPosition(roadConnectionsPositions));

            Vector3 selectedPosition = roadConnectionsPositions.ClosestTo(transform.position);
            int index = roadConnectionsPositions.IndexOf(selectedPosition);
            RoadConnection selectedRoadConnection = roadConnections[index];
            EntryPoint entryPoint = selectedRoadConnection.GetComponentInChildren<EntryPoint>();

            Coroutine fadingOutCoroutine = StartCoroutine(_spriteRenderer.FadeOut(1.0f));

            yield return StartCoroutine(MoveStraightToPosition(transform, entryPoint.transform.position));

            StopCoroutine(fadingOutCoroutine);
            _spriteRenderer.color = new Color(1, 1, 1, 0);

            _isInBuilding = true;
            _currentBuilding = target;
        }

        private IEnumerator MoveToClosestPosition(List<Vector3> targetPositions)
        {
            Transform t = transform;

            List<Vector3> startCoords = new List<Vector3> { t.position };
            if (_isInBuilding)
            {
                List<RoadConnection> roadConnections = _currentBuilding.GetComponentsInChildren<RoadConnection>().ToList();
                List<Vector3> roadConnectionsPositions = roadConnections.Select((RoadConnection rc) => rc.transform.position).ToList();
                startCoords = roadConnectionsPositions;
            }

            List<Vector3> path = PathfindingGraph.Instance.GetPath(startCoords, targetPositions);

            if (_isInBuilding)
            {
                Vector3 selectedStart = path[0];
                int index = startCoords.IndexOf(selectedStart);
                RoadConnection selectedRoadConnection = _currentBuilding.GetComponentsInChildren<RoadConnection>()[index];
                EntryPoint entryPoint = selectedRoadConnection.GetComponentInChildren<EntryPoint>();
                path.Insert(0, entryPoint.transform.position);
                
                StartCoroutine(_spriteRenderer.FadeIn(1.0f));

                _isInBuilding = false;
                _currentBuilding = null;
            }

            for (int i = 0; i < path.Count; ++i)
            {
                yield return StartCoroutine(MoveStraightToPosition(t, path[i]));
            }
        }

        private IEnumerator MoveStraightToPosition(Transform t, Vector3 targetPosition)
        {
            while (Vector3.Distance(t.position, targetPosition) > Mathf.Epsilon)
            {
                t.position = Vector3.MoveTowards(t.position, targetPosition, _speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
