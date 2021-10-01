using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Heavenly.Client.API
{
    public class VRCSlider : RubyButtonAPI.QMButtonBase
    {
        public VRCSlider(RubyButtonAPI.QMNestedButton btnMenu, Vector2 location, Vector4 sliderValues, String sliderLabel, System.Action<float> onValueChanged, Color? labelColor = null)
        {
            btnQMLoc = btnMenu.getMenuName();
            initButton(btnQMLoc, location, sliderValues, sliderLabel, onValueChanged, labelColor);
        }

        public VRCSlider(string btnMenu, Vector2 location, Vector4 sliderValues, String sliderLabel, System.Action<float> onValueChanged, Color? labelColor = null)
        {
            btnQMLoc = btnMenu;
            initButton(btnQMLoc, location, sliderValues, sliderLabel, onValueChanged, labelColor);
        }

        private void initButton(String menu, Vector2 location, Vector4 sliderValues, String sliderLabel, System.Action<float> onValueChanged, Color? labelColor = null)
        {
            var origSlider = VRCUiManager.prop_VRCUiManager_0.field_Public_GameObject_0.transform.Find("/Screens/Settings/AudioDevicePanel/VolumeSlider");

            var origText = RubyButtonAPI.QMStuff.GetQuickMenuInstance().transform.Find($"{menu}/SingleButton(5,2)").GetComponentInChildren<Text>();

            btnType = "VRCSlider"; 

            button = UnityEngine.Object.Instantiate(RubyButtonAPI.QMStuff.SingleSliderTemplate(), RubyButtonAPI.QMStuff.GetQuickMenuInstance().transform.Find(btnQMLoc), true);

            Transform.Destroy(button.GetComponentInChildren<Text>());

            button.GetComponent<RectTransform>().sizeDelta *= new Vector2(1.5f, 1);

            var label = GameObject.Instantiate(origText, button.transform);
            label.name = "SliderLabel";
            var valueLabel = GameObject.Instantiate(origText.gameObject, button.transform);
            valueLabel.name = "SliderValueLabel";

            label.GetComponent<RectTransform>().anchoredPosition -= new Vector2(button.GetComponent<RectTransform>().sizeDelta.x + 15, 0);
            label.GetComponent<Text>().text = sliderLabel;
            label.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            label.GetComponent<Text>().fontSize = 36;

            if (labelColor != null)
            {
                label.GetComponent<Text>().color = (Color)labelColor;
            }
            else
            {
                label.GetComponent<Text>().color = Color.white;
            }

            valueLabel.GetComponent<Text>().text = sliderValues.w.ToString();
            valueLabel.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            valueLabel.GetComponent<Text>().fontSize = 36;
            valueLabel.GetComponent<Text>().color = Color.white;

            button.transform.localPosition = Vector3.zero;
            button.transform.localRotation = Quaternion.identity;
            //button.GetComponent<RectTransform>().rotation = QMStuff.GetQuickMenuInstance().transform.Find(btnQMLoc).GetComponent<RectTransform>().rotation;
            button.GetComponent<RectTransform>().anchoredPosition += new Vector2(location.x * 420, location.y * button.GetComponent<RectTransform>().sizeDelta.y);
            button.GetComponent<Slider>().minValue = sliderValues.x;
            button.GetComponent<Slider>().maxValue = sliderValues.y;
            button.GetComponent<Slider>().onValueChanged = new Slider.SliderEvent();
            button.GetComponent<Slider>().onValueChanged.AddListener(onValueChanged);
            button.GetComponent<Slider>().onValueChanged.AddListener(new Action<float>((value) => { valueLabel.GetComponent<Text>().text = Math.Round(button.GetComponent<Slider>().value, (int)sliderValues.z).ToString(); }));
            button.SetActive(true);
            button.GetComponent<Slider>().value = sliderValues.w;
            valueLabel.GetComponent<Text>().text = sliderValues.w.ToString();
        }

        public float GetValue()
        {
            return button.GetComponent<Slider>().value;
        }

        public void SetValue(float value)
        {
            button.GetComponent<Slider>().value = value;
        }

        public void setButtonText(string buttonText)
        {
            button.GetComponentInChildren<Text>().text = buttonText;
        }

        public void setAction(System.Action buttonAction)
        {
            button.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            if (buttonAction != null)
                button.GetComponent<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>(buttonAction));
        }

        public override void setBackgroundColor(Color buttonBackgroundColor, bool save = true)
        {
            //button.GetComponentInChildren<UnityEngine.UI.Image>().color = buttonBackgroundColor;
            if (save)
                OrigBackground = (Color)buttonBackgroundColor;
            //UnityEngine.UI.Image[] btnBgColorList = ((btnOn.GetComponentsInChildren<UnityEngine.UI.Image>()).Concat(btnOff.GetComponentsInChildren<UnityEngine.UI.Image>()).ToArray()).Concat(button.GetComponentsInChildren<UnityEngine.UI.Image>()).ToArray();
            //foreach (UnityEngine.UI.Image btnBackground in btnBgColorList) btnBackground.color = buttonBackgroundColor;
            button.GetComponentInChildren<UnityEngine.UI.Button>().colors = new ColorBlock()
            {
                colorMultiplier = 1f,
                disabledColor = Color.grey,
                highlightedColor = buttonBackgroundColor * 1.5f,
                normalColor = buttonBackgroundColor / 1.5f,
                pressedColor = Color.grey * 1.5f
            };
        }

        public override void setTextColor(Color buttonTextColor, bool save = true)
        {
            button.GetComponentInChildren<Text>().color = buttonTextColor;
            if (save)
                OrigText = (Color)buttonTextColor;
        }
    }
}
