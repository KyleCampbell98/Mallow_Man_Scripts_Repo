using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star_UI_Animtor : Coin_UI_Animator
{
   [SerializeField] private float delayBeforeAnim = 0;

    // Start is called before the first frame update
    void Start()
    {
        frameArrayIterator = 0;
        imageToAnim = GetComponent<Image>();
        InvokeRepeating("AnimateStarCollection", delayBeforeAnim, timeBetweenFrames);
    }

   private void AnimateStarCollection()
    {
        imageToAnim.sprite = imageFrames[frameArrayIterator];
        if (frameArrayIterator + 1 == imageFrames.Length) { CancelInvoke(); }
        frameArrayIterator++;
        
    }
}
