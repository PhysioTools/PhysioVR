package com.harry.physiosense;

// defaults

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.harry.physiosense.services.MuseListenerService;
import com.harry.physiosense.services.WearListenerService;
import com.harry.physiosense.services.myo.MYOListenerService;


import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketException;
import java.net.UnknownHostException;
import java.util.Timer;
import java.util.TimerTask;


public class PhoneActivity extends Activity {


    final WearListenerService wearListenerService = new WearListenerService();
    final MuseListenerService museListenerService = new MuseListenerService();
    final MYOListenerService myoListenerService = new MYOListenerService();

    public TextView textWear, textMuse, textMyo;
    public ImageView imgWear, imgMuse, imgMyo;

    Timer udpTimer;
    UDPSender udpSender;
    int frequency = 2000;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_phone);

        // status icons and texts
        textWear = (TextView) findViewById(R.id.txtWearStatus);
        imgWear = (ImageView) findViewById(R.id.imgWear);

        textMuse = (TextView) findViewById(R.id.txtMuseStatus);
        imgMuse = (ImageView) findViewById(R.id.imgMuse);

        textMyo = (TextView) findViewById(R.id.txtMyoStatus);
        imgMyo = (ImageView) findViewById(R.id.imgMyo);





        udpTimer = new Timer();
        udpSender = new UDPSender();
        udpTimer.scheduleAtFixedRate(udpSender, 0, frequency);

    }



    public void connectSensors(View view) {
        // connect to Muse
        museListenerService.museConnect();
        Toast.makeText(getApplicationContext(), "Connecting", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onDestroy() {
        super.onDestroy();

        setContentView(R.layout.activity_phone);
        museListenerService.museDisconnect();
    }



    class UDPSender extends TimerTask {



        @Override
        public void run() {


            updateStatus();

            /////////////////////////////
            //////// UDP Sender /////////
            /////////////////////////////
            // String values from the Services

            String wearHRate = "HR: " + wearListenerService.getWearHRate();


            String museValues = "MUSE: " + museListenerService.getMuseValues();


            String myoValues = "MYO" + myoListenerService.getMYOValues();


            int UDP_SERVER_PORT = 11111;

            String udpMsg = museValues + " | " + wearHRate + " | " + myoValues;
            Log.d("UDP Sent: ", wearHRate + " | " + museValues + " | " + myoValues);

            DatagramSocket ds = null;
            try {
                ds = new DatagramSocket();
                InetAddress serverAddr = InetAddress.getByName("127.0.0.1");

                DatagramPacket dp;
                dp = new DatagramPacket(udpMsg.getBytes(), udpMsg.length(), serverAddr, UDP_SERVER_PORT);
                ds.send(dp);

                Log.d("UDP Sent: ", udpMsg + " via " + UDP_SERVER_PORT + "to: " + serverAddr);

            } catch (SocketException e) {
                e.printStackTrace();
            } catch (UnknownHostException e) {
                e.printStackTrace();
            } catch (IOException e) {
                e.printStackTrace();
            } catch (Exception e) {
                e.printStackTrace();
            } finally {
                if (ds != null) {
                    ds.close();
                }
            }
        }
        String wearStatus = wearListenerService.getWearStatus();
        String museStatus = museListenerService.getMuseStatus();
        String myoStatus = myoListenerService.getMyoStatus();

        public void updateStatus() {

            runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    textWear.setText(wearStatus);
                    if (wearStatus.equals("Connected")) {
                        imgWear.setImageResource(R.drawable.hr_on);
                    } else {
                        imgWear.setImageResource(R.drawable.hr_off);
                    }


                    textMuse.setText(museStatus);
                    if (museStatus.equals("Connected")) {
                        imgMuse.setImageResource(R.drawable.bci_on);
                    } else {
                        imgMuse.setImageResource(R.drawable.bci_off);
                    }


                    textMyo.setText(myoStatus);
                    if (myoStatus.equals("Connected")) {
                        imgMyo.setImageResource(R.drawable.emg_on);
                    } else {
                        imgMyo.setImageResource(R.drawable.emg_off);
                    }
                }
            });
        }
    }
    public void exitService(View view) {

        Log.d("PhysioSense", "STOPPED by the USER");
        this.finish();
        System.exit(0);
    }

    public void physioVersion (View view) {
       Toast.makeText(getApplicationContext(), "PhysioSense v1.0 alpha", Toast.LENGTH_SHORT).show();
    }
}
