/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{

    public static GameAssets i;

    private void Awake() {
        FoodList.Add(foodSprite);
        FoodList.Add(foodSprite2);
        FoodList.Add(foodSprite3);

        i = this;
    }
    
    public Sprite snakeHeadSprite;
    public Sprite SnakeNeckSprite;
    public Sprite SnakeEatSprite;
    public Sprite SnakeTurnSprite;
    public Sprite snakeBodySprite;
    public Sprite SnakeTailSprite;
    public Sprite foodSprite;
    public Sprite foodSprite2;
    public Sprite foodSprite3;
    public Sprite DeliverySprite;

    public List<Sprite> FoodList;


    public SoundAudioClip[] soundAudioClipArray;

    [Serializable]
    public class SoundAudioClip {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
