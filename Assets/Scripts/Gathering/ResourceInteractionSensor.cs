using System.Collections.Generic;
using UnityEngine;

namespace SurvivalGame.Gathering
{
    [RequireComponent(typeof(SphereCollider))]
    public class ResourceInteractionSensor : MonoBehaviour
    {
        private readonly List<ResourceNode> nearby = new();

        public ResourceNode Closest
        {
            get
            {
                nearby.RemoveAll(x => !x || x.IsDepleted);

                ResourceNode best = null;
                float bestSqr = float.MaxValue;

                foreach (var node in nearby)
                {
                    float sqr = (node.transform.position - transform.position).sqrMagnitude;
                    if (sqr >= bestSqr) continue;
                    best = node;
                    bestSqr = sqr;
                }

                return best;
            }
        }

        private void Reset()
        {
            var sphere = GetComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = 2f;
        }

        private void OnTriggerEnter(Collider other)
        {
            var node = other.GetComponentInParent<ResourceNode>();
            if (node && !nearby.Contains(node))
                nearby.Add(node);
        }

        private void OnTriggerExit(Collider other)
        {
            var node = other.GetComponentInParent<ResourceNode>();
            if (node)
                nearby.Remove(node);
        }
    }
}
