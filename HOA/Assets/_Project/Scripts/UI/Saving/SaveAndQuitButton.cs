using antoinegleisberg.HOA.EventSystems.SceneManagement;
using UnityEngine;

namespace antoinegleisberg.HOA.UI.Saving
{
    public class SaveAndQuitButton : MonoBehaviour
    {
        public void SaveAndQuit()
        {
            SceneManagementEvents.Instance.SaveAndQuit();
        }
    }
}
