using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPartDropOff : MonoBehaviour
{
    private int numberOfEnemyPartsDroppedOffSoFar;

    public void DropOffEnemyPart(){

        //Add to number of part dropped off
        numberOfEnemyPartsDroppedOffSoFar++;
    }
    
}
