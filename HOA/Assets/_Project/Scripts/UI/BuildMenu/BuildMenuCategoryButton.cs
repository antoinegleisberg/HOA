using UnityEngine;
using UnityEngine.UI;

namespace antoinegleisberg.HOA.UI
{
    public class BuildMenuCategoryButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;

        public void Init(BuildMenu buildMenu, BuildMenuCategory category)
        {
            ScriptableBuildMenuCategory scriptableCategory = ScriptableBuildMenuCategory.GetByCategory(category);
            _image.sprite = scriptableCategory.Icon;
            _button.onClick.AddListener(() => buildMenu.SwitchCategory(category));
        }
    }
}
