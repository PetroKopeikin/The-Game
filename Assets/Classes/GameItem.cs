using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public class GameItem : MonoBehaviour
    {
        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public virtual void Activate()
        {
            gameObject.SetActive(true);
        }
    }
}
