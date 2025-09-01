using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Communication
{
    public class JsonDecoder
    {
        private class Wrapper<T>
        {
            public T[] Items;
        }

        public static T[] FromJson<T>(string json)
        {
            if (json.StartsWith('['))
            {
                json = "{ \"Items\": " + json + "}";
            }

            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items ?? new T[0];
        }

    }
}
