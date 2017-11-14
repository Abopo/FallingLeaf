using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contain the scripts for the Easy Distortion Package
/// </summary>

namespace MeshDistort {


    /// <summary>
    /// Animate distortions and save animations for it.
    /// </summary>
    [RequireComponent(typeof(Distort))]
    public class AnimatedDistort : MonoBehaviour {

        /// <summary>
        /// Used to configure a new animation to be saved
        /// </summary>
        public float animationFramesPerSec = 30;

        /// <summary>
        /// Number of frames to save in a animation
        /// </summary>
        public int animationFrames = 1;

        /// <summary>
        /// Reference to the DistortVertices on this GameObject
        /// </summary>
        protected Distort distort;

        /// <summary>
        /// Types of animations it can do
        /// </summary>
        public enum Animate
        {
            force,
            displacement
        }

        /// <summary>
        /// Animation options
        /// </summary>
        public enum Type
        {
            constant,
            pingpong,
            sin,
            curve
        }

        /// <summary>
        /// Current animation type
        /// </summary>
        public Type type = Type.constant;
        public Animate animate = Animate.displacement;
        public AnimationCurve curveType;

        /// <summary>
        /// Speed of the animation of constant type
        /// </summary>
        public float constantSpeed = 1;

        /// <summary>
        /// Min value for not const animation
        /// </summary>
        public float minValue = 0;
        /// <summary>
        /// Max value for not const animation
        /// </summary>
        public float maxValue = 10;

        
        /// <summary>
        /// Index of the animation that is playing. 0 is a special value to calculate animation in real time
        /// </summary>
        public int playAnimationIndex = 0;

        /// <summary>
        /// Animation index, but remove the special index of 0 and fix the index.
        /// </summary>
        public int currentAnimation
        {
            get
            {
                return playAnimationIndex - 1;
            }
            set
            {
                playAnimationIndex = value + 1;
            }
        }

        /// <summary>
        /// List of saved animations
        /// </summary>
        public List<DistortAnimation> animationList;

        /// <summary>
        /// Is the script changing one animation to another?
        /// </summary>
        public bool updatingAnimation = false;

        private Coroutine changeAnimationCoroutine;

        public bool isPlaying = true;

        void Start() {
            Setup();
        }

        public void Play()
        {
            isPlaying = true;
            playingAnimationTime = 0;
        }
        public void Stop()
        {
            isPlaying = false;
            playingAnimationTime = 0;
        }

        float playingAnimationTime = 0;
        void LateUpdate()
        {
            if (isPlaying)
            {
                //Check if it is currently doing some animation of its own
                if (!updatingAnimation)
                {
                    //If is marked to calculate dinamic
                    if (playAnimationIndex == 0)
                    {
                        //Do animation in real time
                        Animation(playingAnimationTime, Time.deltaTime);
                        playingAnimationTime += Time.deltaTime;
                    }
                    //Else play an recorded animation
                    else
                    {
                        PlayAnimationFrame(currentAnimation);
                    }
                }
            }
        }

        /// <summary>
        /// Do the script setup
        /// </summary>
        void Setup()
        {
            ///Get the DistortVertices of this gameobject
            distort = GetComponent<Distort>();
            ///Make the mesh dynamic, since we will be updating the vertices every frame
            distort.MakeDynamic();
        }

        /// <summary>
        /// Calculate the distortion in real time
        /// </summary>
        public void CalculateInRealTime()
        {
            playAnimationIndex = 0;
        }


        /// <summary>
        /// Calculate a animation in a determined frame
        /// </summary>
        /// <param name="displaceOffset">Time in animation</param>
        /// <param name="delta">Delta time since last animation update</param>
        void Animation(float displaceOffset, float delta) {
            //Call setup if needed
            if (distort == null)
                Setup();

            ///For each distortion in the GameObject update the movementDisplacement, this will make the object animate
            for (int i = 0; i < distort.distort.Count; i++)
            {
                //Animation type constant
                if (type == Type.constant)
                {
                    if (animate == Animate.displacement)
                    {
                        float disp = constantSpeed * delta * distort.distort[i].animationSpeed;
                        distort.distort[i].movementDisplacement += disp;
                    }
                    else
                    {
                        distort.distort[i].force += constantSpeed * delta * distort.distort[i].animationSpeed;
                    }
                }
                //Animation type pingpong
                else if (type == Type.pingpong)
                {
                    if (animate == Animate.displacement)
                    {
                        float disp = Mathf.Lerp(minValue, maxValue, Mathf.PingPong(displaceOffset * constantSpeed * distort.distort[i].animationSpeed, 1));
                        distort.distort[i].movementDisplacement = disp;
                    }
                    else
                    {
                        distort.distort[i].force = Mathf.Lerp(minValue, maxValue, Mathf.PingPong(displaceOffset * constantSpeed * distort.distort[i].animationSpeed, 1));
                    }
                }
                //Animation type sin
                else if (type == Type.sin)
                {
                    if (animate == Animate.displacement)
                    {
                        float disp = Mathf.Lerp(minValue, maxValue, (Mathf.Sin(displaceOffset * constantSpeed * distort.distort[i].animationSpeed) + 1) * 0.5f);
                        distort.distort[i].movementDisplacement = disp;
                    }
                    else
                    {
                        distort.distort[i].force = Mathf.Lerp(minValue, maxValue, (Mathf.Sin(displaceOffset * constantSpeed * distort.distort[i].animationSpeed) + 1) * 0.5f);
                    }
                }
                else if (type == Type.curve)
                {
                    if (animate == Animate.displacement)
                    {
                        float disp = curveType.Evaluate(displaceOffset * constantSpeed * distort.distort[i].animationSpeed);
                        distort.distort[i].movementDisplacement = disp;
                    }
                    else
                    {
                        distort.distort[i].force = curveType.Evaluate(displaceOffset * constantSpeed * distort.distort[i].animationSpeed);
                    }
                }

                if (animate == Animate.force && distort.calculateInGPU && Application.isPlaying && SystemInfo.supportsComputeShaders)
                {
                    distort.distort[i].UpdateObjectDataBuffer();
                }
            }
            //Update the mesh vertices position.
            distort.UpdateDistort();
        }

        /// <summary>
        /// Play a recorded animation
        /// </summary>
        /// <param name="index">Animation index (Starting at 0)</param>
        public void PlayAnimationFrame(int index)
        {
            //Set the current animation
            //currentAnimation = index;

            //Get animation
            DistortAnimation anim = animationList[index];
            //Get frame animation to play in the current frame
            int frame = Mathf.FloorToInt(Mathf.Repeat(Time.timeSinceLevelLoad * animationFramesPerSec, anim.frames));

            //Get the animation for each mesh in this frame.
            FrameAnimation[] animations = anim.frameData[frame].data;

            //Update the vertices to the current animation.
            for (int i = 0; i < animations.Length; i++) {

                animations[i].mesh.vertices = animations[i].vertices;
                animations[i].mesh.RecalculateNormals();
            }


        }

        /// <summary>
        /// Save a new animation
        /// </summary>
        public void SaveAnimation()
        {
            if (animationList == null)
                animationList = new List<DistortAnimation>();
            if (distort == null)
                Setup();

            DistortAnimation anim = new DistortAnimation();

            //Set the animation default name
            anim.animName = "Animation" + animationList.Count;
            //Framesize of animation
            anim.frameData = new FrameCollection[animationFrames];
            //Total frames
            anim.frames = animationFrames;
            // Frames per sec
            anim.framesPerSec = animationFramesPerSec;


            float[] displaceStart = new float[distort.distort.Count];
            float[] forceStart = new float[distort.distort.Count];

            
            //Save th values before the animation
            for (int i = 0; i < distort.distort.Count; i++)
            {
                displaceStart[i] = distort.distort[i].movementDisplacement;
                forceStart[i] = distort.distort[i].force;
            }

            //Create the animation
            for (int i = 0; i < animationFrames; i++)
            {
                //Generate the animation on this frame
                Animation(i, 1 / animationFramesPerSec);
                anim.frameData[i] = new FrameCollection();
                anim.frameData[i].data = new FrameAnimation[distort.meshList.Count];

                //Set the animation data
                for (int m = 0; m < distort.meshList.Count; m++) {
                    anim.frameData[i].data[m] = ScriptableObject.CreateInstance<FrameAnimation>();
                    anim.frameData[i].data[m].transform = distort.meshList[m].meshTransform;
                    anim.frameData[i].data[m].vertices = distort.meshList[m].mesh.vertices;

                }
            }

            //Set the values before the animation
            for (int i = 0; i < distort.distort.Count; i++)
            {
                distort.distort[i].movementDisplacement = displaceStart[i];
                distort.distort[i].force = forceStart[i];
            }

            //Add new animation to list
            animationList.Add(anim);
        }

        /// <summary>
        /// Delete a animation
        /// </summary>
        /// <param name="index">Index of animation to be deleted</param>
        public void DeleteAnimation(int index)
        {
            animationList.RemoveAt(index);
        }

        /// <summary>
        /// Set a animation to play
        /// </summary>
        /// <param name="animationIndex">Animation index</param>
        public void SetAnimation(int animationIndex)
        {
            AnimationChanged();
            currentAnimation = animationIndex;
        }

        /// <summary>
        /// Change an animation to another
        /// </summary>
        /// <param name="indexTo">New index to play</param>
        public void ChangeAnimation(int indexTo)
        {
            ChangeAnimation(currentAnimation, indexTo);
        }

        /// <summary>
        /// Change an animation to another
        /// </summary>
        /// <param name="indexTo">Animation to start</param>
        /// <param name="time">Animation to end</param>
        public void ChangeAnimation(int indexTo, float time)
        {
            ChangeAnimation(currentAnimation, indexTo, time);
        }

        /// <summary>
        /// hange an animation to another
        /// </summary>
        /// <param name="indexFrom">Animation to start</param>
        /// <param name="indexTo">Animation to end</param>
        /// <param name="time">Transition time</param>
        public void ChangeAnimation(int indexFrom, int indexTo, float time = 3)
        {
            //If the old animation is dynamic, it can't merge the animations
            if (indexFrom < 0)
            {
                Debug.LogError("Is not possible to merde dynamic animation to other animation");
                return;
            }
            if (indexFrom == indexTo)
            {
                //No point in setting the same animation to the same
                return;
            }

            //Stop any animation that may be running
            AnimationChanged();
            //Start new animation
            changeAnimationCoroutine = StartCoroutine(ChangeAnimationAsync(indexFrom, indexTo, time));
        }

        /// <summary>
        /// Change an animation to another async
        /// </summary>
        /// <param name="indexFrom">Animation index to start</param>
        /// <param name="indexTo">Animation index to end</param>
        /// <param name="time">Time between the two</param>
        /// <returns></returns>
        private IEnumerator ChangeAnimationAsync(int indexFrom, int indexTo, float time)
        {
            //Set that the animation is updating
            updatingAnimation = true;

            currentAnimation = indexTo;

            float currTime = 0;
            while (currTime <= time) {
                float force = currTime / time;

                MergeAnimation(indexFrom, indexTo, force);
                currTime += Time.deltaTime;
                yield return null;
            }
            //Updating animation finished
            updatingAnimation = false;

        }

        /// <summary>
        /// Called once the script start to run a new animation
        /// </summary>
        private void AnimationChanged()
        {
            if (changeAnimationCoroutine != null)
                StopCoroutine(changeAnimationCoroutine);
            updatingAnimation = false;

        }

        /// <summary>
        /// Merge an animation to another using a percentage
        /// </summary>
        /// <param name="indexFrom">First animation</param>
        /// <param name="indexTo">Second animation</param>
        /// <param name="force">How much force each animation have</param>
        public void MergeAnimation(int indexFrom, int indexTo, float force)
        {
            DistortAnimation from = animationList[indexFrom];
            DistortAnimation to = animationList[indexTo];

            int frameFrom = Mathf.FloorToInt(Mathf.Repeat(Time.timeSinceLevelLoad * animationFramesPerSec, from.frames));
            int frameTo = Mathf.FloorToInt(Mathf.Repeat(Time.timeSinceLevelLoad * animationFramesPerSec, to.frames));

            FrameAnimation[] animFrom = from.frameData[frameFrom].data;
            FrameAnimation[] animTo = to.frameData[frameTo].data;

            for (int x = 0; x < animFrom.Length; x++)
            {
                for (int y = 0; y < animTo.Length; y++)
                {
                    if (animFrom[x].transform == animTo[y].transform)
                    {
                        Vector3[] mergedVertices = new Vector3[animFrom[x].vertices.Length];
                        for (int v = 0; v < animFrom[x].vertices.Length; v++)
                        {
                            mergedVertices[v] = Vector3.Lerp(animFrom[x].vertices[v], animTo[y].vertices[v], force);
                        }

                        animFrom[x].mesh.vertices = mergedVertices;
                        animFrom[x].mesh.RecalculateNormals();
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Hold animation data to b used in AnimatedDistort
    /// </summary>
    [System.Serializable]
    public class DistortAnimation
    {
        /// <summary>
        /// Name of the animation
        /// </summary>
        public string animName;
        /// <summary>
        /// Total frames in animation
        /// </summary>
        public int frames;
        /// <summary>
        /// Frames per sec.
        /// </summary>
        public float framesPerSec;

        /// <summary>
        /// Animation data for each frame
        /// </summary>
        public FrameCollection[] frameData;
    }

    /// <summary>
    /// Hold animation for each mesh in a frame
    /// </summary>
    [System.Serializable]
    public class FrameCollection
    {
        /// <summary>
        /// Collection of meshes and the vertice values to be used in a frame
        /// </summary>
        public FrameAnimation[] data;
    }

}


