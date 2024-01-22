using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIObject : IUIBound
{
    RectTransform Transform { get; }
    bool Visible { get; set; }

    // Enable이 없는 이유 Enable을 가지는 오브젝트가 있을수도 있고 없을수도 있으므로, 별도의 인터페이스로 분리하자. (IUIEnable)
}
