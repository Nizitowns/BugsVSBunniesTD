using DefaultNamespace;
using UnityEngine;

public class LevelButtonManager : UIButtonBase
{
    [Min(1)]
    public int LevelID=1;
    float scale;
    float scale_mod;
    private float _timer = 0;

    public override void OnStart()
    {
        scale=transform.localScale.x;
        scale_mod = 0;
        if(LevelID==1 )
        {
            PlayerPrefs.SetInt("LevelCleared_0", 1);
        }
    }

    bool mouseOver;

    public override void OnUpdate()
    {
        Interactable = PlayerPrefs.GetInt("LevelCleared_" + (LevelID-1), 0)==1;
        
        if(IsMouseOver && Interactable)
        {
            _timer += Time.deltaTime;
            scale_mod =(Mathf.Sin(2.5f*_timer)+1)/66f;
        }
        else
        {
            scale_mod = 0;
            _timer = 0;
        }
        transform.localScale = new Vector3(scale+ scale_mod, scale+ scale_mod, scale+ scale_mod);
    }
}
