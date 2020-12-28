using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.XRSDK.Oculus.Input;

#if OCULUSINTEGRATION_PRESENT
using static OVRSkeleton;
#endif

namespace Tsinghua.HCI.IoThingsLab
{
    public class GestureRecognizerItem : SensorItem
    {
        protected readonly Dictionary<TrackedHandJoint, MixedRealityPose> jointPoses = new Dictionary<TrackedHandJoint, MixedRealityPose>();

        [SerializeField] public GameObject _logger;
        
        private float EPS = 0.005f;

        [Header("播放音乐的")]
        public GameObject audio;
        [Header("播放视频的")]
        public GameObject video;

        /// <summary>
        /// 左手的手势
        /// </summary>
        public static string L_Data;
        /// <summary>
        /// 右手的手势
        /// </summary>
        public static string R_Data;
        /// <summary>
        /// 钩右手食指
        /// </summary>
        public static bool ishookIndexFinger_R;
        /// <summary>
        /// 钩左手食指
        /// </summary>
        public static bool ishookIndexFinger_L;
        // Start is called before the first frame update
        void Start()
        {
        }

        /// <summary>
        /// 右手正处于竖起大拇指的姿势
        /// </summary>
        protected bool IsInThumbsUpPose_R
        {
            get
            {
                Handedness handedness = Handedness.Right;
                //if (jointPoses.TryGetValue(TrackedHandJoint.Palm, out var palmPose)) return false;
                Camera mainCamera = CameraCache.Main;
                if (mainCamera == null)
                {
                    return false;
                }
                Transform cameraTransform = mainCamera.transform;
                MixedRealityPose palmPose = MixedRealityPose.ZeroIdentity;
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, handedness, out MixedRealityPose palmpose))
                {
                    palmPose = palmpose;
                }
                // We check if the palm up is roughly in line with the camera up
                return Vector3.Dot(palmPose.Up, cameraTransform.right) > 0.6f
                       && !HandPoseUtils.IsThumbGrabbing(handedness) && HandPoseUtils.IsMiddleGrabbing(handedness) && HandPoseUtils.IsIndexGrabbing(handedness);
            }
        }

        /// <summary>
        /// 左手正处于竖起大拇指的姿势
        /// </summary>
        protected bool IsInThumbsUpPose_L
        {
            get
            {
                Handedness handedness = Handedness.Left;
                //if (jointPoses.TryGetValue(TrackedHandJoint.Palm, out var palmPose)) return false;
                Camera mainCamera = CameraCache.Main;
                if (mainCamera == null)
                {
                    return false;
                }
                Transform cameraTransform = mainCamera.transform;
                MixedRealityPose palmPose = MixedRealityPose.ZeroIdentity;
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, handedness, out MixedRealityPose palmpose))
                {
                    palmPose = palmpose;
                }
                // We check if the palm up is roughly in line with the camera up
                return Vector3.Dot(palmPose.Up, cameraTransform.right) > 0.6f
                       && !HandPoseUtils.IsThumbGrabbing(handedness) && HandPoseUtils.IsMiddleGrabbing(handedness) && HandPoseUtils.IsIndexGrabbing(handedness);
            }
        }

        /// <summary>
        /// 左手正处于大拇指向右的姿势
        /// </summary>
        protected bool IsInThumbsRightPose_L
        {
            get
            {
                Handedness handedness = Handedness.Left;
                //if (jointPoses.TryGetValue(TrackedHandJoint.Palm, out var palmPose)) return false;
                Camera mainCamera = CameraCache.Main;
                if (mainCamera == null)
                {
                    return false;
                }
                Transform cameraTransform = mainCamera.transform;
                MixedRealityPose palmPose = MixedRealityPose.ZeroIdentity;
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, handedness, out MixedRealityPose palmpose))
                {
                    palmPose = palmpose;
                }
                // We check if the palm up is roughly in line with the camera up
                return Vector3.Dot(palmPose.Right, cameraTransform.right) > 0.6f
                       && !HandPoseUtils.IsThumbGrabbing(handedness) && HandPoseUtils.IsMiddleGrabbing(handedness) && HandPoseUtils.IsIndexGrabbing(handedness);
            }
        }

        /// <summary>
        /// 右手正处于大拇指向右的姿势
        /// </summary>
        protected bool IsInThumbsRightPose_R
        {
            get
            {
                Handedness handedness = Handedness.Right;
                //if (jointPoses.TryGetValue(TrackedHandJoint.Palm, out var palmPose)) return false;
                Camera mainCamera = CameraCache.Main;
                if (mainCamera == null)
                {
                    return false;
                }
                Transform cameraTransform = mainCamera.transform;
                MixedRealityPose palmPose = MixedRealityPose.ZeroIdentity;
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, handedness, out MixedRealityPose palmpose))
                {
                    palmPose = palmpose;
                }
                // We check if the palm up is roughly in line with the camera up
                return Vector3.Dot(palmPose.Right, cameraTransform.right) > 0.6f
                       && !HandPoseUtils.IsThumbGrabbing(handedness) && HandPoseUtils.IsMiddleGrabbing(handedness) && HandPoseUtils.IsIndexGrabbing(handedness);
            }
        }
        /// <summary>
        /// 左手正处于大拇指向左的姿势
        /// </summary>
        protected bool IsInThumbsLeftPose_L
        {
            get
            {
                Handedness handedness = Handedness.Left;
                //if (jointPoses.TryGetValue(TrackedHandJoint.Palm, out var palmPose)) return false;
                Camera mainCamera = CameraCache.Main;
                if (mainCamera == null)
                {
                    return false;
                }
                Transform cameraTransform = mainCamera.transform;
                MixedRealityPose palmPose = MixedRealityPose.ZeroIdentity;
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, handedness, out MixedRealityPose palmpose))
                {
                    palmPose = palmpose;
                }
                // We check if the palm up is roughly in line with the camera up
                return Vector3.Dot(-palmPose.Right, cameraTransform.right) > 0.6f
                       && !HandPoseUtils.IsThumbGrabbing(handedness) && HandPoseUtils.IsMiddleGrabbing(handedness) && HandPoseUtils.IsIndexGrabbing(handedness);
            }
        }
        /// <summary>
        /// 右手正处于大拇指向左的姿势
        /// </summary>
        protected bool IsInThumbsLeftPose_R
        {
            get
            {
                Handedness handedness = Handedness.Right;
                //if (jointPoses.TryGetValue(TrackedHandJoint.Palm, out var palmPose)) return false;
                Camera mainCamera = CameraCache.Main;
                if (mainCamera == null)
                {
                    return false;
                }
                Transform cameraTransform = mainCamera.transform;
                MixedRealityPose palmPose = MixedRealityPose.ZeroIdentity;
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, handedness, out MixedRealityPose palmpose))
                {
                    palmPose = palmpose;
                }
                // We check if the palm up is roughly in line with the camera up
                return Vector3.Dot(-palmPose.Right, cameraTransform.right) > 0.6f
                       && !HandPoseUtils.IsThumbGrabbing(handedness) && HandPoseUtils.IsMiddleGrabbing(handedness) && HandPoseUtils.IsIndexGrabbing(handedness);
            }
        }

        /// <summary>
        /// 右手Ok的姿势
        /// </summary>
        protected bool Is_OK_R
        {
            get
            {
                Handedness handedness = Handedness.Right;
                return HandPoseUtils.ThumbFingerCurl(handedness) > 0.3f && 
                       HandPoseUtils.IndexFingerCurl(handedness) > 0.3f &&
                       HandPoseUtils.MiddleFingerCurl(handedness) < 0.1f &&
                       HandPoseUtils.RingFingerCurl(handedness) < 0.1f &&
                       HandPoseUtils.PinkyFingerCurl(handedness) < 0.1f;
            }
        }


        /// <summary>
        /// 左手Ok的姿势
        /// </summary>
        protected bool Is_OK_L
        {
            get
            {
                Handedness handedness = Handedness.Left;
                return HandPoseUtils.ThumbFingerCurl(handedness) > 0.3f && 
                       HandPoseUtils.IndexFingerCurl(handedness) > 0.3f &&
                       HandPoseUtils.MiddleFingerCurl(handedness) < 0.1f &&
                       HandPoseUtils.RingFingerCurl(handedness) < 0.1f &&
                       HandPoseUtils.PinkyFingerCurl(handedness) < 0.1f;
            }          
        }


        /// <summary>
        /// 左手Ye的姿势
        /// </summary>
        protected bool Is_Ye_L
        {
            get
            {
                Handedness handedness = Handedness.Left;
                return HandPoseUtils.ThumbFingerCurl(handedness) > 0.4f && 
                       HandPoseUtils.IndexFingerCurl(handedness) < 0.3f && 
                       HandPoseUtils.MiddleFingerCurl(handedness) < 0.3f &&
                       HandPoseUtils.RingFingerCurl(handedness) > 0.7f &&
                       HandPoseUtils.PinkyFingerCurl(handedness) > 0.7f;
            }
        }

        /// <summary>
        /// 右手Ye的姿势
        /// </summary>
        protected bool Is_Ye_R
        {
            get
            {
                Handedness handedness = Handedness.Right;
                return HandPoseUtils.ThumbFingerCurl(handedness) > 0.4f && 
                       HandPoseUtils.IndexFingerCurl(handedness) < 0.3f && 
                       HandPoseUtils.MiddleFingerCurl(handedness) < 0.3f &&
                       HandPoseUtils.RingFingerCurl(handedness) > 0.7f &&
                       HandPoseUtils.PinkyFingerCurl(handedness) > 0.7f;
            }
        }

        /// <summary>
        /// 右手食指卷曲度数大于 0.5f，处于弯曲状态
        /// </summary>
        protected bool IsIndexFingerCurl_Bend_R
        {
            get
            {
                Handedness handedness = Handedness.Right;
                return HandPoseUtils.ThumbFingerCurl(handedness) > 0.5f && 
                       HandPoseUtils.IndexFingerCurl(handedness) > 0.5f && 
                       HandPoseUtils.MiddleFingerCurl(handedness) > 0.7f &&
                       HandPoseUtils.RingFingerCurl(handedness) > 0.7f &&
                       HandPoseUtils.PinkyFingerCurl(handedness) > 0.7f;
            }
        }
        /// <summary>
        /// 右手食指卷曲度数小于 0.2f，处于伸开状态
        /// </summary>
        protected bool IsIndexFingerCurl_Stretch_R
        {
            get
            {
                Handedness handedness = Handedness.Right;
                return HandPoseUtils.ThumbFingerCurl(handedness) > 0.5f && 
                       HandPoseUtils.IndexFingerCurl(handedness) < 0.2f && 
                       HandPoseUtils.MiddleFingerCurl(handedness) > 0.7f &&
                       HandPoseUtils.RingFingerCurl(handedness) > 0.7f &&
                       HandPoseUtils.PinkyFingerCurl(handedness) > 0.7f;
            }
        }

        /// <summary>
        /// 左手食指卷曲度数大于 0.5f，处于弯曲状态
        /// </summary>
        protected bool IsIndexFingerCurl_Bend_L
        {
            get
            {
                Handedness handedness = Handedness.Left;
                return HandPoseUtils.ThumbFingerCurl(handedness) > 0.5f && 
                       HandPoseUtils.IndexFingerCurl(handedness) > 0.5f && 
                       HandPoseUtils.MiddleFingerCurl(handedness) > 0.7f &&
                       HandPoseUtils.RingFingerCurl(handedness) > 0.7f &&
                       HandPoseUtils.PinkyFingerCurl(handedness) > 0.7f;
            }
        }
        /// <summary>
        /// 左手食指卷曲度数小于 0.2f，处于伸开状态
        /// </summary>
        protected bool IsIndexFingerCurl_Stretch_L
        {
            get
            {
                Handedness handedness = Handedness.Left;
                return HandPoseUtils.ThumbFingerCurl(handedness) > 0.5f && 
                       HandPoseUtils.IndexFingerCurl(handedness) < 0.2f && 
                       HandPoseUtils.MiddleFingerCurl(handedness) > 0.7f &&
                       HandPoseUtils.RingFingerCurl(handedness) > 0.7f &&
                       HandPoseUtils.PinkyFingerCurl(handedness) > 0.7f;

            }
        }

        /// <summary>
        /// 钩右手食指
        /// </summary>
        protected bool HookIndexFinger_R
        {
            get
            {
                if (IsIndexFingerCurl_Stretch_R)
                {
                    ishookIndexFinger_R = false;
                }
                if (IsIndexFingerCurl_Bend_R && !ishookIndexFinger_R)
                {
                    ishookIndexFinger_R = true;
                }
                return ishookIndexFinger_R;
            }
        }

        /// <summary>
        /// 钩左手食指
        /// </summary>
        protected bool HookIndexFinger_L
        {
            get
            {
                if (IsIndexFingerCurl_Stretch_L)
                {
                    ishookIndexFinger_L = false;
                }
                if (IsIndexFingerCurl_Bend_L && !ishookIndexFinger_L)
                {
                    ishookIndexFinger_L = true;
                }
                return ishookIndexFinger_L;
            }
        }

        public void Update()
        {

            switch (Get_SeeObject.APPLIANCES)
            {
                case APPLIANCES.TV:
                    if(IsInThumbsLeftPose_L||Input.GetKey(KeyCode.Z))
                    {
                        Cut_Video.Open_Video(video);
                    }
                    else
                    {
                        Cut_Video.Stop_Video();
                    }
                break;
                case APPLIANCES.Light:
                    if (IsInThumbsUpPose_R||Input.GetKey(KeyCode.X))
                    {
                        SensorTrigger();
                    }
                    else
                    {
                        SensorUntrigger();
                    }
                break;
                case APPLIANCES.Sound:
                    if(Is_Ye_L || Is_Ye_R || Input.GetKey(KeyCode.C))
                    {
                        Cut_Music.Open_Music(audio);
                    }
                    else
                    {
                        Cut_Music.Stop_Music();
                    }
                break;
            }
            //Get_FingerPose();
        }

        protected void Get_FingerPose()
        {
            if (IsInThumbsUpPose_R)
            {
                R_Data = "右手正处于竖起大拇指的姿势";
            }
            if (IsInThumbsUpPose_L)
            {
                L_Data = "左手正处于竖起大拇指的姿势";
            }
            if (IsInThumbsRightPose_R)
            {
                R_Data = "右手正处于大拇指向右的姿势";
            }
            if (IsInThumbsRightPose_L)
            {
                L_Data = "左手正处于大拇指向右的姿势";
            }
            if (IsInThumbsLeftPose_R)
            {
                R_Data = "右手正处于大拇指向左的姿势";
            }
            if (IsInThumbsLeftPose_L)
            {
                L_Data = "左手正处于大拇指向左的姿势";
            }
           
            if (Is_OK_L)
            {
                L_Data = "左手Ok的姿势";
            }
            if (Is_OK_R)
            {
                R_Data = "右手Ok的姿势";
            }
            if (Is_Ye_L)
            {
                L_Data = "左手Ye的姿势";
            }
            if (Is_Ye_R)
            {
                R_Data = "右手Ye的姿势";
            }
            if (IsIndexFingerCurl_Bend_L)
            {
                L_Data = "左手食指卷曲度数大于 0.5f，处于弯曲状态";
            }
            if (IsIndexFingerCurl_Bend_R)
            {
                R_Data = "右手食指卷曲度数大于 0.5f，处于弯曲状态";
            }
            if (IsIndexFingerCurl_Stretch_L)
            {
                L_Data = "左手食指卷曲度数小于 0.2f，处于伸开状态";
            }
            if (IsIndexFingerCurl_Stretch_R)
            {
                R_Data = "右手食指卷曲度数小于 0.2f，处于伸开状态";
            }
        }

    }
}