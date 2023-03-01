/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Works alongside the Loader class to notify it when the current level has updated the screen
 * */
public class LoaderCallback : MonoBehaviour {

    private bool firstUpdate = true;

    private void Update() {
        if (firstUpdate) {
            firstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
