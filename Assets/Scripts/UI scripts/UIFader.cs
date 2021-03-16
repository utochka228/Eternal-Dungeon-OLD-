using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    bool mFaded = false;
    [SerializeField] float duration = 0.4f;
    CanvasGroup canvasGroup;
    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Fade(){
        canvasGroup.alpha = 0f;
        StartCoroutine(DoFade(canvasGroup, canvasGroup.alpha, mFaded ? 1: 03));
        mFaded = !mFaded;
    }
    public IEnumerator DoFade(CanvasGroup canvasGroup, float start, float end){
        float counter = 0f;
        while(counter < duration){
            counter +=Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }
    }
}
