package com.harry.physiosense.services;

/**
 * Created by Harry on 24/05/2016.
 */

import android.util.Log;

import com.interaxon.libmuse.Accelerometer;
import com.interaxon.libmuse.ConnectionState;
import com.interaxon.libmuse.Eeg;
import com.interaxon.libmuse.Muse;
import com.interaxon.libmuse.MuseArtifactPacket;
import com.interaxon.libmuse.MuseConnectionListener;
import com.interaxon.libmuse.MuseConnectionPacket;
import com.interaxon.libmuse.MuseDataListener;
import com.interaxon.libmuse.MuseDataPacket;
import com.interaxon.libmuse.MuseDataPacketType;
import com.interaxon.libmuse.MuseManager;
import com.interaxon.libmuse.MusePreset;
import com.interaxon.libmuse.MuseVersion;

import java.util.ArrayList;
import java.util.List;

//Muse Library


public class MuseListenerService {

    //Initializing strings

    String elm1 = "00.00";
    String elm2 = "00.00";
    String elm3 = "00.00";
    String elm4 = "00.00";
    String eeg1 = "00.00";
    String eeg2 = "00.00";
    String eeg3 = "00.00";
    String eeg4 = "00.00";
    String accX = "00.00";
    String accY = "00.00";
    String accZ = "00.00";
    String museStatus = "Unknown";
    String museElements = "element01" + "," + elm1 + "element02" + "," + elm2 + "element03" + "," + elm3 + "element04" + "," + elm4;
    String museEEG = "eeg1" + "," + eeg1 + "eeg2" + "," + eeg2 + "eeg3" + "," + eeg3 + "eeg4" + "," + eeg4;
    String museAcc = "accX" + "," + accX + "accY" + "," + accY + "accZ" + "," + accZ;
    String museBattery = "100%";
    String museValues = museElements + museEEG + museAcc + museBattery;

    /////////////////////////////
    /////// MUSE Listener ///////
    /////////////////////////////

    public class ConnectionListener extends MuseConnectionListener {




        @Override
        public void receiveMuseConnectionPacket(MuseConnectionPacket p) {
            final ConnectionState current = p.getCurrentConnectionState();
            final String status = p.getPreviousConnectionState().toString() + " -> " + current;
            final String full = "Muse " + p.getSource().getMacAddress() + " " + status;
            Log.i("Muse Headband", full);

            // UI thread is used here only because we need to update
            // TextView values. You don't have to use another thread, unless
            // you want to run disconnect() or connect() from connection packet
            // handler. In this case creating another thread is required.

                        museStatus = ( status);
                        if (current == ConnectionState.CONNECTED) {
                            MuseVersion museVersion = muse.getMuseVersion();
                            String version = museVersion.getFirmwareType() +
                                    " - " + museVersion.getFirmwareVersion() +
                                    " - " + Integer.toString(
                                    museVersion.getProtocolVersion());
                            Log.i("Muse Headband", version);
                        } else {
                            Log.i("Muse Headband", "unknown version");
                        }




        }
    }
    public class DataListener extends MuseDataListener {



        @Override
        public void receiveMuseDataPacket(MuseDataPacket p) {
            switch (p.getPacketType()) {
                case EEG:
                    updateEeg(p.getValues());
                    break;
                case ACCELEROMETER:
                    updateAccelerometer(p.getValues());
                    break;
                case ALPHA_RELATIVE:
                    updateAlphaRelative(p.getValues());
                    break;
                case BATTERY:

                    // It's library client responsibility to flush the buffer,
                    // otherwise you may get memory overflow.
                    museBattery = p.getValues().toString();
                    break;
                default:
                    break;
            }

        }

        @Override
        public void receiveMuseArtifactPacket(MuseArtifactPacket p) {
            if (p.getHeadbandOn() && p.getBlink()) {
                Log.i("Artifacts", "blink");
            }
        }

        private void updateAccelerometer(final ArrayList<Double> data) {

                        // Convert data type to String
                        accX = data.get(Accelerometer.FORWARD_BACKWARD.ordinal()).toString();
                        accY = data.get(Accelerometer.UP_DOWN.ordinal()).toString();
                        accZ = data.get(Accelerometer.LEFT_RIGHT.ordinal()).toString();


        }

        private void updateEeg(final ArrayList<Double> data) {



                        // Convert data type to String
                        eeg1 = data.get(Eeg.TP9.ordinal()).toString();
                        eeg2 = data.get(Eeg.FP1.ordinal()).toString();
                        eeg3 = data.get(Eeg.FP2.ordinal()).toString();
                        eeg4 = data.get(Eeg.TP10.ordinal()).toString();

                        Log.d("EEG Updated =>", " EEG1: " + eeg1 + " EEG2:" + eeg2 + " EEG3: " + eeg3 + " EEG4: " + eeg4);

        }

        private void updateAlphaRelative(final ArrayList<Double> data) {



                        //Convert data type to String
                        elm1 = data.get(Eeg.TP9.ordinal()).toString();
                        elm2 = data.get(Eeg.FP1.ordinal()).toString();
                        elm3 = data.get(Eeg.FP2.ordinal()).toString();
                        elm4 = data.get(Eeg.TP10.ordinal()).toString();

                        Log.d("Elements Updated =>", " E1: " + elm1 + " E2:" + elm2 + " E3: " + elm3 + " E4: " + elm4);

        }


    }

    private Muse muse = null;
    private ConnectionListener connectionListener = null;
    private DataListener dataListener = null;
    private boolean dataTransmission = true;



    private void configureLibrary() {
        muse.registerConnectionListener(connectionListener);
        muse.registerDataListener(dataListener,
                MuseDataPacketType.ACCELEROMETER);
        muse.registerDataListener(dataListener,
                MuseDataPacketType.EEG);
        muse.registerDataListener(dataListener,
                MuseDataPacketType.ALPHA_RELATIVE);
        muse.registerDataListener(dataListener,
                MuseDataPacketType.ARTIFACTS);
        muse.registerDataListener(dataListener,
                MuseDataPacketType.BATTERY);
        muse.setPreset(MusePreset.PRESET_14);
        muse.enableDataTransmission(dataTransmission);
    }

    public MuseListenerService(){

        connectionListener = new ConnectionListener();
        dataListener = new DataListener();
    }

    public void museConnect (){

        List<Muse> pairedMuses = MuseManager.getPairedMuses();
        if (pairedMuses.size() < 1 ) {
            Log.w("Muse Headband", "There is nothing to connect to");

        } else {
            muse = pairedMuses.get(0);
            ConnectionState state = muse.getConnectionState();
            if (state == ConnectionState.CONNECTED ||
                    state == ConnectionState.CONNECTING) {
                Log.w("Muse Headband",
                        "doesn't make sense to connect second time to the same muse");
                return;
            }
            configureLibrary();
            try {
                muse.runAsynchronously();
            } catch (Exception e) {
                Log.e("Muse Headband", e.toString());
            }
            museStatus = muse.getConnectionState().toString();
    }

    }

    public void museDisconnect (){
        if (muse != null) {
            /**
             * true flag will force libmuse to unregister all listeners,
             * BUT AFTER disconnecting and sending disconnection event.
             * If you don't want to receive disconnection event (for ex.
             * you call disconnect when application is closed), then
             * unregister listeners first and then call disconnect:
             * muse.unregisterAllListeners();
             * muse.disconnect(false);
             */

            muse.disconnect(true);

        }
    }

public String getMuseValues (){

    return museValues;

}

    public String getMuseStatus(){
        return museStatus;
    }
}