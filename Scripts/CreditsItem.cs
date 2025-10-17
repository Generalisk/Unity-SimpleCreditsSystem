using UnityEngine;

namespace Generalisk.Credits
{
    [System.Serializable]
    public class CreditsItem
    {
        public string text = "";
        public float fontSize = 48;

        public Sprite image = null;

        public CreditsItem[] subItems = { };
    }
}
