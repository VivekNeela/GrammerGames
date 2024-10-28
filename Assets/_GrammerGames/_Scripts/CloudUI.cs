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
        [SerializeField] private bool inTransition;
        public bool InTransition { get => inTransition; set => inTransition = value; }
        [SerializeField] private Animator m_Animator;


        protected override void Awake()
        {
            base.Awake();
            m_Animator = GetComponent<Animator>();
        }

        public void OnCloudAnimationFinished()   //this function is on the animation as a key (animation event)
        {
            Debug.Log("animation is over::");
            // GameManager.Instance.LoadSelection();
            InTransition = false;
        }



        [Button]
        public void PlayCloudEnterAnimation()
        {
            Debug.Log("<color=green>playing anim...</color>");
            InTransition = true;
            m_Animator.SetTrigger("entry");
        }

        
        public void PlayCloudExitAnimation()
        {
            InTransition = false;
            m_Animator.SetTrigger("exit");

        }

    }
}
