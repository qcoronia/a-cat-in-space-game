using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("UI/View Management/View Manager")]
public class ViewManager : MonoBehaviour
{

    public ViewType defaultView;
    public ViewType previousView;
    public ViewType currentView;
    public EventHandler OnViewChanged;

    void Start()
    {
        previousView = defaultView;
        currentView = defaultView;

        for (var i = 0; i < transform.childCount; i++)
        {
            var viewObj = transform.GetChild(i);
            viewObj.gameObject.SetActive(false);
        }

        OnExitAnimationEnded();
    }

    public void Goto(ViewType dest)
    {
        previousView = currentView;
        currentView = dest;
        PlayExitAnimations();
        OnViewChanged.Invoke();
    }

    public void PlayExitAnimations()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var viewObj = transform.GetChild(i);
            var view = viewObj.GetComponent<View>();
            if (view.viewType == previousView)
            {
                var viewAnimator = viewObj.GetComponent<Animator>();
                if (viewAnimator != null)
                {
                    viewAnimator.Play("Exit");
                }
                else
                {
                    OnExitAnimationEnded();
                }
            }
        }
    }

    public void OnExitAnimationEnded()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var viewObj = transform.GetChild(i);
            var view = viewObj.GetComponent<View>();
            if (view.viewType == currentView)
            {
                viewObj.gameObject.SetActive(true);
                var viewAnimator = viewObj.GetComponent<Animator>();
                if (viewAnimator != null)
                {
                    viewAnimator.Play("Enter");
                }
            }
            else
            {
                viewObj.gameObject.SetActive(false);
            }
        }
    }
}
