package com.harry.physiosense.services.myo;

/**
 * Created by Harry on 24/05/2016.
 */
public class MYOListenerService {
String myoValues = "myo";
    String myoStatus = "Unknown";

    public String getMYOValues (){

        return myoValues;
    }

    public String getMyoStatus (){
        return myoStatus;
    }
}
