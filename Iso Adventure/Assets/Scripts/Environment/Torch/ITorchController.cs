using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITorchController
{
        int RequiredTorches
        {
            get;
            set;
        }
        int CurrentTorches
        {
            get;
            set;
        }
    void LightTorch();

    void FullTorch();

    void CloseTorch();

}
