using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Represents a face detected by an AR device.
    /// </summary>
    /// <remarks>
    /// Generated by the <see cref="ARFaceManager"/> when an AR device detects
    /// a face in the environment.
    /// </remarks>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(ARUpdateOrder.k_Face)]
    [HelpURL(HelpUrls.ApiWithNamespace + nameof(ARFace) + ".html")]
    public sealed class ARFace : ARTrackable<XRFace, ARFace>
    {
        /// <summary>
        /// Invoked when the face is updated. If face meshes are supported, there will be
        /// updated <see cref="vertices"/>, <see cref="normals"/>, <see cref="indices"/>, and
        /// <see cref="uvs"/>.
        /// </summary>
        public event Action<ARFaceUpdatedEventArgs> updated;

        /// <summary>
        /// The vertices representing the face mesh. Check for existence with <c>vertices.IsCreated</c>.
        /// This array is parallel to <see cref="normals"/> and <see cref="uvs"/>. Vertices are
        /// provided in face space, that is, relative to this <see cref="ARFace"/>'s local
        /// position and rotation.
        /// </summary>
        public NativeArray<Vector3> vertices => GetUndisposable(m_FaceMesh.vertices);

        /// <summary>
        /// The normals representing the face mesh. Check for existence with <c>normals.IsCreated</c>.
        /// This array is parallel to <see cref="vertices"/> and <see cref="uvs"/>.
        /// </summary>
        public unsafe NativeArray<Vector3> normals => GetUndisposable(m_FaceMesh.normals);

        /// <summary>
        /// The indices defining the triangles of the face mesh. Check for existence with <c>indices.IsCreated</c>.
        /// The are three times as many indices as triangles, so this will always be a multiple of 3.
        /// </summary>
        public NativeArray<int> indices => GetUndisposable(m_FaceMesh.indices);

        /// <summary>
        /// The texture coordinates representing the face mesh. Check for existence with <c>uvs.IsCreated</c>.
        /// This array is parallel to <see cref="vertices"/> and <see cref="normals"/>.
        /// </summary>
        public NativeArray<Vector2> uvs => GetUndisposable(m_FaceMesh.uvs);

        /// <summary>
        /// Get a native pointer associated with this face.
        /// </summary>
        /// <remarks>
        /// The data pointed to by this member is implementation defined.
        /// The lifetime of the pointed to object is also
        /// implementation defined, but should be valid at least until the next
        /// <see cref="ARSession"/> update.
        /// </remarks>
        public IntPtr nativePtr => sessionRelativeData.nativePtr;

        /// <summary>
        /// The [transform](https://docs.unity3d.com/ScriptReference/Transform.html) of the left eye of the face, or `null` if there is no data for the left eye.
        /// </summary>
        public Transform leftEye { get; private set; }

        /// <summary>
        /// The [transform](https://docs.unity3d.com/ScriptReference/Transform.html) of the right eye of the face, or `null` if there is no data for the right eye.
        /// </summary>
        public Transform rightEye { get; private set; }

        /// <summary>
        /// The [transform](https://docs.unity3d.com/ScriptReference/Transform.html) representing the point where the eyes are fixated or `null` if there is no fixation data.
        /// </summary>
        public Transform fixationPoint { get; private set; }

        void Update()
        {
            if (m_Updated && updated != null)
            {
                updated(new ARFaceUpdatedEventArgs(this));
                m_Updated = false;
            }
        }

        void OnDestroy()
        {
            m_FaceMesh.Dispose();
        }

        // Creates an alias to the same array, but the caller cannot Dispose it.
        internal static unsafe NativeArray<T> GetUndisposable<T>(NativeArray<T> disposable) where T : struct
        {
            if (!disposable.IsCreated)
                return default;

            var undisposable = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(
                disposable.GetUnsafePtr(),
                disposable.Length,
                Allocator.None);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(
                ref undisposable,
                NativeArrayUnsafeUtility.GetAtomicSafetyHandle(disposable));
#endif

            return undisposable;
        }

        internal void UpdateMesh(XRFaceSubsystem subsystem)
        {
            subsystem.GetFaceMesh(sessionRelativeData.trackableId, Allocator.Persistent, ref m_FaceMesh);
            m_Updated = true;
        }

        internal void UpdateEyes()
        {
            Transform CreateGameObject(string nameOfNewGameObject)
            {
                var newTransform = new GameObject(nameOfNewGameObject).transform;
                newTransform.SetParent(transform, worldPositionStays: false);
                return newTransform;
            }

            if (leftEye == null && rightEye == null && fixationPoint == null)
            {
                leftEye = CreateGameObject("Left eye");
                rightEye = CreateGameObject("Right eye");
                fixationPoint = CreateGameObject("Fixation point");
            }

            UpdateTransformFromPose(leftEye, sessionRelativeData.leftEyePose);
            UpdateTransformFromPose(rightEye, sessionRelativeData.rightEyePose);
            fixationPoint.localPosition = sessionRelativeData.fixationPoint;
        }

        static void UpdateTransformFromPose(Transform eyeTransform, Pose eyePose)
        {
            eyeTransform.localPosition = eyePose.position;
            eyeTransform.localRotation = eyePose.rotation;
        }

        XRFaceMesh m_FaceMesh;

        bool m_Updated;
    }
}
