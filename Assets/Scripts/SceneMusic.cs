using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{

    void Start()
    {
        AudioManager.instance.Play("heroes4");
    }

}
