using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMKOC.Grammer;
using UnityEngine;

namespace TMKOC.Grammer
{
    public class CloudUI : SerializedSingleton<CloudUI>
    {
        [SerializeField] private Animator m_Animator;

        protected override void Awake()
        {
            base.Awake();
            m_Animator = GetComponent<Animator>();
        }

        public void OnCloudAnimationFinished()   //this function is on the animation as a key (animation event)
        {
            Debug.Log("animation is over::");
            GameManager.Instance.LoadSelection();
        }

        public void OnAnimationDone(Action callback)
        {
            callback?.Invoke();
        }



        [Button]
        public void PlayCloudEnterAnimation()
        {
            m_Animator.SetTrigger("entry");
        }

        [Button]
        public void PlayCloudExitAnimation()
        {
            m_Animator.SetTrigger("exit");

        }

    }
}
