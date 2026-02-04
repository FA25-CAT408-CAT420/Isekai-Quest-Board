using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopInterface
{
    string shopItemTag { get; }
    void Initialize(GameObject prefabReference);
}
