using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DNAUpgrade : MonoBehaviour
{
    public abstract void ApplyUpgrade(Player player, bool firstTime);

    public abstract void RemoveUpgrade(Player player);
}
