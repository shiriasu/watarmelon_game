using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FRUITS_TYPE
{
    さくらんぼ = 1,
    いちご,
    ぶどう,
    オレンジ,
    かき,
    りんご,
    なし,
    もも,
    パイナップル,
    メロン,
    すいか,
}

public class Fruits : MonoBehaviour
{
    public FRUITS_TYPE fruitsType;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent(out Fruits otherFruits))
        {
            //相手のフルーツタイプと自分のフルーツタイプとが同じならば削除を行う
            //ex)いちご,いちご -> 消える　いちご,さくらんぼ -> 消えない
            if(otherFruits.fruitsType == fruitsType)
            {
                Destroy(gameObject);
            }
        }
    }
    
}
