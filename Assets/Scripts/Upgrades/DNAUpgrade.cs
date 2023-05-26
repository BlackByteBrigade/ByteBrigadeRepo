using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DNAUpgrade : MonoBehaviour
{
    public abstract void ApplyUpgrade(Player player);

    public abstract void RemoveUpgrade(Player player);
}
