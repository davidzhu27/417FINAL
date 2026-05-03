using UnityEngine;

public class ScreenFadeProxy : MonoBehaviour 
{
    public void FadeToRedFast() => ScreenFade.Instance.FadeToRedFast();
    public void FadeOut(float duration) => ScreenFade.Instance.FadeOut(duration);
}
