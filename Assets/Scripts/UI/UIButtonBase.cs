using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public enum ButtonType
    {
        Info,
        Click,
        Toggle,
    }
    
    public abstract class UIButtonBase : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler
    {
        /*
         * ================================
         *        Common Methods
         * ================================
         */
        
        public virtual void OnAwake(){}
        public virtual void OnStart(){}
        public virtual void OnUpdate(){}

        public virtual void OnClick(){}
        public virtual void OnHoverIndo(bool toggle){}
        public virtual void OnDetailedInfo(bool toggle){}
        public virtual void OnToggle(bool toggle){}
        public virtual void OnCancel(){}
        
        public virtual void OnMouseEnter(){}
        public virtual void OnMouseExit(){}
        
        public virtual void PlaySoundFX(){}
        
        // ============== End ==============
        
        public ButtonType ButtonType;

        public bool 
            isAnimatedOnClick,
            isAnimatedOnHover;

        private bool
            _infoToggle,
            _infoDetailedToggle,
            _toggle;
        
        private float // Animation Adjustments
            _maxScale = 1.05f,
            _speed = 0.05f;
        
        private Tween _routine;
        private Vector3 _savedScale;
        
        public void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnAction);
            _savedScale = transform.localScale;
            OnAwake();
        }

        public void Start()
        {
            OnStart();
        }

        private void Update()
        {
            if (InputGather.Instance.CancelButton)
            {
                OnCancel();
                D_OnDetailedInfo(false);
                D_OnHoverInfo(false);
                D_OnToggle(false);
            }

            OnUpdate();
        }

        public void OnAction()
        {
            switch (ButtonType)
            {
                case ButtonType.Info:
                    D_OnDetailedInfo();
                    break;
                case ButtonType.Click:
                    D_OnClick();
                    break;
                case ButtonType.Toggle:
                    D_OnToggle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            PlaySoundFX();
        }
        
        private void D_OnClick()
        {
            if (isAnimatedOnClick && !_routine.IsPlaying())
                _routine = transform.DOScale(transform.localScale * _maxScale, _speed).SetEase(Ease.OutSine)
                    .OnComplete(() => _routine =  transform.DOScale(transform.localScale / _maxScale, _speed).SetEase(Ease.InSine));
            OnClick();
        }
        
        private void D_OnHoverInfo(bool? overrideToggle = null)
        {
            _infoToggle = overrideToggle ?? !_infoToggle;
            OnHoverIndo(_infoToggle);
        }

        private void D_OnDetailedInfo(bool? overrideToggle = null)
        {
            _infoDetailedToggle = overrideToggle ?? !_infoDetailedToggle;
            OnDetailedInfo(_infoDetailedToggle);
        }

        private void D_OnToggle(bool? overrideToggle = null)
        {
            _toggle = overrideToggle ?? !_toggle;

            if (_toggle)
            {
                if(EventSystem.current.currentSelectedGameObject.TryGetComponent(out UIButtonBase button))  button.D_OnToggle(false);
                EventSystem.current.SetSelectedGameObject(gameObject);
                OnToggle(true);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                OnToggle(false);
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isAnimatedOnHover)
                _routine = transform.DOScale(transform.localScale * _maxScale, _speed).SetEase(Ease.OutSine);
            OnMouseEnter();
            D_OnHoverInfo(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isAnimatedOnHover)
            {
                _routine?.Kill();
                _routine = transform.DOScale(_savedScale, _speed).SetEase(Ease.InSine);
            }
            OnMouseExit();
            D_OnHoverInfo(false);
        }

        private void OnDestroy()
        {
            _routine?.Kill();
        }
    }
}