using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeshDistort
{
    /// <summary>
    /// Save vertices values for a mesh to use in animation.
    /// </summary>
    public class FrameAnimation : ScriptableObject
    {
        /// <summary>
        /// Transform of the mesh to me animated
        /// </summary>
        public Transform transform;

        /// <summary>
        /// Vertices on this frame to be applied to the mesh
        /// </summary>
        public Vector3[] vertices;

        /// <summary>
        /// Get the mesh from the transform
        /// </summary>
        public Mesh mesh
        {
            get
            {
                if (_mesh == null)
                    _mesh = transform.GetComponent<MeshFilter>().mesh;
                return _mesh;
            }
        }

        private Mesh _mesh;
        
        /// <summary>
        /// Create a new Frame animation
        /// </summary>
        /// <param name="transform">transform of the mesh that the animation will be applied</param>
        /// <param name="vertices">Vertices that will be applied in this frame</param>
        public FrameAnimation(Transform transform, Vector3[] vertices)
        {
            this.transform = transform;
            this.vertices = vertices;
        }
    }

}