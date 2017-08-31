using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UITween
{
    public class CurrentAnimation
    {
        private AnimationParts animationPart;

        private float counterTween = 2f;

        private enum States
        {
            AWAKE,
            READY,
            START,
            ENDED,
            FINALENDED,
            COUNT}

        ;

        private States AnimationStates = States.AWAKE;

        public CurrentAnimation(AnimationParts animationPart)
        {
            this.animationPart = animationPart;
        }

        public void AnimationFrame(RectTransform rectTransform)
        {
            if (AnimationStates == States.AWAKE)
                return;

            if (counterTween <= 1f) // Max value is 1f
            {
                SetAnimationOnFrame(rectTransform, counterTween);
            }
        }

        public void SetAnimationOnFrame(RectTransform rectTransform, float percentage)
        {
            // Position Animation
            if (animationPart.PositionPropetiesAnim.IsPositionEnabled())
            {
                MoveAnimation(rectTransform, percentage);
            }
			
            // Rotation Animation
            if (animationPart.RotationPropetiesAnim.IsRotationEnabled())
            {
                RotateAnimation(rectTransform, percentage);
            }
			
            // Scale Animation
            if (animationPart.ScalePropetiesAnim.IsScaleEnabled())
            {
                ScaleAnimation(rectTransform, percentage);
            }
			
            // Fade Animation
            if (animationPart.FadePropetiesAnim.IsFadeEnabled())
            {
                SetAlphaValue(rectTransform.transform, percentage);
            }
        }

        public void LateAnimationFrame(RectTransform rectTransform)
        {
            if (AnimationStates == States.AWAKE)
                return;

            if (counterTween <= 1f) // Max value is 1f
            {
                float deltaTime = (animationPart.UnscaledTimeAnimation) ? Time.unscaledDeltaTime : Time.deltaTime;

                counterTween += deltaTime / animationPart.GetAnimationDuration();
            }
            else
            {
                if (AnimationStates != States.FINALENDED)
                {
                    SetAnimationOnFrame(rectTransform, 1f);

                    if (AnimationStates != States.ENDED
                        && AnimationStates != States.FINALENDED
                        && animationPart.AtomicAnimation)
                    {
                        animationPart.Ended();
                        AnimationStates = States.ENDED;
                    }
                    else
                    {
                        animationPart.FinalEnd();
                        AnimationStates = States.FINALENDED;
                    }
                }
            }

            if (counterTween > .9f && !animationPart.AtomicAnimation)
            {
                if (AnimationStates != States.ENDED
                    && AnimationStates != States.FINALENDED)
                {
                    animationPart.Ended();
                    AnimationStates = States.ENDED;
                }
            }

            animationPart.FrameCheck();
        }

        public void PlayOpenAnimations()
        {
            // Open Pos Anim
            if (animationPart.PositionPropetiesAnim.IsPositionEnabled())
            {
                SetCurrentAnimPos(animationPart.PositionPropetiesAnim.TweenCurveEnterPos,
                    animationPart.PositionPropetiesAnim.StartPos,
                    animationPart.PositionPropetiesAnim.EndPos);
            }

            // Open Rot Anim
            if (animationPart.RotationPropetiesAnim.IsRotationEnabled())
            {
                SetCurrentAnimRot(animationPart.RotationPropetiesAnim.TweenCurveEnterRot,
                    animationPart.RotationPropetiesAnim.StartRot,
                    animationPart.RotationPropetiesAnim.EndRot);
            }

            // Open scale Anim
            if (animationPart.ScalePropetiesAnim.IsScaleEnabled())
            {
                SetCurrentAnimScale(animationPart.ScalePropetiesAnim.TweenCurveEnterScale,
                    animationPart.ScalePropetiesAnim.StartScale,
                    animationPart.ScalePropetiesAnim.EndScale);
            }

            // Open Fade Anim
            if (animationPart.FadePropetiesAnim.IsFadeEnabled())
            {
                SetFadeAnimation(animationPart.FadePropetiesAnim.GetStartFadeValue(), 
                    animationPart.FadePropetiesAnim.GetEndFadeValue());
            }

            counterTween = 0f;

            AnimationStates = States.READY;

            animationPart.ChangeStatus();
            animationPart.CheckCallbackStatus();
        }

        public void SetStatus(bool status)
        {
            animationPart.SetStatus(status);
        }

        public void PlayCloseAnimations()
        {
            // Close Pos Anim
            if (animationPart.PositionPropetiesAnim.IsPositionEnabled())
            {
                SetCurrentAnimPos(animationPart.PositionPropetiesAnim.TweenCurveExitPos,
                    animationPart.PositionPropetiesAnim.EndPos,
                    animationPart.PositionPropetiesAnim.StartPos);
            }

            // Close Rot Anim
            if (animationPart.RotationPropetiesAnim.IsRotationEnabled())
            {
                SetCurrentAnimRot(animationPart.RotationPropetiesAnim.TweenCurveExitRot,
                    animationPart.RotationPropetiesAnim.EndRot,
                    animationPart.RotationPropetiesAnim.StartRot);
            }

            // Close Scale Anim
            if (animationPart.ScalePropetiesAnim.IsScaleEnabled())
            {
                SetCurrentAnimScale(animationPart.ScalePropetiesAnim.TweenCurveExitScale,
                    animationPart.ScalePropetiesAnim.EndScale,
                    animationPart.ScalePropetiesAnim.StartScale);
            }

            // Close Fade Anim
            if (animationPart.FadePropetiesAnim.IsFadeEnabled())
            {
                SetFadeAnimation(animationPart.FadePropetiesAnim.GetEndFadeValue(), 
                    animationPart.FadePropetiesAnim.GetStartFadeValue());
            }

            counterTween = 0f;

            AnimationStates = States.READY;

            animationPart.ChangeStatus();
            animationPart.CheckCallbackStatus();
        }

        public void SetAnimationPos(Vector2 StartAnchoredPos, Vector2 EndAnchoredPos, AnimationCurve EntryTween, AnimationCurve ExitTween, RectTransform rectTransform)
        {
            animationPart.PositionPropetiesAnim.SetPositionEnable(true);
            animationPart.PositionPropetiesAnim.SetPosStart(StartAnchoredPos, rectTransform);
            animationPart.PositionPropetiesAnim.SetPosEnd(EndAnchoredPos, rectTransform.transform);
            animationPart.PositionPropetiesAnim.SetAniamtionsCurve(EntryTween, ExitTween);
        }

        public void SetAnimationScale(Vector2 StartAnchoredScale, Vector2 EndAnchoredScale, AnimationCurve EntryTween, AnimationCurve ExitTween)
        {
            animationPart.ScalePropetiesAnim.StartScale = StartAnchoredScale;
            animationPart.ScalePropetiesAnim.SetScaleEnable(true);
            animationPart.ScalePropetiesAnim.EndScale = EndAnchoredScale;
            animationPart.ScalePropetiesAnim.SetAniamtionsCurve(EntryTween, ExitTween);
        }

        public void SetAnimationRotation(Vector2 StartAnchoredEulerAng, Vector2 EndAnchoredEulerAng, AnimationCurve EntryTween, AnimationCurve ExitTween)
        {
            animationPart.RotationPropetiesAnim.SetRotationEnable(true);
            animationPart.RotationPropetiesAnim.StartRot = StartAnchoredEulerAng;
            animationPart.RotationPropetiesAnim.EndRot = EndAnchoredEulerAng;
            animationPart.RotationPropetiesAnim.SetAniamtionsCurve(EntryTween, ExitTween);
        }

        public void SetFade(bool OverrideFade)
        {
            animationPart.FadePropetiesAnim.SetFadeEnable(true);
            animationPart.FadePropetiesAnim.SetFadeOverride(OverrideFade);
        }

        public void SetFadeValuesStartEnd(float startAlphaValue, float endAlphaValue)
        {
            animationPart.FadePropetiesAnim.SetFadeValues(startAlphaValue, endAlphaValue);
        }

        public bool IsObjectOpened()
        {
            return animationPart.IsObjectOpened();
        }

        public void SetAniamtioDuration(float duration)
        {
            animationPart.SetAniamtioDuration(duration);
        }

        public float GetAnimationDuration()
        {
            return animationPart.GetAnimationDuration();
        }

        #region PositionAnim

        private AnimationCurve currentAnimationCurvePos;
        private Vector3 currentStartPos;
        private Vector3 currentEndPos;

        public void SetCurrentAnimPos(AnimationCurve currentAnimationCurvePos, Vector3 currentStartPos, Vector3 currentEndPos)
        {
            this.currentAnimationCurvePos = currentAnimationCurvePos;
            this.currentStartPos = currentStartPos;
            this.currentEndPos = currentEndPos;
        }

        public void MoveAnimation(RectTransform _rectTransform, float _counterTween)
        {
            float evaluatedValue = currentAnimationCurvePos.Evaluate(_counterTween);
            Vector3 valueAdded = (currentEndPos - currentStartPos) * evaluatedValue;

            _rectTransform.anchoredPosition = (Vector2)(currentStartPos + valueAdded);
        }

        #endregion

        #region ScaleAnim

        private AnimationCurve currentAnimationCurveScale;
        private Vector3 currentStartScale;
        private Vector3 currentEndScale;

        public void SetCurrentAnimScale(AnimationCurve currentAnimationCurveScale, Vector3 currentStartScale, Vector3 currentEndScale)
        {
            this.currentAnimationCurveScale = currentAnimationCurveScale;
            this.currentStartScale = currentStartScale;
            this.currentEndScale = currentEndScale;
        }

        public void ScaleAnimation(RectTransform _rectTransform, float _counterTween)
        {
            float evaluatedValue = currentAnimationCurveScale.Evaluate(_counterTween);
            Vector3 valueAdded = (currentEndScale - currentStartScale) * evaluatedValue;

            _rectTransform.localScale = currentStartScale + valueAdded;
        }

        #endregion

        #region RotationAnim

        private AnimationCurve currentAnimationCurveRot;
        private Vector3 currentStartRot;
        private Vector3 currentEndRot;

        public void SetCurrentAnimRot(AnimationCurve currentAnimationCurveRot, Vector3 currentStartRot, Vector3 currentEndRot)
        {
            this.currentAnimationCurveRot = currentAnimationCurveRot;
            this.currentStartRot = currentStartRot;
            this.currentEndRot = currentEndRot;
        }

        public void RotateAnimation(RectTransform _rectTransform, float _counterTween)
        {
            float evaluatedValue = currentAnimationCurveRot.Evaluate(_counterTween);
            Vector3 valueAdded = (currentEndRot - currentStartRot) * evaluatedValue;

            _rectTransform.localEulerAngles = currentStartRot + valueAdded;
        }

        #endregion

        #region FadeAnim

        private float startAlphaValue;
        private float endAlphaValue;

        public void SetFadeAnimation(float startAlphaValue, float endAlphaValue)
        {
            this.startAlphaValue = startAlphaValue;
            this.endAlphaValue = endAlphaValue;
        }

        public void SetAlphaValue(Transform _objectToSetAlpha, float _counterTween)
        {
            if (_objectToSetAlpha.GetComponent<MaskableGraphic>())
            {
                MaskableGraphic GraphicElement = _objectToSetAlpha.GetComponent<MaskableGraphic>();

                Color objectColor = GraphicElement.color;

                _counterTween = Mathf.Clamp(_counterTween, 0f, 1f);

                objectColor.a = Mathf.Abs(startAlphaValue + (endAlphaValue - startAlphaValue) * _counterTween);
                GraphicElement.color = objectColor;
            }

            if (_objectToSetAlpha.childCount > 0)
            {
                for (int i = 0; i < _objectToSetAlpha.childCount; i++)
                {
                    Transform childNumber = _objectToSetAlpha.GetChild(i);

                    if (childNumber.gameObject.activeSelf &&
                        (!childNumber.GetComponent<ReferencedFrom>() || animationPart.FadePropetiesAnim.IsFadeOverrideEnabled()))
                    {
                        SetAlphaValue(childNumber, _counterTween);
                    }
                }
            }
        }

        #endregion
    }

    [System.Serializable]
    public class PositionPropetiesAnim
    {
        #region PositionEditor

        [SerializeField]
        [HideInInspector]
        private bool positionEnabled;

        public void SetPositionEnable(bool enabled)
        {
            positionEnabled = enabled;
        }

        public bool IsPositionEnabled()
        {
            return positionEnabled;
        }

        [HideInInspector]
        public AnimationCurve TweenCurveEnterPos;
        [HideInInspector]
        public AnimationCurve TweenCurveExitPos;
        [HideInInspector]
        public Vector3 StartPos;
        [HideInInspector]
        public Vector3 EndPos;
        #if UNITY_EDITOR
        [SerializeField] [HideInInspector]
        public Vector3 StartWorldPos;
        [SerializeField] [HideInInspector]
        public Vector3 EndWorldPos;
        #endif

        public void SetPosStart(Vector3 StartPos, RectTransform rectTr)
        {
            this.StartPos = StartPos;
#if UNITY_EDITOR
            float xMes = (rectTr.anchorMin.x + rectTr.anchorMax.x) / 2f;
            float yMes = (rectTr.anchorMin.y + rectTr.anchorMax.y) / 2f;
			
            Transform rootObject = rectTr.root;
			
            Rect rectangleScreen = rootObject.GetComponent<RectTransform>().rect;
			
            StartWorldPos.x = (xMes * rectangleScreen.width + StartPos.x) * rootObject.localScale.x;
            StartWorldPos.y = (yMes * rectangleScreen.height + StartPos.y) * rootObject.localScale.y;
#endif
        }

        public void SetPosEnd(Vector3 EndPos, Transform rectTr)
        {
            this.EndPos = EndPos;
#if UNITY_EDITOR
            EndWorldPos.x = StartWorldPos.x + (EndPos.x - StartPos.x) * rectTr.root.localScale.x;
            EndWorldPos.y = StartWorldPos.y + (EndPos.y - StartPos.y) * rectTr.root.localScale.y;
#endif
        }

        public void SetAniamtionsCurve(AnimationCurve EntryTween, AnimationCurve ExitTween)
        {
            TweenCurveEnterPos = EntryTween;
            TweenCurveExitPos = ExitTween;
        }

        #endregion
    }

    [System.Serializable]
    public class ScalePropetiesAnim
    {
        #region ScaleEditor

        [SerializeField]
        [HideInInspector]
        private bool scaleEnabled;

        public void SetScaleEnable(bool enabled)
        {
            scaleEnabled = enabled;
        }

        public bool IsScaleEnabled()
        {
            return scaleEnabled;
        }

        [HideInInspector]
        public AnimationCurve TweenCurveEnterScale;
        [HideInInspector]
        public AnimationCurve TweenCurveExitScale;
        [HideInInspector]
        public Vector3 StartScale;
        [HideInInspector]
        public Vector3 EndScale;

        public void SetAniamtionsCurve(AnimationCurve EntryTween, AnimationCurve ExitTween)
        {
            TweenCurveEnterScale = EntryTween;
            TweenCurveExitScale = ExitTween;
        }

        #endregion
    }

    [System.Serializable]
    public class RotationPropetiesAnim
    {
        #region RotationEditor

        [SerializeField]
        [HideInInspector]
        private bool rotationEnabled;

        public void SetRotationEnable(bool enabled)
        {
            rotationEnabled = enabled;
        }

        public bool IsRotationEnabled()
        {
            return rotationEnabled;
        }

        [HideInInspector]
        public AnimationCurve TweenCurveEnterRot;
        [HideInInspector]
        public AnimationCurve TweenCurveExitRot;
        [HideInInspector]
        public Vector3 StartRot;
        [HideInInspector]
        public Vector3 EndRot;

        public void SetAniamtionsCurve(AnimationCurve EntryTween, AnimationCurve ExitTween)
        {
            TweenCurveEnterRot = EntryTween;
            TweenCurveExitRot = ExitTween;
        }

        #endregion
    }

    [System.Serializable]
    public class FadePropetiesAnim
    {
        #region FadeEditor

        [SerializeField]
        [HideInInspector]
        private bool fadeInOutEnabled;

        [SerializeField]
        [HideInInspector]
        private bool fadeOverride;

        [SerializeField]
        [HideInInspector]
        private float startFade = 0f;

        [SerializeField]
        [HideInInspector]
        private float endFade = 1f;

        public void SetFadeEnable(bool enabled)
        {
            fadeInOutEnabled = enabled;
        }

        public void SetFadeValues(float startFade, float endFade)
        {
            if (endFade < startFade)
            {
                Debug.LogError("End Value should be greater than the start value, values not changed");
                return;
            } 

            this.startFade = startFade;
            this.endFade = endFade;
        }

        public float GetStartFadeValue()
        {
            return startFade;
        }

        public float GetEndFadeValue()
        {
            return endFade;
        }

        public bool IsFadeEnabled()
        {
            return fadeInOutEnabled;
        }

        public void SetFadeOverride(bool enabled)
        {
            fadeOverride = enabled;
        }

        public bool IsFadeOverrideEnabled()
        {
            return fadeOverride;
        }

        #endregion
    }

    public interface IAniamtionPartProxy
    {
        bool IsObjectOpened();

        void ChangeStatus();

        void SetAniamtioDuration(float duration);

        float GetAnimationDuration();
    }

    [System.Serializable]
    public class AnimationParts : IAniamtionPartProxy
    {
        public delegate void DisableOrDestroy(bool disable,AnimationParts part);

        public static event DisableOrDestroy OnDisableOrDestroy;

        #region PositionEditor

        [HideInInspector]
        public PositionPropetiesAnim PositionPropetiesAnim = new PositionPropetiesAnim();

        #endregion

        #region ScaleEditor

        [HideInInspector]
        public ScalePropetiesAnim ScalePropetiesAnim = new ScalePropetiesAnim();

        #endregion

        #region RotationEditor

        [HideInInspector]
        public RotationPropetiesAnim RotationPropetiesAnim = new RotationPropetiesAnim();

        #endregion

        #region FadeEditor

        [HideInInspector]
        public FadePropetiesAnim FadePropetiesAnim = new FadePropetiesAnim();

        #endregion

        #region PUBLIC_Var

        public void SetAniamtioDuration(float duration)
        {
            if (duration > 0f)
                animationDuration = duration;
            else
                duration = 0.01f;
        }

        public float GetAnimationDuration()
        {
            return animationDuration;
        }

        public bool UnscaledTimeAnimation = false;
        public bool SaveState = false;
        public bool AtomicAnimation = false;

        public enum State
        {
            OPEN,
            CLOSE}

        ;

        public State ObjectState = State.CLOSE;


        public enum EndTweenClose
        {
            DEACTIVATE,
            DESTROY,
            NOTHING}

        ;

        public EndTweenClose EndState = EndTweenClose.DEACTIVATE;

        public enum CallbackCall
        {
            END_OF_INTRO_ANIM,
            END_OF_EXIT_ANIM,
            END_OF_INTRO_AND_END_OF_EXIT_ANIM,
            START_INTRO_ANIM,
            START_INTRO_END_OF_EXIT_ANIM,
            START_INTRO_END_OF_INTRO_ANIM,
            START_INTRO_END_OF_INTRO_AND_END_OF_EXIT_ANIM,
            START_EXIT_ANIM,
            START_EXIT_START_INTRO_ANIM,
            START_EXIT_END_OF_EXIT_ANIM,
            START_EXIT_END_OF_INTRO_ANIM,
            START_EXIT_END_OF_INTRO_AND_END_OF_EXIT_ANIM,
            START_INTRO_AND_START_EXIT_END_OF_EXIT_ANIM,
            START_INTRO_AND_START_EXIT_END_OF_INTRO_ANIM,
            START_INTRO_AND_START_EXIT_END_OF_INTRO_AND_END_OF_EXIT_ANIM,
            NOTHING}

        ;

        public CallbackCall CallCallback = CallbackCall.END_OF_INTRO_ANIM;

        public UnityEvent IntroEvents = new UnityEvent();
        public UnityEvent ExitEvents = new UnityEvent();
        private UnityEvent CallBackObject;

        #endregion

        #region PRIVATE_Var

        private bool CheckNextFrame = false;
        private bool CallOnThisFrame = false;

        [SerializeField]
        [HideInInspector]
        private float animationDuration = 1f;

        #endregion

        #region PUBLIC_Methods

        public AnimationParts(State ObjectState, bool UnscaledTimeAnimation, bool SaveState, bool AtomicAnim, EndTweenClose EndState, CallbackCall CallCallback, UnityEvent IntroEvents, UnityEvent ExitEvents)
        {
            this.ObjectState = ObjectState;
            this.UnscaledTimeAnimation = UnscaledTimeAnimation;
            this.SaveState = SaveState;
            this.AtomicAnimation = AtomicAnim;
            this.EndState = EndState;
            this.CallCallback = CallCallback;
            this.IntroEvents = IntroEvents;
            this.ExitEvents = ExitEvents;
        }

        public void CheckCallbackStatus()
        {
            if (CallCallback != CallbackCall.NOTHING)
            {
                if ((CallCallback == CallbackCall.START_INTRO_END_OF_EXIT_ANIM
                    || CallCallback == CallbackCall.START_INTRO_ANIM
                    || CallCallback == CallbackCall.START_INTRO_END_OF_INTRO_ANIM
                    || CallCallback == CallbackCall.START_INTRO_END_OF_INTRO_AND_END_OF_EXIT_ANIM
                    || CallCallback == CallbackCall.START_INTRO_AND_START_EXIT_END_OF_EXIT_ANIM
                    || CallCallback == CallbackCall.START_INTRO_AND_START_EXIT_END_OF_INTRO_ANIM
                    || CallCallback == CallbackCall.START_INTRO_AND_START_EXIT_END_OF_INTRO_AND_END_OF_EXIT_ANIM
                    || CallCallback == CallbackCall.START_EXIT_START_INTRO_ANIM) && ObjectState == State.OPEN)
                {
                    CheckCallBack(IntroEvents);
                }
                else if ((CallCallback == CallbackCall.START_EXIT_END_OF_EXIT_ANIM
                         || CallCallback == CallbackCall.START_EXIT_ANIM
                         || CallCallback == CallbackCall.START_EXIT_END_OF_INTRO_ANIM
                         || CallCallback == CallbackCall.START_EXIT_END_OF_INTRO_AND_END_OF_EXIT_ANIM
                         || CallCallback == CallbackCall.START_INTRO_AND_START_EXIT_END_OF_EXIT_ANIM
                         || CallCallback == CallbackCall.START_INTRO_AND_START_EXIT_END_OF_INTRO_ANIM
                         || CallCallback == CallbackCall.START_INTRO_AND_START_EXIT_END_OF_INTRO_AND_END_OF_EXIT_ANIM
                         || CallCallback == CallbackCall.START_EXIT_START_INTRO_ANIM) && ObjectState == State.CLOSE)
                {
                    CheckCallBack(ExitEvents);
                }
            }
        }

        public void FinalEnd()
        {
            if (ObjectState == State.CLOSE)
            {
                if (EndState == EndTweenClose.DEACTIVATE)
                {
                    if (OnDisableOrDestroy != null)
                    {
                        OnDisableOrDestroy(true, this);
                    }
                }
                else if (EndState == EndTweenClose.DESTROY)
                {
                    if (OnDisableOrDestroy != null)
                    {
                        OnDisableOrDestroy(false, this);
                    }
                }
            }

            if (SaveState)
            {
                ObjectState = (ObjectState == State.OPEN) ? State.CLOSE : State.OPEN;
            }		
        }

        public void Ended()
        {
            if (CallCallback != CallbackCall.NOTHING)
            {
                if (ObjectState == State.CLOSE)
                {
                    if (CallCallback == CallbackCall.END_OF_EXIT_ANIM
                        || CallCallback == CallbackCall.END_OF_INTRO_AND_END_OF_EXIT_ANIM
                        || CallCallback == CallbackCall.START_INTRO_END_OF_EXIT_ANIM
                        || CallCallback == CallbackCall.START_INTRO_END_OF_INTRO_AND_END_OF_EXIT_ANIM
                        || CallCallback == CallbackCall.START_EXIT_END_OF_EXIT_ANIM
                        || CallCallback == CallbackCall.START_EXIT_END_OF_INTRO_AND_END_OF_EXIT_ANIM
                        || CallCallback == CallbackCall.START_INTRO_AND_START_EXIT_END_OF_EXIT_ANIM
                        || CallCallback == CallbackCall.START_INTRO_AND_START_EXIT_END_OF_INTRO_AND_END_OF_EXIT_ANIM)
                    {
                        CheckCallBack(ExitEvents);
                    }                
                }

                if ((CallCallback == CallbackCall.END_OF_INTRO_ANIM
                    || CallCallback == CallbackCall.END_OF_INTRO_AND_END_OF_EXIT_ANIM
                    || CallCallback == CallbackCall.START_INTRO_END_OF_INTRO_ANIM
                    || CallCallback == CallbackCall.START_INTRO_END_OF_INTRO_AND_END_OF_EXIT_ANIM
                    || CallCallback == CallbackCall.START_EXIT_END_OF_INTRO_ANIM
                    || CallCallback == CallbackCall.START_EXIT_END_OF_INTRO_AND_END_OF_EXIT_ANIM
                    || CallCallback == CallbackCall.START_INTRO_AND_START_EXIT_END_OF_INTRO_ANIM
                    || CallCallback == CallbackCall.START_INTRO_AND_START_EXIT_END_OF_INTRO_AND_END_OF_EXIT_ANIM) && ObjectState == State.OPEN)
                {
                    CheckCallBack(IntroEvents);
                }       
            }
        }

        public void FrameCheck()
        {
            if (CheckNextFrame)
            {
                if (CallOnThisFrame)
                {
                    CallCallbackObjects();
                }

                CallOnThisFrame = !CallOnThisFrame;
            }
        }

        public bool IsObjectOpened()
        {
            if (ObjectState == State.CLOSE)
            {
                return false;
            }

            return true;
        }

        public void ChangeStatus()
        {
            if (ObjectState == State.CLOSE)
            {
                ObjectState = State.OPEN;
            }
            else
            {
                ObjectState = State.CLOSE;
            }
        }

        public void SetStatus(bool open)
        {
            if (open)
            {
                ObjectState = State.OPEN;
            }
            else
            {
                ObjectState = State.CLOSE;
            }
        }

        #endregion

        #region PRIVATE_Methods

        private void CheckCallBack(UnityEvent CallbackObject)
        {
            this.CallBackObject = CallbackObject;
            CheckNextFrame = !CheckNextFrame;        
        }

        private void CallCallbackObjects()
        {           
            CheckNextFrame = !CheckNextFrame;

            CallBackObject.Invoke();     
        }

        #endregion
    }
}