using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IKinetic
    {
        Vector2 Accelerate(Vector2 dir, float initialAcc, float endAcc, float t = 1.0F);
    }
}
