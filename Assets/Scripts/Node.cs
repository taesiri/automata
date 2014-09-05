using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Node : MonoBehaviour
    {
        public List<Node> OutgoingNodes;
        public List<Node> IncomingNodes;
    }
}