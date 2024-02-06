using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DuckTunes.Utility;
public class FadingPanel : MonoBehaviour
{

   [SerializeField] private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = Utilities.TryGetComponentTFromGameObject<LineRenderer>(this.gameObject.transform.transform);
        lineRenderer.material.color = new Color(0f, 0f, 0f, 0f);
        Color c = lineRenderer.material.color;
        c.a = 0f;
        lineRenderer.material.color = c;
        c.a = 0f;
        lineRenderer.material.SetColor("_Color", c);
        StartCoroutine(FadeLineRenderer());
    }
 

    IEnumerator FadeLineRenderer()
    {
        for (float f = 0.05f; f >= 1; f += 0.05f)
        {
            Color c = lineRenderer.material.color;
            c.a = f;
            
            lineRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }








       //Gradient lineRendererGradient = new Gradient();
        float fadeSpeed = 3f;
        float timeElapsed = 0f;
       // float alpha = 1f;

        while (timeElapsed < fadeSpeed)
        {
           // Color lineRenderer = Utilities.TryGetComponentFomGameObject<LineRenderer>this.gameObject.;
            

           //alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeSpeed);
            yield return null;
            
          
          

          
        }

        Destroy(gameObject);
    }
}

















//    [SerializeField] private CanvasGroup canvasGroup;
//    private Tween fadeTween;
//    public LineRenderer linerend;
//    void Start()
//    {

//        StartCoroutine(Fadenow());

//    }

//    public void FadeIn(float duration)
//    {
//        Fade(1, duration, () =>
//        {
//            canvasGroup.interactable = true;
//            canvasGroup.blocksRaycasts = true;
//        });
//    }


//    public void FadeOut(float duration)
//    {
//        Fade(0f, duration, () =>
//        {
//            //linerend = Utilities.TryGetComponentTFromGameObject<LineRenderer>(this.gameObject.transform);
//            //linerend.material.DOFade(0.0f, 2.0f);
//          //  GetComponent<LineRenderer>().material.DOFade(0.0f, 5.0f);
//            canvasGroup.interactable = false;
//            canvasGroup.blocksRaycasts = false;
//        });
//    }

//    private void Fade(float endValue, float duration, TweenCallback onEnd)
//    {
//        if (fadeTween != null)
//        {
//            fadeTween.Kill(false);
//        }

//      fadeTween = canvasGroup.DOFade(endValue, duration);


//        fadeTween.onComplete += onEnd;
//    }


//    private IEnumerator Fadenow()
//    {
//        yield return new WaitForSeconds(2f);
//        FadeOut(0f);
//        yield return new WaitForSeconds(3f);
//        FadeIn(0f);
//    }


//    void Update()
//    {

//    }
//}
