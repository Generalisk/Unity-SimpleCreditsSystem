using UnityEngine;

namespace Generalisk.Credits
{
    [System.Serializable]
    public class CreditsItem
    {
        public string text = "";
        public float fontSize = 48;

        public Sprite image = null;

        // TODO: Resolve depth limit warnings
        //
        // They're mostly harmless unless
        // you go that deep (which is unlikely
        // to happen without doing so intentionally)
        // but the warnings can get annoying
        public CreditsItem[] subItems = { };
    }
}
