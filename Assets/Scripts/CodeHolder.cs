using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeHolder : MonoBehaviour
{

    public void changeCode(string theNewCode){
        InterSceneDataKeeper.Instance.roomCode = theNewCode;
    }

    public void changeName(string theNewName){
        InterSceneDataKeeper.Instance.playerName = theNewName;
    }
}
