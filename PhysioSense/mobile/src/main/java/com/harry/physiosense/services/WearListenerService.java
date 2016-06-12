package com.harry.physiosense.services;

/**
 * Created by Harry on 24/05/2016.
 */

import android.util.Log;

import com.google.android.gms.wearable.MessageEvent;
import com.google.android.gms.wearable.WearableListenerService;
import com.harry.physiosense.PhoneActivity;


public class WearListenerService extends WearableListenerService {
    // initiating TAG as string for debugging
   private static final String TAG = "WLS";
    private static final String SERVICE_CALLED_WEAR = "HR Updated";

    String wearHRate;
     String wearStatus = "Unknown";

    /////////////////////////////
    /////// Wear Listener ///////
    /////////////////////////////


    @Override
    public void onMessageReceived(MessageEvent messageEvent) {
        super.onMessageReceived(messageEvent);

        String event = messageEvent.getPath();

        Log.d("HR Updated", event);

        String[] hRateMessage = event.split("--");

        if (hRateMessage[0].equals(SERVICE_CALLED_WEAR)) {

            wearStatus = "Connected";


        }

        wearHRate = hRateMessage[1].toString();
        getWearHRate();
        Log.d(TAG + "Message 1: ", wearHRate);
        Log.d(TAG + "Message 0: ", hRateMessage[0].toString());




    }



    public synchronized String getWearHRate() {


        return wearHRate;


    }

    public synchronized String getWearStatus() {


        return wearStatus;
    }

}


