using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider ProgressSlider;
    public SliderType ProgressType;
    public enum SliderType {WaveProgress,TotalProgress };


    void Update()
    {
        if(EnemySpawner.LatestLaunched==null)
        {
            ProgressSlider.value = Mathf.Lerp(ProgressSlider.value,1,0.1f);
        }
        else
        {
            float targValue = 0;

            if(ProgressType==SliderType.WaveProgress)
            {
                targValue = EnemySpawner.LatestLaunched.WavePercentage();
            }
            else if (ProgressType == SliderType.TotalProgress)
            {
                targValue = EnemySpawner.LatestLaunched.TotalPercentage();
            }

            ProgressSlider.value = Mathf.Lerp(ProgressSlider.value, targValue, 0.1f);
        }
    }
}
