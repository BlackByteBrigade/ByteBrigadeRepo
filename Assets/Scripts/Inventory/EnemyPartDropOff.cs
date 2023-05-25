using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPartDropOff : MonoBehaviour
{
    public void DropOffEnemyPart(){

        //Add to number of part dropped off
        GameManager.Instance.storedEnemyParts++;
    }
    
}
