using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin_UI_Animator : MonoBehaviour
{
    protected Image imageToAnim;
    [SerializeField] protected Sprite[] imageFrames = new Sprite[20];
    [SerializeField] protected float timeBetweenFrames;
    protected int frameArrayIterator = 0;

    // Start is called before the first frame update
    void Start()
    {
        imageToAnim = GetComponent<Image>();
        InvokeRepeating("CoinAnimator", 0, timeBetweenFrames);
       
         
    }

    private void CoinAnimator()
    {

        imageToAnim.sprite = imageFrames[frameArrayIterator];
        if(frameArrayIterator + 1 == imageFrames.Length ) { frameArrayIterator = 0; }
        else { frameArrayIterator++; }
        
    }
}
