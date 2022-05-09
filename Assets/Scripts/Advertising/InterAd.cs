using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class InterAd : MonoBehaviour
{
    private InterstitialAd interstialAd;
    private string interstialUnitID = "ca-app-pub-3940256099942544/1033173712";
    public bool adClose = false;
    private void Start()
    {
        interstialAd.OnAdClosed += (sender, args) => 
        {
            Debug.Log("Close");
            adClose = true;
        }; 
    }
    private void OnEnable()
    {
        interstialAd = new InterstitialAd(interstialUnitID);
        AdRequest adRequest = new AdRequest.Builder().Build();
        interstialAd.LoadAd(adRequest);
    }
    public void ShowAd()
    {
        if (interstialAd.IsLoaded())
            interstialAd.Show();
    }
    
}
