using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeshDistort
{
    /// <summary>
    /// Hold all the information of a distortion and apply its calculations on the distortion.
    /// </summary>
    [System.Serializable]
    public class DistortData
    {
        
        public class BufferManager
        {
            public struct BufferObjectData
            {
                public int isPingPong;//4b
                public int calculateInWorldSpace; //4b
                public float animationSpeed; //4b
                public int type; //4b
                public float force; //4b
                public Vector3 tile; //12b

            }
            public struct BufferFrameData
            {
                public float movementDisplacement; // 4b
                public Vector3 bMin;
                public Vector3 bMax;
                public Vector3 bNormalized;
                public Vector3 bCenter;
            }

            public BufferObjectData objectStruct;
            public BufferFrameData frameStruct;

            public ComputeBuffer objectData;
            public ComputeBuffer frameData;

            public ComputeBuffer displacedForceX;
            public ComputeBuffer displacedForceY;
            public ComputeBuffer displacedForceZ;

            public ComputeBuffer displacedForceXY;
            public ComputeBuffer displacedForceXZ;
            public ComputeBuffer displacedForceYX;
            public ComputeBuffer displacedForceYZ;
            public ComputeBuffer displacedForceZX;
            public ComputeBuffer displacedForceZY;

            public ComputeBuffer staticForceX;
            public ComputeBuffer staticForceY;
            public ComputeBuffer staticForceZ;


            public void CreateBuffers(){
                ReleaseBuffers();

                objectData = new ComputeBuffer(1, 32);
                frameData = new ComputeBuffer(1, 52);

                displacedForceX = new ComputeBuffer(255, 4);
                displacedForceY = new ComputeBuffer(255, 4);
                displacedForceZ = new ComputeBuffer(255, 4);

                displacedForceXY = new ComputeBuffer(255, 4);
                displacedForceXZ = new ComputeBuffer(255, 4);
                displacedForceYX = new ComputeBuffer(255, 4);
                displacedForceYZ = new ComputeBuffer(255, 4);
                displacedForceZX = new ComputeBuffer(255, 4);
                displacedForceZY = new ComputeBuffer(255, 4);

                staticForceX = new ComputeBuffer(255, 4);
                staticForceY = new ComputeBuffer(255, 4);
                staticForceZ = new ComputeBuffer(255, 4);
            }

            public void SetBuffers(ComputeShader shader, int kernel)
            {
                shader.SetBuffer(kernel, "data", objectData);
                shader.SetBuffer(kernel, "frame", frameData);

                shader.SetBuffer(kernel, "displacedForceX", displacedForceX);
                shader.SetBuffer(kernel, "displacedForceY", displacedForceY);
                shader.SetBuffer(kernel, "displacedForceZ", displacedForceZ);

                shader.SetBuffer(kernel, "displacedForceXY", displacedForceXY);
                shader.SetBuffer(kernel, "displacedForceXZ", displacedForceXZ);
                shader.SetBuffer(kernel, "displacedForceYX", displacedForceYX);
                shader.SetBuffer(kernel, "displacedForceYZ", displacedForceYZ);
                shader.SetBuffer(kernel, "displacedForceZX", displacedForceZX);
                shader.SetBuffer(kernel, "displacedForceZY", displacedForceZY);

                shader.SetBuffer(kernel, "staticCurveX", staticForceX);
                shader.SetBuffer(kernel, "staticCurveY", staticForceY);
                shader.SetBuffer(kernel, "staticCurveZ", staticForceZ);
            }

            public void ReleaseBuffers()
            {
                if (objectData != null) objectData.Release();
                if (frameData != null)  frameData.Release();

                if (displacedForceX != null) displacedForceX.Release();
                if (displacedForceY != null) displacedForceY.Release();
                if (displacedForceZ != null) displacedForceZ.Release();

                if (displacedForceXY != null) displacedForceXY.Release();
                if (displacedForceXZ != null) displacedForceXZ.Release();
                if (displacedForceYX != null) displacedForceYX.Release();
                if (displacedForceYZ != null) displacedForceYZ.Release();
                if (displacedForceZX != null) displacedForceZX.Release();
                if (displacedForceZY != null) displacedForceZY.Release();

                if (staticForceX != null) staticForceX.Release();
                if (staticForceY != null) staticForceY.Release();
                if (staticForceZ != null) staticForceZ.Release();
            }

        
        }

        /// <summary>
        /// If this distortion will be calculated
        /// </summary>
        public bool enabled = true;


        /// <summary>
        /// Name of the effect
        /// </summary>
        public string name = "Effect";

        /// <summary>
        /// Multiplier for the DistortAnimation, will make animation faster or slower
        /// </summary>
        public float animationSpeed = 1f;
        /// <summary>
        /// Type of this distortion
        /// </summary>
        public Distort.Type type;
        /// <summary>
        /// How much force is applied to the distortion
        /// </summary>
        public float force = 1f;
        
        /// <summary>
        /// Displacement for each vertice (only for calculation), used for animation
        /// </summary>
        public float movementDisplacement = 0;

        
        /// <summary>
        /// How much times the distortion will be applied from Bound.min to Bound.max
        /// </summary>
        public Vector3 tile = Vector3.one;

        /// <summary>
        /// Force for a distortion in the axis X, affected by the movementDisplacement param 
        /// </summary>
        public AnimationCurve displacedForceX = new AnimationCurve();
        /// <summary>
        /// Force for a distortion in the axis Y, affected by the movementDisplacement param 
        /// </summary>
        public AnimationCurve displacedForceY = new AnimationCurve();
        /// <summary>
        /// Force for a distortion in the axis Z, affected by the movementDisplacement param 
        /// </summary>
        public AnimationCurve displacedForceZ = new AnimationCurve();

        /// <summary>
        /// Change the value of the X axis of the vertice by its Y value, affected by the movementDisplacement param 
        /// </summary>
        public AnimationCurve displacedForceXY = new AnimationCurve();
        /// <summary>
        /// Change the value of the X axis of the vertice by its Z value, affected by the movementDisplacement param 
        /// </summary>
        public AnimationCurve displacedForceXZ = new AnimationCurve();
        /// <summary>
        /// Change the value of the Y axis of the vertice by its X value, affected by the movementDisplacement param 
        /// </summary>
        public AnimationCurve displacedForceYX = new AnimationCurve();
        /// <summary>
        /// Change the value of the Y axis of the vertice by its Z value, affected by the movementDisplacement param 
        /// </summary>
        public AnimationCurve displacedForceYZ = new AnimationCurve();
        /// <summary>
        /// Change the value of the Z axis of the vertice by its X value, affected by the movementDisplacement param 
        /// </summary>
        public AnimationCurve displacedForceZX = new AnimationCurve();
        /// <summary>
        /// Change the value of the Z axis of the vertice by its Y value, affected by the movementDisplacement param 
        /// </summary>
        public AnimationCurve displacedForceZY = new AnimationCurve();

        /// <summary>
        /// Force for a distortion in the axis X, NOT affected by the movementDisplacement param 
        /// </summary>
        public AnimationCurve staticForceX = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        /// <summary>
        /// Force for a distortion in the axis Y, NOT affected by the movementDisplacement param 
        /// </summary>
        public AnimationCurve staticForceY = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        /// <summary>
        /// Force for a distortion in the axis Z, NOT affected by the movementDisplacement param 
        /// </summary>
        public AnimationCurve staticForceZ = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        /// <summary>
        /// Calculate vertice position inside the bounds using the pingpong algorithm
        /// </summary>
        public bool isPingPong = true;


        /// <summary>
        /// Hide or show the foldout in the editor screen for this distortion
        /// </summary>
        public bool showInEditor = true;

        /// <summary>
        /// Calculate this distortion in world or local space.
        /// </summary>
        public bool calculateInWorldSpace = false;


        /// <summary>
        /// Used in the Distort method to hold the distortion value of X
        /// </summary>
        float x = 0;
        /// <summary>
        /// Used in the Distort method to hold the distortion value of Y
        /// </summary>
        float y = 0;
        /// <summary>
        /// Used in the Distort method to hold the distortion value of Z
        /// </summary>
        float z = 0;

        /// <summary>
        /// Hold the bound for the mesh, to calculate the distortion for the mesh.
        /// </summary>
        Bounds bounds;
        /// <summary>
        /// Hold the minimum value of the bound for calculation
        /// </summary>
        Vector3 bMin;
        /// <summary>
        /// Hold the maximum value of the bound for calculation
        /// </summary>
        Vector3 bMax;
        /// <summary>
        /// Hold the (max - min) value of the bound for calculation
        /// </summary>
        Vector3 bNormalized;
        /// <summary>
        /// Hold the center position of the bounds
        /// </summary>
        Vector3 bCenter;
        public BufferManager bufferManager;

        public void ReleaseBuffers()
        {
            if (bufferManager != null)
                bufferManager.ReleaseBuffers();
            bufferManager = null;
        }

        public void SetupBuffers()
        {
            bufferManager = new BufferManager();
            bufferManager.objectStruct = new BufferManager.BufferObjectData();
            bufferManager.frameStruct = new BufferManager.BufferFrameData();
            bufferManager.CreateBuffers();

            UpdateObjectDataBuffer();
            UpdateFrameDataBuffer();
            UpdateBufferCurves();

        }
        public void UpdateObjectDataBuffer()
        {
            bufferManager.objectStruct.isPingPong = isPingPong ? 1 : 0;
            bufferManager.objectStruct.tile = tile;
            bufferManager.objectStruct.calculateInWorldSpace = calculateInWorldSpace ? 1 : 0;
            bufferManager.objectStruct.animationSpeed = animationSpeed;
            bufferManager.objectStruct.type = (int)type;
            bufferManager.objectStruct.force = force;

            bufferManager.objectData.SetData(new BufferManager.BufferObjectData[] { bufferManager.objectStruct });
        }
        public void UpdateFrameDataBuffer()
        {
            bufferManager.frameStruct.movementDisplacement = movementDisplacement;
            bufferManager.frameStruct.bMin = bMin;
            bufferManager.frameStruct.bMax = bMax;
            bufferManager.frameStruct.bNormalized = bNormalized;
            bufferManager.frameStruct.bCenter = bCenter;

            bufferManager.frameData.SetData(new BufferManager.BufferFrameData[] { bufferManager.frameStruct });
        }
        public void UpdateBufferCurves()
        {
            bufferManager.staticForceX.SetData(Curve2Array(staticForceX));
            bufferManager.staticForceY.SetData(Curve2Array(staticForceY));
            bufferManager.staticForceZ.SetData(Curve2Array(staticForceZ));

            bufferManager.displacedForceX.SetData(Curve2Array(displacedForceX));
            bufferManager.displacedForceY.SetData(Curve2Array(displacedForceY));
            bufferManager.displacedForceZ.SetData(Curve2Array(displacedForceZ));

            bufferManager.displacedForceXY.SetData(Curve2Array(displacedForceXY));
            bufferManager.displacedForceXZ.SetData(Curve2Array(displacedForceXZ));
            bufferManager.displacedForceYX.SetData(Curve2Array(displacedForceYX));
            bufferManager.displacedForceYZ.SetData(Curve2Array(displacedForceYZ));
            bufferManager.displacedForceZX.SetData(Curve2Array(displacedForceZX));
            bufferManager.displacedForceZY.SetData(Curve2Array(displacedForceZY));
        }

        public float[] Curve2Array(AnimationCurve curve)
        {
            float[] curveArray = new float[255];

            float step = 1 / 255f;
            float force = 0;
            for(int i = 0; i < 255; i++, force += step)
            {
                curveArray[i] = curve.Evaluate(force);
            }

            return curveArray;
        }

        /// <summary>
        /// Set the bounds of the mesh to use in the calculations later.
        /// </summary>
        /// <param name="bounds"></param>
        public void SetBounds(Bounds bounds)
        {
            bMin = bounds.min;
            bMax = bounds.max;
            bCenter = bounds.center;
            bNormalized = bounds.max - bounds.min;

            if (bNormalized.x == 0)
                bNormalized.x = 0.1f;
            if (bNormalized.y == 0)
                bNormalized.y = 0.1f;
            if (bNormalized.z == 0)
                bNormalized.z = 0.1f;

            this.bounds = bounds;
        }

        /// <summary>
        /// Used in the DistortVertice method, hold the position of a vertice inside the bounds (0-1)
        /// </summary>
        Vector3 percentage;
        /// <summary>
        /// Using the static force for each axis this hold the multiplier force for a vertice depending of its axis.
        /// </summary>
        float multiplier;

        /// <summary>
        /// Used to hold the direction of the vertice, used in some calculations like Inflate or spin.
        /// </summary>
        Vector3 dir;

        /// <summary>
        /// Calculate the distortion in a position
        /// </summary>
        /// <param name="vertice">Position to calculate the distortion</param>
        public void DistortVertice(ref Vector3 vertice)
        {
            //Distortion values start at zero
            x = 0;
            y = 0;
            z = 0;

            //Set the point param to start the calculations for it.
            percentage = vertice;

            if (calculateInWorldSpace)
            {
                //How much force this distortion will have depending of its position
                multiplier =
                    staticForceX.Evaluate((percentage.x - bMin.x) / bNormalized.x) *
                    staticForceY.Evaluate((percentage.y - bMin.y) / bNormalized.y) *
                    staticForceZ.Evaluate((percentage.z - bMin.z) / bNormalized.z);

                //Point will become between 0 and 1
                percentage.x /= bNormalized.x;
                percentage.y /= bNormalized.y;
                percentage.z /= bNormalized.z;
            }
            //Set point to local space
            else
            {
                percentage.x -= bMin.x;
                percentage.y -= bMin.y;
                percentage.z -= bMin.z;

                //Point will become between 0 and 1
                percentage.x /= bNormalized.x;
                percentage.y /= bNormalized.y;
                percentage.z /= bNormalized.z;

                //How much force this distortion will have depending of its position
                multiplier = staticForceX.Evaluate(percentage.x) * staticForceY.Evaluate(percentage.y) * staticForceZ.Evaluate(percentage.z);
            }

            

            //Force the point to be inside the bounds using the ping pong algorithm
            if (isPingPong)
            {
                //Set the point using its axis, displacement and tile, and ping pong it between 0 and 1
                percentage.x = Math.PingPong((percentage.x + movementDisplacement) * tile.x, 0, 1);
                percentage.y = Math.PingPong((percentage.y + movementDisplacement) * tile.y, 0, 1);
                percentage.z = Math.PingPong((percentage.z + movementDisplacement) * tile.z, 0, 1);

            }
            else
            {
                //Set the point using its axis, displacement and tile
                percentage.x = (percentage.x + movementDisplacement) * tile.x;
                percentage.y = (percentage.y + movementDisplacement) * tile.y;
                percentage.z = (percentage.z + movementDisplacement) * tile.z;
            }
            

            //Type of the animation to do the calculations
            switch (type)
            {
                case MeshDistort.Distort.Type.Inflate:
                    //Get the direction of the point relative to the bound center
                    dir = (vertice - bounds.center).normalized;

                    //Apply the force to each axis
                    x = displacedForceX.Evaluate(percentage.x) * dir.x * force;
                    y = displacedForceY.Evaluate(percentage.y) * dir.y * force;
                    z = displacedForceZ.Evaluate(percentage.z) * dir.z * force;
                    break;
                case MeshDistort.Distort.Type.Random:
                    //Set the seed for the random value, this create a fluid effect on animation and don't generate different results on a static distortion.
                    Random.InitState((int)(percentage.magnitude * 100));

                    x += Random.Range(-force, force) * displacedForceXY.Evaluate(percentage.y);
                    x += Random.Range(-force, force) * displacedForceXZ.Evaluate(percentage.z);

                    y += Random.Range(-force, force) * displacedForceYX.Evaluate(percentage.x);
                    y += Random.Range(-force, force) * displacedForceYZ.Evaluate(percentage.z);

                    z += Random.Range(-force, force) * displacedForceZX.Evaluate(percentage.x);
                    z += Random.Range(-force, force) * displacedForceZY.Evaluate(percentage.y);
                    break;
                case MeshDistort.Distort.Type.Stretch:
                    //Set the force in a axis using another axis
                    x += displacedForceXY.Evaluate(percentage.y) * force;
                    x += displacedForceXZ.Evaluate(percentage.z) * force;

                    y += displacedForceYX.Evaluate(percentage.x) * force;
                    y += displacedForceYZ.Evaluate(percentage.z) * force;

                    z += displacedForceZX.Evaluate(percentage.x) * force;
                    z += displacedForceZY.Evaluate(percentage.y) * force;
                    break;
             
                case MeshDistort.Distort.Type.Spin:

                    //Calculate the spin force of this point
                    dir = Quaternion.Euler(new Vector3(
                    force * multiplier * displacedForceX.Evaluate(percentage.x) * 10,
                    force * multiplier * displacedForceY.Evaluate(percentage.y) * 10,
                    force * multiplier * displacedForceZ.Evaluate(percentage.z) * 10
                    )) * (vertice - bounds.center);



                    vertice.x = bounds.center.x + (dir.x);
                    vertice.y = bounds.center.y + (dir.y);
                    vertice.z = bounds.center.z + (dir.z);
                    return;

                case MeshDistort.Distort.Type.Melt:
                    float xForce = (displacedForceXZ.Evaluate(percentage.z) * force + displacedForceXY.Evaluate(percentage.y) * force) * 0.5f;
                    float yForce = (displacedForceYX.Evaluate(percentage.x) * force + displacedForceYZ.Evaluate(percentage.z) * force) * 0.5f;
                    float zForce = (displacedForceZX.Evaluate(percentage.x) * force + displacedForceZY.Evaluate(percentage.y) * force) * 0.5f;

                    if (xForce > 0)
                        vertice.x =  Mathf.Lerp(vertice.x,
                            bounds.max.x,
                            xForce);
                    else
                        vertice.x = Mathf.Lerp(vertice.x,
                        bounds.min.x,
                        -xForce);


                    if (yForce > 0)
                        vertice.y = Mathf.Lerp(vertice.y,
                            bounds.max.y,
                            yForce);
                    else
                        vertice.y = Mathf.Lerp(vertice.y,
                        bounds.min.y,
                        -yForce);



                    if (zForce > 0)
                        vertice.z = Mathf.Lerp(vertice.z,
                            bounds.max.z,
                            zForce);
                    else
                        vertice.z = Mathf.Lerp(vertice.z,
                        bounds.min.z,
                        -zForce);



                    break;
            }


            //Aplly the distortion to the vertice value
            vertice.x += x * multiplier;
            vertice.y += y * multiplier;
            vertice.z += z * multiplier;

         
        }

    }

}